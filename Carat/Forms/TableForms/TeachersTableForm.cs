﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using Carat.EF.Repositories;
using Carat.Interfaces;
using System.IO;
using System.Reflection;
using NPOI.XSSF.UserModel;

namespace Carat
{
    public partial class TeachersTableForm : Form, IDataUserForm
    {
        MainForm m_parentForm = null;
        ITeacherRepository m_teacherRepository = null;
        IPositionRepository m_positionRepository = null;
        IRankRepository m_rankRepository = null;
        const string IncorrectNameMessage = "Некоректне ім'я викладача!";
        const string IncorrectDataMessage = "Некоректні дані!";
        const string NoPositionsMessage = "У базу даних не введено ні одної посади!";
        const string NoRanksMessage = "У базу даних не введено ні одного наукового ступеню!";
        bool isSortChanging = false;
        int sortColumnIndex = 0;

        private int defaultPositionId;
        private int defaultRankId;

        bool isLoading = false;

        public TeachersTableForm(MainForm parentForm, string dbPath)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_teacherRepository = new TeacherRepository(dbPath);
            m_positionRepository = new PositionRepository(dbPath);
            m_rankRepository = new RankRepository(dbPath);
        }

        private void TeachersTableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.teachersForm = null;
            m_parentForm.SetButtonState();
        }

        public void LoadData()
        {
            isLoading = true;

            var teachers = GetAllSortedTeachers();
            var positions = m_positionRepository.GetAllPositions();
            var ranks = m_rankRepository.GetAllRanks();

            TeacherPosition.Items.Clear();
            TeacherPosition.Items.AddRange(positions.Select(p => p.Name).ToArray());
            if (positions.Count > 0) 
            { 
                TeacherPosition.DefaultCellStyle.NullValue = positions[0].Name;
                defaultPositionId = positions[0].Id;
            }
            TeacherRank.Items.Clear();
            TeacherRank.Items.AddRange(ranks.Select(r => r.Name).ToArray());
            if (ranks.Count > 0) 
            { 
                TeacherRank.DefaultCellStyle.NullValue = ranks[0].Name;
                defaultRankId = ranks[0].Id;
            }

            dataGridViewTeachers.Rows.Clear();
            foreach (var teacher in teachers)
            {
                dataGridViewTeachers.Rows.Add(
                    teacher.Name,
                    teacher.StaffUnit,
                    positions.First(p => p.Id == teacher.PositionId).Name,
                    ranks.First(r => r.Id == teacher.RankId).Name,
                    teacher.Degree,
                    teacher.OccupForm,
                    teacher.Note);
            }

            isLoading = false;
        }

        private void RemoveLastRow()
        {
            int index = dataGridViewTeachers.Rows.Count - 2;

            if (isSortChanging)
            {
                return;
            }

            if (index < 0)
            {
                return;
            }

            dataGridViewTeachers.Rows.Remove(dataGridViewTeachers.Rows[index]);
        }

        private bool isValidName(string name)
        {
            int duplicatesCounter = 0;

            if (name == null || name == "")
            {
                return false;
            }

            for (int i = 0; i < dataGridViewTeachers.Rows.Count; ++i)
            {
                if (dataGridViewTeachers[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter > 1 ? false : true;
        }

        private void dataGridViewTeachers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_teacherRepository == null)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            var teachers = GetAllSortedTeachers();

            if (e.RowIndex < teachers.Count)
            {
                var teacher = teachers[e.RowIndex];

                if (!UpdateData(teacher, e, false))
                {
                    return;
                }

                m_teacherRepository.UpdateTeacher(teacher);
                PerformSort();
            }
            else
            {
                if (TeacherPosition.Items.Count == 0)
                {
                    MessageBox.Show(NoPositionsMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                    return;
                }

                if (TeacherRank.Items.Count == 0)
                {
                    MessageBox.Show(NoRanksMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                    return;
                }

                var teacher = new Teacher();

                if (!UpdateData(teacher, e, true))
                {
                    RemoveLastRow();
                    return;
                }

                m_teacherRepository.AddTeacher(teacher);
                PerformSort();
            }
        }

        private bool UpdateData(Teacher teacher, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var Name = dataGridViewTeachers[0, e.RowIndex].Value?.ToString()?.Trim();
                if (!isValidName(Name))
                {
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!isNewObject)
                    {
                        LoadData();
                    }

                    return false;
                }

                switch (e.ColumnIndex)
                {
                    case 0:
                        {
                            teacher.Name = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            if (isNewObject) 
                            { 
                                teacher.PositionId = defaultPositionId; 
                                teacher.RankId = defaultRankId; 
                            }
                            break;
                        }
                    case 1:
                        {
                            var staffUnit = Convert.ToDouble(dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString());

                            if (Tools.isLessThanZero(staffUnit))
                            {
                                throw new Exception();
                            }

                            teacher.StaffUnit = staffUnit;
                            break;
                        }
                    case 2:
                        {
                            teacher.PositionId = m_positionRepository.GetPosition(dataGridViewTeachers[e.ColumnIndex, e.RowIndex].FormattedValue?.ToString()?.Trim()).Id;
                            break;
                        }
                    case 3:
                        {
                            teacher.RankId = m_rankRepository.GetRank(dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim()).Id;
                            break;
                        }
                    case 4:
                        {
                            teacher.Degree = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 5:
                        {
                            teacher.OccupForm = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 6:
                        {
                            teacher.Note = dataGridViewTeachers[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    default: { return false; }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
                return false;
            }

            return true;
        }

        private void dataGridViewTeachers_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (isLoading) { return; }

            var teachers = GetAllSortedTeachers();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (isSortChanging)
            {
                return;
            }

            if (e.RowIndex >= teachers.Count)
            {
                return;
            }

            for (int i = e.RowIndex, limit = e.RowIndex + e.RowCount; i < limit; ++i)
            {
                if (i < teachers.Count)
                {
                    m_teacherRepository.RemoveTeacher(teachers[i]);
                }
            }
        }

        private void dataGridViewTeachers_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void buttonImportTeachers_Click(object sender, EventArgs e)
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
                        var teachers = new List<Teacher>();

                        if (sheet == null)
                        {
                            return;
                        }

                        dataGridViewTeachers.Rows.Clear();

                        for (int i = 1; i <= sheet.LastRowNum; ++i)
                        {
                            var row = sheet.GetRow(i);
                            var teacher = new Teacher();
                            double staffUnit = 1.0;

                            teacher.Name = row.GetCell(0)?.ToString();

                            try
                            {
                                staffUnit = Convert.ToDouble(row.GetCell(1)?.ToString());
                            }
                            catch (Exception)
                            { }

                            teacher.StaffUnit = staffUnit;

                            var positionName = row.GetCell(2)?.ToString();
                            var position = m_positionRepository.GetPosition(positionName);
                            if (position == null)
                            {
                                m_positionRepository.AddPosition(new Position { Name = positionName });
                                position = m_positionRepository.GetPosition(positionName);
                            }
                            teacher.PositionId = position.Id;

                            var rankName = row.GetCell(3)?.ToString();
                            rankName = rankName == null ? "" : rankName;
                            var rank = m_rankRepository.GetRank(rankName);
                            if (rank == null)
                            {
                                m_rankRepository.AddRank(new Rank { Name = rankName });
                                rank = m_rankRepository.GetRank(rankName);
                            }
                            teacher.RankId = rank.Id;

                            teacher.Degree = row.GetCell(4)?.ToString();

                            teacher.OccupForm = row.GetCell(5)?.ToString();
                            teacher.Note = row.GetCell(6)?.ToString();

                            teachers.Add(teacher);
                        }

                        m_teacherRepository.AddTeachers(teachers);

                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonExportTeachers_Click(object sender, EventArgs e)
        {
            using (var fileData = new FileStream(Tools.GetTempFilePathWithExtension(".xlsx"), FileMode.OpenOrCreate))
            {
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Викладачі");
                var row = sheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("ПІБ");
                row.CreateCell(1).SetCellValue("Кіл-ть штатної од.");
                row.CreateCell(2).SetCellValue("Посада");
                row.CreateCell(3).SetCellValue("Ступінь");
                row.CreateCell(4).SetCellValue("Звання");
                row.CreateCell(5).SetCellValue("Форма зайнятості");
                row.CreateCell(6).SetCellValue("Примітки");

                var allTeachers = GetAllSortedTeachers();
                var positions = m_positionRepository.GetAllPositions();
                var ranks = m_rankRepository.GetAllRanks();

                for (int i = 0, rowIndex = 1; i < allTeachers.Count; ++i, ++rowIndex)
                {
                    var dataRow = sheet.CreateRow(rowIndex);

                    dataRow.CreateCell(0).SetCellValue(allTeachers[i].Name);
                    dataRow.CreateCell(1).SetCellValue(allTeachers[i].StaffUnit.ToString(Tools.HoursAccuracy));
                    dataRow.CreateCell(2).SetCellValue(positions.First(p => p.Id == allTeachers[i].PositionId).Name);
                    dataRow.CreateCell(3).SetCellValue(ranks.First(r => r.Id == allTeachers[i].RankId).Name);
                    dataRow.CreateCell(4).SetCellValue(allTeachers[i].Degree);
                    dataRow.CreateCell(5).SetCellValue(allTeachers[i].OccupForm);
                    dataRow.CreateCell(6).SetCellValue(allTeachers[i].Note);
                }

                for (int i = 0; i <= 6; ++i)
                {
                    sheet.AutoSizeColumn(i);
                }

                workbook.Write(fileData);

                System.Diagnostics.Process.Start(@fileData.Name);
            }
        }

        private List<Teacher> GetAllSortedTeachers()
        {
            if (sortColumnIndex == 0)
            {
                return m_teacherRepository.GetAllTeachers(a => a.Name);
            }
            else
            {
                return m_teacherRepository.GetAllTeachers(a => a.StaffUnit + a.Name);
            }
        }

        private void PerformSort()
        {
            if (!isSortChanging)
            {
                var horizontalScrollingOffset = dataGridViewTeachers.HorizontalScrollingOffset;
                var verticalScrollingOffset = dataGridViewTeachers.VerticalScrollingOffset;
                isSortChanging = true;
                dataGridViewTeachers.Rows.Clear();
                LoadData();
                isSortChanging = false;
                if (verticalScrollingOffset > 0)
                {
                    PropertyInfo verticalOffset = dataGridViewTeachers.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                    verticalOffset.SetValue(this.dataGridViewTeachers, verticalScrollingOffset, null);
                }
            }
        }

        private void dataGridViewTeachers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
            {
                dataGridViewTeachers.EndEdit();
                sortColumnIndex = e.ColumnIndex;

                PerformSort();
            }
        }
    }
}
