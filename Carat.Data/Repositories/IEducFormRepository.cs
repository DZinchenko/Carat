using Carat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Repositories
{
    public interface IEducFormRepository
    {
        EducForm GetEducForm(int id);
        EducForm GetEducForm(string name);
        List<EducForm> GetAllEducForms();
    }
}
