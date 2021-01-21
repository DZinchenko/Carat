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
        void AddSubjects(List<Subject> subjects);
        void UpdateSubjects(List<Subject> subjects);
        void DeleteAllSubjects();
    }
}
