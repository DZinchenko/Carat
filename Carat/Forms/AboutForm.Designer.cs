
namespace Carat.Forms
{
    partial class AboutForm
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
            this.labelCarat = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.labelDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCarat
            // 
            this.labelCarat.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCarat.Location = new System.Drawing.Point(288, 86);
            this.labelCarat.Name = "labelCarat";
            this.labelCarat.Size = new System.Drawing.Size(208, 91);
            this.labelCarat.TabIndex = 3;
            this.labelCarat.Text = "Carat";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::Carat.Properties.Resources.preciousstone_105028;
            this.pictureBox5.Location = new System.Drawing.Point(122, 86);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(165, 150);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 2;
            this.pictureBox5.TabStop = false;
            // 
            // labelDescription
            // 
            this.labelDescription.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDescription.Location = new System.Drawing.Point(299, 154);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(383, 138);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Програма розподілу педагогічного \r\nнавантаження на кафедрі. \r\n\r\nРозробники: Бабич" +
    " Олексій і Зінченко Данило.\r\n\r\n2022 рік";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(771, 450);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelCarat);
            this.Controls.Add(this.pictureBox5);
            this.MinimumSize = new System.Drawing.Size(602, 167);
            this.Name = "AboutForm";
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCarat;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label labelDescription;
    }
}