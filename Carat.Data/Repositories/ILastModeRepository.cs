using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface ILastModeRepository
    {
        LastMode GetLastMode();
        void UpdateLastMode(LastMode lastMode);
    }
}
