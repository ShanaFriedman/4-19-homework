using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace April_19_homework.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
