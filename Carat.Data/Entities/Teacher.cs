using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double StaffUnit { get; set; }
        public string Position { get; set; }
        public string Rank { get; set; }
        public string Degree { get; set; }
        public string Note { get; set; }
    }
}
