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
using Carat.Interfaces;
using Carat.Data.Entities;
using Carat.EF.Repositories;

namespace Carat
{
    public partial class TeacherAssignmentForm : Form, IDataUser
    {
        MainForm m_parentForm = null;
        ISubjectRepository m_subjectRepository = null;
        IGroupRepository m_groupRepository = null;
        ICurriculumItemRepository m_curriculumItemRepository = null;
        IWorkRepository m_workRepository = null;
        IWorkTypeRepository m_workTypeRepository = null;
        ITeacherRepository m_teacherRepository = null;
        IGroupsToTeacherRepository m_groupsToTeacherRepository = null;
        ITAItemRepository m_TAItemRepository = null;

        bool isSelectionChanging = false;

        private double selectedFreeHours = 0;
        private int selectedWorkId = 0;
        private int selectedWorkIndex = 0;
        private int selectedCurriculumSubjectId = 0;

        const string IncorrectDataMessage = "Некоректні дані";

        public TeacherAssignmentForm(MainForm parentForm, string dbPath)
        {
            InitializeComponent();
            comboBoxTATeachers.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTATeachers.Enabled = false;

            m_parentForm = parentForm;
            m_subjectRepository = new SubjectRepository(dbPath);
            m_groupRepository = new GroupRepository(dbPath);
            m_curriculumItemRepository = new CurriculumItemRepository(dbPath);
            m_workRepository = new WorkRepository(dbPath);
            m_workTypeRepository = new WorkTypeRepository(dbPath);
            m_teacherRepository = new TeacherRepository(dbPath);
            m_groupsToTeacherRepository = new GroupsToTeacherRepository(dbPath);
            m_TAItemRepository = new TAItemRepository(dbPath);
        }

        public void LoadData()
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems();
            var teachers = m_teacherRepository.GetAllTeachers();

            foreach (var item in curriculumItems)
            {
                dataGridViewTASubjects.Rows.Add(m_subjectRepository.GetSubject(item.SubjectId)?.Name, item.SubjectHours);
            }

            foreach (var teacher in teachers)
            {
                comboBoxTATeachers.Items.Add(teacher.Name);
            }
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

            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems();
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
                dataGridViewTAWorks.Rows.Add(m_workTypeRepository.GetWorkType(work.WorkTypeId)?.Name?.ToString(), work?.TotalHours);
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
                dataGridViewTATeachers.Rows[dgvIndex].SetValues(teacher.Name, selectedFreeHours, "");
            }
        }

        private void RemoveLastTAItem()
        {
            int index = dataGridViewTATeachers.Rows.Count - 2;

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

            var TAItems = m_TAItemRepository.GetAllTAItems();

            if (e.RowIndex < TAItems.Count)
            {
                var item = TAItems[e.RowIndex];

                if (!UpdateTAItem(item, e, false))
                {
                    return;
                }

                m_TAItemRepository.UpdateTAItem(item);
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
            var TAItems = m_TAItemRepository.GetAllTAItems();

            for (int i = 0; i < TAItems.Count; ++i)
            {
                dataGridViewTATeachers.Rows[i].SetValues(
                    m_teacherRepository.GetTeacher(TAItems[i].TeacherId)?.Name, TAItems[i].WorkHours);
            }
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

                            break;
                        }
                    case 1:
                        {
                            item.WorkHours = Convert.ToDouble(dataGridViewTATeachers[e.ColumnIndex, e.RowIndex].Value?.ToString());
                            var freeHourse = Convert.ToDouble(dataGridViewTAWorks[1, selectedWorkIndex].Value);

                            dataGridViewTAWorks[1, selectedWorkIndex].Value = (freeHourse - item.WorkHours).ToString();

                            if (Tools.isLessThanZero(item.WorkHours))
                            {
                                throw new Exception();
                            }

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
            var items = m_TAItemRepository.GetAllTAItems();

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
            }
        }
    }
}
