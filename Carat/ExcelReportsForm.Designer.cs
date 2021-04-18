
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("за дисциплінами");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Навантаження кафедри", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcelReportsForm));
            this.treeViewExcelReports = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeViewExcelReports
            // 
            this.treeViewExcelReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewExcelReports.Location = new System.Drawing.Point(0, 0);
            this.treeViewExcelReports.Name = "treeViewExcelReports";
            treeNode1.Name = "NodeBySubjects";
            treeNode1.Text = "за дисциплінами";
            treeNode2.Name = "NodeCafedra";
            treeNode2.Text = "Навантаження кафедри";
            this.treeViewExcelReports.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeViewExcelReports.Size = new System.Drawing.Size(899, 519);
            this.treeViewExcelReports.TabIndex = 0;
            this.treeViewExcelReports.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewExcelReports_NodeMouseDoubleClick);
            // 
            // ExcelReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 519);
            this.Controls.Add(this.treeViewExcelReports);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 186);
            this.Name = "ExcelReports";
            this.Text = "Excel звіти";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExcelReports_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewExcelReports;
    }
}