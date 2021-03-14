using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Carat.Interfaces;
using Carat.Data.Repositories;
using Carat.Data.Entities;
using Carat.EF.Repositories;

namespace Carat
{
    public partial class TeacherAssignmentForm : Form, IDataUserForm
    {
        private MainForm m_parentForm = null;
        private ISubjectRepository m_subjectRepository = null;
        private IGroupRepository m_groupRepository = null;
        private ICurriculumItemRepository m_curriculumItemRepository = null;
        private IWorkRepository m_workRepository = null;
        private IWorkTypeRepository m_workTypeRepository = null;
        private ITeacherRepository m_teacherRepository = null;
        private IGroupsToTAItemRepository m_groupsToTeacherRepository = null;
        private ITAItemRepository m_TAItemRepository = null;

        private bool isSelectionChanging = false;

        private double selectedFreeHours = 0;
        private int selectedWorkId = 0;
        private int selectedWorkIndex = 0;
        private int selectedCurriculumSubjectId = 0;

        private string m_educType;
        private string m_educForm;
        private uint m_course;
        private uint m_semestr;
        private string m_educLevel;

        const string IncorrectDataMessage = "Некоректні дані";

        public TeacherAssignmentForm(MainForm parentForm,
                              string dbPath,
                              string educType,
                              string educForm,
                              uint course,
                              uint semestr,
                              string educLevel)
        {
            InitializeComponent();

            m_educType = educType;
            m_educForm = educForm;
            m_course = course;
            m_semestr = semestr;
            m_educLevel = educLevel;

            comboBoxTATeachers.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTATeachers.Enabled = false;

            m_parentForm = parentForm;
            m_subjectRepository = new SubjectRepository(dbPath);
            m_groupRepository = new GroupRepository(dbPath);
            m_curriculumItemRepository = new CurriculumItemRepository(dbPath);
            m_workRepository = new WorkRepository(dbPath);
            m_workTypeRepository = new WorkTypeRepository(dbPath);
            m_teacherRepository = new TeacherRepository(dbPath);
            m_groupsToTeacherRepository = new GroupsToTAItemRepository(dbPath);
            m_TAItemRepository = new TAItemRepository(dbPath);
        }

        public void LoadData()
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            var teachers = m_teacherRepository.GetAllTeachers();

            foreach (var item in curriculumItems)
            {
                dataGridViewTASubjects.Rows.Add(m_subjectRepository.GetSubject(item.SubjectId)?.Name, item.Course);
            }

            foreach (var teacher in teachers)
            {
                comboBoxTATeachers.Items.Add(teacher.Name);
            }
        }

        private uint getCourseBySelected()
        {
            return m_curriculumItemRepository.GetCurriculumItem(m_workRepository.GetWork(selectedWorkId).CurriculumItemId).Course;
        }

        private void TeacherAssignment_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.TAForm = null;
            m_parentForm.SetButtonState();
        }

        private void dataGridViewTASubjects_SelectionChanged(object sender, EventArgs e)
        {
            if (!(dataGridViewTASubjects.SelectedRows.Count == 1))
            {
                return;
            }

            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            var rowIndex = dataGridViewTASubjects.SelectedRows[0].Index;

            if (curriculumItems.Count <= rowIndex)
            {
                return;
            }

            selectedCurriculumSubjectId = curriculumItems[rowIndex].Id;

            LoadWorks(curriculumItems[rowIndex].Id);
        }

        private void RemoveLastRowTAWorks()
        {
            int index = dataGridViewTAWorks.Rows.Count - 1;

            if (index < 0)
            {
                return;
            }

            dataGridViewTAWorks.Rows.Remove(dataGridViewTAWorks.Rows[index]);
        }

        private void LoadWorks(int curriculumItemId)
        {
            for (int i = 0, limit = dataGridViewTAWorks.RowCount; i < limit; ++i)
            {
                RemoveLastRowTAWorks();
            }

            var works = m_workRepository.GetWorks(curriculumItemId);

            foreach (var work in works)
            {
                var taItems = m_TAItemRepository.GetTAItems(work.Id);
                double sum = 0;

                foreach (var taItem in taItems)
                {
                    sum += taItem.WorkHours;
                }

                dataGridViewTAWorks.Rows.Add(m_workTypeRepository.GetWorkType(work.WorkTypeId)?.Name?.ToString(), work.TotalHours - sum);
            }
        }

        private void dataGridViewTAWorks_SelectionChanged(object sender, EventArgs e)
        {
            comboBoxTATeachers.Enabled = false;

            if (!(dataGridViewTAWorks.SelectedCells.Count == 1))
            {
                return;
            }

            var rowIndex = dataGridViewTAWorks.SelectedCells[0].RowIndex;
            var works = m_workRepository.GetWorks(selectedCurriculumSubjectId);

            if (rowIndex >= works.Count)
            {
                return;
            }

            string sFreeHours = dataGridViewTAWorks.Rows[rowIndex].Cells[1].Value.ToString();
            double dFreeHours;

            try
            {
                dFreeHours = Convert.ToDouble(sFreeHours);
            }
            catch (Exception)
            {
                return;
            }
            
            selectedWorkId = works[rowIndex].Id;
            selectedWorkIndex = rowIndex;
            selectedFreeHours = dFreeHours;

            isSelectionChanging = true;
            LoadTAItems();
            isSelectionChanging = false;

            comboBoxTATeachers.Enabled = Tools.isGreaterThanZero(dFreeHours);
        }

        private void LoadTAItems()
        {
            for (int i = 0, limit = dataGridViewTATeachers.Rows.Count; i < limit; ++i)
            {
                RemoveLastTAItem();
            }

            var taItems = m_TAItemRepository.GetTAItems(selectedWorkId);

            foreach (var item in taItems)
            {
                dataGridViewTATeachers.Rows.Add(m_teacherRepository.GetTeacher(item.TeacherId)?.Name, item?.WorkHours, "");
            }           
        }

        private void RemoveLastRowTAItems()
        {
            int index = dataGridViewTATeachers.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            dataGridViewTATeachers.Rows.Remove(dataGridViewTATeachers.Rows[index]);
        }

        private void comboBoxTATeachers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTATeachers.SelectedIndex < 0)
            {
                return;
            }

            var teacher = m_teacherRepository.GetTeacherByName(comboBoxTATeachers.SelectedItem.ToString());

            if (teacher != null)
            {
                var dgvIndex = dataGridViewTATeachers.Rows.Count;

                dataGridViewTATeachers.Rows.Add();
                dataGridViewTATeachers.Rows[dgvIndex].SetValues(teacher.Name, 0, "");
            }
        }

        private void RemoveLastTAItem()
        {
            int index = dataGridViewTATeachers.Rows.Count - 1;

            if (index < 0)
            {
                return;
            }

            dataGridViewTATeachers.Rows.Remove(dataGridViewTATeachers.Rows[index]);
        }

        private void dataGridViewTATeachers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_TAItemRepository == null)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            var TAItems = m_TAItemRepository.GetTAItems(selectedWorkId);

            if (e.RowIndex < TAItems.Count)
            {
                var item = TAItems[e.RowIndex];

                if (!UpdateTAItem(item, e, false))
                {
                    return;
                }

                m_TAItemRepository.UpdateTAItem(item);

                SyncHours(item.WorkId);
            }
            else
            {
                var item = new TAItem();

                if (!UpdateTAItem(item, e, true))
                {
                    RemoveLastTAItem();
                    return;
                }

                m_TAItemRepository.AddTAItem(item);

                SyncHours(item.WorkId);
            }
        }

        private bool isValidName(string name)
        {
            int duplicatesCounter = 0;

            if (name == null || name == "")
            {
                return false;
            }

            for (int i = 0; i < dataGridViewTATeachers.Rows.Count; ++i)
            {
                if (dataGridViewTATeachers[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter > 1 ? false : true;
        }

        private void SyncData()
        {
            var TAItems = m_TAItemRepository.GetAllTAItems(m_educType, m_educForm, getCourseBySelected(), m_semestr, m_educLevel);

            for (int i = 0; i < TAItems.Count; ++i)
            {
                dataGridViewTATeachers.Rows[i].SetValues(
                    m_teacherRepository.GetTeacher(TAItems[i].TeacherId)?.Name, TAItems[i].WorkHours);
            }
        }

        private void SyncHours(int workId)
        {
            var taItems = m_TAItemRepository.GetTAItems(workId);
            double sum = 0;

            foreach (var item in taItems)
            {
                sum += item.WorkHours;
            }

            dataGridViewTAWorks[1, selectedWorkIndex].Value = m_workRepository.GetWork(workId).TotalHours - sum;
        }

        private bool UpdateTAItem(TAItem item, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var Name = dataGridViewTATeachers[0, e.RowIndex].Value?.ToString()?.Trim();

                if (!isValidName(Name))
                {
                    MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            var teacher = m_teacherRepository.GetTeacherByName(
                                dataGridViewTATeachers[e.ColumnIndex, e.RowIndex]?.Value?.ToString()?.Trim());

                            if (teacher == null)
                                throw new Exception();

                            item.TeacherId = teacher.Id;
                            item.WorkId = selectedWorkId;
                            item.Course = getCourseBySelected();
                            item.EducType = m_educType;
                            item.EducForm = m_educForm;
                            item.EducLevel = m_educLevel;
                            item.Semestr = m_semestr;

                            break;
                        }
                    case 1:
                        {
                            var workHours = Convert.ToDouble(dataGridViewTATeachers[e.ColumnIndex, e.RowIndex].Value?.ToString());
                            var freeHours = Convert.ToDouble(dataGridViewTAWorks[1, selectedWorkIndex].Value);
                            var work = m_workRepository.GetWork(item.WorkId);
                            double sum = 0;

                            for (int i = 0; i < dataGridViewTATeachers.RowCount; ++i)
                            {
                                sum += Convert.ToDouble(dataGridViewTATeachers.Rows[i].Cells[1].Value.ToString());
                            }

                            if (Tools.isLessThanZero(workHours) || Tools.isGreaterThan(sum, work.TotalHours))
                            {
                                throw new Exception();
                            }

                            dataGridViewTAWorks[1, selectedWorkIndex].Value = work.TotalHours - sum;

                            item.WorkHours = workHours;

                            break;
                        }
                    case 2:
                        {

                            break;
                        }
                    default: { return false; }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(IncorrectDataMessage);
                SyncData();
                return false;
            }

            return true;
        }

        private void dataGridViewTATeachers_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var items = m_TAItemRepository.GetAllTAItems(m_educType, m_educForm, getCourseBySelected(), m_semestr, m_educLevel);

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= items.Count)
            {
                return;
            }

            if (!isSelectionChanging)
            {
                for (int i = 0; i < e.RowCount; ++i)
                {
                    m_TAItemRepository.RemoveTAItem(items[i + e.RowIndex]);
                }

                SyncHours(selectedWorkId);
            }
        }
    }
}
