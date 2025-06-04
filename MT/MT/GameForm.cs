using System;
using System.Drawing;
using System.Windows.Forms;

namespace MT
{
    public class GameForm : Form, IScoreVisualizer
    {
        public Label ScoreLabel { get; private set; }
        public Label TimeLabel { get; private set; }
        public Panel ClickableArea { get; private set; }
        public Panel[] LaneDividers { get; private set; }
        public ProgressBar ScoreProgressBar { get; private set; }
        public CheckBox SoundCheckBox { get; private set; }
        public ComboBox DifficultyComboBox { get; private set; }

        public GameForm()
        {
            InitializeComponents();
            InitializeVisualControls();
        }

        private void InitializeComponents()
        {
            Text = "Musical Tiles";
            ClientSize = new Size(800, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(20, 20, 30);

            ScoreLabel = new Label
            {
                Text = $"SCORE: {GameSettings.Score}",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            Controls.Add(ScoreLabel);

            TimeLabel = new Label
            {
                Text = $"TIME: {GameSettings.TimeLeft}",
                Location = new Point(700, 20),
                AutoSize = true,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            Controls.Add(TimeLabel);

            ClickableArea = new Panel
            {
                BackColor = Color.FromArgb(50, 50, 70),
                Height = GameSettings.ClickableAreaHeight,
                Width = ClientSize.Width,
                Location = new Point(0, ClientSize.Height - GameSettings.ClickableAreaHeight)
            };
            Controls.Add(ClickableArea);
            ClickableArea.BringToFront();

            InitializeLaneDividers();
        }

        private void InitializeLaneDividers()
        {
            LaneDividers = new Panel[GameSettings.LanesCount - 1];
            int laneWidth = ClientSize.Width / GameSettings.LanesCount;

            for (int i = 0; i < GameSettings.LanesCount - 1; i++)
            {
                LaneDividers[i] = new Panel
                {
                    BackColor = Color.FromArgb(80, 80, 100),
                    Width = 1,
                    Height = ClientSize.Height,
                    Location = new Point((i + 1) * laneWidth, 0)
                };
                Controls.Add(LaneDividers[i]);
                LaneDividers[i].SendToBack();
            }
        }

        private void InitializeVisualControls()
        {
            ScoreProgressBar = new ProgressBar
            {
                Minimum = 0,
                Maximum = 1000,
                Value = GameSettings.Score,
                Location = new Point(20, 50),
                Width = 200,
                Height = 20,
                ForeColor = Color.LightBlue,
                BackColor = Color.FromArgb(30, 30, 40)
            };
        }



        public void UpdateProgressBar(int score)
        {
            if (ScoreProgressBar.InvokeRequired)
            {
                ScoreProgressBar.Invoke(new Action(() =>
                {
                    ScoreProgressBar.Value = Math.Min(score, ScoreProgressBar.Maximum);
                }));
            }
            else
            {
                ScoreProgressBar.Value = Math.Min(score, ScoreProgressBar.Maximum);
            }
        }

        public void UpdateScoreDisplay(int score)
        {
            if (ScoreLabel.InvokeRequired)
            {
                ScoreLabel.Invoke(new Action(() =>
                {
                    ScoreLabel.Text = $"SCORE: {score}";
                    TimeLabel.Text = $"TIME: {GameSettings.TimeLeft}";
                    UpdateProgressBar(score);
                }));
            }
            else
            {
                ScoreLabel.Text = $"SCORE: {score}";
                TimeLabel.Text = $"TIME: {GameSettings.TimeLeft}";
                UpdateProgressBar(score);
            }
        }

        public void ShowGameOver(bool isWin)
        {
            var message = isWin ? "YOU WIN!" : "GAME OVER!";
            var color = isWin ? Color.Green : Color.Red;

            if (InvokeRequired)
            {
                Invoke(new Action(() => ShowGameOverInternal(message, color)));
            }
            else
            {
                ShowGameOverInternal(message, color);
            }
        }

        private void ShowGameOverInternal(string message, Color color)
        {
            var gameOverLabel = new Label
            {
                Text = message,
                Font = new Font("Arial", 36, FontStyle.Bold),
                ForeColor = color,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(
                    ClientSize.Width / 2 - 100,
                    ClientSize.Height / 2 - 50)
            };
            Controls.Add(gameOverLabel);
            gameOverLabel.BringToFront();
        }
    }
}