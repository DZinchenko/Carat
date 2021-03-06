using System;
using System.Collections.Generic;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class CurriculumItemRepository : ICurriculumItemRepository
    {
        private string m_dbPath = "";

        public CurriculumItemRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public List<CurriculumItem> GetAllCurriculumItems()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.CurriculumItems.ToList();
            }
        }
        public CurriculumItem GetCurriculumItem(int curriculumItemId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.CurriculumItems.Where(b => b.Id == curriculumItemId).FirstOrDefault();
            }
        }

        public void AddCurriculumItem(CurriculumItem curriculumItem)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(curriculumItem);
                ctx.SaveChanges();
            }
        }

        public void RemoveCurriculumItem(CurriculumItem curriculumItem)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(curriculumItem);
                ctx.SaveChanges();
            }
        }

        public void UpdateCurriculumItem(CurriculumItem curriculumItem)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(curriculumItem);
                ctx.SaveChanges();
            }
        }

        public void DeleteAllCurriculumItems()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(GetAllCurriculumItems());
                ctx.SaveChanges();
            }
        }
    }
}
