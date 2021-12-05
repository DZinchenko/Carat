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

        public List<Group> GetAllGroups<OrderType>(Func<Group, OrderType> orderFunc)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var result = ctx.Groups.ToList();
                return result.OrderBy(orderFunc).ToList();
            }
        }
        public Group GetGroup(int groupId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Groups.Where(b => b.Id == groupId).FirstOrDefault();
            }
        }

        public List<Group> GetGroups<OrderType>(Func<Group, OrderType> orderFunc, uint course, string educForm, string educLevel)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var result = ctx.Groups.Where(b => b.Course == course
                                          && b.EducForm == educForm
                                          && b.EducLevel == educLevel).ToList();
                return result.OrderBy(orderFunc).ToList();
            }
        }

        public List<Group> GetGroupsForReports<OrderType>(Func<Group, OrderType> orderFunc, uint course, string educForm, string educLevel)
        {
            if (educForm == "<всі>")
            {
                if (educLevel == "<всі>")
                {
                    if (course == 0)
                    {
                        using (var ctx = new CaratDbContext(m_dbPath))
                        {
                            var result = ctx.Groups.ToList();
                            return result.OrderBy(orderFunc).ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new CaratDbContext(m_dbPath))
                        {
                            var result = ctx.Groups.Where(b => b.Course == course).ToList();
                            return result.OrderBy(orderFunc).ToList();
                        }
                    }
                }
                else
                {
                    if (course == 0)
                    {
                        using (var ctx = new CaratDbContext(m_dbPath))
                        {
                            var result = ctx.Groups.Where(b => b.EducLevel == educLevel).ToList();
                            return result.OrderBy(orderFunc).ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new CaratDbContext(m_dbPath))
                        {
                            var result = ctx.Groups.Where(b => b.Course == course
                                                         && b.EducLevel == educLevel).ToList();
                            return result.OrderBy(orderFunc).ToList();
                        }
                    }
                }
            }
            else
            {
                if (educLevel == "<всі>")
                {
                    if (course == 0)
                    {
                        using (var ctx = new CaratDbContext(m_dbPath))
                        {
                            var result = ctx.Groups.Where(b => b.EducForm == educForm).ToList();
                            return result.OrderBy(orderFunc).ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new CaratDbContext(m_dbPath))
                        {
                            var result = ctx.Groups.Where(b => b.Course == course
                                                         && b.EducForm == educForm).ToList();
                            return result.OrderBy(orderFunc).ToList();
                        }
                    }
                }
                else
                {
                    if (course == 0)
                    {
                        using (var ctx = new CaratDbContext(m_dbPath))
                        {
                            var result = ctx.Groups.Where(b => b.EducLevel == educLevel
                                                         && b.EducForm == educForm).ToList();
                            return result.OrderBy(orderFunc).ToList();
                        }
                    }
                    else
                    {
                        using (var ctx = new CaratDbContext(m_dbPath))
                        {
                            var result = ctx.Groups.Where(b => b.Course == course
                                                         && b.EducLevel == educLevel
                                                         && b.EducForm == educForm).ToList();
                            return result.OrderBy(orderFunc).ToList();
                        }
                    }
                }
            }
        }

        public void AddGroup(Group group)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(group);
                ctx.SaveChanges();
            }
        }

        public void AddGroups(List<Group> groups)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.AddRange(groups);
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
                ctx.RemoveRange(GetAllGroups(a => a.Name));
                ctx.SaveChanges();
            }
        }

        public List<Group> GetGroups(List<int> groupIds)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Groups.Where(g => groupIds.Contains(g.Id)).ToList();
            }
        }
    }
}
