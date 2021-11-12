using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;

namespace Carat.Data.DTOs
{
    public class FinalTeachersReportDTO
    {
        public List<Teacher> Teachers { get; set; }
        public Dictionary<int, List<CurriculumItem>> CurriculumItemsByTeacherIds { get; set; }
        public Dictionary<int, Subject> SubjectsByCurriculumItemIds { get; set; }
        public Dictionary<int, List<string>> GroupNamesByCurriculumItemIds { get; set; }
        public Dictionary<int, List<TAItem>> TAItemsByCurriculumItemIds { get; set; }
        public List<Work> WorksForTAItems { get; set; }
    }
}
