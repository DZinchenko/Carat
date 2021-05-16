using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Entities
{
    public class LastMode
    {
        public int Id { get; set; }
        public string DbPath { get; set; } = "";
        public string EducForm { get; set; } = "Денна";
        public string EducLevel { get; set; } = "Бакалавр";
        public string EducType { get; set; } = "Бюджет";
        public uint Course { get; set; } = 0;
    }
}
