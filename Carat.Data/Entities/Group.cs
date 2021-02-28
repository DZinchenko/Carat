using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EducForm { get; set; }
        public uint Course { get; set; }
        public uint BudgetNumber { get; set; }
        public uint ContractNumber { get; set; }
        public string Faculty { get; set; }
        public string Note { get; set; }
    }
}
