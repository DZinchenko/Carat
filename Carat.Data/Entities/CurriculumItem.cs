using System.Collections.Generic;

namespace Carat.Data.Entities
{
    public class CurriculumItem
    {
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public double SubjectHours { get; set; }
        public List<WorkType> WorkTypes { get; set; }
        public double WorkTypeHours { get; set; }
    }
}
