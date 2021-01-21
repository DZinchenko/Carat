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
            
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            panelWorkspace.Controls.Add(childForm);
            panelWorkspace.Tag = childForm;
            childForm.BringToFront();
            
            activeForm = childForm;

            childForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openChildForm(new SubjectsTableForm());
        }
    }
}
