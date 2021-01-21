
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
            this.dataGridSubjects = new System.Windows.Forms.DataGridView();
            this.SubjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSubjects)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSubjects
            // 
            this.dataGridViewSubjects.AllowDrop = true;
            this.dataGridViewSubjects.AllowUserToResizeRows = false;
            this.dataGridViewSubjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSubjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SubjectName,
            this.Notes});
            this.dataGridViewSubjects.Location = new System.Drawing.Point(24, 12);
            this.dataGridViewSubjects.Name = "dataGridViewSubjects";
            this.dataGridViewSubjects.Size = new System.Drawing.Size(866, 525);
            this.dataGridViewSubjects.StandardTab = true;
            this.dataGridViewSubjects.TabIndex = 0;
            // 
            // dataGridSubjects
            // 
            this.dataGridSubjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSubjects.Location = new System.Drawing.Point(501, 225);
            this.dataGridSubjects.Name = "dataGridSubjects";
            this.dataGridSubjects.Size = new System.Drawing.Size(8, 8);
            this.dataGridSubjects.TabIndex = 1;
            // 
            // SubjectName
            // 
            this.SubjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SubjectName.DataPropertyName = "Notes";
            this.SubjectName.HeaderText = "Назва";
            this.SubjectName.Name = "SubjectName";
            // 
            // Notes
            // 
            this.Notes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Notes.DataPropertyName = "Notes";
            this.Notes.HeaderText = "Примітки";
            this.Notes.Name = "Notes";
            // 
            // SubjectsTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 558);
            this.Controls.Add(this.dataGridSubjects);
            this.Controls.Add(this.dataGridViewSubjects);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SubjectsTableForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSubjects)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSubjects;
        private System.Windows.Forms.DataGridView dataGridSubjects;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
    }
}