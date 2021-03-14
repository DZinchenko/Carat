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

        public List<CurriculumItem> GetAllCurriculumItems(string educType, string educForm, uint semestr, string educlevel)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.CurriculumItems.Where(b => b.EducType == educType
                                                   && b.EducForm == educForm
                                                   && b.Semestr == semestr
                                                   && b.EducLevel == educlevel).ToList();
            }
        }

        public List<CurriculumItem> GetAllCurriculumItems(string educType, string educForm, uint course, uint semestr, string educlevel)
        {
            if (course == 0)
                return GetAllCurriculumItems(educType, educForm, semestr, educlevel);

            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.CurriculumItems.Where(b => b.EducType == educType
                                                   && b.EducForm == educForm
                                                   && b.Course == course
                                                   && b.Semestr == semestr
                                                   && b.EducLevel == educlevel).ToList();
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
