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
using Carat.Data.Entities;
using Carat.EF.Repositories;
using Carat.Interfaces;

namespace Carat
{
    public partial class PositionsTableForm : Form, IDataUserForm
    {
        private MainForm m_parentForm = null;
        private IPositionRepository m_positionRepo = null;
        private const string IncorrectNameMessage = "Некоректна назва посади!";
        private const string IncorrectDataMessage = "Некоректні дані!";
        private bool isLoading = false;

        public PositionsTableForm(MainForm parentForm, string dbName)
        {
            InitializeComponent();
            m_parentForm = parentForm;
            m_positionRepo = new PositionRepository(dbName);
        }

        public void LoadData()
        {
            isLoading = true;

            var positions = m_positionRepo.GetAllPositions();

            dataGridViewPositions.Rows.Clear();

            foreach (var position in positions)
            {
                dataGridViewPositions.Rows.Add(new object[] {position.Name, position.MinHours, position.MaxHours});
            }
            dataGridViewPositions[1, dataGridViewPositions.NewRowIndex].ReadOnly = true;
            dataGridViewPositions[2, dataGridViewPositions.NewRowIndex].ReadOnly = true;

            isLoading = false;
        }

        private void PositionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.positionsForm = null;
            m_parentForm.SetButtonState();
        }

        private void RemoveLastRow()
        {
            int index = dataGridViewPositions.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            dataGridViewPositions.Rows.Remove(dataGridViewPositions.Rows[index]);
        }

        private void dataGridViewPositions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_positionRepo == null || isLoading)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            isLoading = true;

            var positions = m_positionRepo.GetAllPositions();

            if (e.RowIndex < positions.Count)
            {
                var position = positions[e.RowIndex];

                if (!UpdateData(position, e, false))
                {
                    isLoading = false;
                    return;
                }

                m_positionRepo.UpdatePosition(position);
            }
            else
            {
                var position = new Position();

                if (!UpdateData(position, e, true))
                {
                    RemoveLastRow();
                    isLoading = false;
                    return;
                }
                m_positionRepo.AddPosition(position);
            }
            isLoading = false;
        }

        private bool isValidName(string name)
        {
            if (name == null || name == "")
            {
                return false;
            }

            var duplicatesCnt = 0;

            for (int i = 0; i < dataGridViewPositions.Rows.Count; ++i)
            {
                if (dataGridViewPositions[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    duplicatesCnt++;
                    if (duplicatesCnt > 1) { return false; }
                }
            }

            return true;
        }

        private bool UpdateData(Position position, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var Name = dataGridViewPositions[0, e.RowIndex].Value?.ToString()?.Trim();

                if (!isValidName(Name))
                {
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!isNewObject)
                    {
                        LoadData();
                    }

                    return false;
                }

                if (e.ColumnIndex == 0) {
                    position.Name = dataGridViewPositions[0, e.RowIndex].Value?.ToString()?.Trim();
                    if (isNewObject)
                    {
                        position.MinHours = 0;
                        position.MaxHours = 0;
                        dataGridViewPositions[1, e.RowIndex].Value = 0;
                        dataGridViewPositions[2, e.RowIndex].Value = 0;
                        dataGridViewPositions[1, e.RowIndex].ReadOnly = false;
                        dataGridViewPositions[2, e.RowIndex].ReadOnly = false;
                        dataGridViewPositions[1, e.RowIndex + 1].ReadOnly = true;
                        dataGridViewPositions[2, e.RowIndex + 1].ReadOnly = true;
                    }
                }
                else if (e.ColumnIndex == 1) {
                    var minHours = 0;
                    if (!int.TryParse(dataGridViewPositions[1, e.RowIndex].Value?.ToString(), out minHours))
                    {
                        MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridViewPositions[1, e.RowIndex].Value = position.MinHours;
                        return false;
                    }
                    position.MinHours = minHours;
                }
                else if (e.ColumnIndex == 2)
                {
                    var maxHours = 0;
                    if (!int.TryParse(dataGridViewPositions[2, e.RowIndex].Value?.ToString(), out maxHours))
                    {
                        MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    position.MaxHours = maxHours;
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

        private void dataGridViewPositions_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (isLoading) { return; }
            var positions = m_positionRepo.GetAllPositions();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= positions.Count)
            {
                return;
            }

            for (int i = 0; i < e.RowCount; ++i)
            {
                m_positionRepo.RemovePosition(positions[i + e.RowIndex]);
            }
        }

        private void dataGridViewPositions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}