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
        public List<WorkType> WorkTypes { get; set; }
    }
}
