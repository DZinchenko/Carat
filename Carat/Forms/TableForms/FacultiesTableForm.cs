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
    public partial class FacultiesTableForm : Form, IDataUserForm
    {
        private IFacultyRepository m_facultyRepository;
        private MainForm m_parentForm = null;
        private const string IncorrectNameMessage = "Некоректна назва факультету!";
        private const string IncorrectDataMessage = "Некоректні дані!";
        private bool isLoading = false;

        public FacultiesTableForm(MainForm parentForm, string dbName)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_facultyRepository = new FacultyRepository(dbName);
        }

        //feturn a list of first n fibonacci numbers


        public void LoadData()
        {
            isLoading = true;

            var faculties = m_facultyRepository.GetFaculties();

            dataGridViewFaculties.Rows.Clear();

            foreach (var faculty in faculties)
            {
                dataGridViewFaculties.Rows.Add(faculty.Name);
            }

            isLoading = false;
        }

        private void FacultiesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.facultiesForm = null;
            m_parentForm.SetButtonState();
        }

        private void RemoveLastRow()
        {
            int index = dataGridViewFaculties.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            dataGridViewFaculties.Rows.Remove(dataGridViewFaculties.Rows[index]);
        }

        private void dataGridViewFaculties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_facultyRepository == null)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            var faculties = m_facultyRepository.GetFaculties();

            if (e.RowIndex < faculties.Count)
            {
                var faculty = faculties[e.RowIndex];

                if (!UpdateData(faculty, e, false))
                {
                    return;
                }

                m_facultyRepository.UpdateFaculty(faculty);
            }
            else
            {
                var faculty = new Faculty();

                if (!UpdateData(faculty, e, true))
                {
                    RemoveLastRow();
                    return;
                }
                m_facultyRepository.AddFaculty(faculty);
            }
        }

        private bool isValidName(string name)
        {
            if (name == null || name == "")
            {
                return false;
            }

            var duplicatesCnt = 0;

            for (int i = 0; i < dataGridViewFaculties.Rows.Count; ++i)
            {
                if (dataGridViewFaculties[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    duplicatesCnt++;
                    if (duplicatesCnt > 1) { return false; }
                }
            }

            return true;
        }

        private bool UpdateData(Faculty faculty, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var Name = dataGridViewFaculties[0, e.RowIndex].Value?.ToString()?.Trim();

                if (!isValidName(Name))
                {
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!isNewObject)
                    {
                        LoadData();
                    }

                    return false;
                }

                faculty.Name = dataGridViewFaculties[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
            }
            catch (Exception)
            {
                MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
                return false;
            }

            return true;
        }

        private void dataGridViewFaculties_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (isLoading) { return; }
            var faculties = m_facultyRepository.GetFaculties();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= faculties.Count)
            {
                return;
            }
            
            for (int i = 0; i < e.RowCount; ++i)
            {
                m_facultyRepository.RemoveFaculty(faculties[i + e.RowIndex]);
            }
        }

        private void dataGridViewFacultues_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
