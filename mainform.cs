using System;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;

namespace EffortCalculator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeComboBoxAutoComplete();
        }

        private void InitializeComboBoxAutoComplete()
        {
        
            var codes = Program.shipTypes.Select(ship => ship.Code).Distinct().ToArray();
            codeEntry.Items.AddRange(codes);

            
            var names = Program.shipTypes.Select(ship => ship.Name).Distinct().ToArray();
            nameEntry.Items.AddRange(names);

            
            codeEntry.SelectedIndexChanged += CodeEntry_SelectedIndexChanged;
            nameEntry.SelectedIndexChanged += NameEntry_SelectedIndexChanged;

            // Открываем выпадающий список при фокусе
            codeEntry.Enter += (sender, e) => codeEntry.DroppedDown = true;
            nameEntry.Enter += (sender, e) => nameEntry.DroppedDown = true;
        }

        private void CodeEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем выбранный код
            string selectedCode = codeEntry.Text;
            // Ищем соответствующее судно и заполняем nameEntry
            var matchingShip = Program.shipTypes.FirstOrDefault(ship => ship.Code == selectedCode);
            if (matchingShip != null)
            {
                nameEntry.Text = matchingShip.Name;
            }
        }

        private void NameEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем выбранное имя судна
            string selectedName = nameEntry.Text;
            // Ищем соответствующее судно и заполняем codeEntry
            var matchingShip = Program.shipTypes.FirstOrDefault(ship => ship.Name == selectedName);
            if (matchingShip != null)
            {
                codeEntry.Text = matchingShip.Code;
            }
        }

        private void EditShipsButton_Click(object sender, EventArgs e)
        {
            EditShipsForm editForm = new EditShipsForm();
            editForm.ShowDialog();
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(DEntry.Text, out double D))
            {
                MessageBox.Show("Invalid displacement D", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string shipCode = codeEntry.Text;
            string shipName = nameEntry.Text;

            ShipType selectedShip = Program.shipTypes.FirstOrDefault(ship => ship.Code == shipCode || ship.Name == shipName);
            if (selectedShip == null)
            {
                MessageBox.Show("Тип судна не найден", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(selectedShip.FormulaLow) || string.IsNullOrEmpty(selectedShip.FormulaHigh))
            {
                MessageBox.Show("Формулы не заданы для выбранного судна", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Получаем базовую формулу в зависимости от водоизмещения
            string formula = D <= selectedShip.MaxDisplacement ? selectedShip.FormulaLow : selectedShip.FormulaHigh;
            string formulaWithReplacements = ReplacePowerOperator(formula, D);

            try
            {
                // Вычисляем базовое время
                double baseResult = CalculateFormula(formulaWithReplacements);

                // Получаем коэффициент сложности (произведение всех коэффициентов)
                double totalCoefficient = 1.0;
                foreach (DataGridViewRow row in coefficientGrid.Rows)
                {
                    if (row.Cells[1].Value != null && !string.IsNullOrWhiteSpace(row.Cells[1].Value.ToString()))
                    {
                        if (double.TryParse(row.Cells[1].Value.ToString(), out double coef))
                        {
                            totalCoefficient *= coef;
                        }
                    }
                }

                // Применяем коэффициент сложности к базовому результату
                double totalResult = baseResult * totalCoefficient;

                // Создаем массив для хранения детальных результатов
                var detailedResults = new List<(string Name, double Percentage, double Hours)>();

                // Вычисляем время для каждого вида работ
                foreach (DataGridViewRow row in percentageGrid.Rows)
                {
                    string workName = row.Cells[0].Value?.ToString() ?? "";
                    if (double.TryParse(row.Cells[1].Value?.ToString(), out double percentage))
                    {
                        double hours = totalResult * percentage;
                        detailedResults.Add((workName, percentage, hours));
                    }
                }

                // Показываем форму с результатами
                ResultForm resultForm = new ResultForm(formulaWithReplacements, totalResult, detailedResults.ToArray());
                resultForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in calculation: {ex.Message}", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public double CalculateFormula(string formula)
        {
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                var result = table.Compute(formula, string.Empty);
                return Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка в формате формулы: {ex.Message}");
            }
        }

        private string ReplacePowerOperator(string formula, double D)
        {
            // Подстановка значения D в формулу
            formula = formula.Replace("D", D.ToString());

            // Регулярное выражение для поиска выражений "base ^ exponent"
            string pattern = @"([0-9.]+|[a-zA-Z]+)\s*\^\s*([0-9.]+|[a-zA-Z]+)";
            formula = Regex.Replace(formula, pattern, match =>
            {
                string basePart = match.Groups[1].Value.Trim();
                string exponentPart = match.Groups[2].Value.Trim();
                //MessageBox.Show($"Base value (string): {basePart}");
                //MessageBox.Show($"Exponent value (string): {exponentPart}");

                // Проверяем, являются ли basePart и exponentPart числами
                bool isBaseNumeric = double.TryParse(basePart, out double baseValue);
                bool isExponentNumeric = double.TryParse(exponentPart, NumberStyles.Any, CultureInfo.InvariantCulture, out double exponentValue);

                // Проверка результатов парсинга
                //MessageBox.Show($"Base value (double): {baseValue}");
                //MessageBox.Show($"Exponent value (double): {exponentValue}");
                //MessageBox.Show($"Is base numeric? {isBaseNumeric}");
                //MessageBox.Show($"Is exponent numeric? {isExponentNumeric}");

                if (isBaseNumeric && isExponentNumeric)
                {
                    // Если оба значения числовые, вычисляем Math.Pow и возвращаем результат
                    double result = Math.Pow(baseValue, exponentValue);
                    return result.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    // Если одно из значений не числовое (например, если basePart — переменная), оставляем Math.Pow() в формуле
                    return $"Math.Pow({basePart}, {exponentPart})";
                }
            });
            //MessageBox.Show($"Результат после ReplacePowerOperator: {formula}");

            return formula;
        }
    }
}
