using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IGroupRepository
    {
        List<Group> GetAllGroups();
        Group GetGroup(int groupId);
        List<Group> GetGroups(uint course, string educForm, string educLevel);
        void AddGroup(Group group);
        void AddGroups(List<Group> groups);
        void RemoveGroup(Group group);
        void UpdateGroup(Group group);
        void DeleteAllGroups();
    }
}
