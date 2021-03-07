using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface ITAItemRepository
    {
        List<TAItem> GetAllTAItems();
        List<TAItem> GetTAItems(int workId);
        TAItem GetTAItem(int TAItemId);
        void AddTAItem(TAItem TAItem);
        void RemoveTAItem(TAItem TAItem);
        void UpdateTAItem(TAItem TAItem);
        void DeleteAllTAItems();
    }
}
