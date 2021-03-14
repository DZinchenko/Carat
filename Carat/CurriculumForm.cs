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
    public partial class CurriculumForm : Form, IDataUserForm, IFilterUserForm
    {
        private ICurriculumItemRepository m_curriculumItemRepository;
        private ISubjectRepository m_subjectRepository;
        private IWorkTypeRepository m_workTypeRepository;
        private IWorkRepository m_workRepository;
        private MainForm m_parentForm = null;
        private const string IncorrectDataMessage = "Некоректні дані!";
        private bool isSelectionChanging = false;

        public CurriculumForm(MainForm parentForm, string dbName)
        {
            InitializeComponent();
            m_parentForm = parentForm;

            FiltersStatesChanged();

            listBoxWorkTypes.Enabled = false;
            listBoxCourse.Visible = false;
            listBoxCourse.SelectedIndex = 0;

            m_curriculumItemRepository = new CurriculumItemRepository(dbName);
            m_workTypeRepository = new WorkTypeRepository(dbName);
            m_subjectRepository = new SubjectRepository(dbName);
            m_workRepository = new WorkRepository(dbName);
        }

        private void EnableListBoxes()
        {
            listBoxCourse.Enabled = m_parentForm.IsRequiredFiltersValuesSelected;
            listBoxSubjects.Enabled = m_parentForm.IsRequiredFiltersValuesSelected;

            listBoxCourse.Visible = m_parentForm.IsRequiredFiltersValuesSelected && !(m_parentForm.IsFiltersValuesSelected);
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
                    curriculumItem.SubjectId)?.Name, curriculumItem?.Course, curriculumItem?.SubjectHours);
            }
        }

        public void FiltersStatesChanged()
        {
            EnableListBoxes();
        }

        private int getCurrentCurriculumId()
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems();
            var rowIndex = dataGridViewCurriculumSubjects.SelectedCells[0].RowIndex;

            return curriculumItems[rowIndex].Id;
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
                var dgvIndex = dataGridViewCurriculumSubjects.Rows.Count;
                var subjects = m_subjectRepository.GetAllSubjects();

                dataGridViewCurriculumSubjects.Rows.Add();
                dataGridViewCurriculumSubjects.Rows[dgvIndex].SetValues(subjects[index].Name, listBoxCourse.SelectedItem, 0);
            }
        }

        private void SyncDataCurriculumSubjects()
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems();

            for (int i = 0; i < curriculumItems.Count; ++i)
            {
                dataGridViewCurriculumSubjects.Rows[i].SetValues(m_subjectRepository.GetSubject(
                    curriculumItems[i].SubjectId)?.Name, curriculumItems[i]?.Course, curriculumItems[i]?.SubjectHours);
            }
        }

        private void RemoveLastRowCurriculumSubjects()
        {
            int index = dataGridViewCurriculumSubjects.Rows.Count - 1;

            if (index < 0)
            {
                return;
            }

            dataGridViewCurriculumSubjects.Rows.Remove(dataGridViewCurriculumSubjects.Rows[index]);
        }

        private void RemoveLastRowCurriculumWorks()
        {
            int index = dataGridViewCurriculumWorkTypes.Rows.Count - 1;

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

        private bool isValidSubject(string name, string course, DataGridView dgv)
        {
            int duplicatesCounter = 0;

            for (int i = 0; i < dgv.Rows.Count; ++i)
            {
                if ((dgv[0, i].Value?.ToString().ToLower() == name.ToLower()) 
                    && dgv[1, i].Value?.ToString().ToLower() == course)
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter >= 1 ? false : true;
        }

        private bool UpdateDataCurriculumSubject(CurriculumItem curriculumItem, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var name = dataGridViewCurriculumSubjects[0, e.RowIndex].Value?.ToString()?.Trim();

                if (name == null)
                {
                    MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SyncDataCurriculumSubjects();
                    return false;
                }

                if (isNewObject)
                {
                    if (!isValidSubject(name, listBoxCourse.SelectedItem.ToString(), dataGridViewCurriculumSubjects))
                    {
                        MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
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
                            curriculumItem.Course = Convert.ToUInt32(dataGridViewCurriculumSubjects[e.ColumnIndex, e.RowIndex].Value?.ToString());

                            break;
                        }
                    case 2:
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

        private void dataGridViewCurriculumSubjects_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCurriculumSubjects.SelectedCells.Count == 1)
            {
                var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems();

                if (curriculumItems.Count == 0)
                {
                    return;
                }

                var rowIndex = dataGridViewCurriculumSubjects.SelectedCells[0].RowIndex;

                if (rowIndex >= curriculumItems.Count)
                {
                    listBoxWorkTypes.Enabled = false;
                    dataGridViewCurriculumWorkTypes.Visible = false;
                    return;
                }

                var curriculumItemId = curriculumItems[rowIndex].Id;
                var curriculumItemWorks = m_workRepository.GetWorks(curriculumItemId);

                isSelectionChanging = true;
                for (int i = 0, limit = dataGridViewCurriculumWorkTypes.RowCount; i < limit; ++i)
                {
                    RemoveLastRowCurriculumWorks();
                }
                isSelectionChanging = false;

                foreach (var work in curriculumItemWorks)
                {
                    var workType = m_workTypeRepository.GetWorkType(work.WorkTypeId);
                    dataGridViewCurriculumWorkTypes.Rows.Add(workType?.Name, work?.TotalHours);
                }

                listBoxWorkTypes.Enabled = true;
            }
        }

        private void listBoxWorkTypes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxWorkTypes.IndexFromPoint(e.Location);

            if (index != ListBox.NoMatches)
            {
                var dgvIndex = dataGridViewCurriculumWorkTypes.Rows.Count;
                var workTypes = m_workTypeRepository.GetAllWorkTypes();

                dataGridViewCurriculumWorkTypes.Rows.Add();
                dataGridViewCurriculumWorkTypes.Rows[dgvIndex].SetValues(workTypes[index].Name, 0);
            }
        }

        private void dataGridViewCurriculumWorkTypes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_workRepository == null)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            var curriculumItemWorks = m_workRepository.GetWorks(getCurrentCurriculumId());

            if (e.RowIndex < curriculumItemWorks.Count)
            {
                var curriculumItemWork = curriculumItemWorks[e.RowIndex];

                if (!UpdateDataCurriculumWork(curriculumItemWork, getCurrentCurriculumId(), e, false))
                {
                    return;
                }

                m_workRepository.UpdateWork(curriculumItemWork);
            }
            else
            {
                var curriculumItemWork = new Work();

                if (!UpdateDataCurriculumWork(curriculumItemWork, getCurrentCurriculumId(), e, true))
                {
                    RemoveLastRowCurriculumWorks();
                    return;
                }

                m_workRepository.AddWork(curriculumItemWork);
            }
        }

        private bool isValidWork(string name, DataGridView dgv)
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

        private void SyncDataCurriculumWorks()
        {
            var works = m_workRepository.GetWorks(getCurrentCurriculumId());

            if (works.Count != dataGridViewCurriculumWorkTypes.Rows.Count)
            {
                throw new Exception("DB error");
            }

            for (int i = 0; i < works.Count; ++i)
            {
                dataGridViewCurriculumWorkTypes.Rows[i].Cells[1].Value = works[i]?.TotalHours;
            }
        }

        private bool UpdateDataCurriculumWork(Work work, int curriculumItemId, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var Name = dataGridViewCurriculumWorkTypes[0, e.RowIndex].Value?.ToString()?.Trim();

                if (!isValidWork(Name, dataGridViewCurriculumWorkTypes))
                {
                    MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    if (!isNewObject)
                    {
                        SyncDataCurriculumWorks();
                    }

                    return false;
                }

                switch (e.ColumnIndex)
                {
                    case 0:
                        {
                            var curriculumItem = m_curriculumItemRepository.GetCurriculumItem(curriculumItemId);
                            var name = dataGridViewCurriculumWorkTypes[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();

                            work.CurriculumItemId = curriculumItemId;
                            work.WorkTypeId = m_workTypeRepository.GetWorkTypeByName(name).Id;

                            break;
                        }
                    case 1:
                        {
                            work.TotalHours = Convert.ToDouble(dataGridViewCurriculumWorkTypes[e.ColumnIndex, e.RowIndex].Value?.ToString());

                            if (Tools.isLessThanZero(work.TotalHours))
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
                SyncDataCurriculumWorks();
                return false;
            }

            return true;
        }

        private void dataGridViewCurriculumWorkTypes_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (!isSelectionChanging)
            {
                var works = m_workRepository.GetAllWorks();

                if (e.RowIndex < 0)
                {
                    return;
                }

                if (e.RowIndex >= works.Count)
                {
                    return;
                }

                for (int i = 0; i < e.RowCount; ++i)
                {
                    m_workRepository.RemoveWork(works[i + e.RowIndex]);
                }
            }
        }
    }
}
