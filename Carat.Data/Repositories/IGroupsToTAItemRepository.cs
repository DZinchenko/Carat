using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IGroupsToTAItemRepository
    {
        List<GroupsToTAItem> GetAllGroupsToTAItem();
        List<GroupsToTAItem> GetGroupsToTAItem(int TAItemId);
        GroupsToTAItem GetGroupsToTAItem(int TAItemId, int groupId);
        void AddGroupsToTAItem(GroupsToTAItem groupsToTAItem);
        void RemoveGroupsToTAItem(GroupsToTAItem groupsToTAItem);
        void UpdateGroupsToTAItem(GroupsToTAItem groupsToTAItem);
        void DeleteAllGroupsToTAItem();
    }
}
