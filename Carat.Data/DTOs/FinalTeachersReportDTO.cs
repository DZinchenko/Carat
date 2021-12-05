using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;

namespace Carat.Data.DTOs
{
    public class FinalTeachersReportDTO
    {
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public Dictionary<int, List<CurriculumItem>> CurriculumItemsByTeacherIds { get; set; } = new Dictionary<int, List<CurriculumItem>>();
        public Dictionary<int, Subject> SubjectsByCurriculumItemIds { get; set; } = new Dictionary<int, Subject>();
        public Dictionary<int, List<string>> GroupNamesByCurriculumItemIds { get; set; } = new Dictionary<int, List<string>>();
        public Dictionary<int, List<TAItem>> TAItemsByCurriculumItemIds { get; set; } = new Dictionary<int, List<TAItem>>();
        public List<Work> WorksForTAItems { get; set; } = new List<Work>();
    }
}
