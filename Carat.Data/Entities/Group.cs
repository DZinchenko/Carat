using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EducForm { get; set; } = "Денна";
        public uint Course { get; set; } = 1;
        public uint BudgetNumber { get; set; } = 0;
        public uint ContractNumber { get; set; } = 0;
        public string Faculty { get; set; } = "ТЕФ";
        public string Note { get; set; }
    }
}
