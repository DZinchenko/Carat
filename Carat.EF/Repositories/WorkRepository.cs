﻿using System;
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

        public List<Work> GetWorks(int curriculumItemId, bool withoutZeroHours)
        {
            if (withoutZeroHours)
            {
                using (var ctx = new CaratDbContext(m_dbPath))
                {
                    return ctx.Works.Where(b => b.CurriculumItemId == curriculumItemId && b.TotalHours > 0.00001).ToList();
                }
            }
            else
            {
                using (var ctx = new CaratDbContext(m_dbPath))
                {
                    var works = ctx.Works.Where(b => b.CurriculumItemId == curriculumItemId).ToList();
                    works.Sort((emp1, emp2) => emp1.WorkTypeId.CompareTo(emp2.WorkTypeId));
                    return works;
                }
            }
        }

        public Dictionary<int, List<Work>> GetWorksForCurriculumItemIds(List<int> curriculumItemIds, bool withoutZeroHours)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Works.Where(w => curriculumItemIds.Contains(w.CurriculumItemId) && (w.TotalHours > 0.00001 || !withoutZeroHours))
                    .AsEnumerable().GroupBy(w => w.CurriculumItemId).ToDictionary(g => g.Key, g => g.ToList());
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

        public void AddWorks(List<Work> works)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.AddRange(works);
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

        public void DeleteWorks(List<Work> works)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(works);
                ctx.SaveChanges();
            }
        }
    }
}
