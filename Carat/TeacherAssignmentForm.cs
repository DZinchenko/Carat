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

            //var groups = m_groupRepository.GetAllGroups();

            //foreach (var group in groups)
            //{
            //    dataGridViewGroups.Rows.Add(
            //        group.Course,
            //        group.Name,
            //        group.EducForm,
            //        group.BudgetNumber,
            //        group.ContractNumber,
            //        group.Faculty,
            //        group.Note);
            //}
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
    }
}
