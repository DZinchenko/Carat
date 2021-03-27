
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubjectsTableForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExportSubjects = new System.Windows.Forms.Button();
            this.dataGridViewSubjects = new System.Windows.Forms.DataGridView();
            this.SubjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonImportSubjects = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonImportSubjects);
            this.panel1.Controls.Add(this.buttonExportSubjects);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(899, 26);
            this.panel1.TabIndex = 1;
            // 
            // buttonExportSubjects
            // 
            this.buttonExportSubjects.BackColor = System.Drawing.Color.Transparent;
            this.buttonExportSubjects.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonExportSubjects.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonExportSubjects.FlatAppearance.BorderSize = 0;
            this.buttonExportSubjects.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttonExportSubjects.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExportSubjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportSubjects.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExportSubjects.Image = global::Carat.Properties.Resources.icons8_экспорт_xls_24;
            this.buttonExportSubjects.Location = new System.Drawing.Point(0, 0);
            this.buttonExportSubjects.Name = "buttonExportSubjects";
            this.buttonExportSubjects.Size = new System.Drawing.Size(24, 26);
            this.buttonExportSubjects.TabIndex = 1;
            this.buttonExportSubjects.UseVisualStyleBackColor = false;
            this.buttonExportSubjects.Click += new System.EventHandler(this.buttonExportSubjects_Click);
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
            this.dataGridViewSubjects.Location = new System.Drawing.Point(0, 26);
            this.dataGridViewSubjects.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridViewSubjects.Name = "dataGridViewSubjects";
            this.dataGridViewSubjects.RowHeadersWidth = 51;
            this.dataGridViewSubjects.Size = new System.Drawing.Size(899, 493);
            this.dataGridViewSubjects.TabIndex = 2;
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
            this.SubjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Notes
            // 
            this.Notes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Notes.DataPropertyName = "Notes";
            this.Notes.HeaderText = "Примітки";
            this.Notes.MinimumWidth = 6;
            this.Notes.Name = "Notes";
            this.Notes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // buttonImportSubjects
            // 
            this.buttonImportSubjects.BackColor = System.Drawing.Color.Transparent;
            this.buttonImportSubjects.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonImportSubjects.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonImportSubjects.FlatAppearance.BorderSize = 0;
            this.buttonImportSubjects.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttonImportSubjects.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonImportSubjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImportSubjects.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonImportSubjects.Image = global::Carat.Properties.Resources.icons8_импорт_xls_24_norm;
            this.buttonImportSubjects.Location = new System.Drawing.Point(24, 0);
            this.buttonImportSubjects.Name = "buttonImportSubjects";
            this.buttonImportSubjects.Size = new System.Drawing.Size(24, 26);
            this.buttonImportSubjects.TabIndex = 2;
            this.buttonImportSubjects.UseVisualStyleBackColor = false;
            // 
            // SubjectsTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.dataGridViewSubjects);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(350, 301);
            this.Name = "SubjectsTableForm";
            this.Text = "Дисципліни";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SubjectsTableForm_FormClosed);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubjects)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridViewSubjects;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.Button buttonExportSubjects;
        private System.Windows.Forms.Button buttonImportSubjects;
    }
}