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
    public partial class CurriculumForm : Form, IDataUserForm
    {
        private ICurriculumItemRepository m_curriculumItemRepository;
        private ISubjectRepository m_subjectRepository;
        private IWorkTypeRepository m_workTypeRepository;
        private IWorkRepository m_workRepository;

        private MainForm m_parentForm = null;
        private const string IncorrectDataMessage = "Некоректні дані!";
        private bool isSelectionChanging = false;
        private string m_educType;
        private string m_educForm;
        private uint m_course;
        private uint m_semestr;
        private string m_educLevel;
        private bool m_isEmptyWorks;

        public CurriculumForm(MainForm parentForm,
                              string dbName,
                              string educType,
                              string educForm,
                              uint course,
                              uint semestr,
                              string educLevel,
                              bool isEmptyWorks)
        {
            InitializeComponent();
            m_parentForm = parentForm;

            m_educType = educType;
            m_educForm = educForm;
            m_course = course;
            m_semestr = semestr;
            m_educLevel = educLevel;
            m_isEmptyWorks = isEmptyWorks;

            EnableListBoxes();

            listBoxWorkTypes.Enabled = false;
            listBoxCourse.Visible = false;
            listBoxWorkTypes.Visible = false;
            listBoxCourse.SelectedIndex = 0;

            if (course == 0)
            {
                listBoxCourse.Visible = true;
            }

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

        private uint GetCourse()
        { 
            return (listBoxCourse.Visible && listBoxCourse.Enabled) ? Convert.ToUInt32(listBoxCourse.SelectedItem.ToString()) : m_course; ;
        }

        public void LoadData()
        {
            var workTypes = m_workTypeRepository.GetAllWorkTypes();
            var subjects = m_subjectRepository.GetAllSubjects();
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

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

        private int getCurrentCurriculumId()
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);
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
                
                dataGridViewCurriculumSubjects.Rows[dgvIndex].SetValues(subjects[index].Name, GetCourse(), 0);
            }
        }

        private void SyncDataCurriculumSubjects()
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

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

            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

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

                var subjectWorks = new List<Work>();

                foreach (var workType in m_workTypeRepository.GetAllWorkTypes())
                {
                    var work = new Work();
                    var course = m_course == 0 ? Convert.ToUInt32(listBoxCourse.SelectedItem.ToString()) : m_course;

                    work.WorkTypeId = workType.Id;
                    work.CurriculumItemId = m_curriculumItemRepository.GetCurriculumItem(m_subjectRepository.GetSubject(curriculumItem.SubjectId).Id, m_educType, m_educForm, course, m_semestr, m_educLevel).Id;
                    work.TotalHours = 0;

                    subjectWorks.Add(work);
                }

                m_workRepository.AddWorks(subjectWorks);

                dataGridViewCurriculumSubjects.ClearSelection();
                dataGridViewCurriculumSubjects.Rows[dataGridViewCurriculumSubjects.Rows.Count - 1].Selected = true;
            }
        }

        private bool isValidSubject(string name, uint course, DataGridView dgv)
        {
            int duplicatesCounter = 0;

            for (int i = 0; i < dgv.Rows.Count; ++i)
            {
                if ((dgv[0, i].Value?.ToString().ToLower() == name.ToLower()) 
                    && dgv[1, i].Value?.ToString().ToLower() == course.ToString())
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
                    if (!isValidSubject(name, GetCourse(), dataGridViewCurriculumSubjects))
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
                            curriculumItem.EducType = m_educType;
                            curriculumItem.EducForm = m_educForm;
                            curriculumItem.Semestr = m_semestr;
                            curriculumItem.EducLevel = m_educLevel;

                            if (listBoxCourse.Visible && listBoxCourse.Enabled)
                            {
                                curriculumItem.Course = Convert.ToUInt32(listBoxCourse.SelectedItem.ToString());
                            }
                            else
                            {
                                curriculumItem.Course = m_course;
                            }

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
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

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
            if (dataGridViewCurriculumSubjects.SelectedCells.Count == 1 || dataGridViewCurriculumSubjects.SelectedRows.Count == 1)
            {
                var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

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
                var curriculumItemWorks = m_workRepository.GetWorks(curriculumItemId, m_isEmptyWorks);

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

            var curriculumItemWorks = m_workRepository.GetWorks(getCurrentCurriculumId(), m_isEmptyWorks);

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
            var works = m_workRepository.GetWorks(getCurrentCurriculumId(), m_isEmptyWorks);

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
                var works = m_workRepository.GetWorks(getCurrentCurriculumId(), m_isEmptyWorks);

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
