﻿using System.Collections.Generic;
using Carat.Data.Entities;

namespace Carat.Data.Repositories
{
    public interface ISubjectRepository
    {
        List<Subject> GetAllSubjects();
        Subject GetSubject(int subjectId);
        Subject GetSubject(string name);
        void AddSubject(Subject subject);
        void RemoveSubject(Subject subject);
        void UpdateSubject(Subject subject);
        void DeleteAllSubjects();
    }
}
