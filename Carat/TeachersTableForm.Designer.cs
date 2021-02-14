
namespace Carat
{
    partial class TeachersTableForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewTeachers = new System.Windows.Forms.DataGridView();
            this.TeacherName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeacherStake = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeacherPosition = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TeacherRank = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TeacherDegree = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TeacherNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTeachers
            // 
            this.dataGridViewTeachers.AllowUserToResizeRows = false;
            this.dataGridViewTeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTeachers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TeacherName,
            this.TeacherStake,
            this.TeacherPosition,
            this.TeacherRank,
            this.TeacherDegree,
            this.TeacherNotes});
            this.dataGridViewTeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTeachers.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridViewTeachers.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTeachers.Name = "dataGridViewTeachers";
            this.dataGridViewTeachers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewTeachers.RowTemplate.Height = 24;
            this.dataGridViewTeachers.Size = new System.Drawing.Size(899, 519);
            this.dataGridViewTeachers.StandardTab = true;
            this.dataGridViewTeachers.TabIndex = 0;
            // 
            // TeacherName
            // 
            this.TeacherName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TeacherName.HeaderText = "ПІБ";
            this.TeacherName.Name = "TeacherName";
            // 
            // TeacherStake
            // 
            this.TeacherStake.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = "1.00";
            this.TeacherStake.DefaultCellStyle = dataGridViewCellStyle11;
            this.TeacherStake.HeaderText = "Кіл-ть штат. один.";
            this.TeacherStake.Name = "TeacherStake";
            this.TeacherStake.Width = 120;
            // 
            // TeacherPosition
            // 
            this.TeacherPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle12.NullValue = "асистент";
            this.TeacherPosition.DefaultCellStyle = dataGridViewCellStyle12;
            this.TeacherPosition.HeaderText = "Посада";
            this.TeacherPosition.Items.AddRange(new object[] {
            "асистент",
            "викладач",
            "старший викладач",
            "доцент",
            "професор"});
            this.TeacherPosition.Name = "TeacherPosition";
            this.TeacherPosition.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherPosition.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TeacherPosition.Width = 80;
            // 
            // TeacherRank
            // 
            this.TeacherRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TeacherRank.HeaderText = "Звання";
            this.TeacherRank.Items.AddRange(new object[] {
            "к. т. н.",
            "д. т. н."});
            this.TeacherRank.Name = "TeacherRank";
            this.TeacherRank.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherRank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TeacherRank.Width = 71;
            // 
            // TeacherDegree
            // 
            this.TeacherDegree.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TeacherDegree.HeaderText = "Ступінь";
            this.TeacherDegree.Items.AddRange(new object[] {
            "доцент",
            "професор"});
            this.TeacherDegree.Name = "TeacherDegree";
            this.TeacherDegree.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherDegree.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TeacherDegree.Width = 74;
            // 
            // TeacherNotes
            // 
            this.TeacherNotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TeacherNotes.HeaderText = "Примітки";
            this.TeacherNotes.Name = "TeacherNotes";
            // 
            // TeachersTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.dataGridViewTeachers);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(700, 301);
            this.Name = "TeachersTableForm";
            this.Text = "Викладачі";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TeachersTableForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewTeachers;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherStake;
        private System.Windows.Forms.DataGridViewComboBoxColumn TeacherPosition;
        private System.Windows.Forms.DataGridViewComboBoxColumn TeacherRank;
        private System.Windows.Forms.DataGridViewComboBoxColumn TeacherDegree;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherNotes;
    }
}