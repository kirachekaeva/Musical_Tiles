using System.Drawing;
using System.Windows.Forms;

namespace MT
{
    public class MainMenuForm : Form
    {
        private ComboBox formatComboBox;

        public MainMenuForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = "Musical Tiles - Main Menu";
            ClientSize = new Size(500, 400);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(30, 30, 40);

            var titleLabel = new Label
            {
                Text = "MUSICAL TILES",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(80, 40),
                AutoSize = true
            };
            Controls.Add(titleLabel);

            var startButton = new Button
            {
                Text = "START GAME",
                Font = new Font("Arial", 14),
                Size = new Size(250, 60),
                Location = new Point(125, 120),
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
                Location = new Point(125, 220),
                AutoSize = true
            };
            Controls.Add(difficultyLabel);

            var difficultyCombo = new ComboBox
            {
                Font = new Font("Arial", 12),
                Size = new Size(150, 30),
                Location = new Point(225, 215),
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

            formatComboBox = new ComboBox();
            formatComboBox.Items.AddRange(new object[] { "JSON", "XML" });
            formatComboBox.SelectedItem = GameSettings.SelectedSerializer == "xml" ? "XML" : "JSON";
            formatComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            formatComboBox.Location = new Point(300, 150);
            formatComboBox.Size = new Size(100, 25);
            formatComboBox.SelectedIndexChanged += FormatComboBox_SelectedIndexChanged;
            Controls.Add(formatComboBox);

            var formatLabel = new Label
            {
                Text = "Save format:",
                Location = new Point(200, 155),
                Size = new Size(100, 20),
                ForeColor = Color.White
            };
            Controls.Add(formatLabel);
        }

        private void FormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newFormat = formatComboBox.SelectedItem.ToString().ToLower();
            if (newFormat != GameSettings.SelectedSerializer)
            {
                GameController.ConvertScores(GameSettings.SelectedSerializer, newFormat);
                GameSettings.SelectedSerializer = newFormat;
            }
        }
    }
}