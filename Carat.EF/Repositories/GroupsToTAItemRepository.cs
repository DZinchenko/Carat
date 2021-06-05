using System;
using System.Collections.Generic;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class GroupsToTAItemRepository : IGroupsToTAItemRepository
    {
        private string m_dbPath = "";

        public GroupsToTAItemRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public List<GroupsToTAItem> GetAllGroupsToTAItem()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.GroupsToTeachers.ToList();
            }
        }
        public List<GroupsToTAItem> GetGroupsToTAItem(int TAItemId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.GroupsToTeachers.Where(b => b.TAItemID == TAItemId).ToList();
            }
        }

        public GroupsToTAItem GetGroupsToTAItem(int TAItemId, int groupId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.GroupsToTeachers.Where(b => b.TAItemID == TAItemId && b.GroupId == groupId).FirstOrDefault();
            }
        }

        public void AddGroupsToTAItem(GroupsToTAItem groupsToTAItem)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(groupsToTAItem);
                ctx.SaveChanges();
            }
        }

        public void RemoveGroupsToTAItem(GroupsToTAItem groupsToTAItem)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(groupsToTAItem);
                ctx.SaveChanges();
            }
        }

        public void UpdateGroupsToTAItem(GroupsToTAItem groupsToTAItem)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(groupsToTAItem);
                ctx.SaveChanges();
            }
        }

        public void DeleteAllGroupsToTAItem()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(GetAllGroupsToTAItem());
                ctx.SaveChanges();
            }
        }

        public List<GroupsToTAItem> GetGroupsToTAItemsByGroupId(int groupId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.GroupsToTeachers.Where(b => b.GroupId == groupId).ToList();
            }
        }

        public List<GroupsToTAItem> GetGroupsToTAItemsByTAItemId(int TAItemId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.GroupsToTeachers.Where(b => b.TAItemID == TAItemId).ToList();
            }
        }

        public void RemoveGroupsToTAItems(List<GroupsToTAItem> groupsToTAItems)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(groupsToTAItems);
                ctx.SaveChanges();
            }
        }
    }
}
