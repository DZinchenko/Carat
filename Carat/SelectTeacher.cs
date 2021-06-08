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
    public enum ByTeacherReportType
    {
        Default,
        Extended,
        Individual
    }

    public partial class SelectTeacher : Form
    {
        private MainForm m_parentForm = null;
        private string m_dbPath;
        private string m_educType;
        private string m_educForm;
        private string m_educLevel;
        private uint m_course;
        private uint m_semestr;
        private ByTeacherReportType m_reportType;
        private List<string> m_specialWorks = new List<string> 
        {
            "Бакалаврський проект",
            "Магістерська дисертація ОПП",
            "Магістерська дисертація ОНП",
            "Вступний іспит",
            "Аспіранти",
            "Науково-дослідна робота за темою магістерської дисертації - 1. Основи наукових досліджень"
        };

        private string IncorrectNameMessageDataIsEmpty = "Дані відсутні!";

        ITeacherRepository m_teacherRepository;
        ICurriculumItemRepository m_curriculumItemRepository;
        IWorkRepository m_workRepository;
        ITAItemRepository m_taItemRepository;
        ISubjectRepository m_subjectRepository;
        IGroupsToTAItemRepository m_groupsToTAItemRepository;
        IGroupRepository m_groupRepository;
        IWorkTypeRepository m_workTypeRepository;

        public SelectTeacher(MainForm parentForm,
                                string dbPath,
                                string educType,
                                string educForm,
                                string educLevel,
                                uint course,
                                uint semestr,
                                ByTeacherReportType reportType)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_dbPath = dbPath;
            m_educType = educType;
            m_educForm = educForm;
            m_educLevel = educLevel;
            m_course = course;
            m_semestr = semestr;
            m_reportType = reportType;

            m_teacherRepository = new TeacherRepository(m_dbPath);
            m_curriculumItemRepository = new CurriculumItemRepository(m_dbPath);
            m_workRepository = new WorkRepository(m_dbPath);
            m_taItemRepository = new TAItemRepository(m_dbPath);
            m_subjectRepository = new SubjectRepository(m_dbPath);
            m_groupsToTAItemRepository = new GroupsToTAItemRepository(m_dbPath);
            m_groupRepository = new GroupRepository(m_dbPath);
            m_workTypeRepository = new WorkTypeRepository(m_dbPath);

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

            switch (m_reportType)
            {
                case ByTeacherReportType.Default:
                    GenerateReport(teachers[e.RowIndex]);
                    break;
                case ByTeacherReportType.Extended:
                    GenerateExtendedReport(teachers[e.RowIndex]);
                    break;
                case ByTeacherReportType.Individual:
                    GenerateIndividualPlan(teachers[e.RowIndex]);
                    break;
                default:
                    break;
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

        private List<Group> GetGroups(List<TAItem> taItems)
        {
            var result = new Dictionary<int, Group>();

            foreach (var taItem in taItems)
            {
                var groupsToTaItem = m_groupsToTAItemRepository.GetGroupsToTAItemsByTAItemId(taItem.Id);

                foreach (var groupToTaItem in groupsToTaItem)
                {
                    var group = m_groupRepository.GetGroup(groupToTaItem.GroupId);
                    result[group.Id] = group;
                }
            }

            return result.Select(item => { return item.Value; }).ToList();
        }

        public static NPOI.SS.UserModel.ICell GetCell(NPOI.SS.UserModel.IRow row, int column)
        {
            NPOI.SS.UserModel.ICell cell = row.GetCell(column);

            if (cell == null)
            {
                cell = row.CreateCell(column);
            }
            return cell;
        }

        private int GetColumnIndexByWorkName(string workName)
        {
            int result = -1;

            switch (workName)
            {
                case "Лекції":
                    result = 38;
                    break;
                case "Практич.заняття (комп. практ. семін.)":
                    result = 42;
                    break;
                case "Лабор.роб. (комп.практ.)":
                    result = 46;
                    break;
                case "Екзамени":
                    result = 54;
                    break;
                case "Заліки":
                    result = 58;
                    break;
                case "Контр.роб. (мод.,темат.)":
                    result = 62;
                    break;
                case "Курсові проекти":
                    result = 66;
                    break;
                case "Курсові роботи":
                    result = 70;
                    break;
                case "РГР, РР, ГР":
                    result = 75;
                    break;
                case "ДКР":
                    result = 79;
                    break;
                case "Реферати":
                    result = 83;
                    break;
                case "Консультації":
                    result = 87;
                    break;
                case "Індивід заняття зі студентами":
                    result = 50;
                    break;
            }

            return result;
        }

        private int GetRowIndexByOtherWorkName(string workName)
        {
            int result = -1;

            switch (workName)
                {
                case "Індивід заняття з магістрами":
                    result = 7;
                    break;
                case "Індивід заняття за змішаною формою навч.":
                    result = 6;
                    break;
                case "Керівництво практиками":
                    result = 9;
                    break;
                case "Керівниц.атестац.роб.(бакалаврів)":
                    result = 13;
                    break;
                case "Керівниц.атестац.роб.(магістр ОПП)":
                    result = 14;
                    break;
                case "Керівниц.атестац.роб.(магістр ОНП)":
                    result = 15;
                    break;
                case "Консульт.атестац.роб.(бакалаврів)":
                    result = 16;
                    break;
                case "Консульт.атестац.роб.(магістр ОПП)":
                    result = 17;
                    break;
                case "Консульт.атестац.роб.(магістр ОНП)":
                    result = 18;
                    break;
                case "Рецензув.атестац.роб.(бакалаврів)":
                    result = 19;
                    break;
                case "Рецензув.атестац.роб.(магістр ОПП)":
                    result = 20;
                    break;
                case "Рецензув.атестац.роб.(магістр ОНП)":
                    result = 21;
                    break;
                case "Вступний іспит (магістр ОПП)":
                    result = 22;
                    break;
                case "Вступний іспит (магістр ОНП)":
                    result = 23;
                    break;
                case "Вступний іспит (аспірант)":
                    result = 24;
                    break;
                case "Робота в ЕК (бакалаврів)":
                    result = 25;
                    break;
                case "Робота в ЕК (магістр ОПП)":
                    result = 28;
                    break;
                case "Робота в ЕК (магістр ОНП)":
                    result = 31;
                    break;
                case "Керівництво (аспірантами)":
                    result = 34;
                    break;
                case "Керівництво (здобувач., стаж.)":
                    result = 35;
                    break;
                case "Заняття з аспірантами":
                    result = -1;
                    break;
                case "Консульт.докторантів":
                    result = 36;
                    break;
            }

            return result;
        }

        private void PrintOtherSubjects(NPOI.SS.UserModel.ISheet sheet, Dictionary<string, List<TAItem>> TAItemsSubjects)
        {
            int facultyColumnIndex = 101;
            int courseColumnIndex = 102;
            int groupColumnIndex = 103;
            int budjetColumnIndex = 104;
            int contractColumnIndex = 105;
            int hoursColumnIndex = 106;

            foreach (var subjectItem in TAItemsSubjects)
            {
                if (subjectItem.Value.Count <= 0)
                {
                    continue;
                }

                if (!m_specialWorks.Contains(subjectItem.Key))
                {
                    continue;
                }

                foreach (var taItem in subjectItem.Value)
                {
                    var work = m_workRepository.GetWork(taItem.WorkId);
                    var workType = m_workTypeRepository.GetWorkType(work.WorkTypeId);
                    var curriculumItem = m_curriculumItemRepository.GetCurriculumItem(work.CurriculumItemId);
                    int rowIndex = GetRowIndexByOtherWorkName(workType.Name);
                    int shiftValue = 0;

                    if (curriculumItem.Semestr == 2)
                    {
                        shiftValue = 10;
                    }

                    if (rowIndex < 0)
                    {
                        continue;
                    }


                    if (rowIndex == 7 && curriculumItem.EducLevel == "Магістр" && curriculumItem.Course == 2)
                    {
                        rowIndex += 1;
                    }
                    else if (rowIndex == 9)
                    {
                        if (subjectItem.Key.Contains("ОПП"))
                        {
                            rowIndex += 1;
                        }
                        else if (subjectItem.Key.Contains("ОНП"))
                        {
                            rowIndex += 2;
                        }
                    }

                    var cell = GetCell(sheet.GetRow(rowIndex), hoursColumnIndex + shiftValue);
                    try
                    {
                        cell.SetCellValue(cell.NumericCellValue + taItem.WorkHours);
                    }
                    catch (Exception) { }

                    var groups = GetGroups(new List<TAItem> { taItem });

                    GetCell(sheet.GetRow(rowIndex), facultyColumnIndex + shiftValue).SetCellValue("ТЕФ");
                    GetCell(sheet.GetRow(rowIndex), courseColumnIndex + shiftValue).SetCellValue(curriculumItem.Course);

                    foreach (var group in groups)
                    {
                        var groupCell = GetCell(sheet.GetRow(rowIndex), groupColumnIndex + shiftValue);
                        if (groupCell.StringCellValue == null)
                        {
                            groupCell.SetCellValue(group.Name + "; ");
                        }
                        else
                        {
                            try
                            {
                                groupCell.SetCellValue(groupCell.StringCellValue + group.Name + "; ");
                            }
                            catch (Exception) { }
                        }

                        var budjetCell = GetCell(sheet.GetRow(rowIndex), budjetColumnIndex + shiftValue);
                        try
                        {
                            budjetCell.SetCellValue(budjetCell.NumericCellValue + group.BudgetNumber);
                        }
                        catch (Exception) { }

                        var contractCell = GetCell(sheet.GetRow(rowIndex), contractColumnIndex + shiftValue);
                        try
                        {
                            contractCell.SetCellValue(contractCell.NumericCellValue + group.ContractNumber);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private int PrintSubjects(NPOI.SS.UserModel.ISheet sheet, Dictionary<string, List<TAItem>> TAItemsSubjects, int subjectCounter, int startIndex)
        {
            foreach (var subjectItem in TAItemsSubjects)
            {
                if (subjectItem.Value.Count <= 0)
                {
                    continue;
                }

                if (m_specialWorks.Contains(subjectItem.Key))
                {
                    continue;
                }

                var curriculumItem = m_curriculumItemRepository.GetCurriculumItem((m_workRepository.GetWork(subjectItem.Value[0].WorkId)).CurriculumItemId);
                var rowIndex = startIndex + subjectCounter;

                GetCell(sheet.GetRow(rowIndex), 22).SetCellValue(subjectItem.Key);
                GetCell(sheet.GetRow(rowIndex), 23).SetCellValue(curriculumItem.SubjectHours);
                GetCell(sheet.GetRow(rowIndex), 25).SetCellValue("ТЕФ");
                GetCell(sheet.GetRow(rowIndex), 26).SetCellValue(curriculumItem.Course);

                var groups = GetGroups(subjectItem.Value);
                uint budjetNumber = 0;
                uint contractNumber = 0;
                string groupsCellText = "";

                foreach (var group in groups)
                {
                    groupsCellText += group.Name + "; ";
                    budjetNumber += group.BudgetNumber;
                    contractNumber += group.ContractNumber;
                }

                GetCell(sheet.GetRow(rowIndex), 24).SetCellValue(groups.Count);
                GetCell(sheet.GetRow(rowIndex), 33).SetCellValue(groupsCellText);
                GetCell(sheet.GetRow(rowIndex), 34).SetCellValue(budjetNumber);
                GetCell(sheet.GetRow(rowIndex), 35).SetCellValue(contractNumber);

                foreach (var taItem in subjectItem.Value)
                {
                    var work = m_workRepository.GetWork(taItem.WorkId);
                    var workType = m_workTypeRepository.GetWorkType(work.WorkTypeId);
                    var curriculumItemOfWork = m_curriculumItemRepository.GetCurriculumItem(work.CurriculumItemId);
                    var columnIndex = GetColumnIndexByWorkName(workType.Name);

                    if (columnIndex < 0)
                    {
                        continue;
                    }

                    if (curriculumItemOfWork.EducType == "Контракт")
                    {
                        columnIndex += 2;
                    }

                    GetCell(sheet.GetRow(rowIndex), columnIndex).SetCellValue(taItem.WorkHours);
                }

                ++subjectCounter;
            }

            return subjectCounter;
        }

        private void GenerateIndividualPlan(Teacher teacher)
        {
            var allTAItems = m_taItemRepository.GetTAItemsByTeacherIdWithouFilters(teacher.Id, 0);
            var firstSemesterTAItemsSubjectsFull = new Dictionary<string, List<TAItem>>();
            var firstSemesterTAItemsSubjectsExternal = new Dictionary<string, List<TAItem>>();
            var firstSemesterTAItemsSubjectsEvening = new Dictionary<string, List<TAItem>>();
            var secondSemesterTAItemsSubjectsFull = new Dictionary<string, List<TAItem>>();
            var secondSemesterTAItemsSubjectsExternal = new Dictionary<string, List<TAItem>>();
            var secondSemesterTAItemsSubjectsEvening = new Dictionary<string, List<TAItem>>();

            allTAItems.RemoveAll(item => { return Tools.isEqual(item.WorkHours, 0); });

            if (allTAItems.Count <= 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var taItem in allTAItems)
            {
                Dictionary<string, List<TAItem>> resultDic = null;   
                
                var work = m_workRepository.GetWork(taItem.WorkId);
                if (work == null)
                {
                    continue;
                }

                var curriculumItem = m_curriculumItemRepository.GetCurriculumItem(work.CurriculumItemId);
                if (curriculumItem == null)
                {
                    continue;
                }
                
                var subject = m_subjectRepository.GetSubject(curriculumItem.SubjectId);
                if (subject == null)
                {
                    continue;
                }

                if (curriculumItem.Semestr == 1)
                {
                    if (curriculumItem.EducForm == "Денна")
                    {
                        resultDic = firstSemesterTAItemsSubjectsFull;
                    }
                    else if (curriculumItem.EducForm == "Заочна")
                    {
                        resultDic = firstSemesterTAItemsSubjectsExternal;
                    }
                    else
                    {
                        resultDic = firstSemesterTAItemsSubjectsEvening;
                    }
                }
                else 
                {
                    if (curriculumItem.EducForm == "Денна")
                    {
                        resultDic = secondSemesterTAItemsSubjectsFull;
                    }
                    else if (curriculumItem.EducForm == "Заочна")
                    {
                        resultDic = secondSemesterTAItemsSubjectsExternal;
                    }
                    else
                    {
                        resultDic = secondSemesterTAItemsSubjectsEvening;
                    }
                }

                if (!resultDic.ContainsKey(subject.Name))
                {
                    resultDic[subject.Name] = new List<TAItem>();
                }

                resultDic[subject.Name].Add(taItem);
            }

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
                templatePath += "\\templates\\IndividualPlan.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(30).Cells[0].SetCellValue(teacher.Name);
                sheet.GetRow(34).Cells[0].SetCellValue(teacher.Rank);
                sheet.GetRow(39).Cells[0].SetCellValue(teacher.Position);

                int firstSemesterCounter = 0;
                int secondSemesterCounter = 0;

                firstSemesterCounter = PrintSubjects(sheet, firstSemesterTAItemsSubjectsFull, firstSemesterCounter, 15);
                firstSemesterCounter = PrintSubjects(sheet, firstSemesterTAItemsSubjectsExternal, firstSemesterCounter, 15);
                firstSemesterCounter = PrintSubjects(sheet, firstSemesterTAItemsSubjectsEvening, firstSemesterCounter, 15);

                secondSemesterCounter = PrintSubjects(sheet, secondSemesterTAItemsSubjectsFull, secondSemesterCounter, 35);
                secondSemesterCounter = PrintSubjects(sheet, secondSemesterTAItemsSubjectsExternal, secondSemesterCounter, 35);
                secondSemesterCounter = PrintSubjects(sheet, secondSemesterTAItemsSubjectsEvening, secondSemesterCounter, 35);

                PrintOtherSubjects(sheet, firstSemesterTAItemsSubjectsFull);
                PrintOtherSubjects(sheet, firstSemesterTAItemsSubjectsExternal);
                PrintOtherSubjects(sheet, firstSemesterTAItemsSubjectsEvening);

                PrintOtherSubjects(sheet, secondSemesterTAItemsSubjectsFull);
                PrintOtherSubjects(sheet, secondSemesterTAItemsSubjectsExternal);
                PrintOtherSubjects(sheet, secondSemesterTAItemsSubjectsEvening);

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
