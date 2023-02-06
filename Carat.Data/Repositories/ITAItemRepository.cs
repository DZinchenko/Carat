using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface ITAItemRepository
    {
        List<TAItem> GetAllTAItems();
        List<TAItem> GetTAItems(int workId);
        Dictionary<int, List<TAItem>> GetTAItems(List<Work> works);
        List<TAItem> GetAllTAItems(int workId, string educType, string educForm, uint semestr, string educlevel);
        List<TAItem> GetAllTAItems(int workId, string educType, string educForm, uint course, uint semestr, string educlevel);
        double GetAssignedTeacherHours(int teacherId);
        double GetAssignedTeacherHours(int teacherId, string educType);
        List<TAItem> GetTAItemsByTeacherId(int teacherId, uint semestr, string educType, string educForm);
        List<TAItem> GetTAItemsByTeacherIdWithouFilters(int teacherId, uint semestr);
        bool IsTeacherAssignedToWork(int teacherId, int workId);
        TAItem GetTAItem(int TAItemId);
        void AddTAItem(TAItem TAItem);
        void AddTAItems(List<TAItem> TAItems);
        void RemoveTAItem(TAItem TAItem);
        void UpdateTAItem(TAItem TAItem);
        void DeleteAllTAItems();
        bool ExistTAItemsForWorks(List<Work> works);
        void DeleteAllTAItemsForWorks(List<int> workIds, string educType, string educForm, uint course, uint semestr, string educlevel);
    }
}
