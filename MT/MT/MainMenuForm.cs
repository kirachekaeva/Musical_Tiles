using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Model.Data;

namespace MT
{
    public class MainMenuForm : Form
    {
        private ComboBox formatComboBox;
        private DataGridView scoresDataGridView;

        public MainMenuForm()
        {
            InitializeComponents();
            LoadHighScores();
        }

        private void InitializeComponents()
        {
            Text = "Musical Tiles - Main Menu";
            ClientSize = new Size(600, 600); 
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(30, 30, 40);

            var titleLabel = new Label
            {
                Text = "MUSICAL TILES",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(150, 20),
                AutoSize = true
            };
            Controls.Add(titleLabel);

            var startButton = new Button
            {
                Text = "START GAME",
                Font = new Font("Arial", 14),
                Size = new Size(250, 60),
                Location = new Point(175, 80),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            startButton.FlatAppearance.BorderSize = 0;
            startButton.Click += (s, e) =>
            {
                Hide();
                var game = new GameController();
                game.StartGame();
                Close();
            };
            Controls.Add(startButton);

            var difficultyLabel = new Label
            {
                Text = "DIFFICULTY:",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                Location = new Point(175, 170),
                AutoSize = true
            };
            Controls.Add(difficultyLabel);

            var difficultyCombo = new ComboBox
            {
                Font = new Font("Arial", 12),
                Size = new Size(150, 30),
                Location = new Point(275, 165),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(50, 50, 60),
                ForeColor = Color.White
            };

            for (int i = 2; i <= 8; i++)
            {
                difficultyCombo.Items.Add($"{i} lanes");
            }
            difficultyCombo.SelectedIndex = 2;
            difficultyCombo.SelectedIndexChanged += (s, e) =>
            {
                GameSettings.LanesCount = difficultyCombo.SelectedIndex + 2;
            };
            Controls.Add(difficultyCombo);

            var formatLabel = new Label
            {
                Text = "Save format:",
                Location = new Point(175, 220),
                Size = new Size(100, 20),
                ForeColor = Color.White,
                Font = new Font("Arial", 12)
            };
            Controls.Add(formatLabel);

            formatComboBox = new ComboBox
            {
                Items = { "JSON", "XML" },
                SelectedItem = GameSettings.SelectedSerializer == "xml" ? "XML" : "JSON",
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(275, 215),
                Size = new Size(150, 30),
                Font = new Font("Arial", 12),
                BackColor = Color.FromArgb(50, 50, 60),
                ForeColor = Color.White
            };
            formatComboBox.SelectedIndexChanged += FormatComboBox_SelectedIndexChanged;
            Controls.Add(formatComboBox);

            var highScoresLabel = new Label
            {
                Text = "HIGH SCORES",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(240, 270),
                AutoSize = true
            };
            Controls.Add(highScoresLabel);


            scoresDataGridView = new DataGridView
            {
                Location = new Point(100, 310),
                Size = new Size(400, 200),
                BackgroundColor = Color.FromArgb(40, 40, 50),
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                ScrollBars = ScrollBars.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(50, 50, 60),
                    ForeColor = Color.White,
                    Font = new Font("Arial", 12),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(70, 130, 180),
                    ForeColor = Color.White,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(60, 60, 70)
                }
            };

            scoresDataGridView.Columns.Add("Rank", "Rank");
            scoresDataGridView.Columns.Add("Score", "Score");

            Controls.Add(scoresDataGridView);
        }

        private void FormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newFormat = formatComboBox.SelectedItem.ToString().ToLower();
            if (newFormat != GameSettings.SelectedSerializer)
            {
                GameController.ConvertScores(GameSettings.SelectedSerializer, newFormat);
                GameSettings.SelectedSerializer = newFormat;
                LoadHighScores(); 
            }
        }

        private void LoadHighScores()
        {
            var scores = GameController.LoadScores();
            scoresDataGridView.Rows.Clear();

            for (int i = 0; i < scores.Count && i < 10; i++)
            {
                scoresDataGridView.Rows.Add(new object[] { i + 1, scores[i] });
            }

            if (scores.Count == 0)
            {
                SaveEmptyScores();
            }
        }

        private void SaveEmptyScores()
        {
            try
            {
                ScoreSerializer serializer = GameSettings.SelectedSerializer == "xml"
                    ? new XmlScoreSerializer()
                    : new JsonScoreSerializer();

                serializer.Serialize(new List<int>());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating scores file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
