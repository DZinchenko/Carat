
namespace Carat
{
    partial class SelectWorkType
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
            this.dataGridViewSelectWorkType = new System.Windows.Forms.DataGridView();
            this.SelectWorkTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectWorkType)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSelectWorkType
            // 
            this.dataGridViewSelectWorkType.AllowUserToAddRows = false;
            this.dataGridViewSelectWorkType.AllowUserToDeleteRows = false;
            this.dataGridViewSelectWorkType.AllowUserToResizeColumns = false;
            this.dataGridViewSelectWorkType.AllowUserToResizeRows = false;
            this.dataGridViewSelectWorkType.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewSelectWorkType.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewSelectWorkType.EnableHeadersVisualStyles = false;
            this.dataGridViewSelectWorkType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSelectWorkType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectWorkTypeName});
            this.dataGridViewSelectWorkType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSelectWorkType.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSelectWorkType.MultiSelect = false;
            this.dataGridViewSelectWorkType.Name = "dataGridViewSelectWorkType";
            this.dataGridViewSelectWorkType.ReadOnly = true;
            this.dataGridViewSelectWorkType.Size = new System.Drawing.Size(933, 519);
            this.dataGridViewSelectWorkType.TabIndex = 0;
            this.dataGridViewSelectWorkType.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSelectWorkType_CellDoubleClick);
            // 
            // SelectWorkTypeName
            // 
            this.SelectWorkTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SelectWorkTypeName.HeaderText = "Назва";
            this.SelectWorkTypeName.Name = "SelectWorkTypeName";
            this.SelectWorkTypeName.ReadOnly = true;
            this.SelectWorkTypeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SelectWorkType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.dataGridViewSelectWorkType);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SelectWorkType";
            this.Text = "SelectWorkType";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectWorkType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSelectWorkType;
        private System.Windows.Forms.DataGridViewTextBoxColumn SelectWorkTypeName;
    }
}