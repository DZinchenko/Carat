using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IEducLevelRepository
    {
        EducLevel GetEducLevel(int id);
        List<EducLevel> GetAllEducLevels();
    }
}
