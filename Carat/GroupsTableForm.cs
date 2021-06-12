using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Carat.EF.Repositories;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using Carat.Interfaces;
using NPOI.XSSF.UserModel;

namespace Carat
{
    public partial class GroupsTableForm : Form, IDataUserForm
    {
        MainForm m_parentForm = null;
        IGroupRepository m_groupRepository = null;
        IGroupsToTAItemRepository m_groupsToTAItemRepository = null;
        const string IncorrectNameMessage = "Некоректна назва групи";
        const string IncorrectDataMessage = "Некоректні дані";
        bool isSortChanging = false;
        int sortColumnIndex = 0;

        public GroupsTableForm(MainForm parentForm, string dbPath)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_groupRepository = new GroupRepository(dbPath);
            m_groupsToTAItemRepository = new GroupsToTAItemRepository(dbPath);
        }

        public void LoadData()
        {
            var groups = GetAllSortedGroups();

            foreach (var group in groups)
            {
                dataGridViewGroups.Rows.Add(
                    group.Name,
                    group.Course.ToString(),
                    group.EducForm,
                    group.EducLevel,
                    group.BudgetNumber, 
                    group.ContractNumber,
                    group.Faculty,
                    group.Note);
            }
        }

        private void SyncData()
        {
            var groups = GetAllSortedGroups();

            for (int i = 0; i < groups.Count; ++i)
            {
                dataGridViewGroups.Rows[i].SetValues(
                    groups[i].Name,
                    groups[i].Course.ToString(),
                    groups[i].EducForm,
                    groups[i].EducLevel,
                    groups[i].BudgetNumber,
                    groups[i].ContractNumber,
                    groups[i].Faculty,
                    groups[i].Note);
            }
        }

        private void RemoveLastRow()
        {
            int index = dataGridViewGroups.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            if (isSortChanging)
            {
                return;
            }

            dataGridViewGroups.Rows.Remove(dataGridViewGroups.Rows[index]);
        }

        private bool isValidName(string name)
        {
            int duplicatesCounter = 0;

            if (name == null || name == "")
            {
                return false;
            }

            for (int i = 0; i < dataGridViewGroups.Rows.Count; ++i)
            {
                if (dataGridViewGroups[1, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter > 1 ? false : true;
        }

        private void GroupsTableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.groupsForm = null;
            m_parentForm.SetButtonState();
        }

        private void dataGridViewGroups_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var groups = GetAllSortedGroups();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= groups.Count)
            {
                return;
            }

            if (isSortChanging)
            {
                return;
            }

            for (int i = e.RowIndex, limit = e.RowIndex + e.RowCount; i < limit; ++i)
            {
                if (i < groups.Count)
                {
                    m_groupsToTAItemRepository.RemoveGroupsToTAItems(m_groupsToTAItemRepository.GetGroupsToTAItemsByGroupId(groups[i].Id));
                    m_groupRepository.RemoveGroup(groups[i]);
                }
            }
        }

        private void dataGridViewGroups_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_groupRepository == null)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            var groups = GetAllSortedGroups();

            if (e.RowIndex < groups.Count)
            {
                var group = groups[e.RowIndex];

                if (!UpdateData(group, e, false))
                {
                    return;
                }

                m_groupRepository.UpdateGroup(group);
                PerformSort();
            }
            else
            {
                var group = new Group();

                if (!UpdateData(group, e, true))
                {
                    RemoveLastRow();
                    return;
                }

                m_groupRepository.AddGroup(group);
                PerformSort();
            }
        }

        private bool UpdateData(Group group, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try {
                var name = dataGridViewGroups[0, e.RowIndex].Value?.ToString()?.Trim();

                if (!isValidName(name))
                {
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (!isNewObject)
                    {
                        SyncData();
                    }

                    return false;
                }

                switch (e.ColumnIndex)
                {
                    case 0:
                        {
                            group.Name = dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString().Trim();
                            break;
                        }
                    case 1:
                        {
                            group.Course = Convert.ToUInt32(dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim());
                            break;
                        }
                    case 2:
                        {
                            group.EducForm = dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 3:
                        {
                            group.EducLevel = dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 4:
                        {
                            group.BudgetNumber = Convert.ToUInt32(dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim());
                            break;
                        }
                    case 5:
                        {
                            group.ContractNumber = Convert.ToUInt32(dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim());
                            break;
                        }
                    case 6:
                        {
                            group.Faculty = dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 7:
                        {
                            group.Note = dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    default: { return false; }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                SyncData();
                return false;
            }

            return true;
        }

        private void dataGridViewGroups_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void buttonExportGroups_Click(object sender, EventArgs e)
        {
            using (var fileData = new FileStream(Tools.GetTempFilePathWithExtension(".xlsx"), FileMode.OpenOrCreate))
            {
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Групи");
                var row = sheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Назва");
                row.CreateCell(1).SetCellValue("Курс");
                row.CreateCell(2).SetCellValue("Форма навчання");
                row.CreateCell(3).SetCellValue("Рівень навчання");
                row.CreateCell(4).SetCellValue("Бюджетників");
                row.CreateCell(5).SetCellValue("Контрактників");
                row.CreateCell(6).SetCellValue("Факультет");
                row.CreateCell(7).SetCellValue("Примітки");

                var allGroups = GetAllSortedGroups();

                for (int i = 0, rowIndex = 1; i < allGroups.Count; ++i, ++rowIndex)
                {
                    var dataRow = sheet.CreateRow(rowIndex);

                    dataRow.CreateCell(0).SetCellValue(allGroups[i].Name);
                    dataRow.CreateCell(1).SetCellValue(allGroups[i].Course);
                    dataRow.CreateCell(2).SetCellValue(allGroups[i].EducForm);
                    dataRow.CreateCell(3).SetCellValue(allGroups[i].EducLevel);
                    dataRow.CreateCell(4).SetCellValue(allGroups[i].BudgetNumber);
                    dataRow.CreateCell(5).SetCellValue(allGroups[i].ContractNumber);
                    dataRow.CreateCell(6).SetCellValue(allGroups[i].Faculty);
                    dataRow.CreateCell(7).SetCellValue(allGroups[i].Note);
                }

                for (int i = 0; i <= 7; ++i)
                {
                    sheet.AutoSizeColumn(i);
                }

                workbook.Write(fileData);

                System.Diagnostics.Process.Start(@fileData.Name);
            }
        }

        private void buttonImportGroups_Click(object sender, EventArgs e)
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
                        var sheet = workbook[0];
                        var groups = new List<Group>();

                        if (sheet == null)
                        {
                            return;
                        }

                        dataGridViewGroups.Rows.Clear();

                        for (int i = 1; i <= sheet.LastRowNum; ++i)
                        {
                            var row = sheet.GetRow(i);
                            var group = new Group();
                            uint contractNumber = 0;
                            uint course = 1;
                            uint budjetNumber = 0;

                            group.Name = row.GetCell(0)?.ToString();

                            try
                            {
                                course = Convert.ToUInt32(row.GetCell(1)?.ToString());
                            }
                            catch (Exception)
                            { }
                            group.Course = course;

                            group.EducForm = row.GetCell(2)?.ToString();
                            group.EducLevel = row.GetCell(3)?.ToString();

                            try
                            {
                                budjetNumber = Convert.ToUInt32(row.GetCell(4)?.ToString());
                            }
                            catch (Exception)
                            { }
                            group.BudgetNumber = budjetNumber;

                            try
                            {
                                contractNumber = Convert.ToUInt32(row.GetCell(5)?.ToString());
                            }
                            catch (Exception)
                            { }
                            group.ContractNumber = contractNumber;

                            group.Faculty = row.GetCell(6)?.ToString();
                            group.Note = row.GetCell(7)?.ToString();

                            groups.Add(group);
                        }

                        m_groupRepository.AddGroups(groups);

                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Group> GetAllSortedGroups()
        {
            if (sortColumnIndex == 0)
            {
                return m_groupRepository.GetAllGroups(a => a.Name);
            }
            else
            {
                return m_groupRepository.GetAllGroups(a => a.Course + a.Name);
            }
        }

        private void dataGridViewGroups_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
            {
                dataGridViewGroups.EndEdit();
                sortColumnIndex = e.ColumnIndex;

                PerformSort();
            }
        }

        private void PerformSort()
        {
            if (!isSortChanging)
            {
                var horizontalScrollingOffset = dataGridViewGroups.HorizontalScrollingOffset;
                var verticalScrollingOffset = dataGridViewGroups.VerticalScrollingOffset;
                isSortChanging = true;
                dataGridViewGroups.Rows.Clear();
                LoadData();
                isSortChanging = false;
                PropertyInfo verticalOffset = dataGridViewGroups.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                verticalOffset.SetValue(this.dataGridViewGroups, verticalScrollingOffset, null);
            }
        }
    }
}
