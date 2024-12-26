using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace EffortCalculator
{
    static class Program
    {
        public static ShipType[] shipTypes;
        public static string configFile = "jsons/shipsData.json";

        [STAThread]
        static void Main()
        {
            // Загрузка конфигурационного файла
            try
            {
                shipTypes = LoadShipTypes(configFile);
            }
            catch (Exception ex)
            {
                if (ex is System.IO.FileNotFoundException)
                {
                    shipTypes = new ShipType[] { };
                }
                else
                {
                    Console.WriteLine("Error loading ship types: " + ex.Message);
                    return;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static ShipType[] LoadShipTypes(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файл конфигурации не найден!");
                    return Array.Empty<ShipType>();
                }

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<ShipType[]>(json) ?? Array.Empty<ShipType>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Ошибка при разборе JSON: " + ex.Message);
                return Array.Empty<ShipType>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка загрузки данных типов судов: " + ex.Message);
                return Array.Empty<ShipType>();
            }
        }
    }
}