using System;
using System.Collections.Generic;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class TAItemRepository : ITAItemRepository
    {
        string m_dbPath = "";

        public TAItemRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public void AddTAItem(TAItem TAItem)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(TAItem);
                ctx.SaveChanges();
            }
        }

        public void RemoveTAItem(TAItem TAItem)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(TAItem);
                ctx.SaveChanges();
            }
        }

        public void UpdateTAItem(TAItem TAItem)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(TAItem);
                ctx.SaveChanges();
            }
        }

        public List<TAItem> GetAllTAItems()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.TAItems.ToList();
            }
        }

        public List<TAItem> GetTAItems(int workId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.TAItems.Where(b => b.WorkId == workId).ToList();
            }
        }

        public TAItem GetTAItem(int TAItemId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.TAItems.Where(b => b.Id == TAItemId).FirstOrDefault();
            }
        }

        public void DeleteAllTAItems()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(GetAllTAItems());
                ctx.SaveChanges();
            }
        }
    }
}
