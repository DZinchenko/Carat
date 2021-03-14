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
    public partial class TeachersTableForm : Form, IDataUserForm
    {
        MainForm m_parentForm = null;
        ITeacherRepository m_teacherRepository = null;
        const string IncorrectNameMessage = "Некоректне ім'я викладача!";
        const string IncorrectDataMessage = "Некоректні дані!";
        const string NotSet = "<not set>";

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
                    teacher.OccupForm,
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
                    teachers[i].OccupForm,
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

            if (e.RowIndex < 0)
            {
                return;
            }

            var teachers = m_teacherRepository.GetAllTeachers();

            if (e.RowIndex < teachers.Count)
            {
                var teacher = teachers[e.RowIndex];

                if (!UpdateData(teacher, e, false))
                {
                    return;
                }

                m_teacherRepository.UpdateTeacher(teacher);
            }
            else
            {
                var teacher = new Teacher();

                if (!UpdateData(teacher, e, true))
                {
                    RemoveLastRow();
                    return;
                }

                m_teacherRepository.AddTeacher(teacher);
            }
        }

        private bool UpdateData(Teacher teacher, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var Name = dataGridViewTeachers[0, e.RowIndex].Value?.ToString()?.Trim();
                if (!isValidName(Name))
                {
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!isNewObject)
                    {
                        SyncData();
                    }

                    return false;
                }

                switch (e.ColumnIndex)
                {
                    case 0:
                        {
                            teacher.Name = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 1:
                        {
                            teacher.StaffUnit = Convert.ToUInt32(dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString());
                            break;
                        }
                    case 2:
                        {
                            teacher.Position = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 3:
                        {
                            teacher.Rank = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 4:
                        {
                            teacher.Degree = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 5:
                        {
                            teacher.OccupForm = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 6:
                        {
                            teacher.Note = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    default: { return false; }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                SyncData();
                return false;
            }

            return true;
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

        private void dataGridViewTeachers_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

    }
}
