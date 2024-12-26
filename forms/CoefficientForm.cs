using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace EffortCalculator
{
    public partial class CoefficientsForm : Form
    {
        public List<CoefficientItem> SelectedCoefficients { get; private set; }

        public CoefficientsForm()
        {
            InitializeComponent();
            LoadCoefficients();
        }

        private void InitializeComponent()
        {
            this.coefficientsGridView = new DataGridView();
            this.okButton = new Button();
            this.cancelButton = new Button();

            this.SuspendLayout();

            // coefficientsGridView
            this.coefficientsGridView.AllowUserToAddRows = false;
            this.coefficientsGridView.AllowUserToDeleteRows = false;
            this.coefficientsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.coefficientsGridView.MultiSelect = false;
            this.coefficientsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.coefficientsGridView.Dock = DockStyle.Fill;
            this.coefficientsGridView.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Select", HeaderText = "Выбрать", Width = 70 });
            this.coefficientsGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Название", Width = 500 });
            this.coefficientsGridView.Columns.Add("Coefficient", "Коэффициент");

            // okButton
            this.okButton.Text = "ОК";
            this.okButton.Dock = DockStyle.Bottom;
            this.okButton.Click += new EventHandler(OkButton_Click);

            // cancelButton
            this.cancelButton.Text = "Отмена";
            this.cancelButton.Dock = DockStyle.Bottom;
            this.cancelButton.Click += new EventHandler(CancelButton_Click);

            // CoefficientsForm
            this.ClientSize = new System.Drawing.Size(750, 500);
            this.Controls.Add(this.coefficientsGridView);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Text = "Коэффициенты";
            this.ResumeLayout(false);
        }

        private void LoadCoefficients()
        {
            var coefficients = LoadCoefficientData();
            foreach (var coefficient in coefficients)
            {
                coefficientsGridView.Rows.Add(false, coefficient.Name, coefficient.Coefficient);
            }
        }

        private List<CoefficientItem> LoadCoefficientData()
        {
            var path = "jsons/CoefficientData.json";
            if (!File.Exists(path))
            {
                return new List<CoefficientItem>();
            }

            var jsonData = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<CoefficientItem>>(jsonData) ?? new List<CoefficientItem>();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            SelectedCoefficients = new List<CoefficientItem>();
            foreach (DataGridViewRow row in coefficientsGridView.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value))
                {
                    SelectedCoefficients.Add(new CoefficientItem
                    {
                        Name = row.Cells["Name"].Value.ToString(),
                        Coefficient = row.Cells["Coefficient"].Value.ToString()
                    });
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private DataGridView coefficientsGridView;
        private Button okButton;
        private Button cancelButton;
    }

    public class CoefficientItem
    {
        public string Name { get; set; }
        public string Coefficient { get; set; }
    }
}