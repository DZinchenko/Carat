using System;
using System.Collections.Generic;
using System.Text;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface IPositionRepository
    {
        Position GetPosition(int id);
        Position GetPosition(string name);
        List<Position> GetPositions();
        void AddPosition(Position position);
        void RemovePosition(Position position);
        void UpdatePosition(Position position);
    }
}
