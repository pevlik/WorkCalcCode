using System;
using System.Windows.Forms;

namespace EffortCalculator
{
    public partial class ResultForm : Form
    {
        private Label formulaLabel;
        private Label resultLabel;
        private CheckedListBox fieldCheckBox;
        private ComboBox formatComboBox;
        private Button saveButton;

        public ResultForm(string formula, string result)
        {
            InitializeComponent(formula, result);
        }

        private void InitializeComponent(string formula, string result)
        {
            this.Text = "Результаты расчета";
            this.ClientSize = new System.Drawing.Size(400, 300);

            // Поле для формулы
            formulaLabel = new Label
            {
                Text = $"Формула расчета:\n{formula}",
                AutoSize = true,
                Location = new System.Drawing.Point(10, 10)
            };

            // Поле для результата
            resultLabel = new Label
            {
                Text = $"Результат:\n{result}",
                AutoSize = true,
                Location = new System.Drawing.Point(10, 60)
            };

            // Чекбоксы для выбора полей
            fieldCheckBox = new CheckedListBox
            {
                Items = { "Сохранить формулу", "Сохранить результат" },
                Location = new System.Drawing.Point(10, 120),
                Size = new System.Drawing.Size(200, 60)
            };

            // Выбор формата сохранения
            formatComboBox = new ComboBox
            {
                Items = { "PDF", "Word", "Excel", "CSV" },
                Location = new System.Drawing.Point(10, 200),
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Кнопка для сохранения
            saveButton = new Button
            {
                Text = "Сохранить",
                Location = new System.Drawing.Point(150, 250),
                Width = 100
            };
            saveButton.Click += SaveButton_Click;

            // Добавление контролов на форму
            this.Controls.Add(formulaLabel);
            this.Controls.Add(resultLabel);
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
