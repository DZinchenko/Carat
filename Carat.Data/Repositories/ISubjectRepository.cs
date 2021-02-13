using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface ISubjectRepository
    {
        List<Subject> GetAllSubjects();
        Subject GetSubject(int subjectId);
        void AddSubject(Subject subject);
        void RemoveSubject(Subject subject);
        void UpdateSubject(Subject subject);
        void DeleteAllSubjects();
    }
}
