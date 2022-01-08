
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonImportCurriculum = new System.Windows.Forms.Button();
            this.panelCurriculumLeftButtom = new System.Windows.Forms.Panel();
            this.listBoxSubjects = new System.Windows.Forms.ListBox();
            this.listBoxCourse = new System.Windows.Forms.ListBox();
            this.panelCurriculumLeft = new System.Windows.Forms.Panel();
            this.dataGridViewCurriculumWorkTypes = new System.Windows.Forms.DataGridView();
            this.CurriculumWorkType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurriculumWorkTypesHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBoxWorkTypes = new System.Windows.Forms.ListBox();
            this.panelCurriculumRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumSubjects)).BeginInit();
            this.panel1.SuspendLayout();
            this.panelCurriculumLeftButtom.SuspendLayout();
            this.panelCurriculumLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumWorkTypes)).BeginInit();
            this.SuspendLayout();
            // 
            // panelCurriculumRight
            // 
            this.panelCurriculumRight.Controls.Add(this.dataGridViewCurriculumSubjects);
            this.panelCurriculumRight.Controls.Add(this.panel1);
            this.panelCurriculumRight.Controls.Add(this.panelCurriculumLeftButtom);
            this.panelCurriculumRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCurriculumRight.Location = new System.Drawing.Point(0, 0);
            this.panelCurriculumRight.Name = "panelCurriculumRight";
            this.panelCurriculumRight.Size = new System.Drawing.Size(529, 519);
            this.panelCurriculumRight.TabIndex = 1;
            // 
            // dataGridViewCurriculumSubjects
            // 
            this.dataGridViewCurriculumSubjects.AllowUserToAddRows = false;
            this.dataGridViewCurriculumSubjects.AllowUserToResizeRows = false;
            this.dataGridViewCurriculumSubjects.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewCurriculumSubjects.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewCurriculumSubjects.EnableHeadersVisualStyles = false;
            this.dataGridViewCurriculumSubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCurriculumSubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CurriculumSubject,
            this.CurriculumSubjectCourse,
            this.CurriculumSubjectHours});
            this.dataGridViewCurriculumSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCurriculumSubjects.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridViewCurriculumSubjects.Location = new System.Drawing.Point(0, 26);
            this.dataGridViewCurriculumSubjects.Name = "dataGridViewCurriculumSubjects";
            this.dataGridViewCurriculumSubjects.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewCurriculumSubjects.Size = new System.Drawing.Size(529, 384);
            this.dataGridViewCurriculumSubjects.TabIndex = 5;
            this.dataGridViewCurriculumSubjects.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCurriculumSubjects_CellValueChanged);
            this.dataGridViewCurriculumSubjects.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewCurriculumSubjects_ColumnHeaderMouseClick);
            this.dataGridViewCurriculumSubjects.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewCurriculumSubjects_DataError);
            this.dataGridViewCurriculumSubjects.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewCurriculumSubjects_RowsRemoved);
            this.dataGridViewCurriculumSubjects.SelectionChanged += new System.EventHandler(this.dataGridViewCurriculumSubjects_SelectionChanged);
            this.dataGridViewCurriculumSubjects.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridViewCurriculumSubjects_UserDeletingRow);
            // 
            // CurriculumSubject
            // 
            this.CurriculumSubject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CurriculumSubject.HeaderText = "Дисципліна";
            this.CurriculumSubject.Name = "CurriculumSubject";
            this.CurriculumSubject.ReadOnly = true;
            this.CurriculumSubject.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // CurriculumSubjectCourse
            // 
            this.CurriculumSubjectCourse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CurriculumSubjectCourse.HeaderText = "Курс";
            this.CurriculumSubjectCourse.Name = "CurriculumSubjectCourse";
            this.CurriculumSubjectCourse.ReadOnly = true;
            this.CurriculumSubjectCourse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.CurriculumSubjectCourse.Width = 58;
            // 
            // CurriculumSubjectHours
            // 
            this.CurriculumSubjectHours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.NullValue = "0";
            this.CurriculumSubjectHours.DefaultCellStyle = dataGridViewCellStyle1;
            this.CurriculumSubjectHours.HeaderText = "Обсяг";
            this.CurriculumSubjectHours.Name = "CurriculumSubjectHours";
            this.CurriculumSubjectHours.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CurriculumSubjectHours.Width = 46;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonImportCurriculum);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(529, 26);
            this.panel1.TabIndex = 6;
            // 
            // buttonImportCurriculum
            // 
            this.buttonImportCurriculum.BackColor = System.Drawing.Color.Transparent;
            this.buttonImportCurriculum.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonImportCurriculum.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonImportCurriculum.FlatAppearance.BorderSize = 0;
            this.buttonImportCurriculum.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttonImportCurriculum.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonImportCurriculum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImportCurriculum.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonImportCurriculum.Image = global::Carat.Properties.Resources.icons8_импорт_xls_24_norm;
            this.buttonImportCurriculum.Location = new System.Drawing.Point(0, 0);
            this.buttonImportCurriculum.Name = "buttonImportCurriculum";
            this.buttonImportCurriculum.Size = new System.Drawing.Size(24, 26);
            this.buttonImportCurriculum.TabIndex = 3;
            this.buttonImportCurriculum.UseVisualStyleBackColor = false;
            this.buttonImportCurriculum.Click += new System.EventHandler(this.buttonImportCurriculum_Click);
            // 
            // panelCurriculumLeftButtom
            // 
            this.panelCurriculumLeftButtom.Controls.Add(this.listBoxSubjects);
            this.panelCurriculumLeftButtom.Controls.Add(this.listBoxCourse);
            this.panelCurriculumLeftButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelCurriculumLeftButtom.Location = new System.Drawing.Point(0, 410);
            this.panelCurriculumLeftButtom.Name = "panelCurriculumLeftButtom";
            this.panelCurriculumLeftButtom.Size = new System.Drawing.Size(529, 109);
            this.panelCurriculumLeftButtom.TabIndex = 4;
            // 
            // listBoxSubjects
            // 
            this.listBoxSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSubjects.FormattingEnabled = true;
            this.listBoxSubjects.ItemHeight = 15;
            this.listBoxSubjects.Location = new System.Drawing.Point(0, 0);
            this.listBoxSubjects.Name = "listBoxSubjects";
            this.listBoxSubjects.Size = new System.Drawing.Size(514, 109);
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
            "4"});
            this.listBoxCourse.Location = new System.Drawing.Point(514, 0);
            this.listBoxCourse.Name = "listBoxCourse";
            this.listBoxCourse.Size = new System.Drawing.Size(15, 109);
            this.listBoxCourse.TabIndex = 6;
            // 
            // panelCurriculumLeft
            // 
            this.panelCurriculumLeft.Controls.Add(this.dataGridViewCurriculumWorkTypes);
            this.panelCurriculumLeft.Controls.Add(this.panel2);
            this.panelCurriculumLeft.Controls.Add(this.listBoxWorkTypes);
            this.panelCurriculumLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelCurriculumLeft.Location = new System.Drawing.Point(529, 0);
            this.panelCurriculumLeft.Name = "panelCurriculumLeft";
            this.panelCurriculumLeft.Size = new System.Drawing.Size(370, 519);
            this.panelCurriculumLeft.TabIndex = 0;
            // 
            // dataGridViewCurriculumWorkTypes
            // 
            this.dataGridViewCurriculumWorkTypes.AllowUserToAddRows = false;
            this.dataGridViewCurriculumWorkTypes.AllowUserToDeleteRows = false;
            this.dataGridViewCurriculumWorkTypes.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewCurriculumWorkTypes.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewCurriculumWorkTypes.EnableHeadersVisualStyles = false;
            this.dataGridViewCurriculumWorkTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCurriculumWorkTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CurriculumWorkType,
            this.CurriculumWorkTypesHours});
            this.dataGridViewCurriculumWorkTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCurriculumWorkTypes.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridViewCurriculumWorkTypes.Location = new System.Drawing.Point(0, 26);
            this.dataGridViewCurriculumWorkTypes.Name = "dataGridViewCurriculumWorkTypes";
            this.dataGridViewCurriculumWorkTypes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewCurriculumWorkTypes.Size = new System.Drawing.Size(370, 384);
            this.dataGridViewCurriculumWorkTypes.TabIndex = 1;
            this.dataGridViewCurriculumWorkTypes.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCurriculumWorkTypes_CellValueChanged);
            this.dataGridViewCurriculumWorkTypes.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewCurriculumWorkTypes_RowsRemoved);
            // 
            // CurriculumWorkType
            // 
            this.CurriculumWorkType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CurriculumWorkType.HeaderText = "Вид роботи";
            this.CurriculumWorkType.Name = "CurriculumWorkType";
            this.CurriculumWorkType.ReadOnly = true;
            this.CurriculumWorkType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CurriculumWorkTypesHours
            // 
            this.CurriculumWorkTypesHours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.NullValue = "0";
            this.CurriculumWorkTypesHours.DefaultCellStyle = dataGridViewCellStyle2;
            this.CurriculumWorkTypesHours.HeaderText = "Години";
            this.CurriculumWorkTypesHours.Name = "CurriculumWorkTypesHours";
            this.CurriculumWorkTypesHours.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CurriculumWorkTypesHours.Width = 53;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(370, 26);
            this.panel2.TabIndex = 2;
            // 
            // listBoxWorkTypes
            // 
            this.listBoxWorkTypes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxWorkTypes.FormattingEnabled = true;
            this.listBoxWorkTypes.ItemHeight = 15;
            this.listBoxWorkTypes.Location = new System.Drawing.Point(0, 410);
            this.listBoxWorkTypes.Name = "listBoxWorkTypes";
            this.listBoxWorkTypes.Size = new System.Drawing.Size(370, 109);
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
            this.MinimumSize = new System.Drawing.Size(700, 186);
            this.Name = "CurriculumForm";
            this.Text = "Навчальний план";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CurriculumForm_FormClosed);
            this.panelCurriculumRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurriculumSubjects)).EndInit();
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridView dataGridViewCurriculumSubjects;
        private System.Windows.Forms.Panel panelCurriculumLeftButtom;
        private System.Windows.Forms.ListBox listBoxSubjects;
        private System.Windows.Forms.ListBox listBoxCourse;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumWorkType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumWorkTypesHours;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonImportCurriculum;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumSubjectCourse;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurriculumSubjectHours;
    }
}