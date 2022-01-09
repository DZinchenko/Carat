using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IWorkRepository
    {
        List<Work> GetAllWorks();
        List<Work> GetWorks(int curriculumItemId, bool withoutZeroHours);
        Dictionary<int, List<Work>> GetWorksForCurriculumItemIds(List<int> curriculumItemIds, bool withoutZeroHours);
        Work GetWork(int workId);
        void AddWork(Work work);
        void AddWorks(List<Work> works);
        void RemoveWork(Work work);
        void UpdateWork(Work work);
        void UpdateWorks(List<Work> works);
        void DeleteAllWorks();
        void DeleteWorks(List<Work> works);
    }
}
