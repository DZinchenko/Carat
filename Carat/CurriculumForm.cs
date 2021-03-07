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
    public partial class CurriculumForm : Form, IDataUser
    {
        private ICurriculumItemRepository m_curriculumItemRepository;
        private ISubjectRepository m_subjectRepository;
        private IWorkTypeRepository m_workTypeRepository;
        private MainForm m_parentForm = null;
        private const string IncorrectDataMessage = "Некоректні дані!";

        public CurriculumForm(MainForm parentForm, string dbName)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_curriculumItemRepository = new CurriculumItemRepository(dbName);
            m_workTypeRepository = new WorkTypeRepository(dbName);
            m_subjectRepository = new SubjectRepository(dbName);
        }

        public void LoadData()
        {
            var workTypes = m_workTypeRepository.GetAllWorkTypes();
            var subjects = m_subjectRepository.GetAllSubjects();
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems();

            foreach (var work in workTypes)
            {
                listBoxWorkTypes.Items.Add(work.Name);
            }

            foreach (var subject in subjects)
            {
                listBoxSubjects.Items.Add(subject.Name);
            }

            foreach (var curriculumItem in curriculumItems)
            {
                dataGridViewCurriculumSubjects.Rows.Add(m_subjectRepository.GetSubject(
                    curriculumItem.SubjectId)?.Name, curriculumItem?.SubjectHours);
            }
        }

        private void CurriculumForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.curriculumForm = null;
            m_parentForm.SetButtonState();
        }

        private void listBoxSubjects_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxSubjects.IndexFromPoint(e.Location);

            if (index != ListBox.NoMatches)
            {
                var dgvIndex = dataGridViewCurriculumSubjects.Rows.Count - 1;
                var subjects = m_subjectRepository.GetAllSubjects();

                dataGridViewCurriculumSubjects.Rows.Add();
                dataGridViewCurriculumSubjects.Rows[dgvIndex].SetValues(subjects[index].Name, 0);
            }
        }

        private void SyncDataCurriculumSubjects()
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems();

            for (int i = 0; i < curriculumItems.Count; ++i)
            {
                dataGridViewCurriculumSubjects.Rows[i].SetValues(m_subjectRepository.GetSubject(
                    curriculumItems[i].SubjectId)?.Name, curriculumItems[i]?.SubjectHours);
            }
        }

        private void RemoveLastRowCurriculumSubjects()
        {
            int index = dataGridViewCurriculumSubjects.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            dataGridViewCurriculumSubjects.Rows.Remove(dataGridViewCurriculumSubjects.Rows[index]);
        }

        private void RemoveLastRowCurriculumWorkTypes()
        {
            int index = dataGridViewCurriculumWorkTypes.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            dataGridViewCurriculumWorkTypes.Rows.Remove(dataGridViewCurriculumWorkTypes.Rows[index]);
        }

        private void dataGridViewCurriculumSubjects_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_curriculumItemRepository == null)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems();

            if (e.RowIndex < curriculumItems.Count)
            {
                var curriculumItem = curriculumItems[e.RowIndex];

                if (!UpdateDataCurriculumSubject(curriculumItem, e, false))
                {
                    return;
                }

                m_curriculumItemRepository.UpdateCurriculumItem(curriculumItem);
            }
            else
            {
                var curriculumItem = new CurriculumItem();

                if (!UpdateDataCurriculumSubject(curriculumItem, e, true))
                {
                    RemoveLastRowCurriculumSubjects();
                    return;
                }

                m_curriculumItemRepository.AddCurriculumItem(curriculumItem);
            }
        }

        private bool isValidName(string name, DataGridView dgv)
        {
            int duplicatesCounter = 0;

            if (name == null || name == "")
            {
                return false;
            }

            for (int i = 0; i < dgv.Rows.Count; ++i)
            {
                if (dgv[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter > 1 ? false : true;
        }

        private bool UpdateDataCurriculumSubject(CurriculumItem curriculumItem, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var name = dataGridViewCurriculumSubjects[0, e.RowIndex].Value?.ToString()?.Trim();

                if (!isValidName(name, dataGridViewCurriculumSubjects))
                {
                    MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (!isNewObject)
                    {
                        SyncDataCurriculumSubjects();
                    }

                    return false;
                }

                switch (e.ColumnIndex)
                {
                    case 0:
                        {
                            var subject = m_subjectRepository.GetSubject(dataGridViewCurriculumSubjects[e.ColumnIndex, e.RowIndex]?.Value?.ToString()?.Trim());

                            if (subject == null)
                                throw new Exception();

                            curriculumItem.SubjectId = subject.Id;
                            break;
                        }
                    case 1:
                        {
                            curriculumItem.SubjectHours = Convert.ToDouble(dataGridViewCurriculumSubjects[e.ColumnIndex, e.RowIndex].Value?.ToString());

                            if (Tools.isLessThanZero(curriculumItem.SubjectHours))
                            {
                                throw new Exception();
                            }

                            break;
                        }
                    default: { return false; }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(IncorrectDataMessage);
                SyncDataCurriculumSubjects();
                return false;
            }

            return true;
        }

        private void dataGridViewCurriculumSubjects_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= curriculumItems.Count)
            {
                return;
            }

            for (int i = 0; i < e.RowCount; ++i)
            {
                m_curriculumItemRepository.RemoveCurriculumItem(curriculumItems[i + e.RowIndex]);
            }
        }

        private void dataGridViewCurriculumSubjects_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
