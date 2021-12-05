using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IRankRepository
    {
        Rank GetRank(int id);
        Rank GetRank(string name);
        List<Rank> GetAllRanks();
        void AddRank(Rank degree);
        void UpdateRank(Rank degree);
        void RemoveRank(Rank degree);
    }
}
