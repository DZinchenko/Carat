using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
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
        private ITAItemRepository m_taItemRepository;

        private MainForm m_parentForm = null;
        private const string IncorrectDataMessage = "Некоректні дані!";
        private const string NotEmptyWorksMessage = "Для того, щоб видалити дисципліну, вам потрібно обнулити години!";
        private const string DistributedHourExistMessage = "Обнулення є неможливим бо ще існують розподілені години!";
        private bool isSelectionChanging = false;
        private string m_educType;
        private string m_educForm;
        private uint m_course;
        private uint m_semestr;
        private string m_educLevel;
        private bool m_isEmptyWorks;
        private bool isSortChanging = false;
        private int sortColumnIndex = 0;
        private bool isWorkSync = false;
        
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
            m_taItemRepository = new TAItemRepository(dbName);
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

            if (dataGridViewCurriculumWorkTypes.SelectedCells.Count == 1)
            {
                var rowIndex = dataGridViewCurriculumWorkTypes.SelectedCells[0].RowIndex;
                dataGridViewCurriculumWorkTypes.ClearSelection();
                dataGridViewCurriculumWorkTypes.Rows[rowIndex].Cells[1].Selected = true;
            }
        }

        public void LoadData()
        {
            var workTypes = m_workTypeRepository.GetAllWorkTypes();
            var subjects = m_subjectRepository.GetAllSubjects();
            var curriculumItems = GetAllSortedCurriculumItems();

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
                var subject = subjects.Find(s => s.Id == curriculumItem.SubjectId);
                var rowInd = dataGridViewCurriculumSubjects.Rows.Add(subject.Name, curriculumItem?.Course, curriculumItem?.SubjectHours.ToString(Tools.HoursAccuracy));

                int getRowInd(string cirrSubName)
                {
                    var allCurriculumItems = GetAllSortedCurriculumItems();
                    return allCurriculumItems.FindIndex((i) => i.SubjectId == m_subjectRepository.GetSubject(cirrSubName).Id);
                }

                var contextMenuStrip = new ContextMenuStrip();
                contextMenuStrip.Items.Add(new ToolStripMenuItem("Обнулити години", null, (obj, e) => { this.ResetCurItemHours(getRowInd(subject.Name)); }));
                contextMenuStrip.Items.Add(new ToolStripMenuItem("Видалити дисципліну", null, (obj, e) => { this.DeleteCurItem(getRowInd(subject.Name)); }));
                dataGridViewCurriculumSubjects.Rows[rowInd].ContextMenuStrip = contextMenuStrip;
            }
        }

        private int getCurrentCurriculumId()
        {
            var curriculumItems = GetAllSortedCurriculumItems();
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
            var curriculumItems = GetAllSortedCurriculumItems();

            for (int i = 0; i < curriculumItems.Count; ++i)
            {
                dataGridViewCurriculumSubjects.Rows[i].SetValues(m_subjectRepository.GetSubject(
                    curriculumItems[i].SubjectId)?.Name, curriculumItems[i]?.Course, curriculumItems[i]?.SubjectHours.ToString(Tools.HoursAccuracy));
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

            var curriculumItems = GetAllSortedCurriculumItems();

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
            PerformSort();
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
            var curriculumItems = GetAllSortedCurriculumItems();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= curriculumItems.Count)
            {
                return;
            }

            if (isSortChanging)
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
                var curriculumItems = GetAllSortedCurriculumItems();

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
                    dataGridViewCurriculumWorkTypes.Rows.Add(workType?.Name, work?.TotalHours.ToString(Tools.HoursAccuracy));
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

            if (isWorkSync)
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
            UpdateWorks(m_isEmptyWorks);
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
            isWorkSync = true;
            for (int i = 0; i < works.Count; ++i)
            {
                dataGridViewCurriculumWorkTypes.Rows[i].Cells[1].Value = works[i]?.TotalHours.ToString(Tools.HoursAccuracy);
            }
            isWorkSync = false;
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
                            var newTotalHours = Convert.ToDouble(dataGridViewCurriculumWorkTypes[e.ColumnIndex, e.RowIndex].Value?.ToString());

                            if (Tools.isLessThanZero(newTotalHours))
                            {
                                throw new Exception(IncorrectDataMessage);
                            }

                            if (Tools.isGreaterThan(work.TotalHours, newTotalHours))
                            {
                                var taItems = m_taItemRepository.GetTAItems(work.Id);
                                double distributedHours = 0;

                                foreach (var taItem in taItems)
                                {
                                    distributedHours += taItem.WorkHours;
                                }

                                if (Tools.isGreaterThan(distributedHours, newTotalHours))
                                {
                                    throw new Exception("Значення не може бути менше ніж розподілені години!");
                                }
                            }
                            
                            work.TotalHours = newTotalHours;

                            break;
                        }
                    default: { return false; }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            e.Cancel = !this.CheckIfCanDeleteCurSubRow(e.Row.Index);
        }

        private void ReadFirstPage(NPOI.SS.UserModel.ISheet sheet, 
                                   uint semestr, 
                                   int startRowIt, 
                                   int endRowIter, 
                                   string educType, 
                                   string educForm)
        {
            var educLevel = "Бакалавр";

            for (int i = startRowIt; i < endRowIter; ++i)
            {
                m_parentForm.IncrementProgress();

                var row = sheet.GetRow(i);
                var individualWorkIndex = 14;
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

                double hoursInCell = 0;

                try
                {
                    hoursInCell = row.GetCell(2).NumericCellValue;
                }
                catch (Exception)
                { }

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
                curriculumItem.BudgetStudCnt = (int)(row.GetCell(19)?.NumericCellValue);
                curriculumItem.ContractStudCnt = (int)(row.GetCell(20)?.NumericCellValue);

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

                List<double> values = new List<double>();
                for (int worksStartIt = 24,worksEndItworksIt = 36, worksIt = worksStartIt; worksIt <= worksEndItworksIt; ++worksIt)
                {
                    var workCellText = row.GetCell(worksIt)?.ToString();
                    double workHours;
                    values.Add(0);

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

                    values[worksIt - worksStartIt] = workHours;
                }

                subjectWorks[0].TotalHours = values[0];
                subjectWorks[1].TotalHours = values[1];
                subjectWorks[2].TotalHours = values[2];

                subjectWorks[individualWorkIndex].TotalHours = values[3];
                subjectWorks[3].TotalHours = values[4];
                subjectWorks[4].TotalHours = values[5];
                subjectWorks[5].TotalHours = values[6];
                subjectWorks[6].TotalHours = values[7];
                subjectWorks[7].TotalHours = values[8];
                subjectWorks[8].TotalHours = values[9];
                subjectWorks[9].TotalHours = values[10];
                subjectWorks[10].TotalHours = values[11];
                subjectWorks[11].TotalHours = values[12];

                m_workRepository.AddWorks(subjectWorks);
            }
        }

        private void LoadCurriculumFromExcel(int firstSemestrStart, 
                                             int firstSemestrEnd, 
                                             int secondSemestrStart, 
                                             int secondSemestrEnd,
                                             string educType,
                                             string educForm)
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

                        var oldCurriculumItems = m_curriculumItemRepository.GetCurriculumItems(a => a.Id, educType, educForm);
                        var oldWorks = new List<Work>();

                        foreach (var curItem in oldCurriculumItems)
                        {
                            oldWorks.AddRange(m_workRepository.GetWorks(curItem.Id, false));
                        }

                        m_curriculumItemRepository.DeleteCurriculumItems(oldCurriculumItems);
                        m_workRepository.DeleteWorks(oldWorks);

                        ReadFirstPage(sheet1, 1, firstSemestrStart, firstSemestrEnd, educType, educForm);
                        ReadFirstPage(sheet1, 2, secondSemestrStart, secondSemestrEnd, educType, educForm);
                        ReadSecondPage(sheet2, educType, educForm);

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

                m_workRepository.AddWorks(subjectWorks);
            }

            var updatedWorks = m_workRepository.GetWorks(m_curriculumItemRepository.GetCurriculumItem(
                    curriculumItem.SubjectId,
                    curriculumItem.EducType,
                    curriculumItem.EducForm,
                    course,
                    curriculumItem.Semestr,
                    curriculumItem.EducLevel).Id, false);

            updatedWorks[workTypeIndex].TotalHours = hours;
            m_workRepository.UpdateWork(updatedWorks[workTypeIndex]);

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
                curriculumItem.SubjectHours = 0;
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
            var subjectName = "Aспіранти";
            var educLevel = "PhD";
            uint course = 1;
            var subject = m_subjectRepository.GetSubject(subjectName);

            if (subject == null)
            {
                subject = new Subject();
                subject.Name = subjectName;
                m_subjectRepository.AddSubject(subject);
                subject = m_subjectRepository.GetSubject(subjectName);
            }

            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(10).GetCell(12).NumericCellValue, subject.Id, 15);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(10).GetCell(17).NumericCellValue, subject.Id, 15);

            subjectName = "Бакалаврський проект";
            educLevel = "Бакалавр";
            course = 4;
            subject = m_subjectRepository.GetSubject(subjectName);

            if (subject == null)
            {
                subject = new Subject();
                subject.Name = subjectName;
                m_subjectRepository.AddSubject(subject);
                subject = m_subjectRepository.GetSubject(subjectName);
            }

            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(7).GetCell(12).NumericCellValue, subject.Id, 15);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(7).GetCell(17).NumericCellValue, subject.Id, 15);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(11).GetCell(12).NumericCellValue, subject.Id, 16);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(11).GetCell(17).NumericCellValue, subject.Id, 16);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(14).GetCell(12).NumericCellValue, subject.Id, 19);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(14).GetCell(17).NumericCellValue, subject.Id, 19);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(17).GetCell(12).NumericCellValue, subject.Id, 22);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(17).GetCell(17).NumericCellValue, subject.Id, 22);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(23).GetCell(12).NumericCellValue, subject.Id, 28);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(23).GetCell(17).NumericCellValue, subject.Id, 28);

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
                    course, educForm, educLevel, educType, 1, sheet.GetRow(8).GetCell(12).NumericCellValue, subject.Id, 15);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(8).GetCell(17).NumericCellValue, subject.Id, 15);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(12).GetCell(12).NumericCellValue, subject.Id, 17);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(12).GetCell(17).NumericCellValue, subject.Id, 17);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(15).GetCell(12).NumericCellValue, subject.Id, 20);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(15).GetCell(17).NumericCellValue, subject.Id, 20);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(18).GetCell(12).NumericCellValue, subject.Id, 23);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(18).GetCell(17).NumericCellValue, subject.Id, 23);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(26).GetCell(12).NumericCellValue, subject.Id, 29);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(26).GetCell(17).NumericCellValue, subject.Id, 29);

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
                    course, educForm, educLevel, educType, 1, sheet.GetRow(9).GetCell(12).NumericCellValue, subject.Id, 15);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(9).GetCell(17).NumericCellValue, subject.Id, 15);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(13).GetCell(12).NumericCellValue, subject.Id, 18);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(13).GetCell(17).NumericCellValue, subject.Id, 18);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(16).GetCell(12).NumericCellValue, subject.Id, 21);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(16).GetCell(17).NumericCellValue, subject.Id, 21);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(19).GetCell(12).NumericCellValue, subject.Id, 24);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(19).GetCell(17).NumericCellValue, subject.Id, 24);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(29).GetCell(12).NumericCellValue, subject.Id, 30);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(29).GetCell(17).NumericCellValue, subject.Id, 30);

            subjectName = "Вступний іспит";
            educLevel = "Магістр";
            course = 1;
            subject = m_subjectRepository.GetSubject(subjectName);

            if (subject == null)
            {
                subject = new Subject();
                subject.Name = subjectName;
                m_subjectRepository.AddSubject(subject);
                subject = m_subjectRepository.GetSubject(subjectName);
            }

            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(33).GetCell(12).NumericCellValue, subject.Id, 25);
            //AddCurriculumItemSecondPage(
            //        course, educForm, educLevel, educType, 2, sheet.GetRow(33).GetCell(17).NumericCellValue, subject.Id, 25);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(34).GetCell(12).NumericCellValue, subject.Id, 26);
            //AddCurriculumItemSecondPage(
            //        course, educForm, educLevel, educType, 2, sheet.GetRow(34).GetCell(17).NumericCellValue, subject.Id, 26);

            subjectName = "Вступний іспит";
            educLevel = "PhD";
            course = 1;
            subject = m_subjectRepository.GetSubject(subjectName);

            if (subject == null)
            {
                subject = new Subject();
                subject.Name = subjectName;
                m_subjectRepository.AddSubject(subject);
                subject = m_subjectRepository.GetSubject(subjectName);
            }

            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 1, sheet.GetRow(35).GetCell(12).NumericCellValue, subject.Id, 27);
            AddCurriculumItemSecondPage(
                    course, educForm, educLevel, educType, 2, sheet.GetRow(35).GetCell(17).NumericCellValue, subject.Id, 27);
        }

        private void ReadSecondPage(NPOI.SS.UserModel.ISheet sheet, string educType, string educForm)
        {
            var aspirantRow = sheet.GetRow(36);
            var firstSemestrCellIndex = 12;
            var secondSemestrCellIndex = 17;
            var firstSemestrAspirantsCellText = aspirantRow.GetCell(firstSemestrCellIndex).ToString();
            var secondSemestrCellText = aspirantRow.GetCell(secondSemestrCellIndex).ToString();

            if (firstSemestrAspirantsCellText != null)
            {
                SecondPageOneRowSubjects(
                    aspirantRow.GetCell(firstSemestrCellIndex).NumericCellValue, "Аспіранти", 31, educForm, educType, "PhD", 1, 1);
            }

            if (secondSemestrCellText != null)
            {
                SecondPageOneRowSubjects(
                    aspirantRow.GetCell(secondSemestrCellIndex).NumericCellValue, "Аспіранти", 31, educForm, educType, "PhD", 2, 1);
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

            var oldCurriculumItems = m_curriculumItemRepository.GetCurriculumItems(a => a.Id, values.educType, values.educForm);
            var works = new List<Work>();
            var taItems = new List<TAItem>();

            foreach (var curItem in oldCurriculumItems)
            {
                works.AddRange(m_workRepository.GetWorks(curItem.Id, true));
            }

            foreach (var work in works)
            {
                taItems.AddRange(m_taItemRepository.GetTAItems(work.Id));
            }

            if (taItems.Any(item => { return Tools.isEqual(item.WorkHours, 0); }))
            {
                var result = MessageBox.Show("Знайдено розподілені години в даному навчальному плані. При імпорті нового" +
                    " навчального плану розподілені години будуть втрачені!", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result != DialogResult.OK)
                {
                    return;
                }
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(progress, "Curriculum loading...");

            LoadCurriculumFromExcel(values.firstSemestrStart, 
                                    values.firstSemestrEnd, 
                                    values.secondSemestrStart, 
                                    values.secondSemestrEnd, 
                                    values.educType, 
                                    values.educForm);

            m_parentForm.HideProgress();
            m_parentForm.Enabled = true;
        }

        private void dataGridViewCurriculumSubjects_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
            {
                dataGridViewCurriculumSubjects.EndEdit();
                dataGridViewCurriculumWorkTypes.EndEdit();

                sortColumnIndex = e.ColumnIndex;

                PerformSort();
            }
        }

        private List<CurriculumItem> GetAllSortedCurriculumItems()
        {
            if (sortColumnIndex == 0)
            {
                return m_curriculumItemRepository.GetAllCurriculumItems(
                    item => m_subjectRepository.GetSubject(item.SubjectId).Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            }
            else
            {
                return m_curriculumItemRepository.GetAllCurriculumItems(
                    item => item.Course + m_subjectRepository.GetSubject(item.SubjectId).Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            }
        }

        private void PerformSort()
        {
            if (!isSortChanging)
            {
                var horizontalScrollingOffset = dataGridViewCurriculumSubjects.HorizontalScrollingOffset;
                var verticalScrollingOffset = dataGridViewCurriculumSubjects.VerticalScrollingOffset;
                isSortChanging = true;
                dataGridViewCurriculumSubjects.Rows.Clear();
                LoadData();
                isSortChanging = false;
                if (verticalScrollingOffset > 0)
                {
                    PropertyInfo verticalOffset = dataGridViewCurriculumSubjects.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                    verticalOffset.SetValue(this.dataGridViewCurriculumSubjects, verticalScrollingOffset, null);
                }
            }
        }

        private void ResetCurItemHours(int currSubRowInd)
        {
            var curriculumItems = GetAllSortedCurriculumItems();
            var currId = curriculumItems[currSubRowInd].Id;
            var curriculumItemWorks = m_workRepository.GetWorks(currId, true);

            if (m_taItemRepository.ExistTAItemsForWorks(curriculumItemWorks))
            {
                MessageBox.Show(DistributedHourExistMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            curriculumItemWorks.ForEach((w) => w.TotalHours = 0);
            m_workRepository.UpdateWorks(curriculumItemWorks);

            if(this.getCurrentCurriculumId() == currId)
            {
                this.SyncDataCurriculumWorks();
            }
        }

        private void DeleteCurItem(int currSubRowInd)
        {
            if (this.CheckIfCanDeleteCurSubRow(currSubRowInd))
            {
                dataGridViewCurriculumSubjects.Rows.RemoveAt(currSubRowInd);
            }
            else
            {
                MessageBox.Show(NotEmptyWorksMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckIfCanDeleteCurSubRow(int currSubRowInd)
        {
            var curriculumItems = GetAllSortedCurriculumItems();
            var curriculumItem = curriculumItems[currSubRowInd];
            var works = m_workRepository.GetWorks(curriculumItem.Id, false);
            return works.All(work => work.TotalHours < 0.00001);
        }
    }
}