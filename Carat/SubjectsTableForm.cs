using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Carat.Data.Repositories;
using Carat.Interfaces;
using Carat.Data.Entities;
using Carat.EF.Repositories;

namespace Carat
{
    public partial class SubjectsTableForm : Form, IDataUser
    {
        private ISubjectRepository subjectRepository = new SubjectRepository();

        public SubjectsTableForm()
        {
            InitializeComponent();
        }

        public void SaveData()
        {
            List<Subject> changedSubjects = new List<Subject>();

            for (int i = 0; i < dataGridViewSubjects.RowCount - 1; ++i)
            { 
                var subject = new Subject();

                subject.Name = dataGridViewSubjects[0, i].Value.ToString();
                subject.Notes = dataGridViewSubjects[1, i].Value != null ? dataGridViewSubjects[1, i].Value.ToString() : "";

                changedSubjects.Add(subject);
            }

            dataGridViewSubjects.Rows.Clear();

            subjectRepository.UpdateSubjects(changedSubjects);
        }

        public void LoadData()
        {
            var subjects = subjectRepository.GetAllSubjects();

            foreach (var subject in subjects)
            {
                dataGridViewSubjects.Rows.Add(subject.Name, subject.Notes);
            }
        }
    }
}
