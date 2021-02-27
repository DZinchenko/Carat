using System;
using System.Collections.Generic;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private string m_dbPath = "";

        public GroupRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public List<Group> GetAllGroups()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Groups.ToList();
            }
        }
        public Group GetGroup(int groupId)
        {
            throw new NotImplementedException();
        }

        public void AddGroup(Group group)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(group);
                ctx.SaveChanges();
            }
        }

        public void RemoveGroup(Group group)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(group);
                ctx.SaveChanges();
            }
        }

        public void UpdateGroup(Group group)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(group);
                ctx.SaveChanges();
            }
        }

        public void DeleteAllGroups()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(GetAllGroups());
                ctx.SaveChanges();
            }
        }
    }
}
