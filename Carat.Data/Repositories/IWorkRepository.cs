using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IWorkRepository
    {
        List<Work> GetAllWorks();
        List<Work> GetWorks(int curriculumItemId, bool withoutZeroHours);
        Work GetWork(int workId);
        void AddWork(Work work);
        void AddWorks(List<Work> works);
        void RemoveWork(Work work);
        void UpdateWork(Work work);
        void DeleteAllWorks();
        void DeleteWorks(List<Work> works);
    }
}
