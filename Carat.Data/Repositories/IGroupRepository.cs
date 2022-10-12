using System;
using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IGroupRepository
    {
        List<Group> GetAllGroups<OrderType>(Func<Group, OrderType> orderFunc);
        Group GetGroup(int groupId);
        List<Group> GetGroups(List<int> groupIds);
        List<Group> GetGroups<OrderType>(Func<Group, OrderType> orderFunc, uint course, string educForm, string educLevel);
        List<Group> GetGroupsForReports<OrderType>(Func<Group, OrderType> orderFunc, uint course, string educForm, string educLevel);
        void AddGroup(Group group);
        void AddGroups(List<Group> groups);
        void RemoveGroup(Group group);
        void UpdateGroup(Group group);
        void DeleteAllGroups();
    }
    public static class GroupRepositoryExtensions
    {
        public static List<Group> GetGroups(this IGroupRepository repository, uint course, string educForm, string educLevel)
        {
            return repository.GetGroups(x => true, course, educForm, educLevel);
        }
    }
}
