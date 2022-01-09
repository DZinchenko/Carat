
namespace Carat
{
    partial class TeacherAssignmentForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeacherAssignmentForm));
            this.dataGridViewTATeachers = new System.Windows.Forms.DataGridView();
            this.TATeacher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TATeacherHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxTATeachers = new System.Windows.Forms.ComboBox();
            this.panelTeacherAssignmentTop = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dataGridViewTASubjects = new System.Windows.Forms.DataGridView();
            this.TASubjects = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TASubjectCourse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dataGridViewTAWorks = new System.Windows.Forms.DataGridView();
            this.TAWork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAWorkFreeHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelGroups = new System.Windows.Forms.Panel();
            this.sqliteCommand1 = new Microsoft.Data.Sqlite.SqliteCommand();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTATeachers)).BeginInit();
            this.panelTeacherAssignmentTop.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTASubjects)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTAWorks)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTATeachers
            // 
            this.dataGridViewTATeachers.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ScrollBar;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTATeachers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTATeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTATeachers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TATeacher,
            this.TATeacherHours});
            this.dataGridViewTATeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTATeachers.EnableHeadersVisualStyles = false;
            this.dataGridViewTATeachers.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTATeachers.Name = "dataGridViewTATeachers";
            this.dataGridViewTATeachers.RowHeadersWidth = 43;
            this.dataGridViewTATeachers.Size = new System.Drawing.Size(543, 162);
            this.dataGridViewTATeachers.TabIndex = 1;
            this.dataGridViewTATeachers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTATeachers_CellValueChanged);
            this.dataGridViewTATeachers.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewTATeachers_RowsRemoved);
            this.dataGridViewTATeachers.SelectionChanged += new System.EventHandler(this.dataGridViewTATeachers_SelectionChanged);
            // 
            // TATeacher
            // 
            this.TATeacher.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TATeacher.HeaderText = "Викладач";
            this.TATeacher.Name = "TATeacher";
            this.TATeacher.ReadOnly = true;
            this.TATeacher.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TATeacherHours
            // 
            this.TATeacherHours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TATeacherHours.HeaderText = "Години";
            this.TATeacherHours.Name = "TATeacherHours";
            this.TATeacherHours.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TATeacherHours.Width = 67;
            // 
            // comboBoxTATeachers
            // 
            this.comboBoxTATeachers.AccessibleDescription = "";
            this.comboBoxTATeachers.AccessibleName = "";
            this.comboBoxTATeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTATeachers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxTATeachers.Location = new System.Drawing.Point(0, 0);
            this.comboBoxTATeachers.Name = "comboBoxTATeachers";
            this.comboBoxTATeachers.Size = new System.Drawing.Size(421, 25);
            this.comboBoxTATeachers.TabIndex = 0;
            this.comboBoxTATeachers.Tag = "";
            this.comboBoxTATeachers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxTATeachers_DrawItem);
            this.comboBoxTATeachers.SelectionChangeCommitted += new System.EventHandler(this.comboBoxTATeachers_SelectionChangeCommitted);
            // 
            // panelTeacherAssignmentTop
            // 
            this.panelTeacherAssignmentTop.Controls.Add(this.panel3);
            this.panelTeacherAssignmentTop.Controls.Add(this.panel2);
            this.panelTeacherAssignmentTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTeacherAssignmentTop.Location = new System.Drawing.Point(0, 0);
            this.panelTeacherAssignmentTop.Name = "panelTeacherAssignmentTop";
            this.panelTeacherAssignmentTop.Size = new System.Drawing.Size(899, 519);
            this.panelTeacherAssignmentTop.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(543, 519);
            this.panel3.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dataGridViewTASubjects);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(543, 334);
            this.panel6.TabIndex = 3;
            // 
            // dataGridViewTASubjects
            // 
            this.dataGridViewTASubjects.AllowUserToAddRows = false;
            this.dataGridViewTASubjects.AllowUserToDeleteRows = false;
            this.dataGridViewTASubjects.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ScrollBar;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTASubjects.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTASubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTASubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TASubjects,
            this.TASubjectCourse});
            this.dataGridViewTASubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTASubjects.EnableHeadersVisualStyles = false;
            this.dataGridViewTASubjects.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridViewTASubjects.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTASubjects.Name = "dataGridViewTASubjects";
            this.dataGridViewTASubjects.RowHeadersWidth = 43;
            this.dataGridViewTASubjects.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewTASubjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTASubjects.Size = new System.Drawing.Size(543, 334);
            this.dataGridViewTASubjects.TabIndex = 1;
            this.dataGridViewTASubjects.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTASubjects_ColumnHeaderMouseClick);
            this.dataGridViewTASubjects.SelectionChanged += new System.EventHandler(this.dataGridViewTASubjects_SelectionChanged);
            this.dataGridViewTASubjects.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewTASubjects_MouseClick);
            // 
            // TASubjects
            // 
            this.TASubjects.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TASubjects.HeaderText = "Дисципліни";
            this.TASubjects.Name = "TASubjects";
            this.TASubjects.ReadOnly = true;
            this.TASubjects.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // TASubjectCourse
            // 
            this.TASubjectCourse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TASubjectCourse.HeaderText = "Курс";
            this.TASubjectCourse.Name = "TASubjectCourse";
            this.TASubjectCourse.ReadOnly = true;
            this.TASubjectCourse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.TASubjectCourse.Width = 67;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 334);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 185);
            this.panel1.TabIndex = 2;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.dataGridViewTATeachers);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(543, 162);
            this.panel8.TabIndex = 3;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.panel9);
            this.panel7.Controls.Add(this.panel4);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 162);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(543, 23);
            this.panel7.TabIndex = 2;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.comboBoxTATeachers);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(122, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(421, 23);
            this.panel9.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(122, 23);
            this.panel4.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Вибрати викладача:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panelGroups);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(543, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(356, 519);
            this.panel2.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dataGridViewTAWorks);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(356, 334);
            this.panel5.TabIndex = 1;
            // 
            // dataGridViewTAWorks
            // 
            this.dataGridViewTAWorks.AllowUserToAddRows = false;
            this.dataGridViewTAWorks.AllowUserToDeleteRows = false;
            this.dataGridViewTAWorks.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ScrollBar;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTAWorks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTAWorks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTAWorks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TAWork,
            this.TAWorkFreeHours});
            this.dataGridViewTAWorks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTAWorks.EnableHeadersVisualStyles = false;
            this.dataGridViewTAWorks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTAWorks.Name = "dataGridViewTAWorks";
            this.dataGridViewTAWorks.RowHeadersWidth = 43;
            this.dataGridViewTAWorks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTAWorks.Size = new System.Drawing.Size(356, 334);
            this.dataGridViewTAWorks.TabIndex = 0;
            this.dataGridViewTAWorks.SelectionChanged += new System.EventHandler(this.dataGridViewTAWorks_SelectionChanged);
            // 
            // TAWork
            // 
            this.TAWork.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TAWork.HeaderText = "Вид роботи";
            this.TAWork.Name = "TAWork";
            this.TAWork.ReadOnly = true;
            this.TAWork.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TAWorkFreeHours
            // 
            this.TAWorkFreeHours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TAWorkFreeHours.HeaderText = "Вільно";
            this.TAWorkFreeHours.Name = "TAWorkFreeHours";
            this.TAWorkFreeHours.ReadOnly = true;
            this.TAWorkFreeHours.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TAWorkFreeHours.Width = 64;
            // 
            // panelGroups
            // 
            this.panelGroups.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelGroups.Location = new System.Drawing.Point(0, 334);
            this.panelGroups.Name = "panelGroups";
            this.panelGroups.Size = new System.Drawing.Size(356, 185);
            this.panelGroups.TabIndex = 0;
            // 
            // sqliteCommand1
            // 
            this.sqliteCommand1.CommandText = null;
            this.sqliteCommand1.CommandTimeout = 30;
            this.sqliteCommand1.Connection = null;
            this.sqliteCommand1.Transaction = null;
            this.sqliteCommand1.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // TeacherAssignmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.panelTeacherAssignmentTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 186);
            this.Name = "TeacherAssignmentForm";
            this.Text = "Навантаження";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TeacherAssignment_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTATeachers)).EndInit();
            this.panelTeacherAssignmentTop.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTASubjects)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTAWorks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelTeacherAssignmentTop;
        private System.Windows.Forms.DataGridView dataGridViewTATeachers;
        private System.Windows.Forms.ComboBox comboBoxTATeachers;
        private System.Windows.Forms.DataGridView dataGridViewTASubjects;
        private System.Windows.Forms.DataGridView dataGridViewTAWorks;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAWork;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAWorkFreeHours;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panelGroups;
        private System.Windows.Forms.DataGridViewTextBoxColumn TATeacher;
        private System.Windows.Forms.DataGridViewTextBoxColumn TATeacherHours;
        private Microsoft.Data.Sqlite.SqliteCommand sqliteCommand1;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TASubjects;
        private System.Windows.Forms.DataGridViewTextBoxColumn TASubjectCourse;
    }
}