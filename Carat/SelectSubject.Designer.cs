
namespace Carat
{
    partial class SelectSubject
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
            this.dataGridViewSelectSubject = new System.Windows.Forms.DataGridView();
            this.SelectSubjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelectSubjectCourse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectSubject)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSelectSubject
            // 
            this.dataGridViewSelectSubject.AllowUserToAddRows = false;
            this.dataGridViewSelectSubject.AllowUserToDeleteRows = false;
            this.dataGridViewSelectSubject.AllowUserToResizeColumns = false;
            this.dataGridViewSelectSubject.AllowUserToResizeRows = false;
            this.dataGridViewSelectSubject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSelectSubject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectSubjectName,
            this.SelectSubjectCourse});
            this.dataGridViewSelectSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSelectSubject.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSelectSubject.MultiSelect = false;
            this.dataGridViewSelectSubject.Name = "dataGridViewSelectSubject";
            this.dataGridViewSelectSubject.ReadOnly = true;
            this.dataGridViewSelectSubject.Size = new System.Drawing.Size(933, 519);
            this.dataGridViewSelectSubject.TabIndex = 0;
            this.dataGridViewSelectSubject.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSelectSubject_CellDoubleClick);
            // 
            // SelectSubjectName
            // 
            this.SelectSubjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SelectSubjectName.HeaderText = "Назва";
            this.SelectSubjectName.Name = "SelectSubjectName";
            this.SelectSubjectName.ReadOnly = true;
            this.SelectSubjectName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SelectSubjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SelectSubjectCourse
            // 
            this.SelectSubjectCourse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SelectSubjectCourse.HeaderText = "Курс";
            this.SelectSubjectCourse.Name = "SelectSubjectCourse";
            this.SelectSubjectCourse.ReadOnly = true;
            this.SelectSubjectCourse.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SelectSubjectCourse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SelectSubjectCourse.Width = 39;
            // 
            // SelectSubject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.dataGridViewSelectSubject);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectSubject";
            this.Text = "SelectSubject";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectSubject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSelectSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn SelectSubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SelectSubjectCourse;
    }
}