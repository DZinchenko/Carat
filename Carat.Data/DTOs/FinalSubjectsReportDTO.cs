using Carat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.DTOs
{
    public class FinalSubjectsReportDTO
    {
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public Dictionary<int, List<CurriculumItem>> CurriculumItemsBySubjectsIds { get; set; } = new Dictionary<int, List<CurriculumItem>>();
        public Dictionary<int, List<Teacher>> TeachersByCurItemsIds { get; set; } = new Dictionary<int, List<Teacher>>();
        public Dictionary<int, Dictionary<int, List<string>>> GroupNamesByTeacherIdsByCurItemIds { get; set; } = new Dictionary<int, Dictionary<int, List<string>>>();
        public Dictionary<int, Dictionary<int, List<TAItem>>> TAItemsByTeacherIdsByCurItemIds { get; set; } = new Dictionary<int, Dictionary<int, List<TAItem>>>();
        public List<Work> WorksForTAItems { get; set; } = new List<Work>();
    }
}
