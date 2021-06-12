
namespace Carat
{
    partial class GroupsTableForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupsTableForm));
            this.dataGridViewGroups = new System.Windows.Forms.DataGridView();
            this.panelGroupsTop = new System.Windows.Forms.Panel();
            this.buttonImportGroups = new System.Windows.Forms.Button();
            this.buttonExportGroups = new System.Windows.Forms.Button();
            this.panelGroupsBottom = new System.Windows.Forms.Panel();
            this.GroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupCourse = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.GroupEduForm = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.GroupsEducLevel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.GroupBudj_cnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupContr_cnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupFaculty = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.GroupNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroups)).BeginInit();
            this.panelGroupsTop.SuspendLayout();
            this.panelGroupsBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewGroups
            // 
            this.dataGridViewGroups.AllowUserToResizeRows = false;
            this.dataGridViewGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GroupName,
            this.GroupCourse,
            this.GroupEduForm,
            this.GroupsEducLevel,
            this.GroupBudj_cnt,
            this.GroupContr_cnt,
            this.GroupFaculty,
            this.GroupNotes});
            this.dataGridViewGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewGroups.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewGroups.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridViewGroups.Name = "dataGridViewGroups";
            this.dataGridViewGroups.RowHeadersWidth = 51;
            this.dataGridViewGroups.Size = new System.Drawing.Size(899, 493);
            this.dataGridViewGroups.TabIndex = 1;
            this.dataGridViewGroups.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGroups_CellValueChanged);
            this.dataGridViewGroups.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewGroups_ColumnHeaderMouseClick);
            this.dataGridViewGroups.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewGroups_DataError);
            this.dataGridViewGroups.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewGroups_RowsRemoved);
            // 
            // panelGroupsTop
            // 
            this.panelGroupsTop.Controls.Add(this.buttonImportGroups);
            this.panelGroupsTop.Controls.Add(this.buttonExportGroups);
            this.panelGroupsTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGroupsTop.Location = new System.Drawing.Point(0, 0);
            this.panelGroupsTop.Name = "panelGroupsTop";
            this.panelGroupsTop.Size = new System.Drawing.Size(899, 26);
            this.panelGroupsTop.TabIndex = 3;
            // 
            // buttonImportGroups
            // 
            this.buttonImportGroups.BackColor = System.Drawing.Color.Transparent;
            this.buttonImportGroups.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonImportGroups.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonImportGroups.FlatAppearance.BorderSize = 0;
            this.buttonImportGroups.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttonImportGroups.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonImportGroups.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImportGroups.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonImportGroups.Image = global::Carat.Properties.Resources.icons8_импорт_xls_24_norm;
            this.buttonImportGroups.Location = new System.Drawing.Point(24, 0);
            this.buttonImportGroups.Name = "buttonImportGroups";
            this.buttonImportGroups.Size = new System.Drawing.Size(24, 26);
            this.buttonImportGroups.TabIndex = 2;
            this.buttonImportGroups.UseVisualStyleBackColor = false;
            this.buttonImportGroups.Click += new System.EventHandler(this.buttonImportGroups_Click);
            // 
            // buttonExportGroups
            // 
            this.buttonExportGroups.BackColor = System.Drawing.Color.Transparent;
            this.buttonExportGroups.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonExportGroups.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonExportGroups.FlatAppearance.BorderSize = 0;
            this.buttonExportGroups.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.buttonExportGroups.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExportGroups.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportGroups.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExportGroups.Image = global::Carat.Properties.Resources.icons8_экспорт_xls_24;
            this.buttonExportGroups.Location = new System.Drawing.Point(0, 0);
            this.buttonExportGroups.Name = "buttonExportGroups";
            this.buttonExportGroups.Size = new System.Drawing.Size(24, 26);
            this.buttonExportGroups.TabIndex = 1;
            this.buttonExportGroups.UseVisualStyleBackColor = false;
            this.buttonExportGroups.Click += new System.EventHandler(this.buttonExportGroups_Click);
            // 
            // panelGroupsBottom
            // 
            this.panelGroupsBottom.Controls.Add(this.dataGridViewGroups);
            this.panelGroupsBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGroupsBottom.Location = new System.Drawing.Point(0, 26);
            this.panelGroupsBottom.Name = "panelGroupsBottom";
            this.panelGroupsBottom.Size = new System.Drawing.Size(899, 493);
            this.panelGroupsBottom.TabIndex = 4;
            // 
            // GroupName
            // 
            this.GroupName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GroupName.HeaderText = "Назва";
            this.GroupName.MinimumWidth = 6;
            this.GroupName.Name = "GroupName";
            this.GroupName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // GroupCourse
            // 
            this.GroupCourse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.NullValue = "1";
            this.GroupCourse.DefaultCellStyle = dataGridViewCellStyle1;
            this.GroupCourse.HeaderText = "Курс";
            this.GroupCourse.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.GroupCourse.MinimumWidth = 6;
            this.GroupCourse.Name = "GroupCourse";
            this.GroupCourse.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GroupCourse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.GroupCourse.Width = 58;
            // 
            // GroupEduForm
            // 
            this.GroupEduForm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.NullValue = "Денна";
            this.GroupEduForm.DefaultCellStyle = dataGridViewCellStyle2;
            this.GroupEduForm.HeaderText = "Форма навчання";
            this.GroupEduForm.Items.AddRange(new object[] {
            "Денна",
            "Заочна",
            "Вечірня"});
            this.GroupEduForm.Name = "GroupEduForm";
            this.GroupEduForm.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GroupEduForm.Width = 95;
            // 
            // GroupsEducLevel
            // 
            this.GroupsEducLevel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.NullValue = "Бакалавр";
            this.GroupsEducLevel.DefaultCellStyle = dataGridViewCellStyle3;
            this.GroupsEducLevel.HeaderText = "Рівень навчання";
            this.GroupsEducLevel.Items.AddRange(new object[] {
            "Бакалавр",
            "Магістр",
            "PhD"});
            this.GroupsEducLevel.Name = "GroupsEducLevel";
            this.GroupsEducLevel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GroupsEducLevel.Width = 93;
            // 
            // GroupBudj_cnt
            // 
            this.GroupBudj_cnt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.NullValue = "0";
            this.GroupBudj_cnt.DefaultCellStyle = dataGridViewCellStyle4;
            this.GroupBudj_cnt.HeaderText = "Бюджетників";
            this.GroupBudj_cnt.Name = "GroupBudj_cnt";
            this.GroupBudj_cnt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GroupBudj_cnt.Width = 85;
            // 
            // GroupContr_cnt
            // 
            this.GroupContr_cnt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.NullValue = "0";
            this.GroupContr_cnt.DefaultCellStyle = dataGridViewCellStyle5;
            this.GroupContr_cnt.HeaderText = "Контрактників";
            this.GroupContr_cnt.Name = "GroupContr_cnt";
            this.GroupContr_cnt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GroupContr_cnt.Width = 92;
            // 
            // GroupFaculty
            // 
            this.GroupFaculty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.NullValue = "ТЕФ";
            this.GroupFaculty.DefaultCellStyle = dataGridViewCellStyle6;
            this.GroupFaculty.HeaderText = "Факультет";
            this.GroupFaculty.Items.AddRange(new object[] {
            "ТЕФ",
            "ФЕЛ",
            "ІПСА",
            "ФЕА",
            "ФІОТ",
            "ФММ",
            "ІФФ",
            "ММІ",
            "ІХФ"});
            this.GroupFaculty.MaxDropDownItems = 20;
            this.GroupFaculty.Name = "GroupFaculty";
            this.GroupFaculty.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GroupFaculty.Width = 69;
            // 
            // GroupNotes
            // 
            this.GroupNotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GroupNotes.HeaderText = "Примітки";
            this.GroupNotes.Name = "GroupNotes";
            this.GroupNotes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // GroupsTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.panelGroupsBottom);
            this.Controls.Add(this.panelGroupsTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(700, 186);
            this.Name = "GroupsTableForm";
            this.Text = "Групи";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GroupsTableForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroups)).EndInit();
            this.panelGroupsTop.ResumeLayout(false);
            this.panelGroupsBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewGroups;
        private System.Windows.Forms.Panel panelGroupsTop;
        private System.Windows.Forms.Button buttonImportGroups;
        private System.Windows.Forms.Button buttonExportGroups;
        private System.Windows.Forms.Panel panelGroupsBottom;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupName;
        private System.Windows.Forms.DataGridViewComboBoxColumn GroupCourse;
        private System.Windows.Forms.DataGridViewComboBoxColumn GroupEduForm;
        private System.Windows.Forms.DataGridViewComboBoxColumn GroupsEducLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupBudj_cnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupContr_cnt;
        private System.Windows.Forms.DataGridViewComboBoxColumn GroupFaculty;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupNotes;
    }
}