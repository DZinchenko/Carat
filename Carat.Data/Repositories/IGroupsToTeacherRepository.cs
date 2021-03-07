using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IGroupsToTeacherRepository
    {
        List<GroupsToTeacher> GetAllGroupsToTeachers();
        GroupsToTeacher GetGroupsToTeacher(int groupsToTeacherId);
        void AddGroupsToTeacher(GroupsToTeacher groupsToTeacher);
        void RemoveGroupsToTeacher(GroupsToTeacher groupsToTeacher);
        void UpdateGroupsToTeacher(GroupsToTeacher groupsToTeacher);
        void DeleteAllGroupsToTeachers();
    }
}
