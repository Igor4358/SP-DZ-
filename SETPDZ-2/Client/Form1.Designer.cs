namespace Client
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
            this.responseLabel = new System.Windows.Forms.Label();
            this.timeButton = new System.Windows.Forms.Button();
            this.dateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // responseLabel
            // 
            this.responseLabel.AutoSize = true;
            this.responseLabel.Location = new System.Drawing.Point(66, 80);
            this.responseLabel.Name = "responseLabel";
            this.responseLabel.Size = new System.Drawing.Size(44, 16);
            this.responseLabel.TabIndex = 0;
            this.responseLabel.Text = "label1";
            // 
            // timeButton
            // 
            this.timeButton.Location = new System.Drawing.Point(69, 240);
            this.timeButton.Name = "timeButton";
            this.timeButton.Size = new System.Drawing.Size(75, 23);
            this.timeButton.TabIndex = 1;
            this.timeButton.Text = "button1";
            this.timeButton.UseVisualStyleBackColor = true;
            this.timeButton.Click += new System.EventHandler(this.timeButton_Click);
            // 
            // dateButton
            // 
            this.dateButton.Location = new System.Drawing.Point(384, 239);
            this.dateButton.Name = "dateButton";
            this.dateButton.Size = new System.Drawing.Size(75, 23);
            this.dateButton.TabIndex = 2;
            this.dateButton.Text = "button2";
            this.dateButton.UseVisualStyleBackColor = true;
            this.dateButton.Click += new System.EventHandler(this.dateButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dateButton);
            this.Controls.Add(this.timeButton);
            this.Controls.Add(this.responseLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label responseLabel;
        private System.Windows.Forms.Button timeButton;
        private System.Windows.Forms.Button dateButton;
    }
}

