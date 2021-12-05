
namespace Carat
{
    partial class FacultiesTableForm
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
            this.dataGridViewFaculties = new System.Windows.Forms.DataGridView();
            this.FacultyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFaculties)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewFaculties
            // 
            this.dataGridViewFaculties.AllowUserToDeleteRows = false;
            this.dataGridViewFaculties.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ScrollBar;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewFaculties.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewFaculties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFaculties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FacultyName});
            this.dataGridViewFaculties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFaculties.EnableHeadersVisualStyles = false;
            this.dataGridViewFaculties.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewFaculties.Name = "dataGridViewFaculties";
            this.dataGridViewFaculties.RowHeadersWidth = 43;
            this.dataGridViewFaculties.Size = new System.Drawing.Size(771, 450);
            this.dataGridViewFaculties.TabIndex = 1;
            this.dataGridViewFaculties.TabStop = false;
            this.dataGridViewFaculties.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFaculties_CellValueChanged);
            this.dataGridViewFaculties.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewFacultues_DataError);
            this.dataGridViewFaculties.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewFaculties_RowsRemoved);
            // 
            // FacultyName
            // 
            this.FacultyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FacultyName.HeaderText = "Назва";
            this.FacultyName.Name = "FacultyName";
            this.FacultyName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FacultiesTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(771, 450);
            this.Controls.Add(this.dataGridViewFaculties);
            this.MinimumSize = new System.Drawing.Size(602, 167);
            this.Name = "FacultiesTableForm";
            this.Text = "Факультети";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FacultiesForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFaculties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewFaculties;
        private System.Windows.Forms.DataGridViewTextBoxColumn FacultyName;
    }
}