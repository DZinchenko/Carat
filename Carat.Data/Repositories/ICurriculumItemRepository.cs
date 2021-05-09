using System.Collections.Generic;

using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface ICurriculumItemRepository
    {
        List<CurriculumItem> GetAllCurriculumItems();
        CurriculumItem GetCurriculumItem(int curriculumItemId);
        CurriculumItem GetCurriculumItem(int subjectId, string educType, string educForm, uint course, uint semestr, string educlevel);
        List<CurriculumItem> GetAllCurriculumItems(string educType, string educForm, uint semestr, string educlevel);
        List<CurriculumItem> GetAllCurriculumItems(string educType, string educForm, uint course, uint semestr, string educlevel);
        List<CurriculumItem> GetCurriculumItems(string educType, string educForm);
        List<CurriculumItem> GetCurriculumItems(string educType, string educForm, uint semestr);
        void AddCurriculumItem(CurriculumItem curriculumItem);
        void RemoveCurriculumItem(CurriculumItem curriculumItem);
        void UpdateCurriculumItem(CurriculumItem curriculumItem);
        void DeleteAllCurriculumItems();
        void DeleteCurriculumItems(List<CurriculumItem> items);
    }
}
