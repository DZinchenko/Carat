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
    public partial class SelectWorkType : Form
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
        IWorkTypeRepository m_workTypeRepository;

        public SelectWorkType(MainForm parentForm,
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
            var workTypes = m_workTypeRepository.GetAllWorkTypes();

            foreach (var workType in workTypes)
            {
                dataGridViewSelectWorkType.Rows.Add(workType.Name);
            }
        }

        private void GenerateReport(WorkType workType)
        {
            var allCurriculumItems = m_curriculumItemRepository.GetAllCurriculumItemsForReports(a => m_subjectRepository.GetSubject(a.SubjectId)?.Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            var allWorks = new List<Work>();
            var allTAItems = new List<TAItem>();

            foreach (var curItem in allCurriculumItems)
            {
                allWorks.AddRange(m_workRepository.GetWorks(curItem.Id, true));
            }

            allWorks.RemoveAll(work => { return work.WorkTypeId != workType.Id; });

            foreach (var work in allWorks)
            {
                allTAItems.AddRange(m_taItemRepository.GetTAItems(work.Id));
            }

            allTAItems.RemoveAll(item => {return Tools.isEqual(0, item.WorkHours); });

            if (allTAItems.Count <= 0)
            {
                MessageBox.Show(IncorrectNameMessageDataIsEmpty, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_parentForm.Enabled = false;
            m_parentForm.ShowProgress(allTAItems.Count, "By work type report generating...");

            try
            {
                var templatePath = Directory.GetCurrentDirectory();
                templatePath += "\\templates\\DefaultByWorkType.xlsx";

                var workbook = new XSSFWorkbook(templatePath);
                var sheet = workbook[0];

                if (sheet == null)
                {
                    return;
                }

                sheet.GetRow(4).Cells[1].SetCellValue("Вид роботи: " + workType.Name + ", " + GetSemesterString() + ", " + GetEducTypeString() + ", " + GetEducFormString());

                int numberCounter = 0;
                foreach (var taItem in allTAItems)
                {
                    var newRow = sheet.CopyRow(8, sheet.LastRowNum);
                    var work = m_workRepository.GetWork(taItem.WorkId);
                    var curriculumItem = m_curriculumItemRepository.GetCurriculumItem(work.CurriculumItemId);
                    var subject = m_subjectRepository.GetSubject(curriculumItem.SubjectId);
                    var teacher = m_teacherRepository.GetTeacher(taItem.TeacherId);
                    var groupsToTAItem = m_groupsToTAItemRepository.GetGroupsToTAItemsByTAItemId(taItem.Id);
                    var groups = new List<Group>();
                    var groupsCellText = "";

                    foreach (var groupToTaItem in groupsToTAItem)
                    {
                        groups.Add(m_groupRepository.GetGroup(groupToTaItem.GroupId));
                    }

                    foreach (var group in groups)
                    {
                        groupsCellText += group.Name + "; ";
                    }

                    newRow.Cells[0].SetCellValue((numberCounter + 1).ToString());
                    newRow.Cells[1].SetCellValue(subject.Name);
                    newRow.Cells[2].SetCellValue(curriculumItem.EducLevel);
                    newRow.Cells[3].SetCellValue(GetEducFormString(curriculumItem));
                    newRow.Cells[4].SetCellValue(curriculumItem.Course);
                    newRow.Cells[5].SetCellValue(groupsCellText);
                    newRow.Cells[6].SetCellValue(teacher.Name);
                    newRow.Cells[7].SetCellValue(taItem.WorkHours);

                    newRow.Height = -1;

                    ++numberCounter;
                    m_parentForm.IncrementProgress();
                }

                sheet.ShiftRows(9, sheet.LastRowNum, -1);

                var firstCell = sheet.GetRow(8).Cells[7].Address;
                var lastCell = sheet.GetRow(sheet.LastRowNum - 1).Cells[7].Address;
                var finalCell = sheet.GetRow(sheet.LastRowNum).Cells[7];

                finalCell.SetCellType(NPOI.SS.UserModel.CellType.Formula);
                finalCell.SetCellFormula(string.Format("SUM(" + firstCell + ":" + lastCell + ")"));

                XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(5);

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

        private void dataGridViewSelectWorkType_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var workTypes = m_workTypeRepository.GetAllWorkTypes();

            if (e.RowIndex < 0 || e.RowIndex >= workTypes.Count)
            {
                return;
            }

            GenerateReport(workTypes[e.RowIndex]);
        }
    }
}
