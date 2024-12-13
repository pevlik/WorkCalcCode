using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace EffortCalculator
{
    public partial class ResultForm : Form
    {
        private Label formulaLabel;
        private Label resultLabel;
        private Label totalCostLabel;
        private DataGridView resultsGrid;
        private CheckedListBox fieldCheckBox;
        private ComboBox formatComboBox;
        private Button saveButton;

        public ResultForm(string formula, double totalResult, double totalCost, (string Name, double Percentage, double Hours, double Price)[] detailedResults)
        {
            InitializeComponent(formula, totalResult, totalCost, detailedResults);
        }

        private void InitializeComponent(string formula, double totalResult, double totalCost, (string Name, double Percentage, double Hours, double Price)[] detailedResults)
        {
            this.Text = "Результаты расчета";
            this.ClientSize = new System.Drawing.Size(700, 600);

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

            // Лейбл для общей стоимости
            totalCostLabel = new Label
            {
                Text = $"Общая стоимость:\n{totalCost:F2}",
                AutoSize = true,
                Location = new System.Drawing.Point(10, 90)
            };

            // Таблица результатов с чекбоксами
            resultsGrid = new DataGridView
            {
                Location = new System.Drawing.Point(10, 130),
                Size = new System.Drawing.Size(660, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = false
            };

            resultsGrid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "include",
                HeaderText = "Включить",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 50
            });
            resultsGrid.Columns.Add("name", "Наименование работы");
            resultsGrid.Columns.Add("percentage", "Процент");
            resultsGrid.Columns.Add("hours", "Часы");
            resultsGrid.Columns.Add("price", "Стоимость");

            foreach (var result in detailedResults)
            {
                resultsGrid.Rows.Add(
                    true, // По умолчанию все строки выбраны
                    result.Name,
                    $"{result.Percentage:P1}",
                    $"{result.Hours:F2}",
                    $"{result.Price:F2}"
                );
            }

            // Выбор формата сохранения
            formatComboBox = new ComboBox
            {
                Items = { "PDF", "Word", "Excel", "CSV" },
                Location = new System.Drawing.Point(10, 450),
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Кнопка для сохранения
            saveButton = new Button
            {
                Text = "Сохранить",
                Location = new System.Drawing.Point(150, 450),
                Width = 100
            };
            saveButton.Click += SaveButton_Click;

            // Добавление контролов на форму
            this.Controls.Add(formulaLabel);
            this.Controls.Add(resultLabel);
            this.Controls.Add(totalCostLabel);
            this.Controls.Add(resultsGrid);
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

            // Список выбранных работ
            var selectedWorks = new List<(string Name, string Percentage, string Hours, string Price)>();
            foreach (DataGridViewRow row in resultsGrid.Rows)
            {
                var isSelected = Convert.ToBoolean(row.Cells["include"].Value);
                if (isSelected)
                {
                    selectedWorks.Add((
                        row.Cells["name"].Value?.ToString(),
                        row.Cells["percentage"].Value?.ToString(),
                        row.Cells["hours"].Value?.ToString(),
                        row.Cells["price"].Value?.ToString()
                    ));
                }
            }

            if (!selectedWorks.Any())
            {
                MessageBox.Show("Выберите хотя бы одну работу для сохранения.", "Ошибка");
                return;
            }

            string message = "Вы выбрали следующие работы для сохранения:\n";
            foreach (var work in selectedWorks)
            {
                message += $"- {work.Name}: {work.Hours} часов, {work.Price}.\n";
            }

            message += $"Формат сохранения: {format}";

            MessageBox.Show(message, "Данные для сохранения");
            // Здесь вы можете реализовать реальное сохранение данных в выбранном формате.
        }
    }
}
