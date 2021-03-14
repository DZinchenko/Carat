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
    public partial class MainForm : Form
    {
        private string m_dbName = "Carat.db";

        public Form subjectsForm = null;
        public Form groupsForm = null;
        public Form teachersForm = null;
        public Form workTypesForm = null;
        public Form curriculumForm = null;
        public Form TAForm = null;
        public bool IsFiltersValuesSelected = false;
        public bool IsRequiredFiltersValuesSelected = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeSubmenu();

            comboBoxEducType.SelectedIndex = 0;
            comboBoxEducForm.SelectedIndex = 0;
            comboBoxCourse.SelectedIndex = 0;
            comboBoxSemestr.SelectedIndex = 0;
            comboBoxEducLevel.SelectedIndex = 0;

            dataBaseStatelabel.Text = m_dbName;
        }

        public void SetButtonState()
        {
            if (subjectsForm == null)
                buttonSubjects.Image = Properties.Resources.icons8_круг_16;

            if (groupsForm == null)
                buttonGroups.Image = Properties.Resources.icons8_круг_16;

            if (teachersForm == null)
                buttonTeachers.Image = Properties.Resources.icons8_круг_16;

            if (curriculumForm == null)
                buttonCurriculum.Image = Properties.Resources.icons8_круг_16;

            if (TAForm == null)
                buttonTA.Image = Properties.Resources.icons8_круг_16;
        }

        private void InitializeSubmenu()
        {
            panelTablesSubmenu.Visible = false;
            panelSectionSubmenu.Visible = false;
            panelReportSubmenu.Visible = false;
        }

        private void UpdateDB(string filePath)
        {
            if (filePath != string.Empty)
            {
                m_dbName = filePath;
                dataBaseStatelabel.Text = m_dbName;

                subjectsForm?.Close();
                groupsForm?.Close();
                teachersForm?.Close();
                workTypesForm?.Close();
                curriculumForm?.Close();
                TAForm?.Close();

                subjectsForm = null;
                groupsForm = null;
                teachersForm = null;
                workTypesForm = null;
                curriculumForm = null;
                TAForm = null;
            }
        }

        private void SetIsFiltersValuesSelected()
        {
            IsRequiredFiltersValuesSelected = (comboBoxEducForm.SelectedIndex != 0)
                                      && (comboBoxEducType.SelectedIndex != 0)
                                      && (comboBoxEducLevel.SelectedIndex != 0);

            IsFiltersValuesSelected = (comboBoxCourse.SelectedIndex != 0) && IsRequiredFiltersValuesSelected;
        }

        private void NotifyIFilterUserForms()
        {
            var tempCurriculumForm = curriculumForm as IFilterUserForm;
            var tempTaForm = TAForm as IFilterUserForm;

            tempCurriculumForm?.FiltersStatesChanged();
            tempTaForm?.FiltersStatesChanged();
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

        private void openChildForm(Form form)
        {
            var loadDataForm = form as IDataUserForm;

            if (loadDataForm != null)
            {
                loadDataForm.LoadData();
            }

            form.TopLevel = false;
            form.WindowState = FormWindowState.Normal;
            form.Size = panelWorkspace.Size;

            panelWorkspace.Controls.Add(form);
            panelWorkspace.Tag = form;
            form.BringToFront();

            form.Show();
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
            curriculumForm?.Close();
            TAForm?.Close();

            if (subjectsForm == null)
            {
                buttonSubjects.Image = Properties.Resources.icons8_заполненный_круг_16;
                subjectsForm = new SubjectsTableForm(this, m_dbName);
                openChildForm(subjectsForm);
            }
            else {
                subjectsForm.BringToFront();
            }
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

        private void buttonGroups_Click(object sender, EventArgs e)
        {
            curriculumForm?.Close();
            TAForm?.Close();

            if (groupsForm == null)
            {
                buttonGroups.Image = Properties.Resources.icons8_заполненный_круг_16;
                groupsForm = new GroupsTableForm(this, m_dbName);
                openChildForm(groupsForm);
            }
            else
            {
                groupsForm.BringToFront();
            }
        }

        private void buttonTeachers_Click(object sender, EventArgs e)
        {
            curriculumForm?.Close();
            TAForm?.Close();

            if (teachersForm == null)
            {
                buttonTeachers.Image = Properties.Resources.icons8_заполненный_круг_16;
                teachersForm = new TeachersTableForm(this, m_dbName);
                openChildForm(teachersForm);
            }
            else
            {
                teachersForm.BringToFront();
            }
        }

        private void buttonHidePanel_Click(object sender, EventArgs e)
        {
            if (panelLeftMain.Visible)
            {
                panelLeftMain.Visible = false;
                buttonHidePanel.Image = Properties.Resources.icons8_закрыть_панель_32;
            }
            else
            {
                panelLeftMain.Visible = true;
                buttonHidePanel.Image = Properties.Resources.icons8_открыть_панель_32;
            }
        }

        private void comboBoxEducType_DropDownClosed(object sender, EventArgs e)
        {
            buttonHidePanel.Focus();
        }

        private void comboBoxEducForm_DropDownClosed(object sender, EventArgs e)
        {
            buttonHidePanel.Focus();
        }

        private void comboBoxCourse_DropDownClosed(object sender, EventArgs e)
        {
            buttonHidePanel.Focus();
        }

        private void comboBoxSemestr_DropDownClosed(object sender, EventArgs e)
        {
            buttonHidePanel.Focus();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void openDB(FileDialog openFileDialog)
        {
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Data bases|*.db";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                UpdateDB(openFileDialog.FileName);
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openDB(openFileDialog);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (SaveFileDialog openFileDialog = new SaveFileDialog())
            {
                openDB(openFileDialog);
            }
        }

        private void panelWorkspace_SizeChanged(object sender, EventArgs e)
        {
            if (subjectsForm != null)
                subjectsForm.Size = panelWorkspace.Size;

            if (groupsForm != null)
                groupsForm.Size = panelWorkspace.Size;

            if (teachersForm != null)
                teachersForm.Size = panelWorkspace.Size;

            if (workTypesForm != null)
                workTypesForm.Size = panelWorkspace.Size;

            if (curriculumForm != null)
                curriculumForm.Size = panelWorkspace.Size;

            if (TAForm != null)
                TAForm.Size = panelWorkspace.Size;
        }

        private void buttonWorkTypes_Click(object sender, EventArgs e)
        {

        }

        private void buttonCurriculum_Click(object sender, EventArgs e)
        {
            subjectsForm?.Close();
            groupsForm?.Close();
            teachersForm?.Close();
            workTypesForm?.Close();
            TAForm?.Close();

            if (curriculumForm == null)
            {
                buttonCurriculum.Image = Properties.Resources.icons8_заполненный_круг_16;
                curriculumForm = new CurriculumForm(this, m_dbName);
                openChildForm(curriculumForm);
            }
            else
            {
                curriculumForm.BringToFront();
            }
        }

        private void buttonTA_Click(object sender, EventArgs e)
        {
            subjectsForm?.Close();
            groupsForm?.Close();
            teachersForm?.Close();
            workTypesForm?.Close();
            curriculumForm?.Close();

            if (TAForm == null)
            {
                buttonTA.Image = Properties.Resources.icons8_заполненный_круг_16;
                TAForm = new TeacherAssignmentForm(this, m_dbName);
                openChildForm(TAForm);
            }
            else
            {
                TAForm.BringToFront();
            }
        }

        private void comboBoxEducType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetIsFiltersValuesSelected();
            NotifyIFilterUserForms();
        }

        private void comboBoxEducForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetIsFiltersValuesSelected();
            NotifyIFilterUserForms();
        }

        private void comboBoxCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetIsFiltersValuesSelected();
            NotifyIFilterUserForms();
        }

        private void comboBoxSemestr_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetIsFiltersValuesSelected();
            NotifyIFilterUserForms();
        }

        private void workTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curriculumForm?.Close();
            TAForm?.Close();

            if (workTypesForm == null)
            {
                workTypesForm = new WorkTypesTableForm(this, m_dbName);
                openChildForm(workTypesForm);
            }
            else
            {
                workTypesForm.BringToFront();
            }
        }

        private void baseToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            baseToolStripMenuItem.ForeColor = Color.Black;
        }

        private void baseToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (!baseToolStripMenuItem.Pressed)
                baseToolStripMenuItem.ForeColor = Color.White;
        }

        private void baseToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            baseToolStripMenuItem.ForeColor = Color.White;
        }

        private void baseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            baseToolStripMenuItem.ForeColor = Color.Black;
        }

        private void comboBoxEducLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetIsFiltersValuesSelected();
            NotifyIFilterUserForms();
        }
    }
}
