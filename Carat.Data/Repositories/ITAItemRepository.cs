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
        TAItem GetTAItem(int TAItemId);
        void AddTAItem(TAItem TAItem);
        void RemoveTAItem(TAItem TAItem);
        void UpdateTAItem(TAItem TAItem);
        void DeleteAllTAItems();
    }
}
