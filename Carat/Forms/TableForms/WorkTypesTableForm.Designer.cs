
namespace Carat
{
    partial class WorkTypesTableForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkTypesTableForm));
            this.dataGridViewWorkTypes = new System.Windows.Forms.DataGridView();
            this.WorkTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkTypeStudentHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWorkTypes)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewWorkTypes
            // 
            this.dataGridViewWorkTypes.AllowUserToDeleteRows = false;
            this.dataGridViewWorkTypes.AllowUserToResizeRows = false;
            this.dataGridViewWorkTypes.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewWorkTypes.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewWorkTypes.EnableHeadersVisualStyles = false;
            this.dataGridViewWorkTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWorkTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WorkTypeName,
            this.WorkTypeStudentHours});
            this.dataGridViewWorkTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewWorkTypes.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewWorkTypes.Name = "dataGridViewWorkTypes";
            this.dataGridViewWorkTypes.Size = new System.Drawing.Size(899, 519);
            this.dataGridViewWorkTypes.TabIndex = 0;
            this.dataGridViewWorkTypes.TabStop = false;
            this.dataGridViewWorkTypes.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewWorkTypes_CellValueChanged);
            this.dataGridViewWorkTypes.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewWorkTypes_DataError);
            this.dataGridViewWorkTypes.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewWorkTypes_RowsRemoved);
            // 
            // WorkTypeName
            // 
            this.WorkTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WorkTypeName.HeaderText = "Назва";
            this.WorkTypeName.Name = "WorkTypeName";
            this.WorkTypeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // WorkTypeStudentHours
            // 
            this.WorkTypeStudentHours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.NullValue = "0";
            this.WorkTypeStudentHours.DefaultCellStyle = dataGridViewCellStyle1;
            this.WorkTypeStudentHours.HeaderText = "год./студ.";
            this.WorkTypeStudentHours.Name = "WorkTypeStudentHours";
            this.WorkTypeStudentHours.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.WorkTypeStudentHours.Width = 66;
            // 
            // WorkTypesTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.dataGridViewWorkTypes);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.MinimumSize = new System.Drawing.Size(350, 279);
            this.Name = "WorkTypesTableForm";
            this.Text = "Види робіт";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WorkTypesTableForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWorkTypes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewWorkTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkTypeStudentHours;
    }
}