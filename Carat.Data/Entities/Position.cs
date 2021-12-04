using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Entities
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinHours { get; set; } = 0;
        public int MaxHours { get; set; } = 0;
    }
}
