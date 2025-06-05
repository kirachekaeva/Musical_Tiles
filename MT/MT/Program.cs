using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Model.Data;

namespace MT
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            InitializeScoresFile();

            Application.Run(new MainMenuForm());
        }

        private static void InitializeScoresFile()
        {
            try
            {
                string scoresPath = "scores.json";
                if (!File.Exists(scoresPath))
                {
                    var emptyScores = new List<int>();
                    new JsonScoreSerializer().Serialize(emptyScores);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось инициализировать файл рекордов: {ex.Message}",
                              "Ошибка инициализации",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
    }
}
