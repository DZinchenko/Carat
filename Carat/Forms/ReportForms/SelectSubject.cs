using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Carat.Data.Repositories;
using Carat.Data.Entities;
using Carat.EF.Repositories;
using NPOI.XSSF.UserModel;
using Carat.Interfaces;

namespace Carat
{
    public partial class SelectSubject : Form, IFiltersChangable
    {
        private MainForm m_parentForm = null;
        private string m_dbPath;
        private string m_educType;
        private string m_educForm;
        private string m_educLevel;
        private uint m_course;
        private uint m_semestr;

        private string IncorrectNameMessageDataIsEmpty = "Дані відсутні!";

        ITeacherRepository m_teacherRepository;
        ICurriculumItemRepository m_curriculumItemRepository;
        IWorkRepository m_workRepository;
        ITAItemRepository m_taItemRepository;
        ISubjectRepository m_subjectRepository;
        IGroupsToTAItemRepository m_groupsToTAItemRepository;
        IGroupRepository m_groupRepository;
        public SelectSubject(MainForm parentForm,
                            string dbPath,
                            string educType,
                            string educForm,
                            string educLevel,
                            uint course,
                            uint semestr)

        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_dbPath = dbPath;
            m_educType = educType;
            m_educForm = educForm;
            m_educLevel = educLevel;
            m_course = course;
            m_semestr = semestr;

            m_teacherRepository = new TeacherRepository(m_dbPath);
            m_curriculumItemRepository = new CurriculumItemRepository(m_dbPath);
            m_workRepository = new WorkRepository(m_dbPath);
            m_taItemRepository = new TAItemRepository(m_dbPath);
            m_subjectRepository = new SubjectRepository(m_dbPath);
            m_groupsToTAItemRepository = new GroupsToTAItemRepository(m_dbPath);
            m_groupRepository = new GroupRepository(m_dbPath);

            LoadData();
        }

        public void SetFilters(string educType,
                                string educForm,
                                string educLevel,
                                uint course,
                                uint semestr)
        {
            m_educType = educType;
            m_educForm = educForm;
            m_educLevel = educLevel;
            m_course = course;
            m_semestr = semestr;
            LoadData();
        }
        private string GetSemesterString()
        {
            string result = "";

            switch (m_semestr)
            {
                case 0:
                    {
                        result = "За два семестри";
                        break;
                    }
                case 1:
                    {
                        result = "I семестр";
                        break;
                    }
                case 2:
                    {
                        result = "II семестр";
                        break;
                    }
                default:
                    {
                        throw new Exception("Semester cast error");
                    }
            }

            return result;
        }

        private string GetEducFormString()
        {
            if (m_educForm == "<всі>")
            {
                return "всі форми навчання";
            }

            return (m_educForm + " форма навчання").ToLower();
        }

        private string GetEducTypeString()
        {
            if (m_educType == "<всі>")
            {
                return "всі види навчання";
            }

            return m_educType.ToLower();
        }

        private string GetEducFormString(CurriculumItem curriculumItem)
        {
            var result = "д";

            if (curriculumItem.EducForm == "Заочна")
            {
                result = "з";
            }
            else if (curriculumItem.EducForm == "Вечірня")
            {
                result = "в";
            }

            return result;
        }

        private void LoadData()
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel)
              .GroupBy(ci => ci.SubjectId)
              .Select(g => g.First())
              .ToList();

            dataGridViewSelectSubject.Rows.Clear();
            foreach (var item in curriculumItems)
            {
                dataGridViewSelectSubject.Rows.Add(m_subjectRepository.GetSubject(item.SubjectId).Name, item.Course);
            }
        }

        private void dataGridViewSelectSubject_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);

            if (e.RowIndex < 0 || e.RowIndex >= curriculumItems.Count)
            {
                return;
            }

            GenerateReport(m_subjectRepository.GetSubject(curriculumItems[e.RowIndex].SubjectId));
        }

        private void GenerateReport(Subject subject)
        {
            var allCurriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            allCurriculumItems.RemoveAll(curItem => { return curItem.SubjectId != subject.Id; });

            if (allCurriculumItems.Count <= 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(allCurriculumItems.Count, "By subject report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\DefaultBySubject.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue("Дисципліна: " + subject.Name + ", " + GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                int numberCounter = 0;
                CurriculumItem lastCurItem = null;
                Dictionary<int, List<TAItem>> lastTeachersDic = null;
                foreach (var curItem in allCurriculumItems)
                {
                    var allWorks = m_workRepository.GetWorks(curItem.Id, true);
                    var allTAItems = new List<TAItem>();
                    var teachersDic = new Dictionary<int, List<TAItem>>();

                    foreach (var work in allWorks)
                    {
                        allTAItems.AddRange(m_taItemRepository.GetTAItems(work.Id));
                    }
                    allTAItems.RemoveAll(item => { return Tools.isEqual(0, item.WorkHours); });

                    foreach (var taItem in allTAItems)
                    {
                        var groups = new List<Group>();

                        if (!teachersDic.ContainsKey(taItem.TeacherId))
                        {
                            teachersDic[taItem.TeacherId] = new List<TAItem>();
                        }

                        teachersDic[taItem.TeacherId].Add(taItem);
                    }

                    foreach (var teacherDicEl in teachersDic)
                    {
                        var teacher = m_teacherRepository.GetTeacher(teacherDicEl.Key);
                        var groupsToTaItems = m_groupsToTAItemRepository.GetGroupsToTAItem(teacherDicEl.Value[0].Id);
                        var groupNames = m_groupRepository.GetGroups(groupsToTaItems.Select(item => item.GroupId).ToList())
                                            .Select(g => g.Name).OrderBy(n => n).ToList();

                        if (lastCurItem != null && curItem.SubjectId == lastCurItem.SubjectId
                                                && curItem.EducForm == lastCurItem.EducForm
                                                && curItem.EducLevel == lastCurItem.EducLevel
                                                && curItem.EducType == lastCurItem.EducType
                                                && curItem.Course == lastCurItem.Course
                                                && lastTeachersDic.ContainsKey(teacher.Id))
                        {
                            var row = sheet.GetRow(sheet.LastRowNum - lastTeachersDic.Count + lastTeachersDic.Keys.ToList().IndexOf(teacher.Id));
                            
                            var lastGroupCellVal = row.Cells[2].StringCellValue;
                            foreach (var groupName in groupNames)
                            {
                                if (!lastGroupCellVal.Contains(groupName))
                                {
                                    lastGroupCellVal += groupName;
                                }
                            }
                            row.Cells[2].SetCellValue(lastGroupCellVal);

                            foreach (var taItem in teacherDicEl.Value)
                            {
                                var work = m_workRepository.GetWork(taItem.WorkId);
                                row.Cells[6 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours + row.Cells[6 + work.WorkTypeId - 1].NumericCellValue);
                            }

                            continue;
                        }

                        var groupsCellText = "";
                        groupsCellText = string.Join("; ", groupNames);

                        var newRow = sheet.CopyRow(8, sheet.LastRowNum);
                        newRow.Cells[0].SetCellValue((numberCounter + 1).ToString());
                        newRow.Cells[1].SetCellValue(teacher.Name);
                        newRow.Cells[2].SetCellValue(groupsCellText);
                        newRow.Cells[3].SetCellValue(curItem.EducLevel);
                        newRow.Cells[4].SetCellValue(GetEducFormString(curItem));
                        newRow.Cells[5].SetCellValue(curItem.Course);

                        foreach (var taItem in teacherDicEl.Value)
                        {
                            var work = m_workRepository.GetWork(taItem.WorkId);
                            newRow.Cells[6 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours);
                        }

                        newRow.Height = -1;

                        ++numberCounter;
                    }

                    lastCurItem = curItem;
                    lastTeachersDic = teachersDic;
                    m_parentForm.IncrementProgress();
                }

                sheet.ShiftRows(9, sheet.LastRowNum, -1);

                for (int i = 6; i < 41; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(sheet.LastRowNum - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(sheet.LastRowNum).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = sheet.LastRowNum; i <= lastIndex; ++i)
                {
                    var firstCell = sheet.GetRow(i).Cells[6].Address;
                    var lastCell = sheet.GetRow(i).Cells[40].Address;
                    var finalCell = sheet.GetRow(i).Cells[41];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);

                using (var fileData = new FileStream(Tools.GetTempFilePathWithExtension(".xlsx"), FileMode.OpenOrCreate))
                {
                    workbook.Write(fileData);

                    System.Diagnostics.Process.Start(@fileData.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            m_parentForm.Enabled = true;
            m_parentForm.HideProgress();
        }
    }
}
