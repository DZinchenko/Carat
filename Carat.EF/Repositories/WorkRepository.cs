using System;
using System.Collections.Generic;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class WorkRepository : IWorkRepository
    {
        private string m_dbPath = "";

        public WorkRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public List<Work> GetAllWorks()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Works.ToList();
            }
        }

        public List<Work> GetWorks(int curriculumItemId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Works.Where(b => b.CurriculumItemId == curriculumItemId).ToList();
            }
        }

        public Work GetWork(int workId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Works.Where(b => b.Id == workId).FirstOrDefault();
            }
        }

        public void AddWork(Work work)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(work);
                ctx.SaveChanges();
            }
        }

        public void RemoveWork(Work work)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(work);
                ctx.SaveChanges();
            }
        }

        public void UpdateWork(Work work)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(work);
                ctx.SaveChanges();
            }
        }

        public void DeleteAllWorks()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(GetAllWorks());
                ctx.SaveChanges();
            }
        }
    }
}
