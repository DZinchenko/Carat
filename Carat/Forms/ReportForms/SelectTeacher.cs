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
    public enum ByTeacherReportType
    {
        Default,
        Extended,
        Individual
    }

    public partial class SelectTeacher : Form, IFiltersChangable
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
        IPositionRepository m_positionRepository;
        IRankRepository m_rankRepository;
        IFacultyRepository m_facultyRepository;

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
            m_positionRepository = new PositionRepository(m_dbPath);
            m_rankRepository = new RankRepository(m_dbPath);
            m_facultyRepository = new FacultyRepository(m_dbPath);

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
            var teachers = m_teacherRepository.GetAllTeachers(a => a.Name);

            dataGridViewSelectTeacher.Rows.Clear();
            foreach (var teacher in teachers)
            {
                dataGridViewSelectTeacher.Rows.Add(teacher.Name);
            }
        }

        private void dataGridViewSelectTeacher_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var teachers = m_teacherRepository.GetAllTeachers(a => a.Name);

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
            allCurriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);

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

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(groupedTAItemsWithCurriculumItem.Count, "Extended by teacher report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
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
                    var groupsList = new List<Group>();
                    var otherTeachers = new Dictionary<int, Teacher>();
                    var otherTeachersGroups = new Dictionary<int, List<Group>>();
                    var otherTAItems = new List<TAItem>();

                    newRow.Cells[0].SetCellValue((numberCounter + 1).ToString());
                    newRow.Cells[4].SetCellValue(curriculumItem.EducLevel);
                    newRow.Cells[5].SetCellValue(GetEducFormString(curriculumItem));
                    newRow.Cells[6].SetCellValue(curriculumItem.Course);

                    foreach (var taItem in taItems)
                    {
                        var groupsToTAItems = m_groupsToTAItemRepository.GetGroupsToTAItemsByTAItemId(taItem.Id);
                        var work = m_workRepository.GetWork(taItem.WorkId);
                        var currItem = m_curriculumItemRepository.GetCurriculumItem(work.CurriculumItemId);
                        var otherWorks = m_workRepository.GetWorks(currItem.Id, true);

                        foreach (var otherWork in otherWorks)
                        {
                            otherTAItems.AddRange(m_taItemRepository.GetTAItems(otherWork.Id));
                        }

                        var groups = m_groupRepository.GetGroups(groupsToTAItems.Select(item => item.GroupId).ToList()).OrderBy(g => g.Name);

                        foreach (var group in groups)
                        {
                            if (!groupsList.Any(g => g.Id == group.Id))
                            {
                                groupsList.Add(group);
                            }
                        }

                        foreach (var otherTAItem in otherTAItems)
                        {
                            if (otherTAItem.Id != taItem.Id && otherTAItem.TeacherId != taItem.TeacherId)
                            {
                                otherTeachers[otherTAItem.TeacherId] = m_teacherRepository.GetTeacher(otherTAItem.TeacherId);
                            }
                        }
                    }

                    string firstCellText = m_subjectRepository.GetSubject(curriculumItem.SubjectId)?.Name;
                    newRow.Cells[1].SetCellValue(firstCellText);


                    string secondCellText = "";
                    foreach (var group in groupsList.OrderBy(g => g.Name).ToList())
                    {
                        secondCellText += group.Name + "; ";
                    }
                    newRow.Cells[2].SetCellValue(secondCellText);

                    foreach (var otherTeacher in otherTeachers)
                    {
                        var newRowForOtherTeacher = sheet.CopyRow(8, sheet.LastRowNum);
                        newRowForOtherTeacher.Cells[3].SetCellValue(otherTeacher.Value.Name);
                        var TAItemsForOtherTeacher = new List<TAItem>();

                        foreach (var otherTAItem in otherTAItems)
                        {
                            if (otherTeacher.Key == otherTAItem.TeacherId && !Tools.isEqual(0, otherTAItem.WorkHours))
                                TAItemsForOtherTeacher.Add(otherTAItem);
                        }

                        var otherTeacherGroups = GetGroups(TAItemsForOtherTeacher);

                        foreach (var otherTeacherGroup in otherTeacherGroups)
                        {
                            var cell = GetCell(newRowForOtherTeacher, 2);
                            cell.SetCellValue(cell.StringCellValue + otherTeacherGroup.Name + "; ");
                        }

                        foreach (var taItem in TAItemsForOtherTeacher)
                        {
                            var work = m_workRepository.GetWork(taItem.WorkId);
                            newRowForOtherTeacher.Cells[7 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours);
                        }
                    }

                    foreach (var taItem in taItems)
                    {
                        var work = m_workRepository.GetWork(taItem.WorkId);
                        newRow.Cells[7 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours);
                    }

                    newRow.Height = -1;

                    ++numberCounter;
                    m_parentForm.IncrementProgress();
                }

                sheet.ShiftRows(9, sheet.LastRowNum, -1);

                for (int i = 7; i < 42; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(sheet.LastRowNum - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(sheet.LastRowNum).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = sheet.LastRowNum; i < lastIndex; ++i)
                {
                    var firstCell = sheet.GetRow(i).Cells[7].Address;
                    var lastCell = sheet.GetRow(i).Cells[41].Address;
                    var finalCell = sheet.GetRow(i).Cells[42];

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

            m_parentForm.Enabled = true;
            m_parentForm.HideProgress();
        }

        private void GenerateReport(Teacher teacher)
        {
            var allCurriculumItems = new List<CurriculumItem>();
            var allWorks = new List<Work>();
            var allTAItem = new List<TAItem>();
            var groupedTAItemsWithCurriculumItem = new Dictionary<int, List<TAItem>>();
            allCurriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);

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

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(groupedTAItemsWithCurriculumItem.Count, "By teacher report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\DefaultByTeacher.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue("Викладач: " + teacher.Name + ", " + GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                int numberCounter = 0;
                CurriculumItem lastCurItem = null;
                foreach (var curriculumItemWithTaItems in groupedTAItemsWithCurriculumItem)
                {
                    var curriculumItem = m_curriculumItemRepository.GetCurriculumItem(curriculumItemWithTaItems.Key);
                    var taItems = curriculumItemWithTaItems.Value;
                    var groupsList = new List<Group>();

                    foreach (var taItem in taItems)
                    {
                        var groupsToTAItems = m_groupsToTAItemRepository.GetGroupsToTAItemsByTAItemId(taItem.Id);
                        var groups = m_groupRepository.GetGroups(groupsToTAItems.Select(item => item.GroupId).ToList());
                        foreach (var group in groups)
                        {
                            if (!groupsList.Any(g => g.Id == group.Id))
                            {
                                groupsList.Add(group);
                            }
                        }
                    }

                    if (lastCurItem != null && curriculumItem.SubjectId == lastCurItem.SubjectId
                                            && curriculumItem.EducForm == lastCurItem.EducForm
                                            && curriculumItem.EducLevel == lastCurItem.EducLevel
                                            && curriculumItem.EducType == lastCurItem.EducType
                                            && curriculumItem.Course == lastCurItem.Course)
                    {
                        var row = sheet.GetRow(sheet.LastRowNum - 1);

                        var lastGroupsCellValue = row.Cells[2].StringCellValue;
                        foreach (var group in groupsList.OrderBy(g => g.Name).ToList())
                        {
                            if (!lastGroupsCellValue.Contains(group.Name))
                            {
                                lastGroupsCellValue += group.Name + "; ";
                            }
                        }
                        row.Cells[2].SetCellValue(lastGroupsCellValue);

                        foreach (var taItem in taItems)
                        {
                            var work = m_workRepository.GetWork(taItem.WorkId);
                            var lastHourCellVal = row.Cells[6 + work.WorkTypeId - 1].NumericCellValue;
                            row.Cells[6 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours + lastHourCellVal);
                        }

                        continue;
                    }

                    string secondCellText = "";
                    foreach (var group in groupsList.OrderBy(g => g.Name).ToList())
                    {
                        secondCellText += group.Name + "; ";
                    }

                    var newRow = sheet.CopyRow(8, sheet.LastRowNum);

                    newRow.Cells[0].SetCellValue((numberCounter + 1).ToString());
                    newRow.Cells[1].SetCellValue(m_subjectRepository.GetSubject(curriculumItem.SubjectId)?.Name);
                    newRow.Cells[2].SetCellValue(secondCellText);
                    newRow.Cells[3].SetCellValue(curriculumItem.EducLevel);
                    newRow.Cells[4].SetCellValue(GetEducFormString(curriculumItem));
                    newRow.Cells[5].SetCellValue(curriculumItem.Course);

                    foreach (var taItem in taItems)
                    {
                        var work = m_workRepository.GetWork(taItem.WorkId);
                        newRow.Cells[6 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours);
                    }

                    newRow.Height = -1;

                    lastCurItem = curriculumItem;
                    ++numberCounter;
                    m_parentForm.IncrementProgress();
                }

                sheet.ShiftRows(9, sheet.LastRowNum, -1);

                for (int i = 6; i <= 41; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(sheet.LastRowNum - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(sheet.LastRowNum).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = sheet.LastRowNum; i < lastIndex; ++i)
                {
                    var firstCell = sheet.GetRow(i).Cells[6].Address;
                    var lastCell = sheet.GetRow(i).Cells[40].Address;
                    var finalCell = sheet.GetRow(i).Cells[41];

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

            m_parentForm.Enabled = true;
            m_parentForm.HideProgress();
        }

        private List<Group> GetGroups(List<TAItem> taItems)
        {
            var result = new Dictionary<int, Group>();
            var groupIds = new List<int>();
            foreach (var taItem in taItems)
            {
                groupIds.AddRange(m_groupsToTAItemRepository.GetGroupsToTAItemsByTAItemId(taItem.Id).Select(item => item.GroupId).ToList());
            }
            return m_groupRepository.GetGroups(groupIds);
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
                case "Індивід заняття за змішаною формою навч.":
                    result = 50;
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
                    result = 74;
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
            }

            return result;
        }

        private bool IsDivisibleWorkType(int workType)
        {
            return workType == 13 || workType == 14 || workType == 15
                || workType == 16 || workType == 17 || workType == 18
                || workType == 19 || workType == 20 || workType == 21 || workType == 34;
        }

        private Dictionary<string, int> workNameRowIndexPairs = new Dictionary<string, int> {
            {"Індивід заняття з магістрами", 7},
            {"Індивід заняття зі студентами", 6},
            {"Керівництво практиками", 9},
            {"Керівниц.атестац.роб.(бакалаврів)", 13},
            {"Керівниц.атестац.роб.(магістр ОПП)", 14},
            {"Керівниц.атестац.роб.(магістр ОНП)", 15},
            {"Консульт.атестац.роб.(бакалаврів)", 16},
            {"Консульт.атестац.роб.(магістр ОПП)", 17},
            {"Консульт.атестац.роб.(магістр ОНП)", 18},
            {"Рецензув.атестац.роб.(бакалаврів)" ,19},
            {"Рецензув.атестац.роб.(магістр ОПП)", 20},
            {"Рецензув.атестац.роб.(магістр ОНП)", 21},
            {"Вступний іспит (магістр ОПП)", 22},
            {"Вступний іспит (магістр ОНП)", 23},
            {"Вступний іспит (аспірант)", 24},
            {"Робота в ЕК (бакалаврів)", 25},
            {"Робота в ЕК (магістр ОПП)", 28},
            {"Робота в ЕК (магістр ОНП)", 31},
            {"Керівництво (аспірантами)", 34},
            {"Керівництво (здобувач., стаж.)", 35},
            {"Консульт.докторантів", 36},
            {"Заняття з аспірантами", -1}
        };

        private int GetRowIndexByOtherWorkName(string workName)
        {
            int result = -1;
            workNameRowIndexPairs.TryGetValue(workName, out result);
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
                    int hoursCellIndexBC = hoursColumnIndex;

                    if (curriculumItem.Semestr == 2)
                    {
                        shiftValue = 10;
                    }

                    if (rowIndex < 0)
                    {
                        continue;
                    }

                    if (curriculumItem.EducType == "Контракт")
                    {
                        hoursCellIndexBC += 2;
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
                        else if (subjectItem.Key == "Аспіранти")
                        {
                            rowIndex += 3;
                        }
                    }

                    var cell = GetCell(sheet.GetRow(rowIndex), hoursCellIndexBC + shiftValue);
                    try
                    {
                        cell.SetCellValue(cell.NumericCellValue + taItem.WorkHours);
                    }
                    catch (Exception) { }

                    var groups = GetGroups(new List<TAItem> { taItem }).Distinct().OrderBy(g => g.Name).ToList();

                    bool isGroupsNotExist = groups.Count == 0;

                    if (!isGroupsNotExist)
                    {
                        GetCell(sheet.GetRow(rowIndex), facultyColumnIndex + shiftValue).SetCellValue(m_facultyRepository.GetFaculty(groups.First().FacultyId).Name);
                    }
                    GetCell(sheet.GetRow(rowIndex), courseColumnIndex + shiftValue).SetCellValue(curriculumItem.Course);

                    foreach (var group in groups)
                    {
                        var groupCell = GetCell(sheet.GetRow(rowIndex), groupColumnIndex + shiftValue);
                        if (groupCell.StringCellValue.Contains(group.Name + ";"))
                        {
                            continue;
                        }

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
                            if (IsDivisibleWorkType(rowIndex))
                            {
                                if (curriculumItem.EducType == "Бюджет")
                                    budjetCell.SetCellValue(Math.Round(cell.NumericCellValue / workType.StudentHours));
                            }
                            else
                            {
                                budjetCell.SetCellValue(budjetCell.NumericCellValue + group.BudgetNumber);
                            }
                        }
                        catch (Exception) { }

                        var contractCell = GetCell(sheet.GetRow(rowIndex), contractColumnIndex + shiftValue);
                        try
                        {
                            if (IsDivisibleWorkType(rowIndex))
                            {
                                if (curriculumItem.EducType == "Контракт")
                                    contractCell.SetCellValue(Math.Round(cell.NumericCellValue / workType.StudentHours));
                            }
                            else
                            {
                                contractCell.SetCellValue(contractCell.NumericCellValue + group.ContractNumber);
                            }
                        }
                        catch (Exception) { }
                    }

                    if (isGroupsNotExist)
                    {
                        var row = sheet.GetRow(rowIndex);
                        try
                        {
                            if (IsDivisibleWorkType(rowIndex) && (curriculumItem.EducType == "Бюджет"))
                            {
                                var budjetCell = GetCell(row, budjetColumnIndex + shiftValue);
                                var value = Math.Round(cell.NumericCellValue / workType.StudentHours);
                                budjetCell.SetCellValue(value);
                            }
                        }
                        catch (Exception) { }

                        try
                        {
                            if (IsDivisibleWorkType(rowIndex) && (curriculumItem.EducType == "Контракт"))
                            {
                                var contractCell = GetCell(row, contractColumnIndex + shiftValue);
                                var value = Math.Round(cell.NumericCellValue / workType.StudentHours);
                                contractCell.SetCellValue(value);
                            }
                        }
                        catch (Exception) { }
                    }


                    var thisBudjetCell = GetCell(sheet.GetRow(rowIndex), budjetColumnIndex + shiftValue);
                    var thisContractCell = GetCell(sheet.GetRow(rowIndex), contractColumnIndex + shiftValue);
                    if (rowIndex >= 9 && rowIndex <= 11)
                    {
                        var attestationBudgetCell = GetCell(sheet.GetRow(rowIndex + 4), budjetColumnIndex + shiftValue);
                        if (attestationBudgetCell.NumericCellValue != thisBudjetCell.NumericCellValue)
                        {
                            thisBudjetCell.SetCellValue(attestationBudgetCell.NumericCellValue);
                        }

                        var attestationContractCell = GetCell(sheet.GetRow(rowIndex + 4), contractColumnIndex + shiftValue);
                        if (attestationContractCell.NumericCellValue != thisContractCell.NumericCellValue)
                        {
                            thisContractCell.SetCellValue(attestationContractCell.NumericCellValue);
                        }
                    }
                    else if (rowIndex >= 13 && rowIndex <= 15)
                    {
                        var practiceBudgetCell = GetCell(sheet.GetRow(rowIndex - 4), budjetColumnIndex + shiftValue);
                        if (practiceBudgetCell.NumericCellValue != thisBudjetCell.NumericCellValue)
                        {
                            practiceBudgetCell.SetCellValue(thisBudjetCell.NumericCellValue);
                        }

                        var practiceContractCell = GetCell(sheet.GetRow(rowIndex - 4), contractColumnIndex + shiftValue);
                        if (practiceContractCell.NumericCellValue != thisContractCell.NumericCellValue)
                        {
                            practiceContractCell.SetCellValue(thisContractCell.NumericCellValue);
                        }
                    }

                    m_parentForm.IncrementProgress();
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

                var taItemsCourseGrouped = subjectItem.Value.GroupBy(x => x.Course);
                foreach (var taItemCourseGroup in taItemsCourseGrouped)
                {
                    var taItems = taItemCourseGroup.ToList();
                    var curriculumItem = m_curriculumItemRepository.GetCurriculumItem((m_workRepository.GetWork(taItems[0].WorkId)).CurriculumItemId);
                    var rowIndex = startIndex + subjectCounter;

                    GetCell(sheet.GetRow(rowIndex), 22).SetCellValue(subjectItem.Key);
                    GetCell(sheet.GetRow(rowIndex), 23).SetCellValue(curriculumItem.SubjectHours);
                    GetCell(sheet.GetRow(rowIndex), 26).SetCellValue(curriculumItem.Course);

                    var groups = GetGroups(taItems).OrderBy(g => g.Name).ToList();
                    uint budjetNumber = 0;
                    uint contractNumber = 0;
                    string groupsCellText = "";

                    if (groups.Count != 0)
                    {
                        GetCell(sheet.GetRow(rowIndex), 25).SetCellValue(m_facultyRepository.GetFaculty(groups.FirstOrDefault().FacultyId).Name);
                    }

                    bool gotStudNumbersFromWork = false;

                    var taItemsGrouped = taItems.GroupBy(x => (Course: x.Course, EducForm: x.EducForm, EducLevel: x.EducLevel));
                    List<Group> allGropsForSubject = new List<Group>();
                    foreach (var taItemGroup in taItemsGrouped)
                    {
                        var key = taItemGroup.Key;
                        allGropsForSubject.AddRange(m_groupRepository.GetGroups(key.Course, key.EducForm, key.EducLevel));
                    }

                    if (curriculumItem.BudgetStudCnt != null && curriculumItem.ContractStudCnt != null && groups.Count == allGropsForSubject.Count)
                    {
                        budjetNumber = (uint)curriculumItem.BudgetStudCnt;
                        contractNumber = (uint)curriculumItem.ContractStudCnt;
                        gotStudNumbersFromWork = true;
                    }

                    foreach (var group in groups)
                    {
                        groupsCellText += group.Name + "; ";
                        if (!gotStudNumbersFromWork)
                        {
                            budjetNumber += group.BudgetNumber;
                            contractNumber += group.ContractNumber;
                        }
                    }

                    GetCell(sheet.GetRow(rowIndex), 24).SetCellValue(groups.Count);
                    GetCell(sheet.GetRow(rowIndex), 33).SetCellValue(groupsCellText);
                    GetCell(sheet.GetRow(rowIndex), 34).SetCellValue(budjetNumber);
                    GetCell(sheet.GetRow(rowIndex), 35).SetCellValue(contractNumber);

                    foreach (var taItem in taItems)
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
                    m_parentForm.IncrementProgress();
                }
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

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(firstSemesterTAItemsSubjectsFull.Count
                + firstSemesterTAItemsSubjectsExternal.Count
                + firstSemesterTAItemsSubjectsEvening.Count
                + secondSemesterTAItemsSubjectsFull.Count
                + secondSemesterTAItemsSubjectsExternal.Count
                + secondSemesterTAItemsSubjectsEvening.Count,
                "Individual report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\IndividualPlan.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                var rankDegreeValue = (teacher.Degree != "-" ? teacher.Degree + ", " : "") +  m_rankRepository.GetRank(teacher.RankId).Name;

                sheet.GetRow(30).Cells[0].SetCellValue(teacher.Name);
                sheet.GetRow(34).Cells[0].SetCellValue(rankDegreeValue);
                sheet.GetRow(39).Cells[0].SetCellValue(m_positionRepository.GetPosition(teacher.PositionId).Name);

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

            m_parentForm.Enabled = true;
            m_parentForm.HideProgress();
        }
    }
}
