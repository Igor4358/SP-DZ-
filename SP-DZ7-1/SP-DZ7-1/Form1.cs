using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP_DZ7_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void analyzeButton_Click(object sender, EventArgs e)
        {
            string text = textBoxInput.Text;
            bool saveToFile = radioButtonSaveToFile.Checked;

            var report = await Task.Run(() => AnalyzeText(text));

            if (saveToFile)
            {
                SaveReportToFile(report);
            }
            else
            {
                DisplayReport(report);
            }
        }
        private string AnalyzeText(string text)
        {
            int sentenceCount = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int charCount = text.Length;
            int wordCount = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int questionCount = text.Split('?').Length - 1;
            int exclamationCount = text.Split('!').Length - 1;

            StringBuilder report = new StringBuilder();
            report.AppendLine($"Количество предложений: {sentenceCount}");
            report.AppendLine($"Количество символов: {charCount}");
            report.AppendLine($"Количество слов: {wordCount}");
            report.AppendLine($"Количество вопросительных предложений: {questionCount}");
            report.AppendLine($"Количество восклицательных предложений: {exclamationCount}");

            return report.ToString();
        }

        private void DisplayReport(string report)
        {
            textBoxReport.Text = report;
        }

        private void SaveReportToFile(string report)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, report);
                }
            }
        }
    }
}