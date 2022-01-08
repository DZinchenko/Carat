
namespace Carat
{
    partial class RanksTableForm
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
            this.dataGridViewRanks = new System.Windows.Forms.DataGridView();
            this.RankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRanks)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewRanks
            // 
            this.dataGridViewRanks.AllowUserToResizeRows = false;
            this.dataGridViewRanks.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewRanks.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewRanks.EnableHeadersVisualStyles = false;
            this.dataGridViewRanks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRanks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RankName});
            this.dataGridViewRanks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRanks.EnableHeadersVisualStyles = false;
            this.dataGridViewRanks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRanks.Name = "dataGridViewRanks";
            this.dataGridViewRanks.RowHeadersWidth = 43;
            this.dataGridViewRanks.Size = new System.Drawing.Size(771, 450);
            this.dataGridViewRanks.TabIndex = 1;
            this.dataGridViewRanks.TabStop = false;
            this.dataGridViewRanks.CellValueChanged += this.dataGridViewRanks_CellValueChanged;
            this.dataGridViewRanks.RowsRemoved += this.dataGridViewRanks_RowsRemoved;
            this.dataGridViewRanks.DataError += this.dataGridViewRanks_DataError;
            // 
            // FacultyName
            // 
            this.RankName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RankName.HeaderText = "Назва";
            this.RankName.Name = "RankName";
            this.RankName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RanksTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(771, 450);
            this.Controls.Add(this.dataGridViewRanks);
            this.MinimumSize = new System.Drawing.Size(602, 167);
            this.Name = "RanksTableForm";
            this.Text = "Наукові ступені";
            this.FormClosed += this.RanksForm_FormClosed;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRanks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewRanks;
        private System.Windows.Forms.DataGridViewTextBoxColumn RankName;
    }
}