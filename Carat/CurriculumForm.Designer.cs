
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurriculumForm));
            this.panelCurriculumRight = new System.Windows.Forms.Panel();
            this.dataGridViewCurriculumSubjects = new System.Windows.Forms.DataGridView();
            this.CurriculumSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurriculumSubjectCourse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurriculumSubjectHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCurriculumLeftButtom = new System.Windows.Forms.Panel();
            this.listBoxSubjects = new System.Windows.Forms.ListBox();
            this.listBoxCourse = new System.Windows.Forms.ListBox();
            this.panelCurriculumLeft = new System.Windows.Forms.Panel();
            this.dataGridViewCurriculumWorkTypes = new System.Windows.Forms.DataGridView();
            this.CurriculumWorkType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurriculumWorkTypesHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.listBoxWorkTypes = new System.Windows.Forms.ListBox();
            this.panelCurriculumRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumSubjects)).BeginInit();
            this.panelCurriculumLeftButtom.SuspendLayout();
            this.panelCurriculumLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumWorkTypes)).BeginInit();
            this.SuspendLayout();
            // 
            // panelCurriculumRight
            // 
            this.panelCurriculumRight.Controls.Add(this.dataGridViewCurriculumSubjects);
            this.panelCurriculumRight.Controls.Add(this.panelCurriculumLeftButtom);
            this.panelCurriculumRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCurriculumRight.Location = new System.Drawing.Point(0, 0);
            this.panelCurriculumRight.Name = "panelCurriculumRight";
            this.panelCurriculumRight.Size = new System.Drawing.Size(619, 519);
            this.panelCurriculumRight.TabIndex = 1;
            // 
            // dataGridViewCurriculumSubjects
            // 
            this.dataGridViewCurriculumSubjects.AllowUserToResizeRows = false;
            this.dataGridViewCurriculumSubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCurriculumSubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CurriculumSubject,
            this.CurriculumSubjectCourse,
            this.CurriculumSubjectHours});
            this.dataGridViewCurriculumSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCurriculumSubjects.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridViewCurriculumSubjects.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCurriculumSubjects.Name = "dataGridViewCurriculumSubjects";
            this.dataGridViewCurriculumSubjects.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewCurriculumSubjects.Size = new System.Drawing.Size(619, 410);
            this.dataGridViewCurriculumSubjects.TabIndex = 5;
            this.dataGridViewCurriculumSubjects.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCurriculumSubjects_CellValueChanged);
            this.dataGridViewCurriculumSubjects.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewCurriculumSubjects_DataError);
            this.dataGridViewCurriculumSubjects.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewCurriculumSubjects_RowsRemoved);
            this.dataGridViewCurriculumSubjects.SelectionChanged += new System.EventHandler(this.dataGridViewCurriculumSubjects_SelectionChanged);
            // 
            // CurriculumSubject
            // 
            this.CurriculumSubject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CurriculumSubject.HeaderText = "Дисципліна";
            this.CurriculumSubject.Name = "CurriculumSubject";
            this.CurriculumSubject.ReadOnly = true;
            // 
            // CurriculumSubjectCourse
            // 
            this.CurriculumSubjectCourse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CurriculumSubjectCourse.HeaderText = "Курс";
            this.CurriculumSubjectCourse.Name = "CurriculumSubjectCourse";
            this.CurriculumSubjectCourse.ReadOnly = true;
            this.CurriculumSubjectCourse.Width = 58;
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
            // panelCurriculumLeftButtom
            // 
            this.panelCurriculumLeftButtom.Controls.Add(this.listBoxSubjects);
            this.panelCurriculumLeftButtom.Controls.Add(this.listBoxCourse);
            this.panelCurriculumLeftButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelCurriculumLeftButtom.Location = new System.Drawing.Point(0, 410);
            this.panelCurriculumLeftButtom.Name = "panelCurriculumLeftButtom";
            this.panelCurriculumLeftButtom.Size = new System.Drawing.Size(619, 109);
            this.panelCurriculumLeftButtom.TabIndex = 4;
            // 
            // listBoxSubjects
            // 
            this.listBoxSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSubjects.FormattingEnabled = true;
            this.listBoxSubjects.ItemHeight = 15;
            this.listBoxSubjects.Location = new System.Drawing.Point(0, 0);
            this.listBoxSubjects.Name = "listBoxSubjects";
            this.listBoxSubjects.Size = new System.Drawing.Size(604, 109);
            this.listBoxSubjects.TabIndex = 7;
            this.listBoxSubjects.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxSubjects_MouseDoubleClick);
            // 
            // listBoxCourse
            // 
            this.listBoxCourse.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBoxCourse.FormattingEnabled = true;
            this.listBoxCourse.ItemHeight = 15;
            this.listBoxCourse.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.listBoxCourse.Location = new System.Drawing.Point(604, 0);
            this.listBoxCourse.Name = "listBoxCourse";
            this.listBoxCourse.Size = new System.Drawing.Size(15, 109);
            this.listBoxCourse.TabIndex = 6;
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
            this.dataGridViewCurriculumWorkTypes.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCurriculumWorkTypes_CellValueChanged);
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
            dataGridViewCellStyle2.NullValue = "0";
            this.CurriculumWorkTypesHours.DefaultCellStyle = dataGridViewCellStyle2;
            this.CurriculumWorkTypesHours.HeaderText = "Години";
            this.CurriculumWorkTypesHours.Name = "CurriculumWorkTypesHours";
            this.CurriculumWorkTypesHours.Width = 72;
            // 
            // listBoxWorkTypes
            // 
            this.listBoxWorkTypes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxWorkTypes.FormattingEnabled = true;
            this.listBoxWorkTypes.ItemHeight = 15;
            this.listBoxWorkTypes.Location = new System.Drawing.Point(0, 410);
            this.listBoxWorkTypes.Name = "listBoxWorkTypes";
            this.listBoxWorkTypes.Size = new System.Drawing.Size(280, 109);
            this.listBoxWorkTypes.TabIndex = 0;
            this.listBoxWorkTypes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxWorkTypes_MouseDoubleClick);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumSubjects)).EndInit();
            this.panelCurriculumLeftButtom.ResumeLayout(false);
            this.panelCurriculumLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumWorkTypes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelCurriculumRight;
        private System.Windows.Forms.Panel panelCurriculumLeft;
        private System.Windows.Forms.ListBox listBoxWorkTypes;
        private System.Windows.Forms.DataGridView dataGridViewCurriculumWorkTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumWorkType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumWorkTypesHours;
        private System.Windows.Forms.DataGridView dataGridViewCurriculumSubjects;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumSubjectCourse;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumSubjectHours;
        private System.Windows.Forms.Panel panelCurriculumLeftButtom;
        private System.Windows.Forms.ListBox listBoxSubjects;
        private System.Windows.Forms.ListBox listBoxCourse;
    }
}