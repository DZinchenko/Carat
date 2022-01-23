using System;
using System.Globalization;
using System.IO;
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
using Carat.Data.Entities;
using Carat.Data.Repositories;
using Carat.EF.Repositories;
using Carat.Forms;

namespace Carat
{
    public partial class MainForm : Form
    {
        private string m_dbName = Directory.GetCurrentDirectory() + "\\Carat.db";
        private string m_lastModeDbName = Directory.GetCurrentDirectory() + "\\LastMode.db";
        private uint radioButtonNotificationCounter = 0;
        private object[] m_coursesValues = new object[] {"1", "2", "3", "4"};
        private uint selectedWindowsStyle = 0;
        private ILastModeRepository m_lastModeRepository = null;

        public Form subjectsForm = null;
        public Form groupsForm = null;
        public Form teachersForm = null;
        public Form workTypesForm = null;
        public Form curriculumForm = null;
        public Form TAForm = null;
        public Form excelReportsForm = null;
        public Form facultiesForm = null;
        public Form positionsForm = null;
        public Form ranksForm = null;
        public Form aboutForm = null;
        public bool IsFiltersValuesSelected = false;
        public bool IsRequiredFiltersValuesSelected = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeSubmenu();

            comboBoxEducType.SelectedIndex = 0;
            comboBoxEducForm.SelectedIndex = 0;
            comboBoxCourse.SelectedIndex = 0;
            comboBoxEducLevel.SelectedIndex = 0;

            radioButtonFirst.Checked = true;
            TurnOffAllSemesters();

            dataBaseStatelabel.Text = m_dbName;
            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ukr-uk");

        m_lastModeRepository = new LastModeRepository(m_lastModeDbName);
            var lastMode = m_lastModeRepository.GetLastMode();
            ApplyLastMode(lastMode);
            LoadWorksTypes();
        }

        private void ApplyLastMode(LastMode lastMode)
        {
            if (lastMode != null)
            {
                if (!File.Exists(lastMode.DbPath))
                {
                    SaveLastMode();
                }
                else
                {
                    setEducFormFilter(lastMode.EducForm);
                    setEducLevelFilter(lastMode.EducLevel);
                    setEducTypeFilter(lastMode.EducType);
                    setCourseFilter(lastMode.Course);
                    setSemestrFilter(lastMode.Semestr);
                    setIsEmptyWorks(lastMode.IsEmptyWorksFlag);
                    UpdateDB(lastMode.DbPath);
                }
            }
        }

        private void SaveLastMode()
        {
            var lastMode = new LastMode();
            lastMode.DbPath = m_dbName;
            lastMode.EducForm = getEducFormFilter();
            lastMode.EducLevel = getEducLevelFilter();
            lastMode.EducType = getEducTypeFilter();
            lastMode.Course = getCourseFilter();
            lastMode.Semestr = getSemesterFilter();
            lastMode.IsEmptyWorksFlag = getIsEmptyWorks();

            m_lastModeRepository.UpdateLastMode(lastMode);
        }

        private void LoadWorksTypes()
        {
            var workTypesForm = new WorkTypesTableForm(this, m_dbName);
            workTypesForm.LoadWorkTypes();
        }

        public void TurnOffAllSemesters()
        {
            if (radioButtonAll.Checked)
            {
                radioButtonAll.Checked = false;
                radioButtonFirst.Checked = true;
            }

            radioButtonAll.Enabled = false;
        }

        public void ShowProgress(int interval, string text)
        {
            workProgressBar.Value = 0;
            labelProgress.Text = text;
            workProgressBar.Maximum = interval;
            panelProgress.Visible = true;
            labelProgress.Visible = false;
            labelProgress.Visible = true;
            panelProgress.Show();
        }

        public void HideProgress()
        {
            panelProgress.Visible = false;
        }

        public void IncrementProgress()
        {
            workProgressBar.Increment(1);
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

            if (excelReportsForm == null)
                buttonExcelReports.Image = Properties.Resources.icons8_круг_16;
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
                excelReportsForm?.Close();
                TAForm?.Close();
                facultiesForm?.Close();
                positionsForm?.Close();
                ranksForm?.Close();
                aboutForm?.Close();

                subjectsForm = null;
                groupsForm = null;
                teachersForm = null;
                workTypesForm = null;
                curriculumForm = null;
                excelReportsForm = null;
                TAForm = null;
                facultiesForm = null;
                positionsForm = null;
                ranksForm = null;
                aboutForm = null;

                LoadWorksTypes();
            }
        }

        private void SetIsFiltersValuesSelected()
        {
            IsRequiredFiltersValuesSelected = (comboBoxEducForm.SelectedIndex != 0)
                                      && (comboBoxEducType.SelectedIndex != 0)
                                      && (comboBoxEducLevel.SelectedIndex != 0);

            IsFiltersValuesSelected = (comboBoxCourse.SelectedIndex != 0) && IsRequiredFiltersValuesSelected;
        }

        private void setEducTypeFilter(string educType)
        {
            foreach (var item in comboBoxEducType.Items)
            {
                if (item.ToString().Equals(educType))
                {
                    comboBoxEducType.SelectedItem = item;
                }
            }
        }

        private void setEducFormFilter(string educForm)
        {
            foreach (var item in comboBoxEducForm.Items)
            {
                if (item.ToString().Equals(educForm))
                {
                    comboBoxEducForm.SelectedItem = item;
                }
            }
        }

        private void setEducLevelFilter(string educLevel)
        {
            foreach (var item in comboBoxEducLevel.Items)
            {
                if (item.ToString().Equals(educLevel))
                {
                    comboBoxEducLevel.SelectedItem = item;
                }
            }
        }

        private void setCourseFilter(uint course)
        {
            string courseFilter = course.ToString();

            foreach (var item in comboBoxCourse.Items)
            {
                if (item.ToString().Equals(courseFilter))
                {
                    comboBoxCourse.SelectedItem = item;
                }
            }
        }

        private void setSemestrFilter(uint semestr)
        {
            switch (semestr)
            {
                case 0: 
                {
                    if (radioButtonAll.Enabled)
                    {
                        radioButtonAll.Checked = true;
                        radioButtonFirst.Checked = false;
                        radioButtonSecond.Checked = false;
                    }
                    break;
                }
                case 1:
                {
                    radioButtonFirst.Checked = true;
                    radioButtonAll.Checked = false;
                    radioButtonSecond.Checked = false;
                    break;
                }
                case 2:
                {
                    radioButtonSecond.Checked = true;
                    radioButtonFirst.Checked = false;
                    radioButtonAll.Checked = false;
                    break;
                }
                default:
                {
                    radioButtonFirst.Checked = true;
                    radioButtonAll.Checked = false;
                    radioButtonSecond.Checked = false;
                    break;
                }
            }
        }

        private void setIsEmptyWorks(bool flag)
        {
            checkBoxEmptyWorks.Checked = flag;
        }

        private string getEducTypeFilter()
        {
            return comboBoxEducType.SelectedItem.ToString();
        }

        private string getEducFormFilter()
        {
            return comboBoxEducForm.SelectedItem.ToString();
        }

        private string getEducLevelFilter()
        {
            return comboBoxEducLevel.SelectedItem.ToString();
        }

        private bool getIsEmptyWorks()
        {
            return checkBoxEmptyWorks.Checked;
        }

        private uint getSemesterFilter()
        {
            uint result = 0;

            if (radioButtonFirst.Checked)
            {
                result = 1;
            }
            else if (radioButtonSecond.Checked)
            {
                result = 2;
            }

            return result;
        }

        private uint getCourseFilter()
        {
            try
            {
                return Convert.ToUInt32(comboBoxCourse.SelectedItem.ToString());
            }
            catch (Exception)
            {
                return 0;
            }
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
            TurnOffAllSemesters();
            curriculumForm?.Close();
            TAForm?.Close();
            excelReportsForm?.Close();

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
            TurnOffAllSemesters();
            curriculumForm?.Close();
            TAForm?.Close();
            excelReportsForm?.Close();

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
            TurnOffAllSemesters();
            curriculumForm?.Close();
            TAForm?.Close();
            excelReportsForm?.Close();

            if (teachersForm == null)
            {
                buttonTeachers.Image = Properties.Resources.icons8_заполненный_круг_16;
                teachersForm = new TeachersTableForm(this, m_dbName);
                openChildForm(teachersForm);
            }
            else
            {
                var loadDataForm = teachersForm as IDataUserForm;
                if (loadDataForm != null)
                {
                    loadDataForm.LoadData();
                }

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
            SaveLastMode();
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

        private void UpdateWindows()
        {
            switch (selectedWindowsStyle)
            {
                case 0:
                case 1:
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

                    if (excelReportsForm != null)
                        excelReportsForm.Size = panelWorkspace.Size;

                    if (facultiesForm != null)
                        facultiesForm.Size = panelWorkspace.Size;

                    if (ranksForm != null)
                        ranksForm.Size = panelWorkspace.Size;

                    if (aboutForm != null)
                        aboutForm.Size = panelWorkspace.Size;

                    break;
                case 2:
                    ApplyShinglesMode();
                    break;
            }
        }

        private void panelWorkspace_SizeChanged(object sender, EventArgs e)
        {
            UpdateWindows();
        }

        private void buttonWorkTypes_Click(object sender, EventArgs e)
        {

        }

        private void buttonCurriculum_Click(object sender, EventArgs e)
        {
            TurnOffAllSemesters();
            subjectsForm?.Close();
            groupsForm?.Close();
            teachersForm?.Close();
            workTypesForm?.Close();
            excelReportsForm?.Close();
            TAForm?.Close();
            facultiesForm?.Close();
            ranksForm?.Close();
            aboutForm?.Close();

            if (curriculumForm == null)
            {
                buttonCurriculum.Image = Properties.Resources.icons8_заполненный_круг_16;
                curriculumForm = new CurriculumForm(this, 
                                                    m_dbName, 
                                                    getEducTypeFilter(), 
                                                    getEducFormFilter(), 
                                                    getCourseFilter(), 
                                                    getSemesterFilter(), 
                                                    getEducLevelFilter(),
                                                    getIsEmptyWorks(),
                                                    m_coursesValues);

                openChildForm(curriculumForm);
            }
            else
            {
                curriculumForm.BringToFront();
            }
        }

        private void buttonTA_Click(object sender, EventArgs e)
        {
            TurnOffAllSemesters();
            subjectsForm?.Close();
            groupsForm?.Close();
            teachersForm?.Close();
            workTypesForm?.Close();
            curriculumForm?.Close();
            excelReportsForm?.Close();
            facultiesForm?.Close();
            positionsForm?.Close();
            ranksForm?.Close();
            aboutForm?.Close();

            if (TAForm == null)
            {
                buttonTA.Image = Properties.Resources.icons8_заполненный_круг_16;
                TAForm = new TeacherAssignmentForm(this,
                                                    m_dbName,
                                                    getEducTypeFilter(),
                                                    getEducFormFilter(),
                                                    getCourseFilter(),
                                                    getSemesterFilter(),
                                                    getEducLevelFilter(),
                                                    getIsEmptyWorks());

                openChildForm(TAForm);
            }
            else
            {
                TAForm.BringToFront();
            }
        }

        private void ReopenFiltersUsersForms()
        {
            if (curriculumForm != null)
            {
                curriculumForm.Close();
                buttonCurriculum.PerformClick();
            }

            if (TAForm != null)
            {
                TAForm.Close();
                buttonTA.PerformClick();
            }

            if (excelReportsForm != null)
            {
                (excelReportsForm as ExcelReportsForm).SetFilters(
                    getEducTypeFilter(),
                    getEducFormFilter(),
                    getEducLevelFilter(),
                    getCourseFilter(),
                    getSemesterFilter());
            }
        }

        private void comboBoxEducType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetIsFiltersValuesSelected();
            ReopenFiltersUsersForms();
        }

        private void comboBoxEducForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetIsFiltersValuesSelected();
            ReopenFiltersUsersForms();
        }

        private void comboBoxCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetIsFiltersValuesSelected();
            ReopenFiltersUsersForms();
        }

        private void comboBoxSemestr_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetIsFiltersValuesSelected();
            ReopenFiltersUsersForms();
        }

        private void comboBoxEducLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex ;

            if (comboBoxEducLevel.SelectedIndex == 2)
            {
                selectedIndex = comboBoxCourse.SelectedIndex > 2 ? 0 : comboBoxCourse.SelectedIndex;
                comboBoxCourse.Items.Clear();
                m_coursesValues = new object[] { "1", "2" };
            }
            else 
            {
                selectedIndex = comboBoxCourse.SelectedIndex;
                comboBoxCourse.Items.Clear();
                m_coursesValues = new object[] { "1", "2", "3", "4" };
            }
            comboBoxCourse.Items.Add("<всі>");

            foreach (var item in m_coursesValues)
            {
                comboBoxCourse.Items.Add(item);
            }

            comboBoxCourse.SelectedIndex = selectedIndex;

            SetIsFiltersValuesSelected();
            ReopenFiltersUsersForms();
        }

        private void workTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TurnOffAllSemesters();
            curriculumForm?.Close();
            TAForm?.Close();
            excelReportsForm?.Close();
            facultiesForm?.Close();
            positionsForm?.Close();
            ranksForm?.Close();
            aboutForm?.Close();

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

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "Data bases|*.db";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(m_dbName,saveFileDialog.FileName);
                }
            }
        }

        private void radioButtonSecond_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNotificationCounter == 0)
            {
                ReopenFiltersUsersForms();
                ++radioButtonNotificationCounter;
            }
            else {
                radioButtonNotificationCounter = 0;
            }
        }

        private void radioButtonFirst_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNotificationCounter == 0)
            {
                ReopenFiltersUsersForms();
                ++radioButtonNotificationCounter;
            }
            else
            {
                radioButtonNotificationCounter = 0;
            }
        }

        private void radioButtonAll_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNotificationCounter == 0)
            {
                ReopenFiltersUsersForms();
                ++radioButtonNotificationCounter;
            }
            else
            {
                radioButtonNotificationCounter = 0;
            }
        }

        private void checkBoxEmptyWorks_CheckedChanged(object sender, EventArgs e)
        {
            if (curriculumForm != null)
            {
                var tempCurrForm = curriculumForm as CurriculumForm;
                tempCurrForm.UpdateWorks(getIsEmptyWorks());
            } else if (TAForm != null)
            {
                var tempTAForm = TAForm as TeacherAssignmentForm;
                tempTAForm.UpdateWorks(getIsEmptyWorks());
            }
        }

        private void windowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windowToolStripMenuItem.ForeColor = Color.Black;
        }

        private void windowToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            windowToolStripMenuItem.ForeColor = Color.White;
        }

        private void windowToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            windowToolStripMenuItem.ForeColor = Color.Black;
        }

        private void windowToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (!windowToolStripMenuItem.Pressed)
                windowToolStripMenuItem.ForeColor = Color.White;
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedWindowsStyle = 0;

            subjectsForm?.Close();
            groupsForm?.Close();
            teachersForm?.Close();
            workTypesForm?.Close();
            curriculumForm?.Close();
            excelReportsForm?.Close();
            TAForm?.Close();
            facultiesForm?.Close();
            positionsForm?.Close();
            ranksForm?.Close();
            aboutForm?.Close();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedWindowsStyle = 1;
            UpdateWindows();
        }

        private void ApplyShinglesMode()
        {
            List<Form> openedForms = new List<Form>();
            selectedWindowsStyle = 2;

            if (subjectsForm != null)
            {
                openedForms.Add(subjectsForm);
            }
            if (groupsForm != null)
            {
                openedForms.Add(groupsForm);
            }
            if (teachersForm != null)
            {
                openedForms.Add(teachersForm);
            }
            if (curriculumForm != null)
            {
                openedForms.Add(curriculumForm);
            }
            if (TAForm != null)
            {
                openedForms.Add(TAForm);
            }
            if (workTypesForm != null)
            {
                openedForms.Add(workTypesForm);
            }
            if (excelReportsForm != null)
            {
                openedForms.Add(excelReportsForm);
            }
            if (facultiesForm != null)
            {
                openedForms.Add(facultiesForm);
            }

            if (openedForms.Count <= 0)
            {
                return;
            }

            int formHeight = panelWorkspace.Height / openedForms.Count;

            foreach (var form in openedForms)
            {
                form.Height = formHeight;
                form.Dock = DockStyle.Top;
            }
        }

        private void shinglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedWindowsStyle = 2;
            //shinglesToolStripMenuItem.CheckOnClick = true;
            UpdateWindows();
        }

        private void buttonExcelReports_Click(object sender, EventArgs e)
        {
            subjectsForm?.Close();
            groupsForm?.Close();
            teachersForm?.Close();
            workTypesForm?.Close();
            curriculumForm?.Close();
            TAForm?.Close();
            facultiesForm?.Close();
            positionsForm?.Close();
            ranksForm?.Close();
            aboutForm?.Close();

            radioButtonAll.Enabled = true;
            if (excelReportsForm == null)
            {
                buttonExcelReports.Image = Properties.Resources.icons8_заполненный_круг_16;
                excelReportsForm = new ExcelReportsForm(this, 
                                                        m_dbName, 
                                                        getEducTypeFilter(), 
                                                        getEducFormFilter(),
                                                        getEducLevelFilter(),
                                                        getCourseFilter(),
                                                        getSemesterFilter());

                openChildForm(excelReportsForm);
            }
            else
            {
                excelReportsForm.BringToFront();
            }
        }

        private void facultiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TurnOffAllSemesters();
            curriculumForm?.Close();
            TAForm?.Close();
            excelReportsForm?.Close();
            workTypesForm?.Close();
            groupsForm?.Close();
            positionsForm?.Close();
            ranksForm?.Close();
            aboutForm?.Close();

            if (facultiesForm == null)
            {
                facultiesForm = new FacultiesTableForm(this, m_dbName);
                openChildForm(facultiesForm);
                facultiesForm.Size = this.panelWorkspace.Size;
            }
            else
            {
                facultiesForm.BringToFront();
            }
        }

        private void positionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TurnOffAllSemesters();
            curriculumForm?.Close();
            TAForm?.Close();
            excelReportsForm?.Close();
            workTypesForm?.Close();
            groupsForm?.Close();
            facultiesForm?.Close();
            ranksForm?.Close();
            aboutForm?.Close();

            if (positionsForm == null)
            {
                positionsForm = new PositionsTableForm(this, m_dbName);
                openChildForm(positionsForm);
                positionsForm.Size = this.panelWorkspace.Size;
            }
            else
            {
                positionsForm.BringToFront();
            }
        }

        private void ranksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TurnOffAllSemesters();
            curriculumForm?.Close();
            TAForm?.Close();
            excelReportsForm?.Close();
            workTypesForm?.Close();
            groupsForm?.Close();
            facultiesForm?.Close();
            positionsForm?.Close();
            aboutForm?.Close();

            if (ranksForm == null)
            {
                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 20);
                ranksForm = new RanksTableForm(this, m_dbName);
                openChildForm(ranksForm);
                ranksForm.Size = this.panelWorkspace.Size;
            }
            else
            {
                ranksForm.BringToFront();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TurnOffAllSemesters();
            curriculumForm?.Close();
            TAForm?.Close();
            excelReportsForm?.Close();
            workTypesForm?.Close();
            groupsForm?.Close();
            facultiesForm?.Close();
            positionsForm?.Close();
            ranksForm?.Close();

            if (aboutForm == null)
            {
                aboutForm = new AboutForm();
                openChildForm(aboutForm);
                aboutForm.Size = this.panelWorkspace.Size;
            }
            else
            {
                aboutForm.BringToFront();
            }
        }
    }
}
