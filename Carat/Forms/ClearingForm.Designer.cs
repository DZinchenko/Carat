
namespace Carat.Forms
{
    partial class ClearingForm
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
            this.infoLabel = new System.Windows.Forms.Label();
            this.checkBoxLoad = new System.Windows.Forms.CheckBox();
            this.checkBoxCurriculum = new System.Windows.Forms.CheckBox();
            this.checkBoxTeachers = new System.Windows.Forms.CheckBox();
            this.checkBoxGroups = new System.Windows.Forms.CheckBox();
            this.checkBoxSubjects = new System.Windows.Forms.CheckBox();
            this.LineLabel = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(14, 14);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(261, 15);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Вибрані елементи будуть повність очищені:";
            // 
            // checkBoxLoad
            // 
            this.checkBoxLoad.AutoSize = true;
            this.checkBoxLoad.Location = new System.Drawing.Point(17, 42);
            this.checkBoxLoad.Name = "checkBoxLoad";
            this.checkBoxLoad.Size = new System.Drawing.Size(114, 19);
            this.checkBoxLoad.TabIndex = 1;
            this.checkBoxLoad.Text = "Навантаження";
            this.checkBoxLoad.UseVisualStyleBackColor = true;
            // 
            // checkBoxCurriculum
            // 
            this.checkBoxCurriculum.AutoSize = true;
            this.checkBoxCurriculum.Location = new System.Drawing.Point(17, 67);
            this.checkBoxCurriculum.Name = "checkBoxCurriculum";
            this.checkBoxCurriculum.Size = new System.Drawing.Size(128, 19);
            this.checkBoxCurriculum.TabIndex = 2;
            this.checkBoxCurriculum.Text = "Навчальний план";
            this.checkBoxCurriculum.UseVisualStyleBackColor = true;
            // 
            // checkBoxTeachers
            // 
            this.checkBoxTeachers.AutoSize = true;
            this.checkBoxTeachers.Location = new System.Drawing.Point(17, 92);
            this.checkBoxTeachers.Name = "checkBoxTeachers";
            this.checkBoxTeachers.Size = new System.Drawing.Size(84, 19);
            this.checkBoxTeachers.TabIndex = 3;
            this.checkBoxTeachers.Text = "Викладачі";
            this.checkBoxTeachers.UseVisualStyleBackColor = true;
            // 
            // checkBoxGroups
            // 
            this.checkBoxGroups.AutoSize = true;
            this.checkBoxGroups.Location = new System.Drawing.Point(17, 117);
            this.checkBoxGroups.Name = "checkBoxGroups";
            this.checkBoxGroups.Size = new System.Drawing.Size(59, 19);
            this.checkBoxGroups.TabIndex = 4;
            this.checkBoxGroups.Text = "Групи";
            this.checkBoxGroups.UseVisualStyleBackColor = true;
            // 
            // checkBoxSubjects
            // 
            this.checkBoxSubjects.AutoSize = true;
            this.checkBoxSubjects.Location = new System.Drawing.Point(17, 142);
            this.checkBoxSubjects.Name = "checkBoxSubjects";
            this.checkBoxSubjects.Size = new System.Drawing.Size(93, 19);
            this.checkBoxSubjects.TabIndex = 5;
            this.checkBoxSubjects.Text = "Дисципліни";
            this.checkBoxSubjects.UseVisualStyleBackColor = true;
            // 
            // LineLabel
            // 
            this.LineLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LineLabel.Location = new System.Drawing.Point(3, 174);
            this.LineLabel.Name = "LineLabel";
            this.LineLabel.Size = new System.Drawing.Size(340, 2);
            this.LineLabel.TabIndex = 6;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(161, 179);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(78, 27);
            this.buttonClear.TabIndex = 7;
            this.buttonClear.Text = "Очистити";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(245, 179);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(78, 27);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // ClearingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 212);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.LineLabel);
            this.Controls.Add(this.checkBoxSubjects);
            this.Controls.Add(this.checkBoxGroups);
            this.Controls.Add(this.checkBoxTeachers);
            this.Controls.Add(this.checkBoxCurriculum);
            this.Controls.Add(this.checkBoxLoad);
            this.Controls.Add(this.infoLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClearingForm";
            this.Text = "Очистити";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.CheckBox checkBoxLoad;
        private System.Windows.Forms.CheckBox checkBoxCurriculum;
        private System.Windows.Forms.CheckBox checkBoxTeachers;
        private System.Windows.Forms.CheckBox checkBoxGroups;
        private System.Windows.Forms.CheckBox checkBoxSubjects;
        private System.Windows.Forms.Label LineLabel;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonClose;
    }
}