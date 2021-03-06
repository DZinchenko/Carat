
namespace Carat
{
    partial class CurriculumForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurriculumForm));
            this.panelCurriculumRight = new System.Windows.Forms.Panel();
            this.panelCurriculumLeft = new System.Windows.Forms.Panel();
            this.listBoxSubjects = new System.Windows.Forms.ListBox();
            this.listBoxWorkTypes = new System.Windows.Forms.ListBox();
            this.dataGridViewCurriculumSubjects = new System.Windows.Forms.DataGridView();
            this.dataGridViewCurriculumWorkTypes = new System.Windows.Forms.DataGridView();
            this.CurriculumSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurriculumSubjectHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurriculumWorkType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurriculumWorkTypesHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCurriculumRight.SuspendLayout();
            this.panelCurriculumLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumSubjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumWorkTypes)).BeginInit();
            this.SuspendLayout();
            // 
            // panelCurriculumRight
            // 
            this.panelCurriculumRight.Controls.Add(this.dataGridViewCurriculumSubjects);
            this.panelCurriculumRight.Controls.Add(this.listBoxSubjects);
            this.panelCurriculumRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCurriculumRight.Location = new System.Drawing.Point(0, 0);
            this.panelCurriculumRight.Name = "panelCurriculumRight";
            this.panelCurriculumRight.Size = new System.Drawing.Size(619, 519);
            this.panelCurriculumRight.TabIndex = 1;
            // 
            // panelCurriculumLeft
            // 
            this.panelCurriculumLeft.Controls.Add(this.dataGridViewCurriculumWorkTypes);
            this.panelCurriculumLeft.Controls.Add(this.listBoxWorkTypes);
            this.panelCurriculumLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelCurriculumLeft.Location = new System.Drawing.Point(619, 0);
            this.panelCurriculumLeft.Name = "panelCurriculumLeft";
            this.panelCurriculumLeft.Size = new System.Drawing.Size(280, 519);
            this.panelCurriculumLeft.TabIndex = 0;
            // 
            // listBoxSubjects
            // 
            this.listBoxSubjects.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxSubjects.FormattingEnabled = true;
            this.listBoxSubjects.ItemHeight = 15;
            this.listBoxSubjects.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.listBoxSubjects.Location = new System.Drawing.Point(0, 410);
            this.listBoxSubjects.Name = "listBoxSubjects";
            this.listBoxSubjects.Size = new System.Drawing.Size(619, 109);
            this.listBoxSubjects.TabIndex = 0;
            // 
            // listBoxWorkTypes
            // 
            this.listBoxWorkTypes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxWorkTypes.FormattingEnabled = true;
            this.listBoxWorkTypes.ItemHeight = 15;
            this.listBoxWorkTypes.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.listBoxWorkTypes.Location = new System.Drawing.Point(0, 410);
            this.listBoxWorkTypes.Name = "listBoxWorkTypes";
            this.listBoxWorkTypes.Size = new System.Drawing.Size(280, 109);
            this.listBoxWorkTypes.TabIndex = 0;
            // 
            // dataGridViewCurriculumSubjects
            // 
            this.dataGridViewCurriculumSubjects.AllowUserToResizeRows = false;
            this.dataGridViewCurriculumSubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCurriculumSubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CurriculumSubject,
            this.CurriculumSubjectHours});
            this.dataGridViewCurriculumSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCurriculumSubjects.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridViewCurriculumSubjects.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCurriculumSubjects.Name = "dataGridViewCurriculumSubjects";
            this.dataGridViewCurriculumSubjects.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewCurriculumSubjects.Size = new System.Drawing.Size(619, 410);
            this.dataGridViewCurriculumSubjects.StandardTab = true;
            this.dataGridViewCurriculumSubjects.TabIndex = 1;
            // 
            // dataGridViewCurriculumWorkTypes
            // 
            this.dataGridViewCurriculumWorkTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCurriculumWorkTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CurriculumWorkType,
            this.CurriculumWorkTypesHours});
            this.dataGridViewCurriculumWorkTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCurriculumWorkTypes.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCurriculumWorkTypes.Name = "dataGridViewCurriculumWorkTypes";
            this.dataGridViewCurriculumWorkTypes.Size = new System.Drawing.Size(280, 410);
            this.dataGridViewCurriculumWorkTypes.TabIndex = 1;
            // 
            // CurriculumSubject
            // 
            this.CurriculumSubject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CurriculumSubject.HeaderText = "Дисципліна";
            this.CurriculumSubject.Name = "CurriculumSubject";
            this.CurriculumSubject.ReadOnly = true;
            // 
            // CurriculumSubjectHours
            // 
            this.CurriculumSubjectHours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.NullValue = "0";
            this.CurriculumSubjectHours.DefaultCellStyle = dataGridViewCellStyle1;
            this.CurriculumSubjectHours.HeaderText = "Обсяг";
            this.CurriculumSubjectHours.Name = "CurriculumSubjectHours";
            this.CurriculumSubjectHours.Width = 65;
            // 
            // CurriculumWorkType
            // 
            this.CurriculumWorkType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CurriculumWorkType.HeaderText = "Вид роботи";
            this.CurriculumWorkType.Name = "CurriculumWorkType";
            this.CurriculumWorkType.ReadOnly = true;
            // 
            // CurriculumWorkTypesHours
            // 
            this.CurriculumWorkTypesHours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CurriculumWorkTypesHours.HeaderText = "Години";
            this.CurriculumWorkTypesHours.Name = "CurriculumWorkTypesHours";
            this.CurriculumWorkTypesHours.Width = 72;
            // 
            // CurriculumForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.panelCurriculumRight);
            this.Controls.Add(this.panelCurriculumLeft);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 301);
            this.Name = "CurriculumForm";
            this.Text = "Навчальний план";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CurriculumForm_FormClosed);
            this.panelCurriculumRight.ResumeLayout(false);
            this.panelCurriculumLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumSubjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumWorkTypes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelCurriculumRight;
        private System.Windows.Forms.Panel panelCurriculumLeft;
        private System.Windows.Forms.ListBox listBoxSubjects;
        private System.Windows.Forms.ListBox listBoxWorkTypes;
        private System.Windows.Forms.DataGridView dataGridViewCurriculumSubjects;
        private System.Windows.Forms.DataGridView dataGridViewCurriculumWorkTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumSubjectHours;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumWorkType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumWorkTypesHours;
    }
}