
namespace Carat
{
    partial class SelectGroups
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectGroups));
            this.checkedListBoxGroups = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelGroups = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBoxGroups
            // 
            this.checkedListBoxGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxGroups.FormattingEnabled = true;
            this.checkedListBoxGroups.Location = new System.Drawing.Point(0, 0);
            this.checkedListBoxGroups.Name = "checkedListBoxGroups";
            this.checkedListBoxGroups.ScrollAlwaysVisible = true;
            this.checkedListBoxGroups.Size = new System.Drawing.Size(441, 250);
            this.checkedListBoxGroups.TabIndex = 0;
            this.checkedListBoxGroups.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxGroups_ItemCheck);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelGroups);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 20);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkedListBoxGroups);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(441, 250);
            this.panel2.TabIndex = 2;
            // 
            // labelGroups
            // 
            this.labelGroups.AutoSize = true;
            this.labelGroups.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelGroups.Location = new System.Drawing.Point(186, 2);
            this.labelGroups.Name = "labelGroups";
            this.labelGroups.Size = new System.Drawing.Size(40, 15);
            this.labelGroups.TabIndex = 0;
            this.labelGroups.Text = "Групи";
            this.labelGroups.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 270);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectGroups";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Вибір груп";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxGroups;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelGroups;
        private System.Windows.Forms.Panel panel2;
    }
}