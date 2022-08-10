using System;
using System.Collections.Generic;
using Carat.Data.Entities;


namespace Carat.Data.Repositories
{
    public interface ISubjectRepository
    {
        List<Subject> GetAllSubjects();
        Dictionary<int, Subject> GetSubjects(List<int> subjectIds);
        Subject GetSubject(int subjectId);
        Subject GetSubject(string name);
        void AddSubject(Subject subject);
        void AddSubjects(List<Subject> subjects);
        void RemoveSubject(Subject subject);
        void UpdateSubject(Subject subject);
        void DeleteAllSubjects();
        bool CheckIfHasHours(Subject subject);
    }
}
