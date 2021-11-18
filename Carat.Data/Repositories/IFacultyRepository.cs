using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IFacultyRepository
    {
        Faculty GetFaculty(string name);
        Faculty GetFaculty(int id);
        List<Faculty> GetFaculties();
        List<Faculty> GetFaculties(List<Group> groups);
        void AddFaculty(Faculty faculty);
        void UpdateFaculty(Faculty faculty);
        void RemoveFaculty(Faculty faculty);
    }
}
