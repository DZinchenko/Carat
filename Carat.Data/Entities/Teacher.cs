using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double StaffUnit { get; set; } = 1.00;
        public string Position { get; set; } = "<not set>";
        public string Rank { get; set; } = "<not set>";
        public string Degree { get; set; } = "<not set>";
        public string Note { get; set; }
    }
}
