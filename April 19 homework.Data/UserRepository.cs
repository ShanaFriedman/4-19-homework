using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace April_19_homework.Data
{
    public class UserRepository
    {
        private string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User u, string password)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO UsersTable(Name, Email, HashPassword) 
                                    VALUES(@name, @email, @hashPassword)";
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            command.Parameters.AddWithValue("@name", u.Name);
            command.Parameters.AddWithValue("@email", u.Email);
            command.Parameters.AddWithValue("@hashPassword", hashPassword);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public User Login(string email, string password)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            User u = GetByEmail(email);
            if(u == null)
            {
                return null;
            };

            if (!BCrypt.Net.BCrypt.Verify(password, u.PasswordHash))
            {
                return null;
            }
            return u;
        }
        public User GetByEmail(string email)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM UsersTable WHERE email = @email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new User()
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Email = email,
                PasswordHash = (string)reader["HashPassword"]
            };
        }
        public List<Ad> GetAds(int id)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT at.*, ut.Name FROM AdsTable at
                                    JOIN UsersTable ut
                                    ON at.UserId = ut.Id";
            if(id != 0)
            {
                command.CommandText += " WHERE at.UserId = @id";
                command.Parameters.AddWithValue("@id", id);
            }
            List<Ad> ads = new List<Ad>();
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(new()
                {
                    Id = (int)reader["Id"],
                    Description = (string)reader["Description"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Date = (DateTime)reader["Date"],
                    Name = (string)reader["Name"],
                    UserId = (int)reader["UserId"]
                });
            }
            return ads;
        }
        public void NewAd(Ad a, int userId)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO AdsTable(Description, PhoneNumber, Date, UserId)
                                    VALUES(@description, @phoneNumber, @date, @userId)";
            command.Parameters.AddWithValue("@description", a.Description);
            command.Parameters.AddWithValue("@phoneNumber", a.PhoneNumber);
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void DeleteAd(int id)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM AdsTable WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
