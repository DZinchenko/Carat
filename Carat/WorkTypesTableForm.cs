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
    public partial class WorkTypesTableForm : Form, IDataUserForm
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

            if (workTypes.Count <= 0)
            {
                LoadWorkTypes();
                return;
            }

            foreach (var work in workTypes)
            {
                dataGridViewWorkTypes.Rows.Add(work.Name, work.StudentHours);
            }
        }

        private void LoadWorkTypes()
        {
            var workTypes = m_workTypeRepository.GetAllWorkTypes();

            if (workTypes.Count > 0)
                return;

            for (int i = 0; i < 38; ++i)
            {
                dataGridViewWorkTypes.Rows.Add();
            }

            dataGridViewWorkTypes.Rows[0].SetValues("Лекції", 0);
            dataGridViewWorkTypes.Rows[1].SetValues("Практич.заняття (семін.)", 0);
            dataGridViewWorkTypes.Rows[2].SetValues("Лабор.роб. (комп.практ.)", 0);
            dataGridViewWorkTypes.Rows[3].SetValues("Індивід.заняття зі студентами", 0);
            dataGridViewWorkTypes.Rows[4].SetValues("Екзамени", 0);
            dataGridViewWorkTypes.Rows[5].SetValues("Заліки", 0);
            dataGridViewWorkTypes.Rows[6].SetValues("Контр.роб. (мод.,темат.)", 0);
            dataGridViewWorkTypes.Rows[7].SetValues("Курсові проекти", 0);
            dataGridViewWorkTypes.Rows[8].SetValues("Курсові роботи", 0);
            dataGridViewWorkTypes.Rows[9].SetValues("РГР, РР, ГР", 0);
            dataGridViewWorkTypes.Rows[10].SetValues("ДКР", 0);
            dataGridViewWorkTypes.Rows[11].SetValues("Реферати", 0);
            dataGridViewWorkTypes.Rows[12].SetValues("Консультації", 0);

            dataGridViewWorkTypes.Rows[13].SetValues("Індивід заняття з магістрами", 10);
            dataGridViewWorkTypes.Rows[14].SetValues("Керівництво практиками", 8);
            dataGridViewWorkTypes.Rows[15].SetValues("Керівниц.атестац.роб.(бакалаврів)", 0);
            dataGridViewWorkTypes.Rows[16].SetValues("Керівниц.атестац.роб.(спеціалістів)", 21);
            dataGridViewWorkTypes.Rows[17].SetValues("Керівниц.атестац.роб.(магістрів)", 34);
            dataGridViewWorkTypes.Rows[18].SetValues("Консульт.атестац.роб.(бакалаврів)", 1);
            dataGridViewWorkTypes.Rows[19].SetValues("Консульт.атестац.роб.(спеціалістів)", 1);
            dataGridViewWorkTypes.Rows[20].SetValues("Консульт.атестац.роб.(магістрів)", 1);
            dataGridViewWorkTypes.Rows[21].SetValues("Консульт.по держ.екзамену (бакалаврів)", 0);
            dataGridViewWorkTypes.Rows[22].SetValues("Консульт.по держ.екзамену (спеціалістів)", 0);
            dataGridViewWorkTypes.Rows[23].SetValues("Консульт.по держ.екзамену (магістрів)", 0);

            dataGridViewWorkTypes.Rows[24].SetValues("Рецензув.атестац.роб.(бакалаврів)", 2);
            dataGridViewWorkTypes.Rows[25].SetValues("Рецензув.атестац.роб.(спеціалістів)", 3);
            dataGridViewWorkTypes.Rows[26].SetValues("Рецензув.атестац.роб.(магістрів)", 4);
            dataGridViewWorkTypes.Rows[27].SetValues("Робота в ДЕК (захист дипл., бакалаври)", 0);
            dataGridViewWorkTypes.Rows[28].SetValues("Робота в ДЕК (екзам.усний, бакалаври)", 0);
            dataGridViewWorkTypes.Rows[29].SetValues("Робота в ДЕК (екз.письмов., бакалаври)", 0);
            dataGridViewWorkTypes.Rows[30].SetValues("Робота в ДЕК (захист дипл., спеціалісти)", 0);
            dataGridViewWorkTypes.Rows[31].SetValues("Робота в ДЕК (екзамен, спеціалісти)", 0);
            dataGridViewWorkTypes.Rows[32].SetValues("Робота в ДЕК (магістрів)", 0);
            dataGridViewWorkTypes.Rows[33].SetValues("Керівництво (аспірантами)", 0);
            dataGridViewWorkTypes.Rows[34].SetValues("Керівництво (здобувач., стаж.)", 0);
            dataGridViewWorkTypes.Rows[35].SetValues("Заняття з аспірантами", 0);
            dataGridViewWorkTypes.Rows[36].SetValues("Консульт.докторантів", 0);
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
