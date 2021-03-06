using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IWorkTypeRepository
    {
        List<WorkType> GetAllWorkTypes();
        WorkType GetWorkType(int workTypeId);
        void AddWorkType(WorkType workType);
        void RemoveWorkType(WorkType workType);
        void UpdateWorkType(WorkType workType);
        void DeleteAllWorkTypes();
    }
}
