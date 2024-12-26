using System;
using System.Drawing;
using System.Windows.Forms;

namespace EffortCalculator
{
    partial class MainForm
    {
        private Label DLabel, HourLabel, CodeLabel, NameLabel;
        private TextBox DEntry, HourEntry;
        private ComboBox codeEntry;
        private ComboBox nameEntry;
        private Label resultLabel;
        //private Label coefficientLabel;
        private DataGridView EskPrGrid, TechPrGrid, PDSPGrid, DevWorkDesDocGrid;
        //private DataGridView coefficientGrid;
        private Button addRowButton1, addRowButton2, addRowButton3, addRowButton4, addRowButton5;
        private Button deleteRowButton1, deleteRowButton2, deleteRowButton3, deleteRowButton4, deleteRowButton5;
        private Label EskPr; // Эскизный проект
        private Label TechPr; // Технический проект
        private Label PDSP; // ПДСП
        private Label DevWorkDesDoc; // Разработка рабочей конструкторской документации
        private Button calculateButton, editShipsButton, coefficientsButton;
        private DataGridView selectedCoefficientsGridView;

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

            this.coefficientsButton = new Button { Text = "Коэффициенты", Width = 120 };
            this.coefficientsButton.Click += new EventHandler(this.CoefficientsButton_Click);

            this.selectedCoefficientsGridView = new DataGridView
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

            // this.coefficientLabel = new Label { Text = "Коэффициенты", AutoSize = true };
            // this.coefficientGrid = new DataGridView
            // {
            //     ColumnCount = 2,
            //     AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            //     Columns = {
            //         [0] = { Name = "Параметр", HeaderText = "Параметр" },
            //         [1] = { Name = "Коэффициент", HeaderText = "Коэффициент" }
            //     },
            //     AllowUserToAddRows = false,
            //     Width = 300,
            //     Height = 150
            // };
            // this.addRowButton1 = new Button { Text = "+", Width = 60 };
            // this.addRowButton1.Click += new EventHandler(this.AddRowButton1_Click);

            this.deleteRowButton1 = new Button { Text = "-", Width = 60 };
            this.deleteRowButton1.Click += new EventHandler(this.DeleteRowButton1_Click);

            // Панель % соотношения работ Эскизный проект
            this.EskPr = new Label { Text = "Эскизный проект", AutoSize = true };
            this.EskPrGrid = new DataGridView
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

            this.EskPrGrid.Rows.Add("Эскизный проект(Документация)", "0.05");
            this.EskPrGrid.Rows.Add("3D модель", "0.15");
            this.EskPrGrid.Rows.Add("ИТТ", "0.035");
            this.EskPrGrid.Rows.Add("Согласование ТС", "0.055");

            this.addRowButton2 = new Button { Text = "+", Width = 60 };
            this.addRowButton2.Click += new EventHandler(this.AddRowButton2_Click);

            this.deleteRowButton2 = new Button { Text = "-", Width = 60 };
            this.deleteRowButton2.Click += new EventHandler(this.DeleteRowButton2_Click);

            // Панель % соотношения работ Технический проект
            this.TechPr = new Label { Text = "Технический проект", AutoSize = true };
            this.TechPrGrid = new DataGridView
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

            this.TechPrGrid.Rows.Add("Технический проект(документация)", "0.20");
            this.TechPrGrid.Rows.Add("3D модель", "0.36");
            this.TechPrGrid.Rows.Add("ИТТ", "0.065");
            this.TechPrGrid.Rows.Add("Согласование ТС", "0.088");

            this.addRowButton3 = new Button { Text = "+", Width = 60 };
            this.addRowButton3.Click += new EventHandler(this.AddRowButton3_Click);

            this.deleteRowButton3 = new Button { Text = "-", Width = 60 };
            this.deleteRowButton3.Click += new EventHandler(this.DeleteRowButton3_Click);

            // Панель % соотношения работ ПДСП
            this.PDSP = new Label { Text = "ПДСП", AutoSize = true };
            this.PDSPGrid = new DataGridView
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

            this.PDSPGrid.Rows.Add("ПДСП(Документация)", "");
            this.PDSPGrid.Rows.Add("3D модель", "0.34");
            this.PDSPGrid.Rows.Add("ИТТ", "0.055");
            this.PDSPGrid.Rows.Add("Согласование ТС", "0.088");

            this.addRowButton4 = new Button { Text = "+", Width = 60 };
            this.addRowButton4.Click += new EventHandler(this.AddRowButton4_Click);

            this.deleteRowButton4 = new Button { Text = "-", Width = 60 };
            this.deleteRowButton4.Click += new EventHandler(this.DeleteRowButton4_Click);

            //Панель % соотношения работ Разработка рабочей конструкторской документации
            this.DevWorkDesDoc = new Label { Text = "Разработка рабочей конструкторской документации", AutoSize = true };
            this.DevWorkDesDocGrid = new DataGridView
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

            this.DevWorkDesDocGrid.Rows.Add("Разработка рабочей КД", "0.75");
            this.DevWorkDesDocGrid.Rows.Add("3D модель", "0.15");
            this.DevWorkDesDocGrid.Rows.Add("Согласование ТС", "0.088");
            this.DevWorkDesDocGrid.Rows.Add("Документация ПТД", "0.1");
            this.DevWorkDesDocGrid.Rows.Add("Документация ЭД", "0.09");
            this.DevWorkDesDocGrid.Rows.Add("Документация ПСД", "0.06");
            this.DevWorkDesDocGrid.Rows.Add("Корректировка ПДСП по результатам разработки РКД и выбора оборудования и материалов", "0.21");
            

            this.addRowButton5 = new Button { Text = "+", Width = 60 };
            this.addRowButton5.Click += new EventHandler(this.AddRowButton5_Click);

            this.deleteRowButton5 = new Button { Text = "-", Width = 60 };
            this.deleteRowButton5.Click += new EventHandler(this.DeleteRowButton5_Click);

            var entryPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var hourPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var codePanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var namePanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };

            var coefficientButtonsPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var EskPrButtonsPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var TechPrButtonsPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var PdspButtonsPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            var DevWorkDesDocButtonsPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };

            // Добавляем кнопки в соответствующие панели
            entryPanel.Controls.Add(this.DLabel);
            entryPanel.Controls.Add(this.DEntry);

            hourPanel.Controls.Add(this.HourLabel);
            hourPanel.Controls.Add(this.HourEntry);

            codePanel.Controls.Add(this.CodeLabel);
            codePanel.Controls.Add(this.codeEntry);

            namePanel.Controls.Add(this.NameLabel);
            namePanel.Controls.Add(this.nameEntry);

            coefficientButtonsPanel.Controls.Add(this.coefficientsButton);
            coefficientButtonsPanel.Controls.Add(this.deleteRowButton1);

            EskPrButtonsPanel.Controls.Add(this.addRowButton2);
            EskPrButtonsPanel.Controls.Add(this.deleteRowButton2);

            TechPrButtonsPanel.Controls.Add(this.addRowButton3);
            TechPrButtonsPanel.Controls.Add(this.deleteRowButton3);

            PdspButtonsPanel.Controls.Add(this.addRowButton4);
            PdspButtonsPanel.Controls.Add(this.deleteRowButton4);

            DevWorkDesDocButtonsPanel.Controls.Add(this.addRowButton5);
            DevWorkDesDocButtonsPanel.Controls.Add(this.deleteRowButton5);

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

            // Коэффициенты
            mainLayout.Controls.Add(this.selectedCoefficientsGridView, 0, 4);
            mainLayout.Controls.Add(coefficientButtonsPanel, 0, 6);

            // // Коэффициенты  
            // mainLayout.Controls.Add(this.coefficientLabel, 0, 4);
            // mainLayout.Controls.Add(this.coefficientGrid, 0, 5);
            // 

            // Эскизны проект
            mainLayout.Controls.Add(this.EskPr, 0, 7);
            mainLayout.Controls.Add(this.EskPrGrid, 0, 8);
            mainLayout.Controls.Add(EskPrButtonsPanel, 0, 9);

            // Технический проект
            mainLayout.Controls.Add(this.TechPr, 1, 7);
            mainLayout.Controls.Add(this.TechPrGrid, 1, 8);
            mainLayout.Controls.Add(TechPrButtonsPanel, 1, 9);

            // ПДСП
            mainLayout.Controls.Add(this.PDSP, 0, 13);
            mainLayout.Controls.Add(this.PDSPGrid, 0, 14);
            mainLayout.Controls.Add(PdspButtonsPanel, 0, 15);

            // Разработка рабочей КД
            mainLayout.Controls.Add(this.DevWorkDesDoc, 1, 13);
            mainLayout.Controls.Add(this.DevWorkDesDocGrid, 1, 14);
            mainLayout.Controls.Add(DevWorkDesDocButtonsPanel, 1, 15);

            // Кнопки рассчета и редактирования списка судов
            mainLayout.Controls.Add(this.calculateButton, 0, 16);
            mainLayout.Controls.Add(this.editShipsButton, 1, 16);

            this.Controls.Add(mainLayout);

            this.Text = "Effort Calculator";
            this.AutoSize = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
