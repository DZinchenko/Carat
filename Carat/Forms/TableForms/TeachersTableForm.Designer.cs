
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeachersTableForm));
            this.dataGridViewTeachers = new System.Windows.Forms.DataGridView();
            this.TeacherName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeacherStake = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeacherPosition = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TeacherRank = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TeacherDegree = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TeachersOccupForm = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TeacherNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTeachersTop = new System.Windows.Forms.Panel();
            this.buttonImportTeachers = new System.Windows.Forms.Button();
            this.buttonExportTeachers = new System.Windows.Forms.Button();
            this.panelTeachersBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).BeginInit();
            this.panelTeachersTop.SuspendLayout();
            this.panelTeachersBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewTeachers
            // 
            this.dataGridViewTeachers.AllowUserToResizeRows = false;
            this.dataGridViewTeachers.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.dataGridViewTeachers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridViewTeachers.EnableHeadersVisualStyles = false;
            this.dataGridViewTeachers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTeachers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TeacherName,
            this.TeacherStake,
            this.TeacherPosition,
            this.TeacherRank,
            this.TeacherDegree,
            this.TeachersOccupForm,
            this.TeacherNotes});
            this.dataGridViewTeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTeachers.EnableHeadersVisualStyles = false;
            this.dataGridViewTeachers.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.dataGridViewTeachers.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTeachers.Name = "dataGridViewTeachers";
            this.dataGridViewTeachers.RowHeadersWidth = 43;
            this.dataGridViewTeachers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewTeachers.RowTemplate.Height = 24;
            this.dataGridViewTeachers.Size = new System.Drawing.Size(899, 493);
            this.dataGridViewTeachers.TabIndex = 0;
            this.dataGridViewTeachers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTeachers_CellValueChanged);
            this.dataGridViewTeachers.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTeachers_ColumnHeaderMouseClick);
            this.dataGridViewTeachers.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewTeachers_DataError);
            this.dataGridViewTeachers.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewTeachers_RowsRemoved);
            // 
            // TeacherName
            // 
            this.TeacherName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TeacherName.HeaderText = "ПІБ";
            this.TeacherName.Name = "TeacherName";
            this.TeacherName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // TeacherStake
            // 
            this.TeacherStake.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "1.00";
            this.TeacherStake.DefaultCellStyle = dataGridViewCellStyle2;
            this.TeacherStake.HeaderText = "Кіл-ть штат. один.";
            this.TeacherStake.Name = "TeacherStake";
            this.TeacherStake.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.TeacherStake.Width = 150;
            // 
            // TeacherPosition
            // 
            this.TeacherPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.NullValue = "<не встановлено>";
            this.TeacherPosition.DefaultCellStyle = dataGridViewCellStyle3;
            this.TeacherPosition.HeaderText = "Посада";
            this.TeacherPosition.Name = "TeacherPosition";
            this.TeacherPosition.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherPosition.Width = 146;
            // 
            // TeacherRank
            // 
            this.TeacherRank.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.NullValue = "<не встановлено>";
            this.TeacherRank.DefaultCellStyle = dataGridViewCellStyle4;
            this.TeacherRank.HeaderText = "Ступінь";
            this.TeacherRank.Name = "TeacherRank";
            this.TeacherRank.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherRank.Width = 146;
            // 
            // TeacherDegree
            // 
            this.TeacherDegree.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.NullValue = "-";
            this.TeacherDegree.DefaultCellStyle = dataGridViewCellStyle5;
            this.TeacherDegree.HeaderText = "Звання";
            this.TeacherDegree.Items.AddRange(new object[] {
            "-",
            "доцент",
            "професор"});
            this.TeacherDegree.Name = "TeacherDegree";
            this.TeacherDegree.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeacherDegree.Width = 67;
            // 
            // TeachersOccupForm
            // 
            this.TeachersOccupForm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.NullValue = "Штатний";
            this.TeachersOccupForm.DefaultCellStyle = dataGridViewCellStyle6;
            this.TeachersOccupForm.HeaderText = "Форма зайнятості";
            this.TeachersOccupForm.Items.AddRange(new object[] {
            "Штатний",
            "Сумісник"});
            this.TeachersOccupForm.Name = "TeachersOccupForm";
            this.TeachersOccupForm.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TeachersOccupForm.Width = 131;
            // 
            // TeacherNotes
            // 
            this.TeacherNotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TeacherNotes.HeaderText = "Примітки";
            this.TeacherNotes.Name = "TeacherNotes";
            this.TeacherNotes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panelTeachersTop
            // 
            this.panelTeachersTop.Controls.Add(this.buttonImportTeachers);
            this.panelTeachersTop.Controls.Add(this.buttonExportTeachers);
            this.panelTeachersTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTeachersTop.Location = new System.Drawing.Point(0, 0);
            this.panelTeachersTop.Name = "panelTeachersTop";
            this.panelTeachersTop.Size = new System.Drawing.Size(899, 26);
            this.panelTeachersTop.TabIndex = 2;
            // 
            // buttonImportTeachers
            // 
            this.buttonImportTeachers.BackColor = System.Drawing.Color.Transparent;
            this.buttonImportTeachers.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonImportTeachers.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonImportTeachers.FlatAppearance.BorderSize = 0;
            this.buttonImportTeachers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttonImportTeachers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonImportTeachers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImportTeachers.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonImportTeachers.Image = global::Carat.Properties.Resources.icons8_импорт_xls_24_norm;
            this.buttonImportTeachers.Location = new System.Drawing.Point(24, 0);
            this.buttonImportTeachers.Name = "buttonImportTeachers";
            this.buttonImportTeachers.Size = new System.Drawing.Size(24, 26);
            this.buttonImportTeachers.TabIndex = 2;
            this.buttonImportTeachers.UseVisualStyleBackColor = false;
            this.buttonImportTeachers.Click += new System.EventHandler(this.buttonImportTeachers_Click);
            // 
            // buttonExportTeachers
            // 
            this.buttonExportTeachers.BackColor = System.Drawing.Color.Transparent;
            this.buttonExportTeachers.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonExportTeachers.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonExportTeachers.FlatAppearance.BorderSize = 0;
            this.buttonExportTeachers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttonExportTeachers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExportTeachers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportTeachers.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExportTeachers.Image = global::Carat.Properties.Resources.icons8_экспорт_xls_24;
            this.buttonExportTeachers.Location = new System.Drawing.Point(0, 0);
            this.buttonExportTeachers.Name = "buttonExportTeachers";
            this.buttonExportTeachers.Size = new System.Drawing.Size(24, 26);
            this.buttonExportTeachers.TabIndex = 1;
            this.buttonExportTeachers.UseVisualStyleBackColor = false;
            this.buttonExportTeachers.Click += new System.EventHandler(this.buttonExportTeachers_Click);
            // 
            // panelTeachersBottom
            // 
            this.panelTeachersBottom.Controls.Add(this.dataGridViewTeachers);
            this.panelTeachersBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTeachersBottom.Location = new System.Drawing.Point(0, 26);
            this.panelTeachersBottom.Name = "panelTeachersBottom";
            this.panelTeachersBottom.Size = new System.Drawing.Size(899, 493);
            this.panelTeachersBottom.TabIndex = 3;
            // 
            // TeachersTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.panelTeachersBottom);
            this.Controls.Add(this.panelTeachersTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 186);
            this.Name = "TeachersTableForm";
            this.Text = "Викладачі";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TeachersTableForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeachers)).EndInit();
            this.panelTeachersTop.ResumeLayout(false);
            this.panelTeachersBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewTeachers;
        private System.Windows.Forms.Panel panelTeachersTop;
        private System.Windows.Forms.Button buttonImportTeachers;
        private System.Windows.Forms.Button buttonExportTeachers;
        private System.Windows.Forms.Panel panelTeachersBottom;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherStake;
        private System.Windows.Forms.DataGridViewComboBoxColumn TeacherPosition;
        private System.Windows.Forms.DataGridViewComboBoxColumn TeacherRank;
        private System.Windows.Forms.DataGridViewComboBoxColumn TeacherDegree;
        private System.Windows.Forms.DataGridViewComboBoxColumn TeachersOccupForm;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeacherNotes;
    }
}