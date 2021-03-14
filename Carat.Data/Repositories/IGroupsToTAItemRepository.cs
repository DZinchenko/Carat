using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IGroupsToTAItemRepository
    {
        List<GroupsToTAItem> GetAllGroupsToTeachers();
        GroupsToTAItem GetGroupsToTeacher(int groupsToTeacherId);
        void AddGroupsToTeacher(GroupsToTAItem groupsToTeacher);
        void RemoveGroupsToTeacher(GroupsToTAItem groupsToTeacher);
        void UpdateGroupsToTeacher(GroupsToTAItem groupsToTeacher);
        void DeleteAllGroupsToTeachers();
    }
}
