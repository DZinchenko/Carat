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

        public CurriculumItem GetCurriculumItem(int subjectId, string educType, string educForm, uint course, uint semestr, string educlevel)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.CurriculumItems.Where(b => b.EducType == educType
                                                   && b.SubjectId == subjectId
                                                   && b.EducForm == educForm
                                                   && b.Course == course
                                                   && b.Semestr == semestr
                                                   && b.EducLevel == educlevel).FirstOrDefault();
            }
        }

        public List<CurriculumItem> GetCurriculumItems<OrderType>(Func<CurriculumItem, OrderType> orderFunc, string educType, string educForm)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                   && b.EducForm == educForm).ToList();
                return result.OrderBy(orderFunc).ToList();
            }
        }

        public List<CurriculumItem> GetAllCurriculumItemsForReports<OrderType>(Func<CurriculumItem, OrderType> orderFunc, string educType, string educForm, uint course, uint semestr, string educlevel)
        {
            if (educType == "<всі>")
            {
                if (educlevel == "<всі>")
                {
                    if (educForm == "<всі>")
                    {
                        if (course == 0)
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Semestr == semestr).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                        else
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Course == course).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Semestr == semestr
                                                                       && b.Course == course).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (course == 0)
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducForm == educForm).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Semestr == semestr
                                                                       && b.EducForm == educForm).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                        else
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Course == course
                                                                       && b.EducForm == educForm).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Semestr == semestr
                                                                       && b.Course == course
                                                                       && b.EducForm == educForm).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (educForm == "<всі>")
                    {
                        if (course == 0)
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Semestr == semestr
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                        else
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Course == course
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Semestr == semestr
                                                                       && b.Course == course
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (course == 0)
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducForm == educForm
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Semestr == semestr
                                                                       && b.EducForm == educForm
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                        else
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Course == course
                                                                       && b.EducForm == educForm
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.Semestr == semestr
                                                                       && b.Course == course
                                                                       && b.EducForm == educForm
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (educlevel == "<всі>")
                {
                    if (educForm == "<всі>")
                    {
                        if (course == 0)
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Semestr == semestr).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                        else
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Course == course).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Semestr == semestr
                                                                       && b.Course == course).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (course == 0)
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.EducForm == educForm).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Semestr == semestr
                                                                       && b.EducForm == educForm).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                        else
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Course == course
                                                                       && b.EducForm == educForm).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Semestr == semestr
                                                                       && b.Course == course
                                                                       && b.EducForm == educForm).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (educForm == "<всі>")
                    {
                        if (course == 0)
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Semestr == semestr
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                        else
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Course == course
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Semestr == semestr
                                                                       && b.Course == course
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (course == 0)
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.EducForm == educForm
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Semestr == semestr
                                                                       && b.EducForm == educForm
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                        else
                        {
                            if (semestr == 0)
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Course == course
                                                                       && b.EducForm == educForm
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                            else
                            {
                                using (var ctx = new CaratDbContext(m_dbPath))
                                {
                                    var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                                       && b.Semestr == semestr
                                                                       && b.Course == course
                                                                       && b.EducForm == educForm
                                                                       && b.EducLevel == educlevel).ToList();
                                    return result.OrderBy(orderFunc).ToList();
                                }
                            }
                        }
                    }
                }
            }
        }

        public List<CurriculumItem> GetAllCurriculumItems<OrderType>(Func<CurriculumItem, OrderType> orderFunc, string educType, string educForm, uint course, uint semestr, string educlevel)
        {
            if (course == 0)
            {
                if (semestr == 0)
                {
                    using (var ctx = new CaratDbContext(m_dbPath))
                    {
                        var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                            && b.EducLevel == educlevel
                                                            && b.EducForm == educForm).ToList();
                        return result.OrderBy(orderFunc).ToList();
                    }
                }
                else
                {
                    using (var ctx = new CaratDbContext(m_dbPath))
                    {
                        var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                            && b.EducLevel == educlevel
                                                            && b.Semestr == semestr
                                                            && b.EducForm == educForm).ToList();
                        return result.OrderBy(orderFunc).ToList();
                    }
                }
            }
            else
            {
                if (semestr == 0)
                {
                    using (var ctx = new CaratDbContext(m_dbPath))
                    {
                        var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                           && b.EducLevel == educlevel
                                                           && b.EducForm == educForm
                                                           && b.Course == course).ToList();
                        return result.OrderBy(orderFunc).ToList();
                    }
                }
                else
                {
                    using (var ctx = new CaratDbContext(m_dbPath))
                    {
                        var result = ctx.CurriculumItems.Where(b => b.EducType == educType
                                                           && b.EducLevel == educlevel
                                                           && b.Semestr == semestr
                                                           && b.EducForm == educForm
                                                           && b.Semestr == semestr
                                                           && b.Course == course).ToList();
                        return result.OrderBy(orderFunc).ToList();
                    }
                }
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

        public void DeleteCurriculumItems(List<CurriculumItem> items)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(items);
                ctx.SaveChanges();
            }
        }

        public void RemoveCurriculumItemsBySubjectId(int subjectId)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.RemoveRange(ctx.CurriculumItems.Where(item => item.SubjectId == subjectId));
                ctx.SaveChanges();
            }
        }
    }
}
