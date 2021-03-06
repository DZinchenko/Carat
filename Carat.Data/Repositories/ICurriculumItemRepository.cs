using System.Collections.Generic;

using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface ICurriculumItemRepository
    {
        List<CurriculumItem> GetAllCurriculumItems();
        CurriculumItem GetCurriculumItem(int curriculumItemId);
        void AddCurriculumItem(CurriculumItem curriculumItem);
        void RemoveCurriculumItem(CurriculumItem curriculumItem);
        void UpdateCurriculumItem(CurriculumItem curriculumItem);
        void DeleteAllCurriculumItems();
    }
}
