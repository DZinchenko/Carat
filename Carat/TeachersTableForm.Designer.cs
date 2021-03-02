
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeachersTableForm));
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
            this.dataGridViewTeachers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTeachers_CellValueChanged);
            this.dataGridViewTeachers.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewTeachers_DataError);
            this.dataGridViewTeachers.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewTeachers_RowsRemoved);
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
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = "1.00";
            this.TeacherStake.DefaultCellStyle = dataGridViewCellStyle1;
            this.TeacherStake.HeaderText = "Кіл-ть штат. один.";
            this.TeacherStake.Name = "TeacherStake";
            this.TeacherStake.Width = 120;
            // 
            // TeacherPosition
            // 
            this.TeacherPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.NullValue = "<not set>";
            this.TeacherPosition.DefaultCellStyle = dataGridViewCellStyle2;
            this.TeacherPosition.HeaderText = "Посада";
            this.TeacherPosition.Items.AddRange(new object[] {
            "<not set>",
            "асистент",
            "викладач",
            "ст. викладач",
            "доцент",
            "професор"});
            this.TeacherPosition.Name = "TeacherPosition";
            this.TeacherPosition.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherPosition.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TeacherPosition.Width = 84;
            // 
            // TeacherRank
            // 
            this.TeacherRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.NullValue = "<not set>";
            this.TeacherRank.DefaultCellStyle = dataGridViewCellStyle3;
            this.TeacherRank.HeaderText = "Звання";
            this.TeacherRank.Items.AddRange(new object[] {
            "<not set>",
            "к. т. н.",
            "д. т. н."});
            this.TeacherRank.Name = "TeacherRank";
            this.TeacherRank.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherRank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TeacherRank.Width = 84;
            // 
            // TeacherDegree
            // 
            this.TeacherDegree.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.NullValue = "<not set>";
            this.TeacherDegree.DefaultCellStyle = dataGridViewCellStyle4;
            this.TeacherDegree.HeaderText = "Ступінь";
            this.TeacherDegree.Items.AddRange(new object[] {
            "<not set>",
            "доцент",
            "професор"});
            this.TeacherDegree.Name = "TeacherDegree";
            this.TeacherDegree.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherDegree.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TeacherDegree.Width = 84;
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
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.dataGridViewTeachers);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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