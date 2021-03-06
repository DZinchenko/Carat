using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Entities
{
    public class WorkType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double StudentHours { get; set; }
        public double TotalHours { get; set; }
    }
}
