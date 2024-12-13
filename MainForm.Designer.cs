using System;
using System.Drawing;
using System.Windows.Forms;

namespace EffortCalculator
{
    partial class MainForm
    {
        private Label DLabel;
        private TextBox DEntry;
        private Label HourLabel;
        private TextBox HourEntry;
        private Label CodeLabel;
        private ComboBox codeEntry;
        private Label NameLabel;
        private ComboBox nameEntry;
        private Label resultLabel;
        private Label coefficientLabel;
        private DataGridView coefficientGrid;
        private Button addRowButton1;
        private Button addRowButton2;
        private Button deleteRowButton1;
        private Button deleteRowButton2;
        private Label percentageLabel;
        private DataGridView percentageGrid;
        private Button calculateButton;
        private Button editShipsButton;

        private void InitializeComponent()
        {
            this.DLabel = new Label { Text = "Водоизмещение", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Width = 100 };
            this.DEntry = new TextBox { Width = 100 };
            this.HourLabel = new Label { Text = "Стоимость Н.Ч.", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Width = 100 };
            this.HourEntry = new TextBox { Width = 100 };
            this.CodeLabel = new Label { Text = "Класс судна", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Width = 100 };
            this.codeEntry = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown, Width = 150 };
            this.codeEntry.DropDownStyle = ComboBoxStyle.DropDown;
            this.NameLabel = new Label { Text = "Назначение", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Width = 100 };
            this.nameEntry = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown, Width = 300 };
            this.nameEntry.DropDownStyle = ComboBoxStyle.DropDown;
            this.calculateButton = new Button { Text = "Рассчитать", Width = 100 };
            this.calculateButton.Click += new EventHandler(this.CalculateButton_Click);
            this.editShipsButton = new Button { Text = "Редактировать судна", Width = 150 };
            this.editShipsButton.Click += new EventHandler(this.EditShipsButton_Click);
            this.resultLabel = new Label { Text = "Результат: ", AutoSize = true };

            this.coefficientLabel = new Label { Text = "Коэффициенты", AutoSize = true };
            this.coefficientGrid = new DataGridView
            {
                ColumnCount = 2,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Columns = {
                    [0] = { Name = "Параметр", HeaderText = "Параметр" },
                    [1] = { Name = "Коэффициент", HeaderText = "Коэффициент" }
                },
                AllowUserToAddRows = false,
                Width = 300,
                Height = 150
            };
            this.addRowButton1 = new Button { Text = "+", Width = 60 };
            this.addRowButton1.Click += new EventHandler(this.AddRowButton1_Click);

            this.deleteRowButton1 = new Button { Text = "-", Width = 60 };
            this.deleteRowButton1.Click += new EventHandler(this.DeleteRowButton1_Click);

            this.percentageLabel = new Label { Text = "Процентное соотношение работ", AutoSize = true };
            this.percentageGrid = new DataGridView
            {
                ColumnCount = 2,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Columns = {
                    [0] = { Name = "Вид работ", HeaderText = "Вид работ" },
                    [1] = { Name = "Процент", HeaderText = "Процент" }
                },
                AllowUserToAddRows = false,
                Width = 300,
                Height = 150
            };
            this.addRowButton2 = new Button { Text = "+", Width = 60 };
            this.addRowButton2.Click += new EventHandler(this.AddRowButton2_Click);

            this.deleteRowButton2 = new Button { Text = "-", Width = 60};
            this.deleteRowButton2.Click += new EventHandler(this.DeleteRowButton2_Click);

            var entryPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var hourPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var codePanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var namePanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };

            var coefficientButtonsPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var percentageButtonsPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };

            // Добавляем кнопки в соответствующие панели
            entryPanel.Controls.Add(this.DLabel);
            entryPanel.Controls.Add(this.DEntry);

            hourPanel.Controls.Add(this.HourLabel);
            hourPanel.Controls.Add(this.HourEntry);

            codePanel.Controls.Add(this.CodeLabel);
            codePanel.Controls.Add(this.codeEntry);

            namePanel.Controls.Add(this.NameLabel);
            namePanel.Controls.Add(this.nameEntry);

            coefficientButtonsPanel.Controls.Add(this.addRowButton1);
            coefficientButtonsPanel.Controls.Add(this.deleteRowButton1);

            percentageButtonsPanel.Controls.Add(this.addRowButton2);
            percentageButtonsPanel.Controls.Add(this.deleteRowButton2);

            // Layout setup
            var mainLayout = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill, AutoSize = true };

            mainLayout.Controls.Add(entryPanel, 0, 0);
            mainLayout.SetColumnSpan(entryPanel, 2);

            mainLayout.Controls.Add(hourPanel, 0, 1);
            mainLayout.SetColumnSpan(hourPanel, 2);

            mainLayout.Controls.Add(codePanel, 0, 2);
            mainLayout.SetColumnSpan(codePanel, 2);

            mainLayout.Controls.Add(namePanel, 0, 3);
            mainLayout.SetColumnSpan(namePanel, 2);

            // mainLayout.Controls.Add(this.DLabel, 0, 0);
            // mainLayout.Controls.Add(this.DEntry, 1, 0);
            // mainLayout.Controls.Add(this.HourLabel, 0, 1);
            // mainLayout.Controls.Add(this.HourEntry, 1, 1);
            // mainLayout.Controls.Add(this.CodeLabel, 0, 2);
            // mainLayout.Controls.Add(this.codeEntry, 1, 2);
            // mainLayout.Controls.Add(this.NameLabel, 0, 3);
            // mainLayout.Controls.Add(this.nameEntry, 1, 3);

            mainLayout.Controls.Add(this.coefficientLabel, 0, 4);
            mainLayout.Controls.Add(this.coefficientGrid, 0, 5);
            mainLayout.Controls.Add(coefficientButtonsPanel, 0, 6);
            mainLayout.SetColumnSpan(coefficientButtonsPanel, 2); // Чтобы панель заняла оба столбца

            // mainLayout.Controls.Add(this.addRowButton1, 0, 6);
            // mainLayout.Controls.Add(this.deleteRowButton1, 1, 6);

            mainLayout.Controls.Add(this.percentageLabel, 0, 7);
            mainLayout.Controls.Add(this.percentageGrid, 0, 8);
            mainLayout.Controls.Add(percentageButtonsPanel, 0, 9);
            mainLayout.SetColumnSpan(percentageButtonsPanel, 2);

            // mainLayout.Controls.Add(this.addRowButton2, 0, 9);
            // mainLayout.Controls.Add(this.deleteRowButton2, 1, 9);

            mainLayout.Controls.Add(this.calculateButton, 0, 10);
            mainLayout.Controls.Add(this.editShipsButton, 1, 10);

            //mainLayout.Controls.Add(this.resultLabel, 0, 8);

            this.Controls.Add(mainLayout);

            this.Text = "Effort Calculator";
            this.AutoSize = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
