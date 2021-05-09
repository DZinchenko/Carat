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

        private ICurriculumItemRepository m_curriculumItemRepository;
        private ISubjectRepository m_subjectRepository;
        private IWorkRepository m_workRepository;
        private ITeacherRepository m_teacherRepository;
        private ITAItemRepository m_taItemRepository;

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

            treeViewExcelReports.ExpandAll();
        }

        public static string GetTempFilePathWithExtension(string extension)
        {
            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + extension;
            return Path.Combine(path, fileName);
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

        private void GenerateLoadBySubjects()
        {
            var curriculumItems = new List<CurriculumItem>();

            curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

            if (curriculumItems.Count == 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var templatePath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
                templatePath += "\\templates\\LoadBySubjects.xlsx";

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

                    newRow.Cells[0].SetCellValue((i +1).ToString());
                    newRow.Cells[1].SetCellValue(m_subjectRepository.GetSubject(curriculumItem.SubjectId)?.Name);
                    newRow.Cells[2].SetCellValue(curriculumItem.Course);

                    if (curriculumWorks.Count <= 0)
                    {
                        ++i;
                        continue;
                    }

                    newRow.Cells[3].SetCellValue(curriculumWorks[0].TotalHours);
                    newRow.Cells[4].SetCellValue(curriculumWorks[1].TotalHours);
                    newRow.Cells[5].SetCellValue(curriculumWorks[2].TotalHours);
                    newRow.Cells[6].SetCellValue(curriculumWorks[4].TotalHours);
                    newRow.Cells[7].SetCellValue(curriculumWorks[5].TotalHours);
                    newRow.Cells[8].SetCellValue(curriculumWorks[6].TotalHours);
                    newRow.Cells[9].SetCellValue(curriculumWorks[7].TotalHours);
                    newRow.Cells[10].SetCellValue(curriculumWorks[8].TotalHours);
                    newRow.Cells[11].SetCellValue(curriculumWorks[9].TotalHours);
                    newRow.Cells[12].SetCellValue(curriculumWorks[10].TotalHours);
                    newRow.Cells[13].SetCellValue(curriculumWorks[11].TotalHours);
                    newRow.Cells[14].SetCellValue(curriculumWorks[12].TotalHours);
                    //newRow.Cells[15].SetCellValue(curriculumWorks[3].TotalHours);
                    newRow.Cells[16].SetCellValue(curriculumWorks[13].TotalHours);
                    newRow.Cells[17].SetCellValue(curriculumWorks[3].TotalHours);
                    newRow.Cells[18].SetCellValue(curriculumWorks[14].TotalHours);
                    newRow.Cells[19].SetCellValue(curriculumWorks[15].TotalHours);
                    newRow.Cells[20].SetCellValue(curriculumWorks[16].TotalHours);
                    newRow.Cells[21].SetCellValue(curriculumWorks[17].TotalHours);
                    newRow.Cells[22].SetCellValue(curriculumWorks[18].TotalHours);
                    newRow.Cells[23].SetCellValue(curriculumWorks[19].TotalHours);
                    newRow.Cells[24].SetCellValue(curriculumWorks[20].TotalHours);
                    newRow.Cells[25].SetCellValue(curriculumWorks[24].TotalHours);
                    newRow.Cells[26].SetCellValue(curriculumWorks[25].TotalHours);
                    newRow.Cells[27].SetCellValue(curriculumWorks[26].TotalHours);
                    newRow.Cells[28].SetCellValue(curriculumWorks[21].TotalHours);
                    newRow.Cells[29].SetCellValue(curriculumWorks[22].TotalHours);
                    newRow.Cells[30].SetCellValue(curriculumWorks[23].TotalHours);
                    newRow.Cells[31].SetCellValue(curriculumWorks[27].TotalHours);
                    newRow.Cells[32].SetCellValue(curriculumWorks[30].TotalHours);
                    newRow.Cells[33].SetCellValue(curriculumWorks[32].TotalHours);
                    newRow.Cells[34].SetCellValue(curriculumWorks[33].TotalHours);
                    newRow.Cells[35].SetCellValue(curriculumWorks[34].TotalHours);
                    newRow.Cells[36].SetCellValue(curriculumWorks[35].TotalHours);
                    newRow.Cells[37].SetCellValue(curriculumWorks[36].TotalHours);

                    newRow.Height = -1;

                    ++i;

                    if (i < curriculumItems.Count)
                    {
                        sheet.CopyRow(8, 8 + i);                      
                    }
                }

                for (int i = 3; i < 38; ++i)
                {
                    var firstCell = sheet.GetRow(8).Cells[i].Address;
                    var lastCell = sheet.GetRow(8 + curriculumItems.Count - 1).Cells[i].Address;
                    var finalCell = sheet.GetRow(8 + curriculumItems.Count).Cells[i];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                for (int i = 8, lastIndex = curriculumItems.Count + 8; i <= lastIndex; ++i)
                {
                    var firstCell = sheet.GetRow(i).Cells[3].Address;
                    var lastCell = sheet.GetRow(i).Cells[37].Address;
                    var finalCell = sheet.GetRow(i).Cells[38];

                    finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                    finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));
                }

                XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                sheet.AutoSizeColumn(1);

                using (var fileData = new FileStream(GetTempFilePathWithExtension(".xlsx"), FileMode.OpenOrCreate))
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
            var teachers = m_teacherRepository.GetAllTeachers();

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

                        if (work.WorkTypeId == 0)
                        {
                            lectureHours1 += taItem.WorkHours;
                        }
                        else if (work.WorkTypeId == 1 || work.WorkTypeId == 2)
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

                        if (work.WorkTypeId == 0)
                        {
                            lectureHours2 += taItem.WorkHours;
                        }
                        else if (work.WorkTypeId == 1 || work.WorkTypeId == 2)
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

                using (var fileData = new FileStream(GetTempFilePathWithExtension(".xlsx"), FileMode.OpenOrCreate))
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
            if (e.Node.FullPath == "Навантаження кафедри\\за дисциплінами")
            {
                GenerateLoadBySubjects();
            }
            if (e.Node.FullPath == "Навантаження кафедри\\за викладачами (скорочений)")
            {
                GenerateLoadShortByTeachers();
            }
        }

        private void ExcelReports_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.excelReportsForm = null;
            m_parentForm.SetButtonState();
        }
    }
}
