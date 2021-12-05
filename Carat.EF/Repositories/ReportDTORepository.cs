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

        public FinalSubjectsReportDTO getFinalSubjectsReportDTO(string educType, string educForm, uint course, uint semestr, string educlevel)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var res = new FinalSubjectsReportDTO();
                var taItemsQuery = ctx.TAItems.Where(ta => (ta.Course == course || course == 0)
                                                && (ta.EducForm == educForm || educForm == "<всі>")
                                                && (ta.EducLevel == educlevel || educlevel == "<всі>")
                                                && (ta.EducType == educType || educType == "<всі>")
                                                && (ta.Semestr == semestr || semestr == 0));

                var mainQuery = taItemsQuery.Join(ctx.Works, ta => ta.WorkId, w => w.Id, (ta, w) => new { Work = w, TAItem = ta })
                    .Join(ctx.CurriculumItems, a => a.Work.CurriculumItemId, ci => ci.Id, (a, ci) => new { CI = ci, Work = a.Work, TAItem = a.TAItem }).AsEnumerable();
                var mainTAItemsQuery = mainQuery.Join(ctx.Teachers, a => a.TAItem.TeacherId, t => t.Id, (a, t) => new { CI = a.CI, TAItem = a.TAItem, Teacher = t });


                var subjectIds = mainQuery.Select(a => a.CI.SubjectId);
                res.Subjects = ctx.Subjects.Where(s => subjectIds.Contains(s.Id)).OrderBy(s => s.Name).ToList();
                res.CurriculumItemsBySubjectsIds = mainQuery.GroupBy(a => a.CI.SubjectId, a => a.CI).ToDictionary(g => g.Key, g => g.Distinct().ToList());
                res.TeachersByCurItemsIds = mainQuery.Join(ctx.Teachers, a => a.TAItem.TeacherId, t => t.Id, (a, t) => new { CIId = a.CI.Id, Teacher = t })
                                                .AsEnumerable().GroupBy(a => a.CIId, a => a.Teacher).ToDictionary(g => g.Key, g => g.Distinct().ToList());
                res.GroupNamesByTeacherIdsByCurItemIds = mainTAItemsQuery.Join(ctx.GroupsToTeachers, a => a.TAItem.Id, b => b.TAItemID, (a, b) => new { CI = a.CI, Teacher = a.Teacher, GroupId = b.GroupId })
                                                        .Join(ctx.Groups, a => a.GroupId, g => g.Id, (a, g) => new { CIId = a.CI.Id, TeacherId = a.Teacher.Id, Group = g })
                                                        .AsEnumerable().GroupBy(a => a.CIId).Select(b => new { Key = b.Key, Group = b.AsEnumerable().GroupBy(c => c.TeacherId, c => c.Group.Name) })
                                                        .ToDictionary(g => g.Key, g => g.Group.ToDictionary(g1 => g1.Key, g1 => g1.Distinct().ToList()));
                res.TAItemsByTeacherIdsByCurItemIds = mainTAItemsQuery.AsEnumerable().GroupBy(a => a.CI.Id).Select(b => new { Key = b.Key, Group = b.AsEnumerable().GroupBy(c => c.Teacher.Id, c => c.TAItem) })
                                                          .ToDictionary(g => g.Key, g => g.Group.ToDictionary(g1 => g1.Key, g1 => g1.Distinct().ToList()));
                res.WorksForTAItems = ctx.Works.Where(w => taItemsQuery.Any(ta => ta.WorkId == w.Id)).Distinct().ToList();

                return res;
            }
        }

        public FinalTeachersReportDTO getFinalTeachersReportDTO(string educType, string educForm, uint course, uint semestr, string educLevel)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var res = new FinalTeachersReportDTO();
                var taItemsQuery = ctx.TAItems.Where(ta => (ta.Course == course || course == 0)
                                                && (ta.EducForm == educForm || educForm == "<всі>")
                                                && (ta.EducLevel == educLevel || educLevel == "<всі>")
                                                && (ta.EducType == educType || educType == "<всі>")
                                                && (ta.Semestr == semestr || semestr == 0));
                var mainQuery = taItemsQuery.Join(ctx.Works, ta => ta.WorkId, w => w.Id, (ta, w) => new { Work = w, TAItem = ta })
                    .Join(ctx.CurriculumItems, a => a.Work.CurriculumItemId, ci => ci.Id, (a, ci) => new { CI = ci, Work = a.Work, TAItem = a.TAItem }).AsEnumerable();
                var teacherIds = taItemsQuery.Select(ta => ta.TeacherId);
                res.Teachers = ctx.Teachers.Where(t => teacherIds.Contains(t.Id)).OrderBy(t => t.Name).ToList();
                var subjectQuery = mainQuery.Join(ctx.Subjects, a => a.CI.SubjectId, s => s.Id, (a, s) => new { Id = a.CI.Id, Subject = s });

                res.GroupNamesByCurriculumItemIds = mainQuery.Join(ctx.GroupsToTeachers, a => a.TAItem.Id, gtt => gtt.TAItemID, (a, gtt) => new { CIId = a.CI.Id, GroupId = gtt.GroupId })
                                                    .Join(ctx.Groups, b => b.GroupId, g => g.Id, (b, g) => new { CIId = b.CIId, GroupName = g.Name })
                                                    .AsEnumerable().GroupBy(c => c.CIId, c => c.GroupName).ToDictionary(g => g.Key, g => g.Distinct().ToList());
                res.TAItemsByCurriculumItemIds = mainQuery.GroupBy(a => a.CI.Id, a => a.TAItem).ToDictionary(g => g.Key, g => g.ToList());
                res.WorksForTAItems = ctx.Works.Where(w => taItemsQuery.Any(ta => ta.WorkId == w.Id)).Distinct().ToList();
                res.CurriculumItemsByTeacherIds = mainQuery.GroupBy(a => a.TAItem.TeacherId, a => a.CI)
                                                .ToDictionary(g => g.Key, g => g.Distinct().OrderBy(ci => subjectQuery.First(s => s.Id == ci.Id).Subject.Name).ToList());
                res.SubjectsByCurriculumItemIds = subjectQuery.Distinct().ToDictionary(a => a.Id, a => a.Subject);

                return res;
            }
        }
    }
}
