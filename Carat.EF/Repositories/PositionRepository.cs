using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private string m_dbPath = "";

        public PositionRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public void AddPosition(Position position)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(position);
                ctx.SaveChanges();
            }
        }

        public Position GetPosition(int id)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Positions.FirstOrDefault(p => p.Id == id);
            }
        }

        public Position GetPosition(string name)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Positions.FirstOrDefault(p => p.Name == name);
            }
        }

        public List<Position> GetPositions()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Positions.ToList();
            }
        }

        public void RemovePosition(Position position)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(position);
                ctx.SaveChanges();
            }
        }

        public void UpdatePosition(Position position)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(position);
                ctx.SaveChanges();
            }
        }
    }
}
