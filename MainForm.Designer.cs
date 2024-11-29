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
        private Button addRowButton;
        private Label percentageLabel;
        private DataGridView percentageGrid;
        private Button calculateButton;
        private Button editShipsButton;

        private void InitializeComponent()
        
        {
            this.DLabel = new Label { Text = "Водоизмещение", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            this.DEntry = new TextBox { Text = "" };
            this.HourLabel = new Label { Text = "Стоимость Н.Ч", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            this.HourEntry = new TextBox { Text = "" };
            this.CodeLabel = new Label { Text = "Класс судна", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            this.codeEntry = new ComboBox { Text = "" };
            this.NameLabel = new Label { Text = "Назначение", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            this.nameEntry = new ComboBox { Text = "", Width = 400 };
            this.coefficientLabel = new Label { Text = "Коэффициенты", AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            this.addRowButton = new Button { Text = "+" };
            this.percentageLabel = new Label { Text = "Процентное соотношение работ", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill };
            this.resultLabel = new Label { Text = "", AutoSize = true };
            this.calculateButton = new Button { Text = "Расчитать",  Anchor = AnchorStyles.Right };
            this.editShipsButton = new Button { Text = "Список судов", AutoSize = true };

            // Таблица для процентного соотношения работ
            this.percentageGrid = new DataGridView
            {
                ColumnCount = 2,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false, // Ручное добавление через кнопку
                Dock = DockStyle.Fill
            };
            percentageGrid.Columns[0].HeaderText = "Наименование";
            percentageGrid.Columns[1].HeaderText = "Значение";
            percentageGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            percentageGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            percentageGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            percentageGrid.EnableHeadersVisualStyles = false;
            percentageGrid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            percentageGrid.BackgroundColor = System.Drawing.Color.White;

            // Добавление начальных строк
            percentageGrid.Rows.Add("Эскизный проект", "0.05");
            percentageGrid.Rows.Add("Технический проект", "0.25");
            percentageGrid.Rows.Add("Разработка рабочей КД", "0.7");
            
            // Таблица коэффициентов
            this.coefficientGrid = new DataGridView
            {
                ColumnCount = 2,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false, // Ручное добавление через кнопку
                Dock = DockStyle.Fill
            };
            coefficientGrid.Columns[0].HeaderText = "Наименование";
            coefficientGrid.Columns[1].HeaderText = "Значение";
            coefficientGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            coefficientGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            coefficientGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            coefficientGrid.EnableHeadersVisualStyles = false;
            coefficientGrid.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            coefficientGrid.BackgroundColor = System.Drawing.Color.White;

            // Добавление начальных строк
            coefficientGrid.Rows.Add("Эскизный проект", "");
            coefficientGrid.Rows.Add("Технический проект", "");
            coefficientGrid.Rows.Add("Разработка рабочей КД", "");

            this.calculateButton.Click += CalculateButton_Click;
            this.editShipsButton.Click += EditShipsButton_Click;
            this.addRowButton.Click += AddRowButton_Click;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 10,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            layout.Controls.Add(DLabel, 0, 0);
            layout.Controls.Add(DEntry, 1, 0);
            layout.Controls.Add(HourLabel, 0, 1);
            layout.Controls.Add(HourEntry, 1, 1);
            layout.Controls.Add(CodeLabel, 0, 2);
            layout.Controls.Add(codeEntry, 1, 2);
            layout.Controls.Add(NameLabel, 0, 3);
            layout.Controls.Add(nameEntry, 1, 3);
            layout.Controls.Add(coefficientLabel, 0, 4);
            layout.Controls.Add(coefficientGrid, 0, 5);
            layout.SetColumnSpan(coefficientGrid, 2); // Таблица на два столбца
            layout.Controls.Add(addRowButton, 0, 6);
            layout.Controls.Add(percentageLabel, 0, 7);
            layout.Controls.Add(percentageGrid, 0, 8);
            layout.SetColumnSpan(percentageGrid, 2);
            layout.Controls.Add(editShipsButton, 0, 9);
            layout.Controls.Add(calculateButton, 1, 9);
            layout.Controls.Add(resultLabel, 0, 10);

            // Задаем привязку кнопки editShipsButton к двум столбцам
            layout.SetColumnSpan(editShipsButton, 1);

            this.Controls.Add(layout);
            this.Text = "Effort Calculator";
            this.ClientSize = new System.Drawing.Size(600, 600); // Увеличенный размер для таблицы
        }

        private void AddRowButton_Click(object sender, EventArgs e)
        {
            coefficientGrid.Rows.Add("", ""); // Добавление пустой строки
        }
    }
}
