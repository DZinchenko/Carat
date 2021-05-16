using System;
using System.Collections.Generic;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class LastModeRepository : ILastModeRepository
    {
        private string m_dbPath = "";

        public LastModeRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public LastMode GetLastMode()
        {
            using (var ctx = new LastModeDbContext(m_dbPath))
            {
                return ctx.LastModes.ToList().FirstOrDefault();
            }
        }

        public void UpdateLastMode(LastMode lastMode)
        {
            using (var ctx = new LastModeDbContext(m_dbPath))
            {
                ctx.RemoveRange(ctx.LastModes.ToList());
                ctx.Add(lastMode);
                ctx.SaveChanges();
            }
        }
    }
}
