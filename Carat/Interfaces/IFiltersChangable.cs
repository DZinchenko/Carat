using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carat.Interfaces
{
    interface IFiltersChangable
    {
        void SetFilters(string educType, string educForm, string educLevel, uint course, uint semestr);
    }
}
