using System.Collections.Generic;

namespace Carat.Data.Entities
{
    public class CurriculumItem
    {
        public int Id { get; set; }
        public uint Course { get; set; }
        public int SubjectId { get; set; }
        public double SubjectHours { get; set; }
        public double WorkTypeHours { get; set; }
        public string EducType { get; set; }
        public string EducForm { get; set; }
        public uint Semestr { get; set; }
        public string EducLevel { get; set; }
    }
}
