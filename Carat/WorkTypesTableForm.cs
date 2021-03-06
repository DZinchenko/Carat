using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Carat.Data.Repositories;
using Carat.Interfaces;
using Carat.Data.Entities;
using Carat.EF.Repositories;

namespace Carat
{
    public partial class WorkTypesTableForm : Form, IDataUser
    {
        private IWorkTypeRepository m_workTypeRepository;
        private MainForm m_parentForm = null;
        private const string IncorrectNameMessage = "Некоректна назва типу роботи!";
        private const string IncorrectDataMessage = "Некоректні дані!";

        public WorkTypesTableForm(MainForm parentForm, string dbName)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_workTypeRepository = new WorkTypeRepository(dbName);
        }

        public void LoadData()
        {
            var workTypes = m_workTypeRepository.GetAllWorkTypes();

            foreach (var work in workTypes)
            {
                dataGridViewWorkTypes.Rows.Add(work.Name, work.StudentHours);
            }
        }

        private void WorkTypesTableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.workTypesForm = null;
            m_parentForm.SetButtonState();
        }

        private void SyncData()
        {
            var workTypes = m_workTypeRepository.GetAllWorkTypes();

            for (int i = 0; i < workTypes.Count; ++i)
            {
                dataGridViewWorkTypes.Rows[i].SetValues(workTypes[i].Name, workTypes[i].StudentHours);
            }
        }

        private void RemoveLastRow()
        {
            int index = dataGridViewWorkTypes.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            dataGridViewWorkTypes.Rows.Remove(dataGridViewWorkTypes.Rows[index]);
        }

        private void dataGridViewWorkTypes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_workTypeRepository == null)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            var workTypes = m_workTypeRepository.GetAllWorkTypes();

            if (e.RowIndex < workTypes.Count)
            {
                var work = workTypes[e.RowIndex];

                if (!UpdateData(work, e, false))
                {
                    return;
                }

                m_workTypeRepository.UpdateWorkType(work);
            }
            else
            {
                var work = new WorkType();

                if (!UpdateData(work, e, true))
                {
                    RemoveLastRow();
                    return;
                }
                m_workTypeRepository.AddWorkType(work);
            }
        }

        private bool isValidName(string name)
        {
            int duplicatesCounter = 0;

            if (name == null || name == "")
            {
                return false;
            }

            for (int i = 0; i < dataGridViewWorkTypes.Rows.Count; ++i)
            {
                if (dataGridViewWorkTypes[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter > 1 ? false : true;
        }

        private bool UpdateData(WorkType work, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var Name = dataGridViewWorkTypes[0, e.RowIndex].Value?.ToString()?.Trim();
                
                if (!isValidName(Name))
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
                            work.Name = dataGridViewWorkTypes[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
                            break;
                        }
                    case 1:
                        {
                            work.StudentHours = Convert.ToDouble(dataGridViewWorkTypes[e.ColumnIndex, e.RowIndex].Value?.ToString());

                            if (Tools.isLessThanZero(work.StudentHours))
                            {
                                throw new Exception();
                            }

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

        private void dataGridViewWorkTypes_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var workTypes = m_workTypeRepository.GetAllWorkTypes();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= workTypes.Count)
            {
                return;
            }

            for (int i = 0; i < e.RowCount; ++i)
            {
                m_workTypeRepository.RemoveWorkType(workTypes[i + e.RowIndex]);
            }
        }

        private void dataGridViewWorkTypes_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
