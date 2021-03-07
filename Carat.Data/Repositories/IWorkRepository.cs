using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IWorkRepository
    {
        List<Work> GetAllWorks();
        List<Work> GetWorks(int curriculumItemId);
        Work GetWork(int workId);
        void AddWork(Work work);
        void RemoveWork(Work work);
        void UpdateWork(Work work);
        void DeleteAllWorks();
    }
}
