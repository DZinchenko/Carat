
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("за дисциплінами (заплановані)");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("за дисциплінами (розподілені)");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("за викладачами (повний)");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("за викладачами (скорочений)");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("розклад");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Навантаження кафедри", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("за вибраною дисципліною");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("за вибраним викладачем");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("за вибраним викладачем (розширений)");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("за вибраним видом роботи");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Вибіркові", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("нерозподілені години");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("індивідуальний план");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Інші", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13});
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
            treeNode1.Text = "за дисциплінами (заплановані)";
            treeNode2.Name = "NodeBySubjectsDistributed";
            treeNode2.Text = "за дисциплінами (розподілені)";
            treeNode3.Name = "NodeByTeachers";
            treeNode3.Text = "за викладачами (повний)";
            treeNode4.Name = "NodeShortByTeachers";
            treeNode4.Text = "за викладачами (скорочений)";
            treeNode5.Name = "NodeSchedule";
            treeNode5.Text = "розклад";
            treeNode6.Name = "NodeCafedra";
            treeNode6.Text = "Навантаження кафедри";
            treeNode7.Name = "NodeSelectedSubject";
            treeNode7.Text = "за вибраною дисципліною";
            treeNode8.Name = "NodeSelectedTeacher";
            treeNode8.Text = "за вибраним викладачем";
            treeNode9.Name = "NodeSelectedTeacherExtended";
            treeNode9.Text = "за вибраним викладачем (розширений)";
            treeNode10.Name = "NodeSelectedWork";
            treeNode10.Text = "за вибраним видом роботи";
            treeNode11.Name = "NodeSelective";
            treeNode11.Text = "Вибіркові";
            treeNode12.Name = "NodeUnallocated";
            treeNode12.Text = "нерозподілені години";
            treeNode13.Name = "NodeIndividualPlan";
            treeNode13.Text = "індивідуальний план";
            treeNode14.Name = "NodeOther";
            treeNode14.Text = "Інші";
            this.treeViewExcelReports.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode11,
            treeNode14});
            this.treeViewExcelReports.Size = new System.Drawing.Size(899, 260);
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
            this.panel1.Size = new System.Drawing.Size(899, 260);
            this.panel1.TabIndex = 1;
            // 
            // panelContainer
            // 
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 260);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(899, 259);
            this.panelContainer.TabIndex = 2;
            // 
            // ExcelReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
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