using System;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json;

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
            Console.WriteLine($"Codes: {string.Join(", ", codes)}");
			codeEntry.Items.AddRange(codes);
			var names = Program.shipTypes.Select(ship => ship.Name).Distinct().ToArray();
            Console.WriteLine($"Names: {string.Join(", ", names)}");
			nameEntry.Items.AddRange(names);
			codeEntry.SelectedIndexChanged += CodeEntry_SelectedIndexChanged;
			nameEntry.SelectedIndexChanged += NameEntry_SelectedIndexChanged;

			// Открываем выпадающий список при фокусе
			codeEntry.Enter += (sender, e) => codeEntry.DroppedDown = true;
			nameEntry.Enter += (sender, e) => nameEntry.DroppedDown = true;
		}

		private void CoefficientsButton_Click(object sender, EventArgs e)
        {
            using (var form = new CoefficientsForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    selectedCoefficientsGridView.Rows.Clear();
                    foreach (var coefficient in form.SelectedCoefficients)
                    {
                        selectedCoefficientsGridView.Rows.Add(coefficient.Name, coefficient.Coefficient);
                    }
                }
            }
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

		private void DeleteRowButton1_Click(object sender, EventArgs e)
		{
			DeleteRowSelected(selectedCoefficientsGridView);
		}

		private void DeleteRowButton2_Click(object sender, EventArgs e)
		{
			DeleteRowSelected(EskPrGrid);
		}

		private void DeleteRowButton3_Click(object sender, EventArgs e)
		{
			DeleteRowSelected(TechPrGrid);
		}

		private void DeleteRowButton4_Click(object sender, EventArgs e)
		{
			DeleteRowSelected(PDSPGrid);
		}

		private void DeleteRowButton5_Click(object sender, EventArgs e)
		{
			DeleteRowSelected(DevWorkDesDocGrid);
		}

		private void DeleteRowSelected(DataGridView grid)
		{
			// Получаем индекс выделенной строки
			int rowIndex = grid.CurrentCell?.RowIndex ?? -1;

			if (rowIndex >= 0 && rowIndex < grid.Rows.Count)
			{
				grid.Rows.RemoveAt(rowIndex);
			}

		}	
		
		private void AddRowButton2_Click(object sender, EventArgs e)
		{
			AddRowAfterSelected(EskPrGrid);
		}

		private void AddRowButton3_Click(object sender, EventArgs e)
		{
			AddRowAfterSelected(TechPrGrid);
		}

		private void AddRowButton4_Click(object sender, EventArgs e)
		{
			AddRowAfterSelected(PDSPGrid);
		}

		private void AddRowButton5_Click(object sender, EventArgs e)
		{
			AddRowAfterSelected(DevWorkDesDocGrid);
		}

		private void AddRowAfterSelected(DataGridView grid)
		{
			// Получаем индекс выделенной строки
			int rowIndex = grid.CurrentCell?.RowIndex ?? grid.Rows.Count - 1;

			// Если есть выделенная строка, вставляем новую строку после неё
			if (rowIndex >= 0 && rowIndex < grid.Rows.Count)
			{
				grid.Rows.Insert(rowIndex + 1);
				rowIndex++; // Смещаем индекс для работы с новой строкой
			}
			else
			{
				// Если выделенной строки нет, добавляем новую строку в конец
				rowIndex = grid.Rows.Add();
			}

			// Получаем новую строку
			DataGridViewRow newRow = grid.Rows[rowIndex];

			// Устанавливаем значения по умолчанию для новой строки
			newRow.Cells[0].Value = "";  // Параметр или название работы
			newRow.Cells[1].Value = 0.0; // Коэффициент или процент
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
				MessageBox.Show("Введите корректное водоизмещение (D).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!double.TryParse(HourEntry.Text, out double hourlyRate))
			{
				MessageBox.Show("Введите корректную стоимость нормо-часа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string shipCode = codeEntry.Text;
			string shipName = nameEntry.Text;

			ShipType selectedShip = Program.shipTypes.FirstOrDefault(ship => ship.Code == shipCode || ship.Name == shipName);
			if (selectedShip == null)
			{
				MessageBox.Show("Тип судна не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (string.IsNullOrEmpty(selectedShip.FormulaLow) || string.IsNullOrEmpty(selectedShip.FormulaHigh))
			{
				MessageBox.Show("Формулы для выбранного судна не заданы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				foreach (DataGridViewRow row in selectedCoefficientsGridView.Rows)
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
				double totalCost = 0.0;
				
				// Создаем массив для хранения детальных результатов
				var detailedResults = new List<(string Name, double Percentage, double Hours, double Price)>();

				// Вычисляем время для каждого вида работ
				foreach (DataGridViewRow row in EskPrGrid.Rows)
				{
					string workName = row.Cells[0].Value?.ToString() ?? "";
					if (double.TryParse(row.Cells[1].Value?.ToString(), out double percentage))
					{
						double hours = totalResult * percentage;
						double price = hours * hourlyRate; // Используем пользовательскую стоимость нормо-часа
						totalCost += price;
						detailedResults.Add((workName, percentage, hours, price));
					}
				}

				foreach (DataGridViewRow row in TechPrGrid.Rows)
				{
					string workName = row.Cells[0].Value?.ToString() ?? "";
					if (double.TryParse(row.Cells[1].Value?.ToString(), out double percentage))
					{
						double hours = totalResult * percentage;
						double price = hours * hourlyRate; // Используем пользовательскую стоимость нормо-часа
						totalCost += price;
						detailedResults.Add((workName, percentage, hours, price));
					}
				}

				foreach (DataGridViewRow row in PDSPGrid.Rows)
				{
					string workName = row.Cells[0].Value?.ToString() ?? "";
					if (double.TryParse(row.Cells[1].Value?.ToString(), out double percentage))
					{
						double hours = totalResult * percentage;
						double price = hours * hourlyRate; // Используем пользовательскую стоимость нормо-часа
						totalCost += price;
						detailedResults.Add((workName, percentage, hours, price));
					}
				}

				foreach (DataGridViewRow row in DevWorkDesDocGrid.Rows)
				{
					string workName = row.Cells[0].Value?.ToString() ?? "";
					if (double.TryParse(row.Cells[1].Value?.ToString(), out double percentage))
					{
						double hours = totalResult * percentage;
						double price = hours * hourlyRate; // Используем пользовательскую стоимость нормо-часа
						totalCost += price;
						detailedResults.Add((workName, percentage, hours, price));
					}
				}

				// Показываем форму с результатами
				ResultForm resultForm = new ResultForm(formulaWithReplacements, totalResult, totalCost, detailedResults.ToArray());
				resultForm.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при расчете: {ex.Message}", "Ошибка расчета", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

				// Проверяем, являются ли basePart и exponentPart числами
				bool isBaseNumeric = double.TryParse(basePart, out double baseValue);
				bool isExponentNumeric = double.TryParse(exponentPart, NumberStyles.Any, CultureInfo.InvariantCulture, out double exponentValue);

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

			return formula;
		}
	}
}