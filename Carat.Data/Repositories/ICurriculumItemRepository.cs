using System;
using System.Collections.Generic;

using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface ICurriculumItemRepository
    {
        List<CurriculumItem> GetAllCurriculumItems();
        CurriculumItem GetCurriculumItem(int curriculumItemId);
        CurriculumItem GetCurriculumItem(int subjectId, string educType, string educForm, uint course, uint semestr, string educlevel);
        List<CurriculumItem> GetAllCurriculumItems<OrderType>(Func<CurriculumItem, OrderType> orderFunc, string educType, string educForm, uint course, uint semestr, string educlevel);
        List<CurriculumItem> GetCurriculumItems<OrderType>(Func<CurriculumItem, OrderType> orderFunc, string educType, string educForm);
        List<CurriculumItem> GetAllCurriculumItemsForReports<OrderType>(Func<CurriculumItem, OrderType> orderFunc, string educType, string educForm, uint course, uint semestr, string educlevel);
        void AddCurriculumItem(CurriculumItem curriculumItem);
        void RemoveCurriculumItem(CurriculumItem curriculumItem);
        void RemoveCurriculumItemsBySubjectId(int subjectId);
        void UpdateCurriculumItem(CurriculumItem curriculumItem);
        void DeleteAllCurriculumItems();
        void DeleteCurriculumItems(List<CurriculumItem> items);
    }
}
