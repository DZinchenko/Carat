using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface ITAItemRepository
    {
        List<TAItem> GetAllTAItems();
        List<TAItem> GetTAItems(int workId);
        List<TAItem> GetAllTAItems(int workId, string educType, string educForm, uint semestr, string educlevel);
        List<TAItem> GetAllTAItems(int workId, string educType, string educForm, uint course, uint semestr, string educlevel);
        double GetAssignedTeacherHours(int teacherId);
        List<TAItem> GetTAItemsByTeacherId(int teacherId, uint semestr, string educType, string educForm);
        List<TAItem> GetTAItemsByTeacherIdWithouFilters(int teacherId, uint semestr);
        TAItem GetTAItem(int TAItemId);
        void AddTAItem(TAItem TAItem);
        void RemoveTAItem(TAItem TAItem);
        void UpdateTAItem(TAItem TAItem);
        void DeleteAllTAItems();
    }
}
