namespace SP_DZ7_1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.analyzeButton = new System.Windows.Forms.Button();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.radioButtonDisplayOnScreen = new System.Windows.Forms.RadioButton();
            this.textBoxReport = new System.Windows.Forms.TextBox();
            this.radioButtonSaveToFile = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // analyzeButton
            // 
            this.analyzeButton.Location = new System.Drawing.Point(93, 202);
            this.analyzeButton.Name = "analyzeButton";
            this.analyzeButton.Size = new System.Drawing.Size(75, 23);
            this.analyzeButton.TabIndex = 0;
            this.analyzeButton.Text = "button";
            this.analyzeButton.UseVisualStyleBackColor = true;
            this.analyzeButton.Click += new System.EventHandler(this.analyzeButton_Click);
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(93, 73);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(128, 22);
            this.textBoxInput.TabIndex = 1;
            // 
            // radioButtonDisplayOnScreen
            // 
            this.radioButtonDisplayOnScreen.AutoSize = true;
            this.radioButtonDisplayOnScreen.Location = new System.Drawing.Point(277, 203);
            this.radioButtonDisplayOnScreen.Name = "radioButtonDisplayOnScreen";
            this.radioButtonDisplayOnScreen.Size = new System.Drawing.Size(103, 20);
            this.radioButtonDisplayOnScreen.TabIndex = 2;
            this.radioButtonDisplayOnScreen.TabStop = true;
            this.radioButtonDisplayOnScreen.Text = "radioButton1";
            this.radioButtonDisplayOnScreen.UseVisualStyleBackColor = true;
            // 
            // textBoxReport
            // 
            this.textBoxReport.Location = new System.Drawing.Point(290, 72);
            this.textBoxReport.Multiline = true;
            this.textBoxReport.Name = "textBoxReport";
            this.textBoxReport.Size = new System.Drawing.Size(332, 22);
            this.textBoxReport.TabIndex = 3;
            // 
            // radioButtonSaveToFile
            // 
            this.radioButtonSaveToFile.AutoSize = true;
            this.radioButtonSaveToFile.Location = new System.Drawing.Point(558, 202);
            this.radioButtonSaveToFile.Name = "radioButtonSaveToFile";
            this.radioButtonSaveToFile.Size = new System.Drawing.Size(103, 20);
            this.radioButtonSaveToFile.TabIndex = 4;
            this.radioButtonSaveToFile.TabStop = true;
            this.radioButtonSaveToFile.Text = "radioButton1";
            this.radioButtonSaveToFile.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.radioButtonSaveToFile);
            this.Controls.Add(this.textBoxReport);
            this.Controls.Add(this.radioButtonDisplayOnScreen);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.analyzeButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button analyzeButton;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.RadioButton radioButtonDisplayOnScreen;
        private System.Windows.Forms.TextBox textBoxReport;
        private System.Windows.Forms.RadioButton radioButtonSaveToFile;
    }
}

