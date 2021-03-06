using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Entities
{
    public class WorkType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public uint StudentHours { get; set; }
        public uint TotalHours { get; set; }
    }
}
