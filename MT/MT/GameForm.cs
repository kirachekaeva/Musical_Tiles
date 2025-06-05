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
        }

        private void InitializeComponents()
        {
            ((Form)this).Text = "Musical Tiles";
            ((Form)this).ClientSize = new Size(800, 600);
            ((Form)this).FormBorderStyle = FormBorderStyle.FixedSingle;
            ((Form)this).MaximizeBox = false;
            ((Form)this).BackColor = Color.FromArgb(20, 20, 30);

            ScoreLabel = new Label
            {
                Text = $"SCORE: {GameSettings.Score}",
                Location = new Point(20, 30),
                AutoSize = true,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            ((Form)this).Controls.Add(ScoreLabel);

            TimeLabel = new Label
            {
                Text = $"TIME: {GameSettings.TimeLeft}",
                Location = new Point(700, 20),
                AutoSize = true,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            ((Form)this).Controls.Add(TimeLabel);

            ClickableArea = new Panel
            {
                BackColor = Color.FromArgb(50, 50, 70),
                Height = GameSettings.ClickableAreaHeight,
                Width = ((Form)this).ClientSize.Width,
                Location = new Point(0, ((Form)this).ClientSize.Height - GameSettings.ClickableAreaHeight)
            };
            ((Form)this).Controls.Add(ClickableArea);
            ClickableArea.BringToFront();

            InitializeLaneDividers();
        }

        private void InitializeLaneDividers()
        {
            LaneDividers = new Panel[GameSettings.LanesCount - 1];
            int laneWidth = ((Form)this).ClientSize.Width / GameSettings.LanesCount;

            for (int i = 0; i < GameSettings.LanesCount - 1; i++)
            {
                LaneDividers[i] = new Panel
                {
                    BackColor = Color.FromArgb(80, 80, 100),
                    Width = 1,
                    Height = ((Form)this).ClientSize.Height,
                    Location = new Point((i + 1) * laneWidth, 0)
                };
                ((Form)this).Controls.Add(LaneDividers[i]);
                LaneDividers[i].SendToBack();
            }
        }


        public void UpdateScoreDisplay(int score)
        {
            if (ScoreLabel.InvokeRequired)
            {
                ((Form)this).Invoke(new Action(() =>
                {
                    ScoreLabel.Text = $"SCORE: {score}";
                    TimeLabel.Text = $"TIME: {GameSettings.TimeLeft}";
                }));
            }
            else
            {
                ScoreLabel.Text = $"SCORE: {score}";
                TimeLabel.Text = $"TIME: {GameSettings.TimeLeft}";
            }
        }



        public void ShowGameOver(bool isWin)
        {
            var message = isWin ? "TIME'S UP!" : "GAME OVER!";
            var color = isWin ? Color.Orange : Color.Red;
            ShowGameOverInternal(message, color, GameSettings.Score);
        }

        public void ShowGameOver(string customMessage, Color messageColor)
        {
            ShowGameOverInternal(customMessage, messageColor, GameSettings.Score);
        }

        private void ShowGameOverInternal(string message, Color color, int score)
        {
            ClearGameOverUI();

            var gameOverLabel = new Label
            {
                Text = message,
                Font = new Font("Arial", 36, FontStyle.Bold),
                ForeColor = color,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(
                    ClientSize.Width / 2 - 150,
                    ClientSize.Height / 2 - 50)
            };
            Controls.Add(gameOverLabel);
            gameOverLabel.BringToFront();

            var scoreLabel = new Label
            {
                Text = $"YOUR SCORE: {score}",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(
                    ClientSize.Width / 2 - 120,
                    ClientSize.Height / 2 + 20)
            };
            Controls.Add(scoreLabel);
            scoreLabel.BringToFront();
        }

        public void ShowRestartButton(Action restartAction)
        {
            var restartButton = new Button
            {
                Text = "PLAY AGAIN",
                Font = new Font("Arial", 14),
                Size = new Size(200, 50),
                Location = new Point(
                    ClientSize.Width / 2 - 100,
                    ClientSize.Height / 2 + 100),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            restartButton.FlatAppearance.BorderSize = 0;
            restartButton.Click += (s, e) => restartAction();

            Controls.Add(restartButton);
            restartButton.BringToFront();
        }

        public void ClearGameOverUI()
        {
            var controlsToRemove = Controls.OfType<Label>()
                .Where(l => l.Text == "TIME'S UP!" ||
                           l.Text == "GAME OVER!" ||
                           l.Text.StartsWith("YOUR SCORE:") ||
                           l.Text == "YOU WIN!")
                .Concat<Control>(Controls.OfType<Button>().Where(b => b.Text == "PLAY AGAIN"))
                .ToList();

            foreach (var control in controlsToRemove)
            {
                Controls.Remove(control);
                control.Dispose();
            }
        }
    }
}
