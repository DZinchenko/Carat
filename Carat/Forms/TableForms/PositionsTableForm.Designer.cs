
namespace Carat
{
    partial class PositionsTableForm
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
            this.dataGridViewPositions = new System.Windows.Forms.DataGridView();
            this.PositionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionMinHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionMaxHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPositions)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPositions
            // 
            this.dataGridViewPositions.AllowUserToDeleteRows = true;
            this.dataGridViewPositions.AllowUserToResizeRows = false;
            this.dataGridViewPositions.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewPositions.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewPositions.EnableHeadersVisualStyles = false;
            this.dataGridViewPositions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPositions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PositionName,
            this.PositionMinHours,
            this.PositionMaxHours});
            this.dataGridViewPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPositions.EnableHeadersVisualStyles = false;
            this.dataGridViewPositions.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPositions.Name = "dataGridViewPositions";
            this.dataGridViewPositions.RowHeadersWidth = 43;
            this.dataGridViewPositions.Size = new System.Drawing.Size(800, 450);
            this.dataGridViewPositions.TabIndex = 2;
            this.dataGridViewPositions.TabStop = false;
            this.dataGridViewPositions.CellValueChanged += this.dataGridViewPositions_CellValueChanged;
            this.dataGridViewPositions.RowsRemoved += this.dataGridViewPositions_RowsRemoved;
            this.dataGridViewPositions.DataError += dataGridViewPositions_DataError;
            // 
            // PositionName
            // 
            this.PositionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PositionName.HeaderText = "Назва посади";
            this.PositionName.Name = "PositionName";
            this.PositionName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PositionMinHours
            // 
            this.PositionMinHours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PositionMinHours.HeaderText = "Мінімальна кількість годин";
            this.PositionMinHours.Name = "PositionMinHours";
            this.PositionMinHours.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PositionMaxHours
            // 
            this.PositionMaxHours.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PositionMaxHours.HeaderText = "Максимальна кількість годин";
            this.PositionMaxHours.Name = "PositionMaxHours";
            this.PositionMaxHours.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PositionsTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewPositions);
            this.Name = "PositionsTableForm";
            this.Text = "Посади";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPositions)).EndInit();
            this.FormClosed += this.PositionsForm_FormClosed;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewPositions;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionMinHours;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionMaxHours;
    }
}