
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeacherAssignmentForm));
            this.panelTeacherAssignmentBottom = new System.Windows.Forms.Panel();
            this.dataGridViewTATeachers = new System.Windows.Forms.DataGridView();
            this.comboBoxTATeachers = new System.Windows.Forms.ComboBox();
            this.panelTeacherAssignmentTop = new System.Windows.Forms.Panel();
            this.dataGridViewTASubjects = new System.Windows.Forms.DataGridView();
            this.dataGridViewTAWorks = new System.Windows.Forms.DataGridView();
            this.TASubjects = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TASubjectCourse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAWork = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TAWorkFreeHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TATeacher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TATeacherHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TATeacherGroups = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelTeacherAssignmentBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTATeachers)).BeginInit();
            this.panelTeacherAssignmentTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTASubjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTAWorks)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTeacherAssignmentBottom
            // 
            this.panelTeacherAssignmentBottom.Controls.Add(this.dataGridViewTATeachers);
            this.panelTeacherAssignmentBottom.Controls.Add(this.comboBoxTATeachers);
            this.panelTeacherAssignmentBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTeacherAssignmentBottom.Location = new System.Drawing.Point(0, 363);
            this.panelTeacherAssignmentBottom.Name = "panelTeacherAssignmentBottom";
            this.panelTeacherAssignmentBottom.Size = new System.Drawing.Size(899, 156);
            this.panelTeacherAssignmentBottom.TabIndex = 0;
            // 
            // dataGridViewTATeachers
            // 
            this.dataGridViewTATeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTATeachers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TATeacher,
            this.TATeacherHours,
            this.TATeacherGroups});
            this.dataGridViewTATeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTATeachers.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTATeachers.Name = "dataGridViewTATeachers";
            this.dataGridViewTATeachers.Size = new System.Drawing.Size(899, 133);
            this.dataGridViewTATeachers.TabIndex = 1;
            this.dataGridViewTATeachers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTATeachers_CellValueChanged);
            this.dataGridViewTATeachers.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewTATeachers_RowsRemoved);
            // 
            // comboBoxTATeachers
            // 
            this.comboBoxTATeachers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.comboBoxTATeachers.FormattingEnabled = true;
            this.comboBoxTATeachers.Location = new System.Drawing.Point(0, 133);
            this.comboBoxTATeachers.Name = "comboBoxTATeachers";
            this.comboBoxTATeachers.Size = new System.Drawing.Size(899, 23);
            this.comboBoxTATeachers.TabIndex = 0;
            this.comboBoxTATeachers.SelectedIndexChanged += new System.EventHandler(this.comboBoxTATeachers_SelectedIndexChanged);
            // 
            // panelTeacherAssignmentTop
            // 
            this.panelTeacherAssignmentTop.Controls.Add(this.dataGridViewTASubjects);
            this.panelTeacherAssignmentTop.Controls.Add(this.dataGridViewTAWorks);
            this.panelTeacherAssignmentTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTeacherAssignmentTop.Location = new System.Drawing.Point(0, 0);
            this.panelTeacherAssignmentTop.Name = "panelTeacherAssignmentTop";
            this.panelTeacherAssignmentTop.Size = new System.Drawing.Size(899, 363);
            this.panelTeacherAssignmentTop.TabIndex = 1;
            // 
            // dataGridViewTASubjects
            // 
            this.dataGridViewTASubjects.AllowUserToAddRows = false;
            this.dataGridViewTASubjects.AllowUserToDeleteRows = false;
            this.dataGridViewTASubjects.AllowUserToResizeRows = false;
            this.dataGridViewTASubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTASubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TASubjects,
            this.TASubjectCourse});
            this.dataGridViewTASubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTASubjects.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridViewTASubjects.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTASubjects.Name = "dataGridViewTASubjects";
            this.dataGridViewTASubjects.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewTASubjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTASubjects.Size = new System.Drawing.Size(582, 363);
            this.dataGridViewTASubjects.TabIndex = 1;
            this.dataGridViewTASubjects.SelectionChanged += new System.EventHandler(this.dataGridViewTASubjects_SelectionChanged);
            // 
            // dataGridViewTAWorks
            // 
            this.dataGridViewTAWorks.AllowUserToAddRows = false;
            this.dataGridViewTAWorks.AllowUserToDeleteRows = false;
            this.dataGridViewTAWorks.AllowUserToResizeRows = false;
            this.dataGridViewTAWorks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTAWorks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TAWork,
            this.TAWorkFreeHours});
            this.dataGridViewTAWorks.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridViewTAWorks.Location = new System.Drawing.Point(582, 0);
            this.dataGridViewTAWorks.Name = "dataGridViewTAWorks";
            this.dataGridViewTAWorks.Size = new System.Drawing.Size(317, 363);
            this.dataGridViewTAWorks.TabIndex = 0;
            this.dataGridViewTAWorks.SelectionChanged += new System.EventHandler(this.dataGridViewTAWorks_SelectionChanged);
            // 
            // TASubjects
            // 
            this.TASubjects.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TASubjects.HeaderText = "Дисципліни";
            this.TASubjects.Name = "TASubjects";
            this.TASubjects.ReadOnly = true;
            this.TASubjects.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TASubjectCourse
            // 
            this.TASubjectCourse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TASubjectCourse.HeaderText = "Курс";
            this.TASubjectCourse.Name = "TASubjectCourse";
            this.TASubjectCourse.ReadOnly = true;
            this.TASubjectCourse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TASubjectCourse.Width = 39;
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
            this.TAWorkFreeHours.Width = 50;
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
            this.TATeacherHours.Width = 53;
            // 
            // TATeacherGroups
            // 
            this.TATeacherGroups.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TATeacherGroups.HeaderText = "Групи";
            this.TATeacherGroups.Name = "TATeacherGroups";
            this.TATeacherGroups.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // TeacherAssignmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.panelTeacherAssignmentTop);
            this.Controls.Add(this.panelTeacherAssignmentBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 301);
            this.Name = "TeacherAssignmentForm";
            this.Text = "Навантаження";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TeacherAssignment_FormClosed);
            this.panelTeacherAssignmentBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTATeachers)).EndInit();
            this.panelTeacherAssignmentTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTASubjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTAWorks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTeacherAssignmentBottom;
        private System.Windows.Forms.Panel panelTeacherAssignmentTop;
        private System.Windows.Forms.DataGridView dataGridViewTATeachers;
        private System.Windows.Forms.ComboBox comboBoxTATeachers;
        private System.Windows.Forms.DataGridView dataGridViewTASubjects;
        private System.Windows.Forms.DataGridView dataGridViewTAWorks;
        private System.Windows.Forms.DataGridViewTextBoxColumn TATeacher;
        private System.Windows.Forms.DataGridViewTextBoxColumn TATeacherHours;
        private System.Windows.Forms.DataGridViewButtonColumn TATeacherGroups;
        private System.Windows.Forms.DataGridViewTextBoxColumn TASubjects;
        private System.Windows.Forms.DataGridViewTextBoxColumn TASubjectCourse;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAWork;
        private System.Windows.Forms.DataGridViewTextBoxColumn TAWorkFreeHours;
    }
}