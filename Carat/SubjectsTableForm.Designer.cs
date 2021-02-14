
namespace Carat
{
    partial class SubjectsTableForm
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
            this.dataGridViewSubjects = new System.Windows.Forms.DataGridView();
            this.SubjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSubjects
            // 
            this.dataGridViewSubjects.AllowDrop = true;
            this.dataGridViewSubjects.AllowUserToOrderColumns = true;
            this.dataGridViewSubjects.AllowUserToResizeRows = false;
            this.dataGridViewSubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SubjectName,
            this.Notes});
            this.dataGridViewSubjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSubjects.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSubjects.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridViewSubjects.Name = "dataGridViewSubjects";
            this.dataGridViewSubjects.RowHeadersWidth = 51;
            this.dataGridViewSubjects.Size = new System.Drawing.Size(899, 519);
            this.dataGridViewSubjects.StandardTab = true;
            this.dataGridViewSubjects.TabIndex = 0;
            this.dataGridViewSubjects.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSubjects_CellValueChanged);
            this.dataGridViewSubjects.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewSubjects_RowsRemoved);
            // 
            // SubjectName
            // 
            this.SubjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SubjectName.DataPropertyName = "Notes";
            this.SubjectName.HeaderText = "Назва";
            this.SubjectName.MinimumWidth = 6;
            this.SubjectName.Name = "SubjectName";
            // 
            // Notes
            // 
            this.Notes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Notes.DataPropertyName = "Notes";
            this.Notes.HeaderText = "Примітки";
            this.Notes.MinimumWidth = 6;
            this.Notes.Name = "Notes";
            // 
            // SubjectsTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.dataGridViewSubjects);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(350, 301);
            this.Name = "SubjectsTableForm";
            this.Text = "Дисципліни";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SubjectsTableForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSubjects;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
    }
}