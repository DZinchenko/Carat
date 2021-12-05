using System;
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

namespace Carat
{
    public partial class RanksTableForm : Form, IDataUserForm
    {
        private IRankRepository m_rankRepo;
        private MainForm m_parentForm = null;
        private const string IncorrectNameMessage = "Некоректна назва наукового ступеня!";
        private const string IncorrectDataMessage = "Некоректні дані!";
        private bool isLoading = false;

        public RanksTableForm(MainForm parentForm, string dbName)
        {
            InitializeComponent();

            m_parentForm = parentForm;
            m_rankRepo = new RankRepository(dbName);
        }

        public void LoadData()
        {
            isLoading = true;

            var ranks = m_rankRepo.GetAllRanks();

            dataGridViewRanks.Rows.Clear();

            foreach (var rank in ranks)
            {
                dataGridViewRanks.Rows.Add(rank.Name);
            }

            isLoading = false;
        }

        private void RanksForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.ranksForm = null;
            m_parentForm.SetButtonState();
        }

        private void RemoveLastRow()
        {
            int index = dataGridViewRanks.Rows.Count - 2;

            if (index < 0)
            {
                return;
            }

            dataGridViewRanks.Rows.Remove(dataGridViewRanks.Rows[index]);
        }

        private void dataGridViewRanks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_rankRepo == null)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            var ranks = m_rankRepo.GetAllRanks();

            if (e.RowIndex < ranks.Count)
            {
                var rank = ranks[e.RowIndex];

                if (!UpdateData(rank, e, false))
                {
                    return;
                }

                m_rankRepo.UpdateRank(rank);
            }
            else
            {
                var rank = new Rank();

                if (!UpdateData(rank, e, true))
                {
                    RemoveLastRow();
                    return;
                }
                m_rankRepo.AddRank(rank);
            }
        }

        private bool isValidName(string name)
        {
            if (name == null || name == "")
            {
                return false;
            }

            var duplicatesCnt = 0;

            for (int i = 0; i < dataGridViewRanks.Rows.Count; ++i)
            {
                if (dataGridViewRanks[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    duplicatesCnt++;
                    if (duplicatesCnt > 1) { return false; }
                }
            }

            return true;
        }

        private bool UpdateData(Rank rank, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var Name = dataGridViewRanks[0, e.RowIndex].Value?.ToString()?.Trim();

                if (!isValidName(Name))
                {
                    MessageBox.Show(IncorrectNameMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!isNewObject)
                    {
                        LoadData();
                    }

                    return false;
                }

                rank.Name = dataGridViewRanks[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim();
            }
            catch (Exception)
            {
                MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadData();
                return false;
            }

            return true;
        }

        private void dataGridViewRanks_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (isLoading) { return; }
            var ranks = m_rankRepo.GetAllRanks();

            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= ranks.Count)
            {
                return;
            }
            
            for (int i = 0; i < e.RowCount; ++i)
            {
                m_rankRepo.RemoveRank(ranks[i + e.RowIndex]);
            }
        }

        private void dataGridViewRanks_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
