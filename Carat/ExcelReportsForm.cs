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

        private void GeneratePlannedBySubjects()
        {
            var curriculumItems = new List<CurriculumItem>();

            curriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

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

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
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
                    newRow.Cells[3].SetCellValue(curriculumItem.Course);

                    for (int cellIndex = 4, workTypeIndex = 0; cellIndex <= 38; ++cellIndex, ++workTypeIndex)
                    {
                        newRow.Cells[cellIndex].SetCellValue(curriculumWorks[workTypeIndex].TotalHours);
                    }

                    newRow.Height = -1;

                    ++i;

                    if (i < curriculumItems.Count)
                    {
                        sheet.CopyRow(8, 8 + i);                      
                    }
                }

                for (int i = 4; i < 39; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(8 + curriculumItems.Count - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(8 + curriculumItems.Count).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = curriculumItems.Count + 8; i <= lastIndex; ++i)
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

        private void GenerateDistributedBySubjects()
        {
            var curriculumItems = new List<CurriculumItem>();

            curriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

            curriculumItems.RemoveAll(curriculumItem =>
            {
                var curriculumWorks = m_workRepository.GetWorks(curriculumItem.Id, false);
                return curriculumWorks.TrueForAll(work => {
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

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
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
                    newRow.Cells[3].SetCellValue(curriculumItem.Course);

                    for (int cellIndex = 4, workTypeIndex = 0; cellIndex <= 38; ++cellIndex, ++workTypeIndex)
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
                }

                for (int i = 4; i < 39; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(8 + curriculumItems.Count - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(8 + curriculumItems.Count).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = curriculumItems.Count + 8; i <= lastIndex; ++i)
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

        private void GenerateNotDistributedBySubjects()
        {
            var curriculumItems = new List<CurriculumItem>();

            curriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

            curriculumItems.RemoveAll(curriculumItem =>
            {
                var curriculumWorks = m_workRepository.GetWorks(curriculumItem.Id, false);
                return curriculumWorks.TrueForAll(work => {
                    var taItems = m_taItemRepository.GetTAItems(work.Id);
                    double distributedHours = 0;

                    foreach (var taItem in taItems)
                    {
                        distributedHours += taItem.WorkHours;
                    }

                    return Tools.isEqual(work.TotalHours, 0) || (!Tools.isEqual(0, distributedHours));
                });
            });

            if (curriculumItems.Count == 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
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
                    newRow.Cells[3].SetCellValue(curriculumItem.Course);

                    for (int cellIndex = 4, workTypeIndex = 0; cellIndex <= 38; ++cellIndex, ++workTypeIndex)
                    {
                        var taItems = m_taItemRepository.GetTAItems(curriculumWorks[workTypeIndex].Id);
                        double distributedHours = 0;

                        foreach (var taItem in taItems)
                        {
                            distributedHours += taItem.WorkHours;
                        }

                        newRow.Cells[cellIndex].SetCellValue(curriculumWorks[workTypeIndex].TotalHours - distributedHours);
                    }

                    newRow.Height = -1;

                    ++i;

                    if (i < curriculumItems.Count)
                    {
                        sheet.CopyRow(8, 8 + i);
                    }
                }

                for (int i = 4; i < 39; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(8 + curriculumItems.Count - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(8 + curriculumItems.Count).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = curriculumItems.Count + 8; i <= lastIndex; ++i)
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

        private void GenerateLoadShortByTeachers()
        {
            var fullTeachers = m_teacherRepository.GetAllTeachers();
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

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
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

                foreach (var teacher in teachers)
                {
                    var newRow = sheet.GetRow(9 + i);

                    newRow.Cells[0].SetCellValue(i+1);
                    newRow.Cells[1].SetCellValue(teacher.Name + " (" + teacher.Position + "; " +  teacher.StaffUnit.ToString("F2") + ")");

                    var taItems1 = m_taItemRepository.GetTAItemsByTeacherId(teacher.Id, m_semestr, m_educType, "Денна");
                    double lectureHours1 = 0.00;
                    double otherAudi1 = 0.00;
                    double other1 = 0.00;

                    foreach (var taItem in taItems1)
                    {
                        var work = m_workRepository.GetWork(taItem.WorkId);

                        if (work == null)
                            continue;

                        if (work.WorkTypeId == workTypes[0].Id)
                        {
                            lectureHours1 += taItem.WorkHours;
                        }
                        else if (work.WorkTypeId == workTypes[1].Id || work.WorkTypeId == workTypes[2].Id)
                        {
                            otherAudi1 += taItem.WorkHours;
                        }
                        else
                        {
                            other1 += taItem.WorkHours;
                        }
                    }

                    var taItems2 = m_taItemRepository.GetTAItemsByTeacherId(teacher.Id, m_semestr, m_educType, "Заочна");
                    taItems2.AddRange(m_taItemRepository.GetTAItemsByTeacherId(teacher.Id, m_semestr, m_educType, "Вечірня"));
                    double lectureHours2 = 0.00;
                    double otherAudi2 = 0.00;
                    double other2 = 0.00;

                    foreach (var taItem in taItems2)
                    {
                        var work = m_workRepository.GetWork(taItem.WorkId);

                        if (work.WorkTypeId == workTypes[0].Id)
                        {
                            lectureHours2 += taItem.WorkHours;
                        }
                        else if (work.WorkTypeId == workTypes[1].Id || work.WorkTypeId == workTypes[2].Id)
                        {
                            otherAudi2 += taItem.WorkHours;
                        }
                        else
                        {
                            other2 += taItem.WorkHours;
                        }
                    }

                    newRow.Cells[2].SetCellValue(lectureHours1);
                    newRow.Cells[3].SetCellValue(otherAudi1);
                    newRow.Cells[4].SetCellValue(other1);

                    newRow.Cells[6].SetCellValue(lectureHours2);
                    newRow.Cells[7].SetCellValue(otherAudi2);
                    newRow.Cells[8].SetCellValue(other2);

                    newRow.Height = -1;

                    ++i;

                    if (i < teachers.Count)
                    {
                        sheet.CopyRow(9, 9 + i);
                    }
                }            

                for (int j = 2; j < 10; ++j)
                {
                    var firstCell = sheet.GetRow(9).Cells[j].Address;
                    var lastCell = sheet.GetRow(9 + teachers.Count - 1).Cells[j].Address;
                    var finalCell = sheet.GetRow(9 + teachers.Count).Cells[j];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int j = 9, lastIndex = teachers.Count + 9; j <= lastIndex; ++j)
                {
                    var firstCell = sheet.GetRow(j).Cells[2].Address;
                    var lastCell = sheet.GetRow(j).Cells[4].Address;
                    var finalCell = sheet.GetRow(j).Cells[5];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int j = 9, lastIndex = teachers.Count + 9; j <= lastIndex; ++j)
                {
                    var firstCell = sheet.GetRow(j).Cells[6].Address;
                    var lastCell = sheet.GetRow(j).Cells[7].Address;
                    var finalCell = sheet.GetRow(j).Cells[9];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int j = 9, lastIndex = teachers.Count + 9; j <= lastIndex; ++j)
                {
                    var firstSumCell = sheet.GetRow(j).Cells[5].Address;
                    var lastSumCell = sheet.GetRow(j).Cells[9].Address;
                    var finalCell = sheet.GetRow(j).Cells[10];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format(firstSumCell + "+" + lastSumCell));
                }

                XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
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
        }

        private void GenerateLoadFullByTeachers()
        {
            var fullTeachers = m_teacherRepository.GetAllTeachers();
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

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
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

                foreach (var teacher in teachers)
                {
                    var newRow = sheet.GetRow(8 + rowCounter);

                    newRow.Cells[0].SetCellValue(rowCounter + 1);
                    newRow.Cells[1].SetCellValue(teacher.Name + " (" + teacher.Position + "; " + teacher.StaffUnit.ToString("F2") + ")");

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

                    for (int cellIndex = 2, workTypeIndex = 0; cellIndex <= 36; ++cellIndex, ++workTypeIndex)
                    {
                        newRow.Cells[cellIndex].SetCellValue(totalWorksHours[workTypes[workTypeIndex].Id]);
                    }

                    newRow.Height = -1;

                    ++rowCounter;

                    if (rowCounter < teachers.Count)
                    {
                        sheet.CopyRow(8, 8 + rowCounter);
                    }
                }

                for (int i = 2; i < 38; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(8 + rowCounter - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(8 + rowCounter).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = rowCounter + 8; i < lastIndex; ++i)
                {
                    var firstCell = sheet.GetRow(i).Cells[2].Address;
                    var lastCell = sheet.GetRow(i).Cells[36].Address;
                    var finalCell = sheet.GetRow(i).Cells[37];

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

        private void GenerateSchedule()
        {
            var groups = new List<Group>();
            groups = m_groupRepository.GetGroupsForReports(m_course, m_educForm, m_educLevel);

            groups.RemoveAll(group =>
            {
                var groupToTAItems = m_groupsToTAItemRepository.GetGroupsToTAItemsByGroupId(group.Id);
                double distributedHours = 0;
                bool semestrFlag = true;

                foreach (var item in groupToTAItems)
                {
                    var taItem = m_taItemRepository.GetTAItem(item.TAItemID);
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

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
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
                        var work = m_workRepository.GetWork(taItem.WorkId);
                        var currItem = m_curriculumItemRepository.GetCurriculumItem(work.CurriculumItemId);
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
        }

        private void treeViewExcelReports_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.FullPath == "Навантаження кафедри\\за дисциплінами (заплановані)")
            {
                GeneratePlannedBySubjects();
            }
            if (e.Node.FullPath == "Навантаження кафедри\\за дисциплінами (розподілені)")
            {
                GenerateDistributedBySubjects();
            }
            if (e.Node.FullPath == "Інші\\нерозподілені години")
            {
                GenerateNotDistributedBySubjects();
            }
            if (e.Node.FullPath == "Навантаження кафедри\\за викладачами (скорочений)")
            {
                GenerateLoadShortByTeachers();
            }
            if (e.Node.FullPath == "Навантаження кафедри\\за викладачами (повний)")
            {
                GenerateLoadFullByTeachers();
            }
            if (e.Node.FullPath == "Навантаження кафедри\\розклад")
            {
                GenerateSchedule();
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
            if (e.Node.FullPath == "Вибіркові\\за вибраним викладачем (розширений)")
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
            if (e.Node.FullPath == "Інші\\індивідуальний план")
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
