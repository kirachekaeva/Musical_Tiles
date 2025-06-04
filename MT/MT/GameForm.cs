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
            var message = isWin ? "YOU WIN!" : "GAME OVER!";
            var color = isWin ? Color.Green : Color.Red;

            if (((Form)this).InvokeRequired)
            {
                ((Form)this).Invoke(new Action(() => ShowGameOverInternal(message, color)));
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
                    ((Form)this).ClientSize.Width / 2 - 100,
                    ((Form)this).ClientSize.Height / 2 - 50)
            };
            ((Form)this).Controls.Add(gameOverLabel);
            gameOverLabel.BringToFront();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawString("Musical Tiles",
                new Font("Arial", 12),
                Brushes.White,
                new Point(10, 10));
        }
    }
}
