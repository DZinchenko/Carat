using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Carat.EF;
using Carat.Interfaces;

namespace Carat
{
    public partial class mainForm : Form
    {
        private Form activeForm = null;

        public mainForm()
        {
            InitializeComponent();
            InitializeSubmenu();

            using (var db = new CaratDbContext())
            {
            }
        }

        private void InitializeSubmenu()
        {
            panelTablesSubmenu.Visible = false;
            panelSectionSubmenu.Visible = false;
            panelReportSubmenu.Visible = false;
        }

        private void changeViewStateOfSubmenu(Panel panel)
        {
            panel.Visible = !(panel.Visible);
        }

        private void buttonTables_Click(object sender, EventArgs e)
        {
            changeViewStateOfSubmenu(panelTablesSubmenu);
        }

        private void buttonSection_Click(object sender, EventArgs e)
        {
            changeViewStateOfSubmenu(panelSectionSubmenu);
        }

        private void buttonReport_Click(object sender, EventArgs e)
        {
            changeViewStateOfSubmenu(panelReportSubmenu);
        }

        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            var loadDataForm = childForm as IDataUser;

            if (loadDataForm != null)
            {
                loadDataForm.LoadData();
            }

            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            panelWorkspace.Controls.Add(childForm);
            panelWorkspace.Tag = childForm;
            childForm.BringToFront();
            
            activeForm = childForm;

            childForm.Show();
        }

        private void toolStripMenuItem_Click(ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.ForeColor = Color.Black;
        }

        private void toolStripMenuItem_MouseEnter(ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.ForeColor = Color.Black;
        }

        private void toolStripMenuItem_DropDownClosed(ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.ForeColor = Color.White;
        }

        private void toolStripMenuItem_MouseLeave(ToolStripMenuItem toolStripMenuItem)
        {
            if (!toolStripMenuItem.Pressed)
            {
                toolStripMenuItem.ForeColor = Color.White;
            }
        }

        private void buttonSubjects_Click(object sender, EventArgs e)
        {
            openChildForm(new SubjectsTableForm());
        }

        private void fileToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            toolStripMenuItem_DropDownClosed(fileToolStripMenuItem);
        }

        private void fileToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            toolStripMenuItem_MouseEnter(fileToolStripMenuItem);
        }

        private void fileToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            toolStripMenuItem_MouseLeave(fileToolStripMenuItem);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_Click(fileToolStripMenuItem);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_Click(settingsToolStripMenuItem);
        }

        private void settingsToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            toolStripMenuItem_MouseEnter(settingsToolStripMenuItem);
        }

        private void settingsToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            toolStripMenuItem_MouseLeave(settingsToolStripMenuItem);
        }

        private void settingsToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            toolStripMenuItem_DropDownClosed(settingsToolStripMenuItem);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            toolStripMenuItem5.Checked = !toolStripMenuItem5.Checked;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            toolStripMenuItem6.Checked = !toolStripMenuItem6.Checked;
        }
    }
}
