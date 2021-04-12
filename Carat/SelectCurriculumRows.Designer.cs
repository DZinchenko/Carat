
namespace Carat
{
    partial class SelectCurriculumRows
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectCurriculumRows));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownStart1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownEnd1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownStart2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownEnd2 = new System.Windows.Forms.NumericUpDown();
            this.buttonOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEnd1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEnd2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Початок 1-го семестру: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Кінець 1-го семестру:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Початок 2-го семетру:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Кінець 2-го семестру:";
            // 
            // numericUpDownStart1
            // 
            this.numericUpDownStart1.Location = new System.Drawing.Point(158, 9);
            this.numericUpDownStart1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownStart1.Name = "numericUpDownStart1";
            this.numericUpDownStart1.Size = new System.Drawing.Size(42, 23);
            this.numericUpDownStart1.TabIndex = 8;
            this.numericUpDownStart1.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // numericUpDownEnd1
            // 
            this.numericUpDownEnd1.Location = new System.Drawing.Point(158, 36);
            this.numericUpDownEnd1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownEnd1.Name = "numericUpDownEnd1";
            this.numericUpDownEnd1.Size = new System.Drawing.Size(42, 23);
            this.numericUpDownEnd1.TabIndex = 9;
            this.numericUpDownEnd1.Value = new decimal(new int[] {
            57,
            0,
            0,
            0});
            // 
            // numericUpDownStart2
            // 
            this.numericUpDownStart2.Location = new System.Drawing.Point(158, 63);
            this.numericUpDownStart2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownStart2.Name = "numericUpDownStart2";
            this.numericUpDownStart2.Size = new System.Drawing.Size(42, 23);
            this.numericUpDownStart2.TabIndex = 10;
            this.numericUpDownStart2.Value = new decimal(new int[] {
            61,
            0,
            0,
            0});
            // 
            // numericUpDownEnd2
            // 
            this.numericUpDownEnd2.Location = new System.Drawing.Point(158, 90);
            this.numericUpDownEnd2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownEnd2.Name = "numericUpDownEnd2";
            this.numericUpDownEnd2.Size = new System.Drawing.Size(42, 23);
            this.numericUpDownEnd2.TabIndex = 11;
            this.numericUpDownEnd2.Value = new decimal(new int[] {
            102,
            0,
            0,
            0});
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(15, 119);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(185, 23);
            this.buttonOK.TabIndex = 12;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // SelectCurriculumRows
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 150);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.numericUpDownEnd2);
            this.Controls.Add(this.numericUpDownStart2);
            this.Controls.Add(this.numericUpDownEnd1);
            this.Controls.Add(this.numericUpDownStart1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectCurriculumRows";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Діапазон";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEnd1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEnd2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownStart1;
        private System.Windows.Forms.NumericUpDown numericUpDownEnd1;
        private System.Windows.Forms.NumericUpDown numericUpDownStart2;
        private System.Windows.Forms.NumericUpDown numericUpDownEnd2;
        private System.Windows.Forms.Button buttonOK;
    }
}