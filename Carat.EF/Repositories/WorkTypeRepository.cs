using System;
using System.Collections.Generic;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class WorkTypeRepository : IWorkTypeRepository
    {
        private string m_dbPath = "";

        public WorkTypeRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public List<WorkType> GetAllWorkTypes()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.WorkTypes.ToList();
            }
        }
        public WorkType GetWorkType(int workTypeId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.WorkTypes.Where(b => b.Id == workTypeId).FirstOrDefault();
            }
        }

        public void AddWorkType(WorkType workType)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(workType);
                ctx.SaveChanges();
            }
        }

        public void RemoveWorkType(WorkType workType)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(workType);
                ctx.SaveChanges();
            }
        }

        public void UpdateWorkType(WorkType workType)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(workType);
                ctx.SaveChanges();
            }
        }

        public void DeleteAllWorkTypes()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(GetAllWorkTypes());
                ctx.SaveChanges();
            }
        }
    }
}
