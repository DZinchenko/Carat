﻿using System;
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
using System.Reflection;

namespace Carat
{
    public partial class SubjectsTableForm : Form, IDataUserForm
    {
        private ISubjectRepository m_subjectRepository;
        private ICurriculumItemRepository m_curriculumItemRepository;
        private MainForm m_parentForm = null;
        private bool isSortChanging = false;
        private List<Subject> subjects;

        private const string IncorrectNameMessage = "Некоректна назва предмета!";
        private const string DeletionWithHoursSingleMessage = "Видалення не може бути виконано, тому що у дисципліни {disc} є ненульовий розподіл навчального часу.";

        public SubjectsTableForm(MainForm parentForm, string dbName)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_subjectRepository = new SubjectRepository(dbName);
            m_curriculumItemRepository = new CurriculumItemRepository(dbName);
        }

        public void LoadData()
        {
            this.subjects = m_subjectRepository.GetAllSubjects();
            dataGridViewSubjects.Rows.Clear();

            foreach (var subject in this.subjects)
            {
                dataGridViewSubjects.Rows.Add(subject.Name, subject.Notes);
            }
        }

        private void RemoveLastRow()
        {
            int index = dataGridViewSubjects.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            if (isSortChanging)
            {
                return;
            }

            dataGridViewSubjects.Rows.Remove(dataGridViewSubjects.Rows[index]);
        }

        private void SubjectsTableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.subjectsForm = null;
            m_parentForm.SetButtonState();
        }

        private void dataGridViewSubjects_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check that repository with data is available
            if (m_subjectRepository == null)
            {
                return;
            }

            var subject = new Subject();

            // Check that changed cell index is in data range 
            if (e.RowIndex < 0)
            {
                return;
            }

            // Getting data to object from dataGridView
            subject.Name = dataGridViewSubjects[0, e.RowIndex].Value?.ToString()?.Trim(); ;
            subject.Notes = dataGridViewSubjects[1, e.RowIndex].Value?.ToString();

            // Check that changed row is modified or new created 
            if (e.RowIndex < this.subjects.Count)
            {
                subject.Id = this.subjects[e.RowIndex].Id;

                // Call isValidName function wich checks that changed data is valid
                if (!isValidName(subject.Name))
                {
                    // Throw user readable error message and sync dataGridView data with DB data
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadData();
                    return;
                }

                // Save changed data to DB
                m_subjectRepository.UpdateSubject(subject);
            }
            else
            {
                // Call isValidName function wich checks that changed data is valid
                if (!isValidName(subject.Name))
                {
                    // Throw user readable error message and remove added row
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RemoveLastRow();
                    return;
                }

                // Add to DB new data
                m_subjectRepository.AddSubject(subject);
            }

            PerformSort();
        }

        private void dataGridViewSubjects_userDeletingRows(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (isSortChanging)
            {
                return;
            }

            var deletedSubject = this.subjects.ElementAt(e.Row.Index);

            if (m_subjectRepository.CheckIfHasHours(deletedSubject))
            {
                MessageBox.Show(
                    DeletionWithHoursSingleMessage.Replace("{disc}", deletedSubject.Name),
                    Tools.MessageBoxErrorTitle(),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else
            {
                m_curriculumItemRepository.RemoveCurriculumItemsBySubjectId(deletedSubject.Id);
                m_subjectRepository.RemoveSubject(deletedSubject);
                this.subjects.RemoveAt(e.Row.Index);
            }
        }

        private bool isValidName(string name)
        {
            int duplicatesCounter = 0;

            if (name == null || name == "")
            {
                return false;
            }

            for (int i = 0; i < dataGridViewSubjects.Rows.Count; ++i)
            {
                if (dataGridViewSubjects[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter > 1 ? false : true;
        }

        private void buttonExportSubjects_Click(object sender, EventArgs e)
        {
            using (var fileData = new FileStream(Tools.GetTempFilePathWithExtension(".xlsx"), FileMode.OpenOrCreate))
            {
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Дисципліни");
                var row = sheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Назва");
                row.CreateCell(1).SetCellValue("Примітки");

                var allSubjects = m_subjectRepository.GetAllSubjects();

                for (int i = 0, rowIndex = 1; i < allSubjects.Count; ++i, ++rowIndex)
                {
                    var dataRow = sheet.CreateRow(rowIndex);

                    dataRow.CreateCell(0).SetCellValue(allSubjects[i].Name);
                    dataRow.CreateCell(1).SetCellValue(allSubjects[i].Notes);
                }

                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);

                workbook.Write(fileData);

                System.Diagnostics.Process.Start(@fileData.Name);
            }
        }

        private void buttonImportSubjects_Click(object sender, EventArgs e)
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
                        var subjects = new List<Subject>();

                        if (sheet == null)
                        {
                            return;
                        }

                        dataGridViewSubjects.Rows.Clear();

                        for (int i = 1; i <= sheet.LastRowNum; ++i)
                        {
                            var row = sheet.GetRow(i);
                            var subject = new Subject();

                            subject.Name = row.GetCell(0)?.ToString();
                            subject.Notes = row.GetCell(1)?.ToString();
                            this.subjects.Add(subject);
                        }

                        m_subjectRepository.AddSubjects(subjects);

                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PerformSort()
        {
            if (!isSortChanging)
            {
                var horizontalScrollingOffset = dataGridViewSubjects.HorizontalScrollingOffset;
                var verticalScrollingOffset = dataGridViewSubjects.VerticalScrollingOffset;
                isSortChanging = true;
                dataGridViewSubjects.Rows.Clear();
                LoadData();
                isSortChanging = false;
                dataGridViewSubjects.HorizontalScrollingOffset = horizontalScrollingOffset;
                if (verticalScrollingOffset > 0)
                {
                    PropertyInfo verticalOffset = dataGridViewSubjects.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                    verticalOffset.SetValue(this.dataGridViewSubjects, verticalScrollingOffset, null);
                }
            }
        }
    }
}
