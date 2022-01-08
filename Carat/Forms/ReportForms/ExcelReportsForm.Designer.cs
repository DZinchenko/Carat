
namespace Carat
{
    partial class ExcelReportsForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Навчальний план");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Навантаження за дисциплінами");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Навантаження за викладачами детальне");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Навантаження кафедри");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Розклад для груп");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Нерозподілені години");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Підсумковий за дисциплінами");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Підсумковий за викладачами");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Зведені", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("за вибраним викладачем");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("за вибраним викладачем розширений");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("за вибраною дисципліною");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("за вибраним видом роботи");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Індивідуальний план");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Вибіркові", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode14});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcelReportsForm));
            this.treeViewExcelReports = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewExcelReports
            // 
            this.treeViewExcelReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewExcelReports.Location = new System.Drawing.Point(0, 0);
            this.treeViewExcelReports.Name = "treeViewExcelReports";
            treeNode1.Name = "NodeBySubjectsScheduled";
            treeNode1.Text = "Навчальний план";
            treeNode2.Name = "NodeBySubjectsDistributed";
            treeNode2.Text = "Навантаження за дисциплінами";
            treeNode3.Name = "NodeByTeachers";
            treeNode3.Text = "Навантаження за викладачами детальне";
            treeNode4.Name = "NodeShortByTeachers";
            treeNode4.Text = "Навантаження кафедри";
            treeNode5.Name = "NodeSchedule";
            treeNode5.Text = "Розклад для груп";
            treeNode6.Name = "NodeUnallocated";
            treeNode6.Text = "Нерозподілені години";
            treeNode7.Name = "NodeFinalBySubjects";
            treeNode7.Text = "Підсумковий за дисциплінами";
            treeNode8.Name = "NodeFinalByTeachers";
            treeNode8.Text = "Підсумковий за викладачами";
            treeNode9.Name = "NodeTotal";
            treeNode9.Text = "Зведені";
            treeNode10.Name = "NodeSelectedTeacher";
            treeNode10.Text = "за вибраним викладачем";
            treeNode11.Name = "NodeSelectedTeacherExtended";
            treeNode11.Text = "за вибраним викладачем розширений";
            treeNode12.Name = "NodeSelectedSubject";
            treeNode12.Text = "за вибраною дисципліною";
            treeNode13.Name = "NodeSelectedWork";
            treeNode13.Text = "за вибраним видом роботи";
            treeNode14.Name = "NodeIndividualPlan";
            treeNode14.Text = "Індивідуальний план";
            treeNode15.Name = "NodeSelective";
            treeNode15.Text = "Вибіркові";
            this.treeViewExcelReports.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode15});
            this.treeViewExcelReports.Size = new System.Drawing.Size(899, 281);
            this.treeViewExcelReports.TabIndex = 0;
            this.treeViewExcelReports.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewExcelReports_AfterSelect);
            this.treeViewExcelReports.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewExcelReports_NodeMouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeViewExcelReports);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(899, 281);
            this.panel1.TabIndex = 1;
            // 
            // panelContainer
            // 
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 281);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(899, 238);
            this.panelContainer.TabIndex = 2;
            // 
            // ExcelReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 186);
            this.Name = "ExcelReportsForm";
            this.Text = "Excel звіти";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExcelReports_FormClosed);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewExcelReports;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelContainer;
    }
}