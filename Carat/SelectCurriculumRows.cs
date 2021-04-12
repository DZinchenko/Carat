using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carat
{
    public struct SelectedRows
    {
        public int firstSemestrStart;
        public int firstSemestrEnd;
        public int secondSemestrStart;
        public int secondSemestrEnd;
    }
    public partial class SelectCurriculumRows : Form
    {
        private SelectedRows m_selectedRows;

        public SelectCurriculumRows()
        {
            InitializeComponent();

            buttonOK.DialogResult = DialogResult.OK;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_selectedRows.firstSemestrStart = Convert.ToInt32(numericUpDownStart1.Value);
            m_selectedRows.firstSemestrEnd = Convert.ToInt32(numericUpDownEnd1.Value);
            m_selectedRows.secondSemestrStart = Convert.ToInt32(numericUpDownStart2.Value);
            m_selectedRows.secondSemestrEnd = Convert.ToInt32(numericUpDownEnd2.Value);
        }

        public SelectedRows getValues()
        {
            return m_selectedRows;
        }
    }
}
