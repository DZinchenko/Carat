using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class RankRepository: IRankRepository
    {
        private string m_dbPath = "";

        public RankRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public void AddRank(Rank degree)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(degree);
                ctx.SaveChanges();
            }
        }

        public List<Rank> GetAllRanks()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Degrees.ToList();
            }
        }

        public Rank GetRank(int id)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Degrees.FirstOrDefault(d => d.Id == id);
            }
        }

        public Rank GetRank(string name)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Degrees.FirstOrDefault(d => d.Name == name);
            }
        }

        public void RemoveRank(Rank degree)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(degree);
                ctx.SaveChanges();
            }
        }

        public void UpdateRank(Rank degree)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(degree);
                ctx.SaveChanges();
            }
        }
    }
}
