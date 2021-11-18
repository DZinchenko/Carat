using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using System.Linq;

namespace Carat.EF.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private string m_dbPath = "";

        public FacultyRepository(string dbPath)
        {
            m_dbPath = dbPath;
        }

        public void AddFaculty(Faculty faculty)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Add(faculty);
                ctx.SaveChanges();
            }
        }

        public List<Faculty> GetFaculties(List<Group> groups)
        {
            var distinctIds = groups.Select(g => g.FacultyId).Distinct();
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Faculties.Where(f => distinctIds.Contains(f.Id)).ToList();
            }
        }

        public List<Faculty> GetFaculties()
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Faculties.ToList();
            }
        }

        public Faculty GetFaculty(string name)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                var res = ctx.Faculties.FirstOrDefault(f => f.Name == name);
                if (res == null)
                {
                    AddFaculty(new Faculty() { Name = name });
                    res = ctx.Faculties.First(f => f.Name == name);
                }
                return res;
            }
        }

        public Faculty GetFaculty(int id)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                return ctx.Faculties.First(f => f.Id == id);
            }
        }

        public void RemoveFaculty(Faculty faculty)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Remove(faculty);
                ctx.SaveChanges();
            }
        }

        public void UpdateFaculty(Faculty faculty)
        {
            using (var ctx = new CaratDbContext(m_dbPath))
            {
                ctx.Update(faculty);
                ctx.SaveChanges();
            }
        }
    }
}
