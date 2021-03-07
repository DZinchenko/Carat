using System.Collections.Generic;

namespace Carat.Data.Entities
{
    public class CurriculumItem
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public double SubjectHours { get; set; }
        //public virtual ICollection<WorkType> WorkTypes { get; set; } = new List<WorkType>();
        public double WorkTypeHours { get; set; }
    }
}
