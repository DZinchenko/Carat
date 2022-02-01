using Carat.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.EF.Repositories
{
    public class ClearingRepository : IClearingRepository
    {
        private string m_dbPath = "";

        public ClearingRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public void ClearLoad()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(ctx.TAItems);
                ctx.RemoveRange(ctx.GroupsToTeachers);
                ctx.SaveChanges();
            }
        }

        public void ClearCurriculum()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(ctx.CurriculumItems);
                ctx.RemoveRange(ctx.Works);
                ctx.SaveChanges();
            }
        }

        public void ClearGroups()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(ctx.Groups);
                ctx.SaveChanges();
            }
        }

        public void ClearSubjects()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(ctx.Subjects);
                ctx.SaveChanges();
            }
        }

        public void ClearTeachers()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(ctx.Teachers);
                ctx.SaveChanges();
            }
        }
    }
}
