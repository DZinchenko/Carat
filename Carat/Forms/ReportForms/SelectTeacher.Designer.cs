
namespace Carat
{
    partial class SelectTeacher
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
            this.dataGridViewSelectTeacher = new System.Windows.Forms.DataGridView();
            this.SelectTeacherName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectTeacher)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSelectTeacher
            // 
            this.dataGridViewSelectTeacher.AllowUserToAddRows = false;
            this.dataGridViewSelectTeacher.AllowUserToDeleteRows = false;
            this.dataGridViewSelectTeacher.AllowUserToResizeColumns = false;
            this.dataGridViewSelectTeacher.AllowUserToResizeRows = false;
            this.dataGridViewSelectTeacher.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewSelectTeacher.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewSelectTeacher.EnableHeadersVisualStyles = false;
            this.dataGridViewSelectTeacher.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSelectTeacher.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectTeacherName});
            this.dataGridViewSelectTeacher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSelectTeacher.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSelectTeacher.MultiSelect = false;
            this.dataGridViewSelectTeacher.Name = "dataGridViewSelectTeacher";
            this.dataGridViewSelectTeacher.ReadOnly = true;
            this.dataGridViewSelectTeacher.Size = new System.Drawing.Size(933, 519);
            this.dataGridViewSelectTeacher.TabIndex = 0;
            this.dataGridViewSelectTeacher.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSelectTeacher_CellDoubleClick);
            // 
            // SelectTeacherName
            // 
            this.SelectTeacherName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SelectTeacherName.HeaderText = "ПІБ";
            this.SelectTeacherName.Name = "SelectTeacherName";
            this.SelectTeacherName.ReadOnly = true;
            this.SelectTeacherName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SelectTeacherName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SelectTeacher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.dataGridViewSelectTeacher);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectTeacher";
            this.Text = "SelectTeacher";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectTeacher)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSelectTeacher;
        private System.Windows.Forms.DataGridViewTextBoxColumn SelectTeacherName;
    }
}