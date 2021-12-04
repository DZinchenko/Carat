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
using Carat.Interfaces;
using Carat.Data.Entities;
using Carat.EF.Repositories;
using NPOI.XSSF.UserModel;

namespace Carat
{
    public partial class ExcelReportsForm : Form
    {
        private string m_dbPath;
        private string m_educType;
        private string m_educForm;
        private string m_educLevel;
        private uint m_course;
        private uint m_semestr;

        private MainForm m_parentForm;
        private Form m_loadedSelectForm = null;

        private ICurriculumItemRepository m_curriculumItemRepository;
        private ISubjectRepository m_subjectRepository;
        private IWorkRepository m_workRepository;
        private ITeacherRepository m_teacherRepository;
        private ITAItemRepository m_taItemRepository;
        private IWorkTypeRepository m_workTypeRepository;
        private IGroupRepository m_groupRepository;
        private IGroupsToTAItemRepository m_groupsToTAItemRepository;
        private IReportDTORepository m_reportDTORepository;
        private IPositionRepository m_positionRepository;

        private string IncorrectNameMessageDataIsEmpty = "Дані відсутні!";

        public ExcelReportsForm(MainForm parentForm,
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

            m_curriculumItemRepository = new CurriculumItemRepository(dbPath);
            m_subjectRepository = new SubjectRepository(dbPath);
            m_workRepository = new WorkRepository(dbPath);
            m_teacherRepository = new TeacherRepository(dbPath);
            m_taItemRepository = new TAItemRepository(dbPath);
            m_workTypeRepository = new WorkTypeRepository(dbPath);
            m_groupRepository = new GroupRepository(dbPath);
            m_groupsToTAItemRepository = new GroupsToTAItemRepository(dbPath);
            m_reportDTORepository = new ReportDTORepository(dbPath);
            m_positionRepository = new PositionRepository(dbPath);

            treeViewExcelReports.ExpandAll();
            panelContainer.Visible = false;
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

        private void GeneratePlannedBySubjects()
        {
            var curriculumItems = new List<CurriculumItem>();

            curriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(
                a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);

            curriculumItems.RemoveAll(curriculumItem =>
            {
                var curriculumWorks = m_workRepository.GetWorks(curriculumItem.Id, false);
                return curriculumWorks.TrueForAll(work => { return Tools.isEqual(work.TotalHours, 0); });
            });

            if (curriculumItems.Count == 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(curriculumItems.Count, "Planned by subjects report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\PlannedBySubjects.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue(GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                for (int i = 0; i < curriculumItems.Count;)
                {
                    var newRow = sheet.GetRow(8 + i);
                    var curriculumItem = curriculumItems[i];
                    var curriculumWorks = m_workRepository.GetWorks(curriculumItem.Id, false);

                    if (
                        curriculumWorks.Count <= 0)
                    {
                        ++i;
                        continue;
                    }

                    newRow.Cells[0].SetCellValue((i + 1).ToString());
                    newRow.Cells[1].SetCellValue(m_subjectRepository.GetSubject(curriculumItem.SubjectId)?.Name);
                    newRow.Cells[2].SetCellValue(curriculumItem.EducLevel);
                    newRow.Cells[3].SetCellValue(GetEducFormString(curriculumItem));
                    newRow.Cells[4].SetCellValue(curriculumItem.Course);

                    for (int cellIndex = 5, workTypeIndex = 0; cellIndex <= 39; ++cellIndex, ++workTypeIndex)
                    {
                        newRow.Cells[cellIndex].SetCellValue(curriculumWorks[workTypeIndex].TotalHours);
                    }

                    newRow.Height = -1;

                    ++i;

                    if (i < curriculumItems.Count)
                    {
                        sheet.CopyRow(8, 8 + i);
                    }

                    m_parentForm.IncrementProgress();
                }

                for (int i = 5; i < 40; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(8 + curriculumItems.Count - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(8 + curriculumItems.Count).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                    m_parentForm.IncrementProgress();
                }

                for (int i = 8, lastIndex = curriculumItems.Count + 8; i <= lastIndex; ++i)
                {
                    var firstCell = sheet.GetRow(i).Cells[5].Address;
                    var lastCell = sheet.GetRow(i).Cells[39].Address;
                    var finalCell = sheet.GetRow(i).Cells[40];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                    m_parentForm.IncrementProgress();
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
            m_parentForm.HideProgress();
            m_parentForm.Enabled = true;
        }

        private void GenerateDistributedBySubjects()
        {
            var curriculumItems = new List<CurriculumItem>();

            curriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(
                a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);

            curriculumItems.RemoveAll(curriculumItem =>
            {
                var curriculumWorks = m_workRepository.GetWorks(curriculumItem.Id, false);
                return curriculumWorks.TrueForAll(work =>
                {
                    var taItems = m_taItemRepository.GetTAItems(work.Id);
                    double distributedHours = 0;

                    foreach (var taItem in taItems)
                    {
                        distributedHours += taItem.WorkHours;
                    }

                    return Tools.isEqual(work.TotalHours, 0) || Tools.isEqual(0, distributedHours);
                });
            });

            if (curriculumItems.Count == 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(curriculumItems.Count, "Distributed by subjects report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\DistributedBySubjects.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue(GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                for (int i = 0; i < curriculumItems.Count;)
                {
                    var newRow = sheet.GetRow(8 + i);
                    var curriculumItem = curriculumItems[i];
                    var curriculumWorks = m_workRepository.GetWorks(curriculumItem.Id, false);

                    if (
                        curriculumWorks.Count <= 0)
                    {
                        ++i;
                        continue;
                    }

                    newRow.Cells[0].SetCellValue((i + 1).ToString());
                    newRow.Cells[1].SetCellValue(m_subjectRepository.GetSubject(curriculumItem.SubjectId)?.Name);
                    newRow.Cells[2].SetCellValue(curriculumItem.EducLevel);
                    newRow.Cells[3].SetCellValue(GetEducFormString(curriculumItem));
                    newRow.Cells[4].SetCellValue(curriculumItem.Course);

                    for (int cellIndex = 5, workTypeIndex = 0; cellIndex <= 39; ++cellIndex, ++workTypeIndex)
                    {
                        var taItems = m_taItemRepository.GetTAItems(curriculumWorks[workTypeIndex].Id);
                        double distributedHours = 0;

                        foreach (var taItem in taItems)
                        {
                            distributedHours += taItem.WorkHours;
                        }

                        newRow.Cells[cellIndex].SetCellValue(distributedHours);
                    }

                    newRow.Height = -1;

                    ++i;

                    if (i < curriculumItems.Count)
                    {
                        sheet.CopyRow(8, 8 + i);
                    }

                    m_parentForm.IncrementProgress();
                }

                for (int i = 5; i < 40; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(8 + curriculumItems.Count - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(8 + curriculumItems.Count).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = curriculumItems.Count + 8; i <= lastIndex; ++i)
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

            m_parentForm.HideProgress();
            m_parentForm.Enabled = true;
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


        private void GenerateNotDistributedBySubjects()
        {
            var parsedCurriculumItems = new List<CurriculumItem>();
            parsedCurriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(
                a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            var total = 0.0;

            parsedCurriculumItems.RemoveAll(curriculumItem =>
            {
                var curriculumWorks = m_workRepository.GetWorks(curriculumItem.Id, true);

                return !curriculumWorks.Any(work =>
                {
                    var taItems = m_taItemRepository.GetTAItems(work.Id);
                    double distributedHours = 0.0;
                    double notDistributedHours = 0.0;

                    foreach (var taItem in taItems)
                    {
                        distributedHours += taItem.WorkHours;
                    }

                    notDistributedHours = work.TotalHours - distributedHours;
                    total += notDistributedHours;
                    return !(notDistributedHours < 0.01);
                });
            });

            if (parsedCurriculumItems.Count == 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var inputSubjects = new Dictionary<string, List<int>>();

            foreach (var item in parsedCurriculumItems)
            {
                var subject = m_subjectRepository.GetSubject(item.SubjectId);
                var key = subject.Name + item.EducLevel + item.Course;
                if (!inputSubjects.ContainsKey(key))
                {
                    inputSubjects[key] = new List<int>();
                }

                if (!inputSubjects[key].Contains(item.Id))
                {
                    inputSubjects[key].Add(item.Id);
                }
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(parsedCurriculumItems.Count, "Not distributed by subjects report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\NotDistributedBySubjects.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue(GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                int rowCounter = 0;
                foreach (var currSubject in inputSubjects)
                {
                    var newRow = sheet.CopyRow(8, sheet.LastRowNum);
                    var curriculumItems = new List<CurriculumItem>();

                    foreach (var currId in currSubject.Value)
                    {
                        curriculumItems.Add(m_curriculumItemRepository.GetCurriculumItem(currId));
                    }

                    if (curriculumItems.Count <= 0)
                    {
                        ++rowCounter;
                        continue;
                    }

                    newRow.Cells[0].SetCellValue((rowCounter + 1).ToString());
                    newRow.Cells[1].SetCellValue(m_subjectRepository.GetSubject(curriculumItems[0].SubjectId).Name);
                    newRow.Cells[2].SetCellValue(curriculumItems[0].EducLevel);
                    newRow.Cells[3].SetCellValue(GetEducFormString(curriculumItems[0]));
                    newRow.Cells[4].SetCellValue(curriculumItems[0].Course);

                    foreach (var curItem in curriculumItems)
                    {
                        var curItemWorks = m_workRepository.GetWorks(curItem.Id, false);
                        for (int cellIndex = 5, workTypeIndex = 0; cellIndex <= 39; ++cellIndex, ++workTypeIndex)
                        {
                            var taItems = m_taItemRepository.GetTAItems(curItemWorks[workTypeIndex].Id);

                            double distributedHours = 0;

                            foreach (var taItem in taItems)
                            {
                                distributedHours += taItem.WorkHours;
                            }

                            var resultCell = GetCell(newRow, cellIndex);
                            try
                            {
                                resultCell.SetCellValue(resultCell.NumericCellValue + (curItemWorks[workTypeIndex].TotalHours - distributedHours));
                            }
                            catch (Exception) { }
                        }
                    }

                    newRow.Height = -1;

                    ++rowCounter;
                    m_parentForm.IncrementProgress();
                }

                sheet.ShiftRows(9, sheet.LastRowNum, -1);

                for (int i = 5; i < 41; ++i)
                {
                    var firstCell = GetCell(sheet.GetRow(8), i).Address;
                    var lastCell = GetCell(sheet.GetRow(sheet.LastRowNum - 1), i).Address;
                    var finalCell = GetCell(sheet.GetRow(sheet.LastRowNum), i);

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = sheet.LastRowNum; i < lastIndex; ++i)
                {
                    var firstCell = GetCell(sheet.GetRow(i), 5).Address;
                    var lastCell = GetCell(sheet.GetRow(i), 39).Address;
                    var finalCell = GetCell(sheet.GetRow(i), 40);

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

            m_parentForm.HideProgress();
            m_parentForm.Enabled = true;
        }

        private void GenerateLoadShortByTeachers()
        {
            var fullTeachers = m_teacherRepository.GetAllTeachers(a => a.Name);
            var teachers = new List<Teacher>();

            foreach (var teacher in fullTeachers)
            {
                var items = m_taItemRepository.GetTAItemsByTeacherId(teacher.Id, m_semestr, m_educType, "<всі>");

                if (!items.TrueForAll(item => { return Tools.isEqual(item.WorkHours, 0); }))
                {
                    teachers.Add(teacher);
                }
            }

            if (teachers.Count == 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(teachers.Count, "Short by teachers report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\ShortByTeachers.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(4).Cells[0].SetCellValue(GetSemesterString() + ", " + GetEducTypeString());

                int i = 0;
                var workTypes = m_workTypeRepository.GetAllWorkTypes();
                var positions = m_positionRepository.GetPositions();

                foreach (var teacher in teachers)
                {
                    var newRow = sheet.GetRow(8 + i);

                    newRow.Cells[0].SetCellValue(i + 1);
                    newRow.Cells[1].SetCellValue(teacher.Name);
                    newRow.Cells[2].SetCellValue(positions.First(p => p.Id == teacher.PositionId).Name);
                    newRow.Cells[3].SetCellValue(teacher.StaffUnit);
                    newRow.Cells[4].SetCellValue(GetOccupationFormString(teacher));

                    var taItems = m_taItemRepository.GetTAItemsByTeacherId(teacher.Id, m_semestr, m_educType, "Денна");
                    taItems.AddRange(m_taItemRepository.GetTAItemsByTeacherId(teacher.Id, m_semestr, m_educType, "Заочна"));
                    taItems.AddRange(m_taItemRepository.GetTAItemsByTeacherId(teacher.Id, m_semestr, m_educType, "Вечірня"));
                    double lectureHours = 0.00;
                    double otherAudi = 0.00;
                    double other = 0.00;

                    foreach (var taItem in taItems)
                    {
                        var work = m_workRepository.GetWork(taItem.WorkId);

                        if (work == null)
                            continue;

                        if (work.WorkTypeId == workTypes[0].Id)
                        {
                            lectureHours += taItem.WorkHours;
                        }
                        else if (work.WorkTypeId == workTypes[1].Id || work.WorkTypeId == workTypes[2].Id)
                        {
                            otherAudi += taItem.WorkHours;
                        }
                        else
                        {
                            other += taItem.WorkHours;
                        }
                    }

                    newRow.Cells[5].SetCellValue(lectureHours);
                    newRow.Cells[6].SetCellValue(otherAudi);
                    newRow.Cells[7].SetCellValue(other);

                    newRow.Height = -1;

                    ++i;

                    if (i < teachers.Count)
                    {
                        sheet.CopyRow(8, 8 + i);
                    }

                    m_parentForm.IncrementProgress();
                }

                for (int j = 5; j < 8; ++j)
                {
                    var firstCell = sheet.GetRow(8).Cells[j].Address;
                    var lastCell = sheet.GetRow(sheet.LastRowNum - 1).Cells[j].Address;
                    var finalCell = sheet.GetRow(sheet.LastRowNum).Cells[j];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int j = 8, lastIndex = sheet.LastRowNum; j <= lastIndex; ++j)
                {
                    var firstCell = sheet.GetRow(j).Cells[5].Address;
                    var lastCell = sheet.GetRow(j).Cells[7].Address;
                    var finalCell = sheet.GetRow(j).Cells[8];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                sheet.AutoSizeColumn(2);
                sheet.AutoSizeColumn(3);

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

            m_parentForm.HideProgress();
            m_parentForm.Enabled = true;
        }

        private string GetOccupationFormString(Teacher teacher)
        {
            var result = "ш";

            if (teacher.OccupForm == "Сумісник")
            {
                result = "с";
            }

            return result;
        }

        private void GenerateLoadFullByTeachers()
        {
            var fullTeachers = m_teacherRepository.GetAllTeachers(a => a.Name);
            var teachers = new List<Teacher>();

            foreach (var teacher in fullTeachers)
            {
                var items = m_taItemRepository.GetTAItemsByTeacherId(teacher.Id, m_semestr, m_educType, m_educForm);

                if (!items.TrueForAll(item => { return Tools.isEqual(item.WorkHours, 0); }))
                {
                    teachers.Add(teacher);
                }
            }

            if (teachers.Count == 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(teachers.Count, "Full by teachers report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\FullByTeachers.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue(GetSemesterString() + ", " + GetEducFormString() + ", " + GetEducTypeString());

                int rowCounter = 0;
                var workTypes = m_workTypeRepository.GetAllWorkTypes();
                var positions = m_positionRepository.GetPositions();

                foreach (var teacher in teachers)
                {
                    var newRow = sheet.GetRow(8 + rowCounter);

                    newRow.Cells[0].SetCellValue(rowCounter + 1);
                    newRow.Cells[1].SetCellValue(teacher.Name);
                    newRow.Cells[2].SetCellValue(positions.First(p => p.Id == teacher.PositionId).Name);
                    newRow.Cells[3].SetCellValue(teacher.StaffUnit);
                    newRow.Cells[4].SetCellValue(GetOccupationFormString(teacher));

                    var taItems = m_taItemRepository.GetTAItemsByTeacherId(teacher.Id, m_semestr, m_educType, m_educForm);
                    var totalWorksHours = new Dictionary<int, double>();

                    foreach (var workType in workTypes)
                    {
                        totalWorksHours.Add(workType.Id, 0);
                    }

                    foreach (var taItem in taItems)
                    {
                        var work = m_workRepository.GetWork(taItem.WorkId);

                        if (work == null)
                            continue;

                        if (work.WorkTypeId < totalWorksHours.Count)
                        {
                            totalWorksHours[work.WorkTypeId] += taItem.WorkHours;
                        }
                    }

                    for (int cellIndex = 5, workTypeIndex = 0; cellIndex <= 38; ++cellIndex, ++workTypeIndex)
                    {
                        newRow.Cells[cellIndex].SetCellValue(totalWorksHours[workTypes[workTypeIndex].Id]);
                    }

                    newRow.Height = -1;

                    ++rowCounter;

                    if (rowCounter < teachers.Count)
                    {
                        sheet.CopyRow(8, 8 + rowCounter);
                    }

                    m_parentForm.IncrementProgress();
                }

                for (int i = 5; i < 41; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(8 + rowCounter - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(8 + rowCounter).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = rowCounter + 8; i < lastIndex; ++i)
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

            m_parentForm.HideProgress();
            m_parentForm.Enabled = true;
        }

        private void GenerateSchedule()
        {
            var groups = new List<Group>();
            groups = m_groupRepository.GetGroupsForReports(a => a.Name, m_course, m_educForm, m_educLevel);

            groups.RemoveAll(group =>
            {
                var groupToTAItems = m_groupsToTAItemRepository.GetGroupsToTAItemsByGroupId(group.Id);
                double distributedHours = 0;
                bool semestrFlag = true;

                foreach (var item in groupToTAItems)
                {
                    var taItem = m_taItemRepository.GetTAItem(item.TAItemID);

                    if (taItem == null)
                    {
                        m_groupsToTAItemRepository.RemoveGroupsToTAItem(item);
                        continue;
                    }

                    distributedHours += taItem.WorkHours;

                    if (taItem.Semestr == m_semestr || m_semestr == 0)
                    {
                        semestrFlag = false;
                    }
                }

                return semestrFlag || Tools.isEqual(distributedHours, 0);
            });

            if (groups.Count == 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(groups.Count, "Schedule report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\Schedule.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue(GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                for (int groupCounter = 0; groupCounter < groups.Count;)
                {
                    var groupRow = sheet.CopyRow(8, sheet.LastRowNum);
                    var group = groups[groupCounter];
                    var groupToTAItems = m_groupsToTAItemRepository.GetGroupsToTAItemsByGroupId(group.Id);
                    var curriculumItems = new Dictionary<int, CurriculumItem>();

                    groupRow.Cells[0].SetCellValue((groupCounter + 1).ToString());
                    groupRow.Cells[1].SetCellValue(group.Name + ", " + group.Course + " курс, " + group.EducLevel.ToLower() + ", " + group.EducForm.ToLower() + " форма навчання");

                    foreach (var groupToTAItem in groupToTAItems)
                    {
                        var taItem = m_taItemRepository.GetTAItem(groupToTAItem.TAItemID);
                        if (taItem == null)
                            continue;

                        var work = m_workRepository.GetWork(taItem.WorkId);
                        if (work == null)
                            continue;

                        var currItem = m_curriculumItemRepository.GetCurriculumItem(work.CurriculumItemId);
                        if (currItem == null)
                            continue;

                        if (m_semestr != 0 && currItem.Semestr != m_semestr)
                        {
                            continue;
                        }

                        curriculumItems[currItem.Id] = currItem;
                    }

                    int curriculumItemCounter = 0;
                    foreach (var curriculumItem in curriculumItems)
                    {
                        var subjectRow = sheet.CopyRow(9, sheet.LastRowNum);
                        var subject = m_subjectRepository.GetSubject(curriculumItem.Value.SubjectId);
                        var works = m_workRepository.GetWorks(curriculumItem.Key, true);
                        var taItems = new List<TAItem>();
                        var resultTeachersMap = new Dictionary<int, List<Work>>();

                        subjectRow.Cells[0].SetCellValue((groupCounter + 1).ToString() + "." + (curriculumItemCounter + 1).ToString());
                        subjectRow.Cells[1].SetCellValue(subject.Name + ", " + curriculumItem.Value.Semestr + " семестр, " + curriculumItem.Value.EducLevel.ToLower() + ", " + curriculumItem.Value.EducForm.ToLower() + " форма навчання, " + curriculumItem.Value.EducType.ToLower());

                        foreach (var work in works)
                        {
                            taItems.AddRange(m_taItemRepository.GetTAItems(work.Id));
                        }

                        foreach (var taItem in taItems)
                        {
                            if (!resultTeachersMap.ContainsKey(taItem.TeacherId))
                            {
                                resultTeachersMap.Add(taItem.TeacherId, new List<Work>());
                            }

                            resultTeachersMap[taItem.TeacherId].Add(m_workRepository.GetWork(taItem.WorkId));
                        }

                        foreach (var teacherItem in resultTeachersMap)
                        {
                            var teacherRow = sheet.CopyRow(10, sheet.LastRowNum);
                            var singleTeacher = m_teacherRepository.GetTeacher(teacherItem.Key);
                            teacherRow.Cells[1].SetCellValue(singleTeacher.Name);

                            foreach (var workItem in teacherItem.Value)
                            {
                                var taItem = taItems.Find(item => { return (item.TeacherId == teacherItem.Key) && (item.WorkId == workItem.Id); });
                                if (taItem != null)
                                {
                                    teacherRow.Cells[2 + workItem.WorkTypeId - 1].SetCellValue(taItem.WorkHours);
                                }
                            }

                            teacherRow.Height = -1;
                        }

                        subjectRow.Height = -1;
                        ++curriculumItemCounter;
                    }

                    groupRow.Height = -1;
                    ++groupCounter;

                    m_parentForm.IncrementProgress();
                }

                sheet.ShiftRows(11, sheet.LastRowNum, -3);

                for (int i = 2; i < 38; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(sheet.LastRowNum - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(sheet.LastRowNum).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = sheet.LastRowNum - 1; i <= lastIndex; ++i)
                {
                    var firstCell = sheet.GetRow(i).Cells[2].Address;
                    var lastCell = sheet.GetRow(i).Cells[36].Address;
                    var finalCell = sheet.GetRow(i).Cells[37];

                    if (finalCell.CellType == NPOI.SS.UserModel.CellType.Formula)
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

            m_parentForm.HideProgress();
            m_parentForm.Enabled = true;
        }

        private void GenerateFinalBySubjects()
        {
            var allCurriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            var allSubjects = m_subjectRepository.GetAllSubjects();

            if (allCurriculumItems.Count <= 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(allCurriculumItems.Count, "Final by subjects report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\FinalBySubjects.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue(GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                var subjectFont = workbook.CreateFont();
                subjectFont.FontHeightInPoints = 11;
                subjectFont.FontName = "Calibri";
                subjectFont.IsBold = true;

                var subjectCellStyle = new XSSFCellStyle(workbook.GetStylesSource());
                subjectCellStyle.SetFont(subjectFont);
                subjectCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                subjectCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                var subjectRowEmptyCellStyle = new XSSFCellStyle(workbook.GetStylesSource());
                subjectRowEmptyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
                subjectRowEmptyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
                subjectRowEmptyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                var subjectRowNums = new List<int>();

                var groupedCurriculumItems = allCurriculumItems.GroupBy(ci => ci.SubjectId).OrderBy(ci => allSubjects.First(s => s.Id == ci.Key).Name).ToList();

                foreach (var cirItemsGroup in groupedCurriculumItems)
                {
                    var allWorks = m_workRepository.GetWorksForCurriculumItemIds(cirItemsGroup.Select(cig => cig.Id).ToList(), true);

                    if (allWorks.Count == 0 || cirItemsGroup.Count() == 0) { continue; }

                    subjectRowNums.Add(sheet.LastRowNum);
                    var newSubjectRow = sheet.CopyRow(8, sheet.LastRowNum);
                    var subjectcell = newSubjectRow.GetCell(1);
                    subjectcell.SetCellValue(allSubjects.Find(s => s.Id == cirItemsGroup.Key).Name);
                    subjectcell.CellStyle = subjectCellStyle;

                    for (int i = 2; i < 41; i++)
                    {
                        newSubjectRow.Cells[i].CellStyle = subjectRowEmptyCellStyle;
                    }


                    int numberCounter = 0;
                    foreach (var curItem in cirItemsGroup)
                    {
                        if (!allWorks.ContainsKey(curItem.Id)) { continue; }

                        var allTAItems = new List<TAItem>();
                        var teachersDic = new Dictionary<int, List<TAItem>>();

                        foreach (var work in allWorks[curItem.Id])
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
                        var teachers = m_teacherRepository.GetTeachersById(teachersDic.Keys.ToList()).OrderBy(t => t.Name).ToList();

                        if (teachers.Count == 0) { continue; }

                        foreach (var teacher in teachers)
                        {
                            var newRow = sheet.CopyRow(8, sheet.LastRowNum);
                            var groupsDic = new Dictionary<int, Group>();
                            var groupsToTaItems = m_groupsToTAItemRepository.GetGroupsToTAItem(teachersDic[teacher.Id][0].Id);
                            var groupsCellText = "";

                            foreach (var groupsToTaItem in groupsToTaItems)
                            {
                                groupsDic[groupsToTaItem.Id] = m_groupRepository.GetGroup(groupsToTaItem.GroupId);
                            }

                            foreach (var group in groupsDic)
                            {
                                groupsCellText += group.Value.Name + "; ";
                            }

                            newRow.Cells[0].SetCellValue((numberCounter + 1).ToString());
                            newRow.Cells[1].SetCellValue(teacher.Name);
                            newRow.Cells[2].SetCellValue(groupsCellText);
                            newRow.Cells[3].SetCellValue(curItem.EducLevel);
                            newRow.Cells[4].SetCellValue(GetEducFormString(curItem));
                            newRow.Cells[5].SetCellValue(curItem.Course);

                            foreach (var taItem in teachersDic[teacher.Id])
                            {
                                var work = m_workRepository.GetWork(taItem.WorkId);
                                newRow.Cells[6 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours);
                            }

                            newRow.Height = -1;

                            ++numberCounter;
                        }

                        m_parentForm.IncrementProgress();
                    }
                }

                sheet.ShiftRows(9, sheet.LastRowNum, -1);
                subjectRowNums = subjectRowNums.Select(n => n - 1).ToList();

                for (int i = 6; i < 41; ++i)
                {
                    var firstCell = sheet.GetRow(9).Cells[i].Address;
                    var lastCell = sheet.GetRow(sheet.LastRowNum - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(sheet.LastRowNum).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = sheet.LastRowNum; i <= lastIndex; ++i)
                {
                    if (subjectRowNums.Contains(i)) { continue; }

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

        private void GenerateFinalByTeachers()
        {
            var data = m_reportDTORepository.getFinalTeachersReportDTO(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(data.Teachers.Count, "By teacher report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\FinalByTeachers.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(3).Cells[0].SetCellValue(GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                var teacherFont = workbook.CreateFont();
                teacherFont.FontHeightInPoints = 11;
                teacherFont.FontName = "Calibri";
                teacherFont.IsBold = true;

                var teacherCellStyle = new XSSFCellStyle(workbook.GetStylesSource());
                teacherCellStyle.SetFont(teacherFont);
                teacherCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                teacherCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

                var teacherRowEmptyCellStyle = new XSSFCellStyle(workbook.GetStylesSource());
                teacherRowEmptyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
                teacherRowEmptyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
                teacherRowEmptyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                foreach (var teacher in data.Teachers)
                {
                    var newTeacherRow = sheet.CopyRow(8, sheet.LastRowNum);
                    var teacherCell = newTeacherRow.Cells[1];
                    teacherCell.SetCellValue(teacher.Name);
                    teacherCell.CellStyle = teacherCellStyle;

                    for (int i = 2; i < 41; i++)
                    {
                        newTeacherRow.Cells[i].CellStyle = teacherRowEmptyCellStyle;
                    }

                    int numberCounter = 0;
                    foreach (var curriculumItem in data.CurriculumItemsByTeacherIds[teacher.Id])
                    {
                        var newRow = sheet.CopyRow(8, sheet.LastRowNum);
                        newRow.Cells[0].SetCellValue((numberCounter + 1).ToString());
                        newRow.Cells[1].SetCellValue(data.SubjectsByCurriculumItemIds[curriculumItem.Id].Name);
                        newRow.Cells[2].SetCellValue(data.GroupNamesByCurriculumItemIds.ContainsKey(curriculumItem.Id) 
                            ? string.Join("; ", data.GroupNamesByCurriculumItemIds[curriculumItem.Id]) : "");
                        newRow.Cells[3].SetCellValue(curriculumItem.EducLevel);
                        newRow.Cells[4].SetCellValue(GetEducFormString(curriculumItem));
                        newRow.Cells[5].SetCellValue(curriculumItem.Course);

                        foreach (var taItem in data.TAItemsByCurriculumItemIds[curriculumItem.Id])
                        {
                            var work = data.WorksForTAItems.Find(w => w.Id == taItem.WorkId);
                            newRow.Cells[6 + work.WorkTypeId - 1].SetCellValue(taItem.WorkHours);
                        }

                        newRow.Height = -1;
                        ++numberCounter;
                    }
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

        private void treeViewExcelReports_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.FullPath == "Зведені\\Навчальний план")
            {
                GeneratePlannedBySubjects();
            }
            if (e.Node.FullPath == "Зведені\\Навантаження за дисциплінами")
            {
                GenerateDistributedBySubjects();
            }
            if (e.Node.FullPath == "Зведені\\Нерозподілені години")
            {
                GenerateNotDistributedBySubjects();
            }
            if (e.Node.FullPath == "Зведені\\Навантаження за викладачами")
            {
                GenerateLoadShortByTeachers();
            }
            if (e.Node.FullPath == "Зведені\\Навантаження за викладачами детальне")
            {
                GenerateLoadFullByTeachers();
            }
            if (e.Node.FullPath == "Зведені\\Розклад для груп")
            {
                GenerateSchedule();
            }
            if (e.Node.FullPath == "Зведені\\Підсумковий за дисциплінами")
            {
                GenerateFinalBySubjects();
            }
            if (e.Node.FullPath == "Зведені\\Підсумковий за викладачами")
            {
                GenerateFinalByTeachers();
            }
        }

        private void ExcelReports_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.excelReportsForm = null;
            m_parentForm.SetButtonState();
        }

        private void treeViewExcelReports_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.FullPath == "Вибіркові\\за вибраним викладачем")
            {
                panelContainer.Visible = true;

                m_loadedSelectForm = new SelectTeacher(m_parentForm, m_dbPath, m_educType, m_educForm, m_educLevel, m_course, m_semestr, ByTeacherReportType.Default);

                m_loadedSelectForm.TopLevel = false;
                m_loadedSelectForm.Size = panelContainer.Size;

                panelContainer.Controls.Add(m_loadedSelectForm);
                panelContainer.Tag = m_loadedSelectForm;
                m_loadedSelectForm.BringToFront();
                m_loadedSelectForm.Dock = DockStyle.Fill;

                m_loadedSelectForm.Show();

                return;
            }
            if (e.Node.FullPath == "Вибіркові\\за вибраним викладачем розширений")
            {
                panelContainer.Visible = true;

                m_loadedSelectForm = new SelectTeacher(m_parentForm, m_dbPath, m_educType, m_educForm, m_educLevel, m_course, m_semestr, ByTeacherReportType.Extended);

                m_loadedSelectForm.TopLevel = false;
                m_loadedSelectForm.Size = panelContainer.Size;

                panelContainer.Controls.Add(m_loadedSelectForm);
                panelContainer.Tag = m_loadedSelectForm;
                m_loadedSelectForm.BringToFront();
                m_loadedSelectForm.Dock = DockStyle.Fill;

                m_loadedSelectForm.Show();

                return;
            }
            if (e.Node.FullPath == "Вибіркові\\за вибраною дисципліною")
            {
                panelContainer.Visible = true;

                m_loadedSelectForm = new SelectSubject(m_parentForm, m_dbPath, m_educType, m_educForm, m_educLevel, m_course, m_semestr);

                m_loadedSelectForm.TopLevel = false;
                m_loadedSelectForm.Size = panelContainer.Size;

                panelContainer.Controls.Add(m_loadedSelectForm);
                panelContainer.Tag = m_loadedSelectForm;
                m_loadedSelectForm.BringToFront();
                m_loadedSelectForm.Dock = DockStyle.Fill;

                m_loadedSelectForm.Show();

                return;
            }
            if (e.Node.FullPath == "Вибіркові\\за вибраним видом роботи")
            {
                panelContainer.Visible = true;

                m_loadedSelectForm = new SelectWorkType(m_parentForm, m_dbPath, m_educType, m_educForm, m_educLevel, m_course, m_semestr);

                m_loadedSelectForm.TopLevel = false;
                m_loadedSelectForm.Size = panelContainer.Size;

                panelContainer.Controls.Add(m_loadedSelectForm);
                panelContainer.Tag = m_loadedSelectForm;
                m_loadedSelectForm.BringToFront();
                m_loadedSelectForm.Dock = DockStyle.Fill;

                m_loadedSelectForm.Show();

                return;
            }
            if (e.Node.FullPath == "Вибіркові\\Індивідуальний план")
            {
                panelContainer.Visible = true;

                m_loadedSelectForm = new SelectTeacher(m_parentForm, m_dbPath, m_educType, m_educForm, m_educLevel, m_course, m_semestr, ByTeacherReportType.Individual);

                m_loadedSelectForm.TopLevel = false;
                m_loadedSelectForm.Size = panelContainer.Size;

                panelContainer.Controls.Add(m_loadedSelectForm);
                panelContainer.Tag = m_loadedSelectForm;
                m_loadedSelectForm.BringToFront();
                m_loadedSelectForm.Dock = DockStyle.Fill;

                m_loadedSelectForm.Show();

                return;
            }

            m_loadedSelectForm?.Close();
            panelContainer.Visible = false;
        }
    }
}
