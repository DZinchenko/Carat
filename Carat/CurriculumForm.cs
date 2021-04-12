﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Carat.Data.Repositories;
using Carat.Interfaces;
using Carat.Data.Entities;
using Carat.EF.Repositories;
using NPOI.XSSF.UserModel;

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
                              bool isEmptyWorks,
                              object[] courses)
        {
            InitializeComponent();
            m_parentForm = parentForm;

            m_educType = educType;
            m_educForm = educForm;
            m_course = course;
            m_semestr = semestr;
            m_educLevel = educLevel;
            m_isEmptyWorks = isEmptyWorks;

            listBoxCourse.Items.Clear();

            foreach (var item in courses)
            {
                listBoxCourse.Items.Add(item);
            }

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

        public void UpdateWorks(bool isEmptyWorks)
        {
            m_isEmptyWorks = isEmptyWorks;

            if (dataGridViewCurriculumSubjects.SelectedCells.Count > 0)
            {
                var selectedRowIndex = dataGridViewCurriculumSubjects.SelectedCells[0].RowIndex;
                dataGridViewCurriculumSubjects.Rows[selectedRowIndex].Selected = false;
                dataGridViewCurriculumSubjects.Rows[selectedRowIndex].Selected = true;
            }
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
            int index = listBoxSubjects.IndexFromPoint(e.Location);

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
                var workTypes = m_workTypeRepository.GetAllWorkTypes();

                foreach (var workType in workTypes)
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

        private void dataGridViewCurriculumSubjects_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            var curriculumItem = curriculumItems[e.Row.Index];
            var works = m_workRepository.GetWorks(curriculumItem.Id, false);

            e.Cancel = !(works.All(work => work.TotalHours < 0.00001));
        }

        private void ReadFirstPage(NPOI.SS.UserModel.ISheet sheet, uint semestr, int startRowIt, int endRowIter)
        {
            var educLevel = "Бакалавр";
            var educForm = "Денна";
            var educType = "Бюджет";

            for (int i = startRowIt; i < endRowIter; ++i)
            {
                m_parentForm.IncrementProgress();

                var row = sheet.GetRow(i);
                var cellText = row.GetCell(1).ToString();

                if (Tools.GetEducLevel(cellText) != "")
                {
                    educLevel = Tools.GetEducLevel(cellText);
                    continue;
                }

                var subjectName = Tools.GetSubjectNameFromCell(cellText);
                var subject = m_subjectRepository.GetSubject(subjectName);

                if (subject == null)
                {
                    subject = new Subject();
                    subject.Name = subjectName;
                    m_subjectRepository.AddSubject(subject);
                    subject = m_subjectRepository.GetSubject(subjectName);
                }

                var course = Tools.GetCurriculumItemCourse(cellText);
                var hoursInCell = Convert.ToDouble(row.GetCell(2).ToString());
                var isExam = row.GetCell(7)?.ToString();
                var curriculumItem = new CurriculumItem();

                if (isExam == "1")
                {
                    hoursInCell += 30;
                }

                curriculumItem.Course = course;
                curriculumItem.EducForm = educForm;
                curriculumItem.EducLevel = educLevel;
                curriculumItem.EducType = educType;
                curriculumItem.Semestr = semestr;
                curriculumItem.SubjectHours = hoursInCell;
                curriculumItem.SubjectId = subject.Id;

                m_curriculumItemRepository.AddCurriculumItem(curriculumItem);
                var workTypes = m_workTypeRepository.GetAllWorkTypes();
                var curriculumId = m_curriculumItemRepository.GetCurriculumItem(
                    curriculumItem.SubjectId,
                    curriculumItem.EducType,
                    curriculumItem.EducForm,
                    course,
                    curriculumItem.Semestr,
                    curriculumItem.EducLevel).Id;
                var subjectWorks = new List<Work>();

                foreach (var workType in workTypes)
                {
                    var work = new Work();

                    work.WorkTypeId = workType.Id;
                    work.CurriculumItemId = curriculumId;
                    work.TotalHours = 0;

                    subjectWorks.Add(work);
                }

                for (int worksStartIt = 25, worksEndItworksIt = 37, worksIt = worksStartIt; worksIt <= worksEndItworksIt; ++worksIt)
                {
                    var workCellText = row.GetCell(worksIt)?.ToString();
                    double workHours;

                    if (workCellText == null)
                    {
                        continue;
                    }

                    try
                    {
                        workHours = row.GetCell(worksIt).NumericCellValue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    subjectWorks[worksIt - worksStartIt].TotalHours = workHours;
                }

                m_workRepository.AddWorks(subjectWorks);
            }
        }

        private void LoadCurriculumFromExcel(int firstSemestrStart, int firstSemestrEnd, int secondSemestrStart, int secondSemestrEnd)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "Excel Files|*.xlsx";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var workbook = new XSSFWorkbook(openFileDialog.FileName);
                        var sheet1 = workbook[0];
                        var sheet2 = workbook[1];
                        var curriculumItems = new List<CurriculumItem>();

                        if (sheet1 == null || sheet2 == null)
                        {
                            return;
                        }

                        m_curriculumItemRepository.DeleteAllCurriculumItems();
                        m_subjectRepository.DeleteAllSubjects();
                        m_workRepository.DeleteAllWorks();

                        ReadFirstPage(sheet1, 1, firstSemestrStart, firstSemestrEnd);
                        ReadFirstPage(sheet1, 2, secondSemestrStart, secondSemestrEnd);
                        ReadSecondPage(sheet2);

                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }

        private void SecondPageOneRowSubjects(double hours, 
                                              string subjectName, 
                                              int workTypeIndex, 
                                              string educForm, 
                                              string educType, 
                                              string educLevel, 
                                              uint semestr, 
                                              uint course)
        {
            var subject = m_subjectRepository.GetSubject(subjectName);

            if (subject == null)
            {
                subject = new Subject();
                subject.Name = subjectName;
                m_subjectRepository.AddSubject(subject);
                subject = m_subjectRepository.GetSubject(subjectName);
            }

            CurriculumItem curriculumItem = m_curriculumItemRepository.GetCurriculumItem(
                subject.Id,
                educType,
                educForm,
                course,
                semestr,
                educLevel);

            if (curriculumItem == null)
            {
                curriculumItem = new CurriculumItem();

                curriculumItem.Course = course;
                curriculumItem.EducForm = educForm;
                curriculumItem.EducLevel = educLevel;
                curriculumItem.EducType = educType;
                curriculumItem.Semestr = semestr;
                curriculumItem.SubjectHours = hours;
                curriculumItem.SubjectId = subject.Id;

                m_curriculumItemRepository.AddCurriculumItem(curriculumItem);
            }

            var curriculumId = m_curriculumItemRepository.GetCurriculumItem(
                curriculumItem.SubjectId,
                curriculumItem.EducType,
                curriculumItem.EducForm,
                course,
                curriculumItem.Semestr,
                curriculumItem.EducLevel).Id;

            var workTypes = m_workTypeRepository.GetAllWorkTypes();
            var subjectWorks = new List<Work>();

            foreach (var workType in workTypes)
            {
                var work = new Work();

                work.WorkTypeId = workType.Id;
                work.CurriculumItemId = curriculumId;
                work.TotalHours = 0;

                subjectWorks.Add(work);
            }

            subjectWorks[workTypeIndex].TotalHours = hours;

            m_workRepository.AddWorks(subjectWorks);
        }

        private void AddCurriculumItemSecondPage(uint course, 
                                                 string educForm, 
                                                 string educLevel, 
                                                 string educType, 
                                                 uint semestr, 
                                                 double hours, 
                                                 int subjectId,
                                                 int workTypeIndex)
        {
            CurriculumItem curriculumItem = m_curriculumItemRepository.GetCurriculumItem(
                subjectId,
                educType,
                educForm,
                course,
                semestr,
                educLevel);

            if (curriculumItem == null)
            {
                curriculumItem = new CurriculumItem();

                curriculumItem.Course = course;
                curriculumItem.EducForm = educForm;
                curriculumItem.EducLevel = educLevel;
                curriculumItem.EducType = educType;
                curriculumItem.Semestr = semestr;
                curriculumItem.SubjectHours = hours;
                curriculumItem.SubjectId = subjectId;

                m_curriculumItemRepository.AddCurriculumItem(curriculumItem);
            }

            var curriculumId = m_curriculumItemRepository.GetCurriculumItem(
                curriculumItem.SubjectId,
                curriculumItem.EducType,
                curriculumItem.EducForm,
                course,
                curriculumItem.Semestr,
                curriculumItem.EducLevel).Id;

            List<WorkType> workTypes = m_workTypeRepository.GetAllWorkTypes();
            List<Work> subjectWorks = m_workRepository.GetWorks(curriculumId, false);

            if (subjectWorks.Count == 0)
            {
                subjectWorks = new List<Work>();

                foreach (var workType in workTypes)
                {
                    var work = new Work();

                    work.WorkTypeId = workType.Id;
                    work.CurriculumItemId = curriculumId;
                    work.TotalHours = 0;

                    subjectWorks.Add(work);
                }

                subjectWorks[workTypeIndex].TotalHours = hours;

                m_workRepository.AddWorks(subjectWorks);
            }
            else 
            {
                subjectWorks[workTypeIndex].TotalHours = hours;

                m_workRepository.UpdateWork(subjectWorks[workTypeIndex]);
            }
        }

        private void SecondPageSeveralRowsSubjects(NPOI.SS.UserModel.ISheet sheet,
                                                   string educForm,
                                                   string educType)
        {
            var subjectName = "Бакалаврський проект";
            var educLevel = "Бакалавр";
            uint course = 4;
            var subject = m_subjectRepository.GetSubject(subjectName);

            if (subject == null)
            {
                subject = new Subject();
                subject.Name = subjectName;
                m_subjectRepository.AddSubject(subject);
                subject = m_subjectRepository.GetSubject(subjectName);
            }

            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(7).GetCell(12).NumericCellValue, subject.Id, 14);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(7).GetCell(17).NumericCellValue, subject.Id, 14);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(11).GetCell(12).NumericCellValue, subject.Id, 15);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(11).GetCell(17).NumericCellValue, subject.Id, 15);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(14).GetCell(12).NumericCellValue, subject.Id, 18);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(14).GetCell(17).NumericCellValue, subject.Id, 18);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(17).GetCell(12).NumericCellValue, subject.Id, 24);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(17).GetCell(17).NumericCellValue, subject.Id, 24);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(20).GetCell(12).NumericCellValue, subject.Id, 21);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(20).GetCell(17).NumericCellValue, subject.Id, 21);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(23).GetCell(12).NumericCellValue, subject.Id, 27);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(23).GetCell(17).NumericCellValue, subject.Id, 27);

            subjectName = "Магістерська дисертація ОПП";
            educLevel = "Магістр";
            course = 2;
            subject = m_subjectRepository.GetSubject(subjectName);

            if (subject == null)
            {
                subject = new Subject();
                subject.Name = subjectName;
                m_subjectRepository.AddSubject(subject);
                subject = m_subjectRepository.GetSubject(subjectName);
            }

            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(8).GetCell(12).NumericCellValue, subject.Id, 14);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(8).GetCell(17).NumericCellValue, subject.Id, 14);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(12).GetCell(12).NumericCellValue, subject.Id, 16);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(12).GetCell(17).NumericCellValue, subject.Id, 16);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(15).GetCell(12).NumericCellValue, subject.Id, 19);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(15).GetCell(17).NumericCellValue, subject.Id, 19);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(18).GetCell(12).NumericCellValue, subject.Id, 25);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(18).GetCell(17).NumericCellValue, subject.Id, 25);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(21).GetCell(12).NumericCellValue, subject.Id, 22);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(21).GetCell(17).NumericCellValue, subject.Id, 22);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(26).GetCell(12).NumericCellValue, subject.Id, 30);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(26).GetCell(17).NumericCellValue, subject.Id, 30);

            subjectName = "Магістерська дисертація ОНП";
            educLevel = "Магістр";
            course = 2;
            subject = m_subjectRepository.GetSubject(subjectName);

            if (subject == null)
            {
                subject = new Subject();
                subject.Name = subjectName;
                m_subjectRepository.AddSubject(subject);
                subject = m_subjectRepository.GetSubject(subjectName);
            }

            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(9).GetCell(12).NumericCellValue, subject.Id, 14);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(9).GetCell(17).NumericCellValue, subject.Id, 14);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(13).GetCell(12).NumericCellValue, subject.Id, 17);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(13).GetCell(17).NumericCellValue, subject.Id, 17);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(16).GetCell(12).NumericCellValue, subject.Id, 20);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(16).GetCell(17).NumericCellValue, subject.Id, 20);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(19).GetCell(12).NumericCellValue, subject.Id, 26);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(19).GetCell(17).NumericCellValue, subject.Id, 26);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(22).GetCell(12).NumericCellValue, subject.Id, 23);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(22).GetCell(17).NumericCellValue, subject.Id, 23);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(29).GetCell(12).NumericCellValue, subject.Id, 32);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(29).GetCell(17).NumericCellValue, subject.Id, 32);
        }

        private void ReadSecondPage(NPOI.SS.UserModel.ISheet sheet)
        {
            var educForm = "Денна";
            var educType = "Бюджет";

            var aspirantRow = sheet.GetRow(36);
            var firstSemestrCellIndex = 12;
            var secondSemestrCellIndex = 17;
            var firstSemestrAspirantsCellText = aspirantRow.GetCell(firstSemestrCellIndex).ToString();
            var secondSemestrCellText = aspirantRow.GetCell(secondSemestrCellIndex).ToString();

            if (firstSemestrAspirantsCellText != null)
            {
                SecondPageOneRowSubjects(
                    aspirantRow.GetCell(firstSemestrCellIndex).NumericCellValue, "Аспіранти", 32, educForm, educType, "PhD", 1, 1);
            }

            if (secondSemestrCellText != null)
            {
                SecondPageOneRowSubjects(
                    aspirantRow.GetCell(secondSemestrCellIndex).NumericCellValue, "Аспіранти", 32, educForm, educType, "PhD", 2, 1);
            }

            for (int i = 0; i < 2; ++i)
            {
                var magisterRow = sheet.GetRow(5 + i);
                var firstSemestrMagistersCellText = aspirantRow.GetCell(8).ToString();
                var secondSemestrMagistersCellText = aspirantRow.GetCell(13).ToString();

                if (firstSemestrMagistersCellText != null)
                {
                    SecondPageOneRowSubjects(
                        magisterRow.GetCell(firstSemestrCellIndex).NumericCellValue,
                        "Науково-дослідна робота за темою магістерської дисертації - 1. Основи наукових досліджень",
                        13,
                        educForm, educType,
                        "Магістр",
                        1,
                        Convert.ToUInt32(1 + i));
                }

                if (secondSemestrMagistersCellText != null)
                {
                    SecondPageOneRowSubjects(
                        magisterRow.GetCell(secondSemestrCellIndex).NumericCellValue,
                        "Науково-дослідна робота за темою магістерської дисертації - 1. Основи наукових досліджень",
                        13,
                        educForm, educType,
                        "Магістр",
                        2,
                        Convert.ToUInt32(1 + i));
                }

                SecondPageSeveralRowsSubjects(sheet, educForm, educType);
            }
        }

        private void buttonImportCurriculum_Click(object sender, EventArgs e)
        {
            var selectRowsForm = new SelectCurriculumRows();
            selectRowsForm.ShowDialog();

            if (selectRowsForm.DialogResult != DialogResult.OK)
            {
                return;
            }

            var values = selectRowsForm.getValues();
            var progress = (values.firstSemestrEnd - values.firstSemestrStart) + (values.secondSemestrEnd - values.secondSemestrStart);

            if (values.firstSemestrEnd <= values.firstSemestrStart || values.secondSemestrEnd <= values.secondSemestrStart || progress <= 0)
                return;

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(progress, "Curriculum loading...");

            LoadCurriculumFromExcel(values.firstSemestrStart, 
                                    values.firstSemestrEnd, 
                                    values.secondSemestrStart, 
                                    values.secondSemestrEnd);

            m_parentForm.HideProgress();
            m_parentForm.Enabled = true;
        }
    }
}
