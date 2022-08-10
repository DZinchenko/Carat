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

        public List<TAItem> GetAllTAItems(int workId, string educType, string educForm, uint semestr, string educlevel)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.TAItems.Where(b => b.EducType == educType
                                                   && b.EducForm == educForm
                                                   && b.Semestr == semestr
                                                   && b.EducLevel == educlevel
                                                   && b.WorkId == workId).ToList();
            }
        }

        public List<TAItem> GetAllTAItems(int workId, string educType, string educForm, uint course, uint semestr, string educlevel)
        {
            if (course == 0)
                return GetAllTAItems(workId, educType, educForm, semestr, educlevel);

            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.TAItems.Where(b => b.EducType == educType
                                                   && b.EducForm == educForm
                                                   && b.Course == course
                                                   && b.Semestr == semestr
                                                   && b.EducLevel == educlevel
                                                   && b.WorkId == workId).ToList();
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

        public double GetAssignedTeacherHours(int teacherId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var taItems = ctx.TAItems.Where(b => b.TeacherId == teacherId).ToList();
                double result = 0;

                foreach (var item in taItems)
                {
                    result += item.WorkHours;
                }

                return result;
            }
        }

        public double GetAssignedTeacherHours(int teacherId, string educType)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var taItems = ctx.TAItems.Where(b => b.TeacherId == teacherId && b.EducType == educType).ToList();
                double result = 0;

                foreach (var item in taItems)
                {
                    result += item.WorkHours;
                }

                return result;
            }
        }

        public List<TAItem> GetTAItemsByTeacherId(int teacherId, uint semestr, string educType, string educForm)
        {
            if (educForm == "<всі>")
            {
                if (educType == "<всі>")
                {
                    using (var ctx = new CaratDbContext(m_dbPath))
                    {
                        if (semestr != 0)
                        {
                            return ctx.TAItems.Where(b => b.TeacherId == teacherId
                                               && b.Semestr == semestr).ToList();
                        }
                        else
                        {
                            return ctx.TAItems.Where(b => b.TeacherId == teacherId).ToList();
                        }
                    }
                }
                else
                {
                    using (var ctx = new CaratDbContext(m_dbPath))
                    {
                        if (semestr != 0)
                        {
                            return ctx.TAItems.Where(b => b.EducType == educType
                                               && b.TeacherId == teacherId
                                               && b.Semestr == semestr).ToList();
                        }
                        else
                        {
                            return ctx.TAItems.Where(b => b.EducType == educType
                                               && b.TeacherId == teacherId).ToList();
                        }
                    }
                }
            }
            else {
                if (educType == "<всі>")
                {
                    using (var ctx = new CaratDbContext(m_dbPath))
                    {
                        if (semestr != 0)
                        {
                            return ctx.TAItems.Where(b => b.TeacherId == teacherId
                                               && b.EducForm == educForm
                                               && b.Semestr == semestr).ToList();
                        }
                        else
                        {
                            return ctx.TAItems.Where(b => b.EducForm == educForm && b.TeacherId == teacherId).ToList();
                        }
                    }
                }
                else
                {
                    using (var ctx = new CaratDbContext(m_dbPath))
                    {
                        if (semestr != 0)
                        {
                            return ctx.TAItems.Where(b => b.EducType == educType
                                               && b.EducForm == educForm
                                               && b.TeacherId == teacherId
                                               && b.Semestr == semestr).ToList();
                        }
                        else
                        {
                            return ctx.TAItems.Where(b => b.EducType == educType
                                               && b.EducForm == educForm
                                               && b.TeacherId == teacherId).ToList();
                        }
                    }
                }
            }
        }

        public List<TAItem> GetTAItemsByTeacherIdWithouFilters(int teacherId, uint semestr)
        {
            if (semestr == 0)
            {
                using (var ctx = new CaratDbContext(m_dbPath))
                {
                    return ctx.TAItems.Where(b => b.TeacherId == teacherId).ToList();
                }
            }
            else
            {
                using (var ctx = new CaratDbContext(m_dbPath))
                {
                    return ctx.TAItems.Where(b => b.TeacherId == teacherId && b.Semestr == semestr).ToList();
                }
            }

        }

        public void DeleteAllTAItems()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(ctx.TAItems);
                ctx.SaveChanges();
            }
        }

        public bool ExistTAItemsForWorks(List<Work> works)
        {
            var workIds = works.Select(w => w.Id);
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.TAItems.Where(tai => workIds.Contains(tai.WorkId)).Any();
            }
        }

        public void DeleteAllTAItemsForWorks(List<int> workIds, string educType, string educForm, uint course, uint semestr, string educlevel)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var items =  ctx.TAItems.Where(b => (b.EducType == educType || educType == "<всі>")
                                                   && (b.EducForm == educForm || educForm == "<всі>")
                                                   && b.Semestr == semestr
                                                   && b.EducLevel == educlevel
                                                   && b.Course == course
                                                   && workIds.Contains(b.WorkId));

                ctx.RemoveRange(items);
                ctx.SaveChanges();
            }
        }

        public void AddTAItems(List<TAItem> TAItems)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.AddRange(TAItems);
                ctx.SaveChanges();
            }
        }
    }
}
