using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Carat.EF.Repositories;
using Carat.Data.Entities;
using Carat.Data.Repositories;
using Carat.Interfaces;

namespace Carat
{
    public partial class GroupsTableForm : Form, IDataUserForm
    {
        MainForm m_parentForm = null;
        IGroupRepository m_groupRepository = null;
        const string IncorrectNameMessage = "Некоректна назва групи";
        const string IncorrectDataMessage = "Некоректні дані";

        public GroupsTableForm(MainForm parentForm, string dbPath)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_groupRepository = new GroupRepository(dbPath);
        }

        public void LoadData()
        {
            var groups = m_groupRepository.GetAllGroups();

            foreach (var group in groups)
            {
                dataGridViewGroups.Rows.Add(
                    group.Course,
                    group.Name, 
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
            var groups = m_groupRepository.GetAllGroups();

            for (int i = 0; i < groups.Count; ++i)
            {
                dataGridViewGroups.Rows[i].SetValues(
                    groups[i].Course,
                    groups[i].Name,
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
            var groups = m_groupRepository.GetAllGroups();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= groups.Count)
            {
                return;
            }

            for (int i = 0; i < e.RowCount; ++i)
            {
                m_groupRepository.RemoveGroup(groups[i + e.RowIndex]);
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

            var groups = m_groupRepository.GetAllGroups();

            if (e.RowIndex < groups.Count)
            {
                var group = groups[e.RowIndex];

                if (!UpdateData(group, e, false))
                {
                    return;
                }

                m_groupRepository.UpdateGroup(group);
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
            }
        }

        private bool UpdateData(Group group, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try {
                var name = dataGridViewGroups[1, e.RowIndex].Value?.ToString()?.Trim();

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
                            group.Course = Convert.ToUInt32(dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim());
                            break;
                        }
                    case 1:
                        {
                            group.Name = dataGridViewGroups[e.ColumnIndex, e.RowIndex].Value?.ToString().Trim();
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
    }
}
