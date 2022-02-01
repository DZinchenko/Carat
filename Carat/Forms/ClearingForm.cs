using Carat.Data.Repositories;
using Carat.EF.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carat.Forms
{
    public partial class ClearingForm : Form
    {
        private bool clearLoad { get => this.checkBoxLoad.Checked; }
        private bool clearCurriculum { get => this.checkBoxCurriculum.Checked; }
        private bool clearTeachers { get => this.checkBoxTeachers.Checked; }
        private bool clearGroups { get => this.checkBoxGroups.Checked; }
        private bool clearSubjects { get => this.checkBoxSubjects.Checked; }

        private IClearingRepository clearingRepository;
        private Action closeAllMainFormWindows;

        public ClearingForm(Action closeAllMainFormWindows, string dbName)
        {
            InitializeComponent();

            this.clearingRepository = new ClearingRepository(dbName);
            this.closeAllMainFormWindows = closeAllMainFormWindows;

            this.buttonClose.Click += (sender, e) => this.Close();

            this.checkBoxLoad.CheckedChanged += (sender, e) => this.SetElementsState();
            this.checkBoxCurriculum.CheckedChanged += (sender, e) => this.SetElementsState();
            this.checkBoxTeachers.CheckedChanged += (sender, e) => this.SetElementsState();
            this.checkBoxGroups.CheckedChanged += (sender, e) => this.SetElementsState();
            this.checkBoxSubjects.CheckedChanged += (sender, e) => this.SetElementsState();
            SetElementsState();
        }


        private void SetElementsState()
        {
            this.checkBoxCurriculum.Enabled = this.clearLoad;
            this.buttonClear.Enabled = this.clearLoad;
            this.checkBoxTeachers.Enabled = this.clearLoad && this.clearCurriculum;
            this.checkBoxGroups.Enabled = this.clearLoad && this.clearCurriculum;
            this.checkBoxSubjects.Enabled = this.clearLoad && this.clearCurriculum;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.closeAllMainFormWindows();
            if (this.clearLoad)
                this.clearingRepository.ClearLoad();
            if (this.clearCurriculum)
                this.clearingRepository.ClearCurriculum();
            if (this.clearTeachers)
                this.clearingRepository.ClearTeachers();
            if (this.clearGroups)
                this.clearingRepository.ClearGroups();
            if (this.clearSubjects)
                this.clearingRepository.ClearSubjects();
            this.Close();
        }
    }
}
