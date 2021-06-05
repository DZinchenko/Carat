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

namespace Carat
{
    public partial class SelectTeacher : Form
    {
        private MainForm m_parentForm = null;
        private string m_dbPath;
        private string m_educType;
        private string m_educForm;
        private string m_educLevel;
        private uint m_course;
        private uint m_semestr;
        private bool m_isExtendedReport;

        private string IncorrectNameMessageDataIsEmpty = "Дані відсутні!";

        ITeacherRepository m_teacherRepository;
        ICurriculumItemRepository m_curriculumItemRepository;
        IWorkRepository m_workRepository;
        ITAItemRepository m_taItemRepository;
        ISubjectRepository m_subjectRepository;
        IGroupsToTAItemRepository m_groupsToTAItemRepository;
        IGroupRepository m_groupRepository;

        public SelectTeacher(MainForm parentForm,
                                string dbPath,
                                string educType,
                                string educForm,
                                string educLevel,
                                uint course,
                                uint semestr,
                                bool isExtendedReport)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_dbPath = dbPath;
            m_educType = educType;
            m_educForm = educForm;
            m_educLevel = educLevel;
            m_course = course;
            m_semestr = semestr;
            m_isExtendedReport = isExtendedReport;

            m_teacherRepository = new TeacherRepository(m_dbPath);
            m_curriculumItemRepository = new CurriculumItemRepository(m_dbPath);
            m_workRepository = new WorkRepository(m_dbPath);
            m_taItemRepository = new TAItemRepository(m_dbPath);
            m_subjectRepository = new SubjectRepository(m_dbPath);
            m_groupsToTAItemRepository = new GroupsToTAItemRepository(m_dbPath);
            m_groupRepository = new GroupRepository(m_dbPath);

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

            return (m_educForm + " форма начвання").ToLower();
        }

        private string GetEducTypeString()
        {
            if (m_educType == "<всі>")
            {
                return "всі види навчання";
            }

            return m_educType.ToLower();
        }

        private void LoadData()
        {
            var teachers = m_teacherRepository.GetAllTeachers();

            foreach (var teacher in teachers)
            {
                dataGridViewSelectTeacher.Rows.Add(teacher.Name);
            }
        }

        private void dataGridViewSelectTeacher_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var teachers = m_teacherRepository.GetAllTeachers();

            if (e.RowIndex < 0 || e.RowIndex >= teachers.Count)
            {
                return;
            }

            if (m_isExtendedReport)
            {
                GenerateExtendedReport(teachers[e.RowIndex]);
            }
            else
            {
                GenerateReport(teachers[e.RowIndex]);
            }
        }

        private void GenerateExtendedReport(Teacher teacher)
        {
            var allCurriculumItems = new List<CurriculumItem>();
            var allWorks = new List<Work>();
            var allTAItem = new List<TAItem>();
            var groupedTAItemsWithCurriculumItem = new Dictionary<int, List<TAItem>>();
            allCurriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

            foreach (var curItem in allCurriculumItems)
            {
                allWorks.AddRange(m_workRepository.GetWorks(curItem.Id, false));
            }

            foreach (var work in allWorks)
            {
                allTAItem.AddRange(m_taItemRepository.GetTAItems(work.Id));
            }

            allTAItem.RemoveAll(item => { return item.TeacherId != teacher.Id; });

            if (allTAItem.Count <= 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var taItem in allTAItem)
            {
                var work = m_workRepository.GetWork(taItem.WorkId);

                if (!groupedTAItemsWithCurriculumItem.ContainsKey(work.CurriculumItemId))
                {
                    groupedTAItemsWithCurriculumItem[work.CurriculumItemId] = new List<TAItem>();
                }
                groupedTAItemsWithCurriculumItem[work.CurriculumItemId].Add(taItem);
            }

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
                templatePath += "\\templates\\ExtendedByTeacher.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue("Викладач: " + teacher.Name + ", " + GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                int numberCounter = 0;
                foreach (var curriculumItemWithTaItems in groupedTAItemsWithCurriculumItem)
                {
                    var newRow = sheet.CopyRow(8, sheet.LastRowNum);
                    var curriculumItem = m_curriculumItemRepository.GetCurriculumItem(curriculumItemWithTaItems.Key);
                    var taItems = curriculumItemWithTaItems.Value;
                    var groupsDic = new Dictionary<int, Group>();
                    var otherTeachers = new Dictionary<int, Teacher>();

                    newRow.Cells[0].SetCellValue((numberCounter + 1).ToString());
                    newRow.Cells[3].SetCellValue(curriculumItem.EducLevel);
                    newRow.Cells[4].SetCellValue(curriculumItem.Course);

                    foreach (var taItem in taItems)
                    {
                        var groups = m_groupsToTAItemRepository.GetGroupsToTAItemsByTAItemId(taItem.Id);
                        var work = m_workRepository.GetWork(taItem.WorkId);
                        var currItem = m_curriculumItemRepository.GetCurriculumItem(work.CurriculumItemId);
                        var otherWorks = m_workRepository.GetWorks(currItem.Id, true);
                        var otherTAItems = new List<TAItem>();

                        foreach (var otherWork in otherWorks)
                        {
                            otherTAItems.AddRange(m_taItemRepository.GetTAItems(otherWork.Id));
                        }

                        foreach (var group in groups)
                        {
                            groupsDic[group.GroupId] = m_groupRepository.GetGroup(group.GroupId);
                        }

                        foreach (var otherTAItem in otherTAItems)
                        {
                            if (otherTAItem.Id != taItem.Id && otherTAItem.TeacherId != taItem.TeacherId)
                            {
                                otherTeachers[otherTAItem.TeacherId] = m_teacherRepository.GetTeacher(otherTAItem.TeacherId);
                            }
                        }
                    }

                    string firstCellText = m_subjectRepository.GetSubject(curriculumItem.SubjectId)?.Name + " ( ";
                    foreach (var group in groupsDic)
                    {
                        firstCellText += group.Value.Name + "; ";
                    }
                    firstCellText += ")";
                    newRow.Cells[1].SetCellValue(firstCellText);

                    string secondCellText = "";
                    foreach (var otherTeacher in otherTeachers)
                    {
                        secondCellText += otherTeacher.Value.Name + "; ";
                    }
                    newRow.Cells[2].SetCellValue(secondCellText);

                    foreach (var taItem in taItems)
                    {
                        var work = m_workRepository.GetWork(taItem.WorkId);
                        newRow.Cells[5 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours);
                    }

                    newRow.Height = -1;

                    ++numberCounter;
                }

                sheet.ShiftRows(9, sheet.LastRowNum, -1);

                for (int i = 5; i < 40; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(sheet.LastRowNum - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(sheet.LastRowNum).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = sheet.LastRowNum; i < lastIndex; ++i)
                {
                    var firstCell = sheet.GetRow(i).Cells[5].Address;
                    var lastCell = sheet.GetRow(i).Cells[39].Address;
                    var finalCell = sheet.GetRow(i).Cells[40];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                sheet.AutoSizeColumn(1);

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
        }

        private void GenerateReport(Teacher teacher)
        {
            var allCurriculumItems = new List<CurriculumItem>();
            var allWorks = new List<Work>();
            var allTAItem = new List<TAItem>();
            var groupedTAItemsWithCurriculumItem = new Dictionary<int, List<TAItem>>();
            allCurriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

            foreach (var curItem in allCurriculumItems)
            {
                allWorks.AddRange(m_workRepository.GetWorks(curItem.Id, false));
            }

            foreach (var work in allWorks)
            {
                allTAItem.AddRange(m_taItemRepository.GetTAItems(work.Id));
            }

            allTAItem.RemoveAll(item => { return item.TeacherId != teacher.Id; });

            if (allTAItem.Count <= 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var taItem in allTAItem)
            {
                var work = m_workRepository.GetWork(taItem.WorkId);

                if (!groupedTAItemsWithCurriculumItem.ContainsKey(work.CurriculumItemId))
                {
                    groupedTAItemsWithCurriculumItem[work.CurriculumItemId] = new List<TAItem>();
                }
                groupedTAItemsWithCurriculumItem[work.CurriculumItemId].Add(taItem);
            }

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
                templatePath += "\\templates\\DefaultByTeacher.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue("Викладач: " + teacher.Name + ", " + GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                int numberCounter = 0;
                foreach (var curriculumItemWithTaItems in groupedTAItemsWithCurriculumItem)
                {
                    var newRow = sheet.CopyRow(8, sheet.LastRowNum);
                    var curriculumItem = m_curriculumItemRepository.GetCurriculumItem(curriculumItemWithTaItems.Key);
                    var taItems = curriculumItemWithTaItems.Value;
                    var groupsDic = new Dictionary<int, Group>();

                    newRow.Cells[0].SetCellValue((numberCounter + 1).ToString());
                    newRow.Cells[2].SetCellValue(curriculumItem.EducLevel);
                    newRow.Cells[3].SetCellValue(curriculumItem.Course);

                    foreach (var taItem in taItems)
                    {
                        var groups = m_groupsToTAItemRepository.GetGroupsToTAItemsByTAItemId(taItem.Id);
                        foreach (var group in groups)
                        {
                            groupsDic[group.GroupId] = m_groupRepository.GetGroup(group.GroupId);
                        }
                    }

                    string firstCellText = m_subjectRepository.GetSubject(curriculumItem.SubjectId)?.Name + " ( ";
                    foreach (var group in groupsDic)
                    {
                        firstCellText += group.Value.Name + "; ";
                    }
                    firstCellText += ")";

                    newRow.Cells[1].SetCellValue(firstCellText);

                    foreach (var taItem in taItems)
                    {
                        var work = m_workRepository.GetWork(taItem.WorkId);
                        newRow.Cells[4 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours);
                    }

                    newRow.Height = -1;

                    ++numberCounter;
                }

                sheet.ShiftRows(9, sheet.LastRowNum, -1);

                for (int i = 4; i < 39; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(sheet.LastRowNum - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(sheet.LastRowNum).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = sheet.LastRowNum; i < lastIndex; ++i)
                {
                    var firstCell = sheet.GetRow(i).Cells[4].Address;
                    var lastCell = sheet.GetRow(i).Cells[38].Address;
                    var finalCell = sheet.GetRow(i).Cells[39];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                sheet.AutoSizeColumn(1);

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
        }
    }
}
