using Carat.Data.DTOs;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class ReportDTORepository : IReportDTORepository
    {
        private string m_dbPath = "";

        public ReportDTORepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public FinalTeachersReportDTO getFinalTeachersReportDTO(string educType, string educForm, uint course, uint semestr, string educLevel)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var res = new FinalTeachersReportDTO();
                var taItemqQuery = ctx.TAItems.Where(ta => (ta.Course == course || course == 0)
                                                && (ta.EducForm == educForm || educForm == "<всі>")
                                                && (ta.EducLevel == educLevel || educLevel == "<всі>")
                                                && (ta.EducType == educType || educType == "<всі>")
                                                && (ta.Semestr == semestr || semestr == 0));
                var mainQuery = taItemqQuery.Join(ctx.Works, ta => ta.WorkId, w => w.Id, (ta, w) => new { Work = w, TAItem = ta })
                    .Join(ctx.CurriculumItems, a => a.Work.CurriculumItemId, ci => ci.Id, (a, ci) => new { CI = ci, Work = a.Work, TAItem = a.TAItem }).AsEnumerable();

                res.Teachers = ctx.Teachers.Where(t => taItemqQuery.Select(ta => ta.TeacherId).Contains(t.Id)).OrderBy(t => t.Name).ToList();
                var subjectQuery = mainQuery.Join(ctx.Subjects, a => a.CI.SubjectId, s => s.Id, (a, s) => new { Id = a.CI.Id, Subject = s });

                res.GroupNamesByCurriculumItemIds = mainQuery.Join(ctx.GroupsToTeachers, a => a.TAItem.Id, gtt => gtt.TAItemID, (a, gtt) => new { CIId = a.CI.Id, GroupId = gtt.GroupId })
                                                    .Join(ctx.Groups, b => b.GroupId, g => g.Id, (b, g) => new { CIId = b.CIId, GroupName = g.Name })
                                                    .AsEnumerable().GroupBy(c => c.CIId, c => c.GroupName).ToDictionary(g => g.Key, g => g.Distinct().ToList());
                res.TAItemsByCurriculumItemIds = mainQuery.GroupBy(a => a.CI.Id, a => a.TAItem).ToDictionary(g => g.Key, g => g.ToList());
                res.WorksForTAItems = ctx.Works.Where(w => taItemqQuery.Any(ta => ta.WorkId == w.Id)).Distinct().ToList();
                res.CurriculumItemsByTeacherIds = mainQuery.GroupBy(a => a.TAItem.TeacherId, a => a.CI)
                                                .ToDictionary(g => g.Key, g => g.Distinct().OrderBy(ci => subjectQuery.First(s => s.Id == ci.Id).Subject.Name).ToList());
                res.SubjectsByCurriculumItemIds = subjectQuery.Distinct().ToDictionary(a => a.Id, a => a.Subject);

                return res;
            }
        }
    }
}
