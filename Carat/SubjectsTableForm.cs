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
        private ISubjectRepository m_subjectRepository;
        private MainForm m_parentForm = null;
        private const string IncorrectNameMessage = "Некоректна назва предмета!";

        public SubjectsTableForm(MainForm parentForm, string dbName)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_subjectRepository = new SubjectRepository(dbName);
        }

        public void LoadData()
        {
            var subjects = m_subjectRepository.GetAllSubjects();

            foreach (var subject in subjects)
            {
                dataGridViewSubjects.Rows.Add(subject.Name, subject.Notes);
            }
        }

        private void SyncData()
        {
            var subjects = m_subjectRepository.GetAllSubjects();

            for (int i = 0; i < subjects.Count; ++i)
            {
                dataGridViewSubjects.Rows[i].SetValues(subjects[i].Name, subjects[i].Notes);
            }
        }

        private void RemoveLastRow()
        {
            int index = dataGridViewSubjects.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            dataGridViewSubjects.Rows.Remove(dataGridViewSubjects.Rows[index]);
        }

        private void SubjectsTableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.subjectsForm = null;
            m_parentForm.SetButtonState();
        }

        private void dataGridViewSubjects_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_subjectRepository == null)
            {
                return;
            }

            var subjects = m_subjectRepository.GetAllSubjects();
            var subject = new Subject();

            if (e.RowIndex < 0)
            {
                return;
            }

            subject.Name = dataGridViewSubjects[0, e.RowIndex].Value?.ToString()?.Trim(); ;
            subject.Notes = dataGridViewSubjects[1, e.RowIndex].Value?.ToString();

            if (e.RowIndex < subjects.Count)
            {
                subject.Id = subjects[e.RowIndex].Id;

                if (!isValidName(subject.Name))
                {
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SyncData();
                    return;
                }

                m_subjectRepository.UpdateSubject(subject);
            }
            else 
            {
                if (!isValidName(subject.Name))
                {
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RemoveLastRow();
                    return;
                }

                m_subjectRepository.AddSubject(subject);
            }
        }

        private void dataGridViewSubjects_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var subjects = m_subjectRepository.GetAllSubjects();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= subjects.Count)
            {
                return;
            }

            for (int i = 0; i < e.RowCount; ++i)
            {
                m_subjectRepository.RemoveSubject(subjects[i+e.RowIndex]);
            }
        }

        private bool isValidName(string name)
        {
            int duplicatesCounter = 0;

            if (name == null || name == "")
            {
                return false;
            }

            for (int i = 0; i < dataGridViewSubjects.Rows.Count; ++i)
            {
                if (dataGridViewSubjects[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter > 1 ? false : true;
        }
    }
}
