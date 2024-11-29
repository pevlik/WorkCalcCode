using System;
using System.Windows.Forms;
using System.Drawing;

namespace EffortCalculator
{
    public partial class ResultForm : Form
    {
        private Label formulaLabel;
        private Label resultLabel;
        private DataGridView resultsGrid;
        private CheckedListBox fieldCheckBox;
        private ComboBox formatComboBox;
        private Button saveButton;

        public ResultForm(string formula, double totalResult, (string Name, double Percentage, double Hours)[] detailedResults)
        {
            InitializeComponent(formula, totalResult, detailedResults);
        }

        private void InitializeComponent(string formula, double totalResult, (string Name, double Percentage, double Hours)[] detailedResults)
        {
            this.Text = "Результаты расчета";
            this.ClientSize = new System.Drawing.Size(600, 500);

            // Поле для формулы
            formulaLabel = new Label
            {
                Text = $"Формула расчета:\n{formula}",
                AutoSize = true,
                Location = new System.Drawing.Point(10, 10)
            };

            // Поле для общего результата
            resultLabel = new Label
            {
                Text = $"Общее время:\n{totalResult:F2} часов",
                AutoSize = true,
                Location = new System.Drawing.Point(10, 60)
            };

            // Таблица результатов
            resultsGrid = new DataGridView
            {
                Location = new System.Drawing.Point(10, 100),
                Size = new System.Drawing.Size(560, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            resultsGrid.Columns.Add("name", "Наименование работы");
            resultsGrid.Columns.Add("percentage", "Процент");
            resultsGrid.Columns.Add("hours", "Часы");

            foreach (var result in detailedResults)
            {
                resultsGrid.Rows.Add(
                    result.Name,
                    $"{result.Percentage:P1}",
                    $"{result.Hours:F2}"
                );
            }

            // Чекбоксы для выбора полей
            fieldCheckBox = new CheckedListBox
            {
                Items = { "Сохранить формулу", "Сохранить общий результат", "Сохранить детальные результаты" },
                Location = new System.Drawing.Point(10, 320),
                Size = new System.Drawing.Size(200, 80)
            };

            // Выбор формата сохранения
            formatComboBox = new ComboBox
            {
                Items = { "PDF", "Word", "Excel", "CSV" },
                Location = new System.Drawing.Point(10, 420),
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Кнопка для сохранения
            saveButton = new Button
            {
                Text = "Сохранить",
                Location = new System.Drawing.Point(150, 420),
                Width = 100
            };
            saveButton.Click += SaveButton_Click;

            // Добавление контролов на форму
            this.Controls.Add(formulaLabel);
            this.Controls.Add(resultLabel);
            this.Controls.Add(resultsGrid);
            this.Controls.Add(fieldCheckBox);
            this.Controls.Add(formatComboBox);
            this.Controls.Add(saveButton);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (formatComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите формат для сохранения.", "Ошибка");
                return;
            }

            string format = formatComboBox.SelectedItem.ToString();
            string message = "Выбраны для сохранения:\n";

            foreach (var item in fieldCheckBox.CheckedItems)
            {
                message += $"- {item}\n";
            }

            message += $"Формат сохранения: {format}";

            MessageBox.Show(message, "Данные для сохранения");
            // Здесь вы можете реализовать реальное сохранение данных.
        }
    }
}
