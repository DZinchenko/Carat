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
        }

        public static string GetTempFilePathWithExtension(string extension)
        {
            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + extension;
            return Path.Combine(path, fileName);
        }

        private void GenerateLoadBySubjects()
        {
            var curriculumItems = m_curriculumItemRepository.GetAllCurriculumItems(m_educType, m_educForm, m_course, m_semestr, m_educLevel);

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
                    newRow.Cells[15].SetCellValue(curriculumWorks[3].TotalHours);
                    newRow.Cells[16].SetCellValue(curriculumWorks[13].TotalHours);
                    newRow.Cells[17].SetCellValue(curriculumWorks[14].TotalHours);
                    newRow.Cells[18].SetCellValue(curriculumWorks[15].TotalHours);
                    newRow.Cells[19].SetCellValue(curriculumWorks[16].TotalHours);
                    newRow.Cells[20].SetCellValue(curriculumWorks[17].TotalHours);
                    newRow.Cells[21].SetCellValue(curriculumWorks[18].TotalHours);
                    newRow.Cells[22].SetCellValue(curriculumWorks[19].TotalHours);
                    newRow.Cells[23].SetCellValue(curriculumWorks[20].TotalHours);
                    newRow.Cells[24].SetCellValue(curriculumWorks[21].TotalHours);
                    newRow.Cells[25].SetCellValue(curriculumWorks[22].TotalHours);
                    newRow.Cells[26].SetCellValue(curriculumWorks[23].TotalHours);
                    newRow.Cells[27].SetCellValue(curriculumWorks[24].TotalHours);
                    newRow.Cells[28].SetCellValue(curriculumWorks[25].TotalHours);
                    newRow.Cells[29].SetCellValue(curriculumWorks[26].TotalHours);
                    newRow.Cells[30].SetCellValue(curriculumWorks[27].TotalHours);
                    newRow.Cells[31].SetCellValue(curriculumWorks[30].TotalHours);
                    newRow.Cells[32].SetCellValue(curriculumWorks[32].TotalHours);
                    newRow.Cells[33].SetCellValue(curriculumWorks[33].TotalHours);
                    newRow.Cells[34].SetCellValue(curriculumWorks[34].TotalHours);
                    newRow.Cells[35].SetCellValue(curriculumWorks[35].TotalHours);
                    newRow.Cells[36].SetCellValue(curriculumWorks[36].TotalHours);

                    ++i;

                    if (i < curriculumItems.Count)
                    {
                        sheet.CopyRow(8, 8 + i);                      
                    }
                }

                for (int i = 3; i < 37; ++i)
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
                    var lastCell = sheet.GetRow(i).Cells[36].Address;
                    var finalCell = sheet.GetRow(i).Cells[37];

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

        private void treeViewExcelReports_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.FullPath == "Навантаження кафедри\\за дисциплінами")
            {
                GenerateLoadBySubjects();
            }
        }

        private void ExcelReports_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.excelReportsForm = null;
            m_parentForm.SetButtonState();
        }
    }
}
