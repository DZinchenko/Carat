using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using Carat.EF.Repositories;
using Carat.Interfaces;

namespace Carat
{
    public partial class TeachersTableForm : Form, IDataUser
    {
        MainForm m_parentForm = null;
        ITeacherRepository m_teacherRepository = null;
        const string WarningMessage = "Некоректне ім'я викладача!";

        public TeachersTableForm(MainForm parentForm, string dbPath)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_teacherRepository = new TeacherRepository(dbPath);
        }

        private void TeachersTableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.teachersForm = null;
            m_parentForm.SetButtonState();
        }

        public void LoadData()
        {
            var teachers = m_teacherRepository.GetAllTeachers();

            foreach (var teacher in teachers)
            {
                dataGridViewTeachers.Rows.Add(
                    teacher.Name, 
                    teacher.StaffUnit, 
                    teacher.Position, 
                    teacher.Rank, 
                    teacher.Degree, 
                    teacher.Note);
            }
        }

        private void SyncData()
        {
            var teachers = m_teacherRepository.GetAllTeachers();

            for (int i = 0; i < teachers.Count; ++i)
            {
                dataGridViewTeachers.Rows[i].SetValues(
                    teachers[i].Name,
                    teachers[i].StaffUnit,
                    teachers[i].Position,
                    teachers[i].Rank,
                    teachers[i].Degree,
                    teachers[i].Note);
            }
        }

        private void RemoveLastRow()
        {
            int index = dataGridViewTeachers.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            dataGridViewTeachers.Rows.Remove(dataGridViewTeachers.Rows[index]);
        }

        private bool isValidName(string name)
        {
            int duplicatesCounter = 0;

            if (name == null || name == "")
            {
                return false;
            }

            for (int i = 0; i < dataGridViewTeachers.Rows.Count; ++i)
            {
                if (dataGridViewTeachers[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter > 1 ? false : true;
        }

        private void dataGridViewTeachers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_teacherRepository == null)
            {
                return;
            }

            var teachers = m_teacherRepository.GetAllTeachers();
            var teacher = new Teacher();

            if (e.RowIndex < 0)
            {
                return;
            }

            teacher.Name = dataGridViewTeachers[0, e.RowIndex].Value?.ToString();
            teacher.StaffUnit = Convert.ToDouble(dataGridViewTeachers[1, e.RowIndex].Value?.ToString());
            teacher.Position = dataGridViewTeachers[2, e.RowIndex].Value?.ToString();
            teacher.Rank = dataGridViewTeachers[3, e.RowIndex].Value?.ToString();
            teacher.Degree = dataGridViewTeachers[4, e.RowIndex].Value?.ToString();
            teacher.Note = dataGridViewTeachers[5, e.RowIndex].Value?.ToString();

            if (e.RowIndex < teachers.Count)
            {
                teacher.Id = teachers[e.RowIndex].Id;

                if (!isValidName(teacher.Name))
                {
                    MessageBox.Show(WarningMessage);
                    SyncData();
                    return;
                }

                m_teacherRepository.UpdateTeacher(teacher);
            }
            else
            {
                if (!isValidName(teacher.Name))
                {
                    MessageBox.Show(WarningMessage);
                    RemoveLastRow();
                    return;
                }

                m_teacherRepository.AddTeacher(teacher);
            }
        }

        private void dataGridViewTeachers_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var teachers = m_teacherRepository.GetAllTeachers();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= teachers.Count)
            {
                return;
            }

            for (int i = 0; i < e.RowCount; ++i)
            {
                m_teacherRepository.RemoveTeacher(teachers[i + e.RowIndex]);
            }
        }
    }
}
