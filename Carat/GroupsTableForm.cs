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
    public partial class GroupsTableForm : Form
    {
        MainForm m_parentForm = null;
        public GroupsTableForm(MainForm parentForm, string dbPath)
        {
            InitializeComponent();

            m_parentForm = parentForm;
        }

        private void GroupsTableForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.groupsForm = null;
            m_parentForm.SetButtonState();
        }
    }
}
