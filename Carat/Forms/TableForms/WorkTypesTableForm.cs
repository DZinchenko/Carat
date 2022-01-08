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
                dataGridViewWorkTypes.Rows.Add(work.Name, work.StudentHours.ToString(Tools.HoursAccuracy));
            }
        }

        public void LoadWorkTypes()
        {
            var workTypes = m_workTypeRepository.GetAllWorkTypes();

            if (workTypes.Count > 0)
                return;

            for (int i = 0; i < 38; ++i)
            {
                dataGridViewWorkTypes.Rows.Add();
            }

            dataGridViewWorkTypes.Rows[0].SetValues("Лекції", 0);
            dataGridViewWorkTypes.Rows[1].SetValues("Практич.заняття (комп. практ. семін.)", 0);
            dataGridViewWorkTypes.Rows[2].SetValues("Лабор.роб. (комп.практ.)", 0);
            dataGridViewWorkTypes.Rows[3].SetValues("Екзамени", 0);
            dataGridViewWorkTypes.Rows[4].SetValues("Заліки", 0);
            dataGridViewWorkTypes.Rows[5].SetValues("Контр.роб. (мод.,темат.)", 0);
            dataGridViewWorkTypes.Rows[6].SetValues("Курсові проекти", 0);
            dataGridViewWorkTypes.Rows[7].SetValues("Курсові роботи", 0);
            dataGridViewWorkTypes.Rows[8].SetValues("РГР, РР, ГР", 0);
            dataGridViewWorkTypes.Rows[9].SetValues("ДКР", 0);
            dataGridViewWorkTypes.Rows[10].SetValues("Реферати", 0);
            dataGridViewWorkTypes.Rows[11].SetValues("Консультації", 0);

            dataGridViewWorkTypes.Rows[12].SetValues("Індивід заняття зі студентами", 0);
            dataGridViewWorkTypes.Rows[13].SetValues("Індивід заняття з магістрами", 0);
            dataGridViewWorkTypes.Rows[14].SetValues("Індивід заняття за змішаною формою навч.", 0);

            dataGridViewWorkTypes.Rows[15].SetValues("Керівництво практиками", 0);

            dataGridViewWorkTypes.Rows[16].SetValues("Керівниц.атестац.роб.(бакалаврів)", 0);
            dataGridViewWorkTypes.Rows[17].SetValues("Керівниц.атестац.роб.(магістр ОПП)", 0);
            dataGridViewWorkTypes.Rows[18].SetValues("Керівниц.атестац.роб.(магістр ОНП)", 0);

            dataGridViewWorkTypes.Rows[19].SetValues("Консульт.атестац.роб.(бакалаврів)", 0);
            dataGridViewWorkTypes.Rows[20].SetValues("Консульт.атестац.роб.(магістр ОПП)", 0);
            dataGridViewWorkTypes.Rows[21].SetValues("Консульт.атестац.роб.(магістр ОНП)", 0);

            dataGridViewWorkTypes.Rows[22].SetValues("Рецензув.атестац.роб.(бакалаврів)", 0);
            dataGridViewWorkTypes.Rows[23].SetValues("Рецензув.атестац.роб.(магістр ОПП)", 0);
            dataGridViewWorkTypes.Rows[24].SetValues("Рецензув.атестац.роб.(магістр ОНП)", 0);

            dataGridViewWorkTypes.Rows[25].SetValues("Вступний іспит (магістр ОПП)", 0);
            dataGridViewWorkTypes.Rows[26].SetValues("Вступний іспит (магістр ОНП)", 0);
            dataGridViewWorkTypes.Rows[27].SetValues("Вступний іспит (аспірант)", 0);

            dataGridViewWorkTypes.Rows[28].SetValues("Робота в ЕК (бакалаврів)", 0);
            dataGridViewWorkTypes.Rows[29].SetValues("Робота в ЕК (магістр ОПП)", 0);
            dataGridViewWorkTypes.Rows[30].SetValues("Робота в ЕК (магістр ОНП)", 0);

            dataGridViewWorkTypes.Rows[31].SetValues("Керівництво (аспірантами)", 0);
            dataGridViewWorkTypes.Rows[32].SetValues("Керівництво (здобувач., стаж.)", 0);
            dataGridViewWorkTypes.Rows[33].SetValues("Заняття з аспірантами", 0);
            dataGridViewWorkTypes.Rows[34].SetValues("Консульт.докторантів", 0);
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
                dataGridViewWorkTypes.Rows[i].SetValues(workTypes[i].Name, workTypes[i].StudentHours.ToString(Tools.HoursAccuracy));
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
