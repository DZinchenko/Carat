using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IGroupRepository
    {
        List<Group> GetAllGroups();
        Group GetGroup(int groupId);
        void AddGroup(Group group);
        void RemoveGroup(Group group);
        void UpdateGroup(Group group);
        void DeleteAllGroups();
    }
}
