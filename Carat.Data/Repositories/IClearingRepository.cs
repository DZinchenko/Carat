using System;
using System.Collections.Generic;
using System.Text;

namespace Carat.Data.Repositories
{
    public interface IClearingRepository
    {
        void ClearLoad();

        void ClearCurriculum();

        void ClearTeachers();

        void ClearSubjects();

        void ClearGroups();
    }
}
