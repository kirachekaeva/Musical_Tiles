using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MT
{
    public class GameForm : Form
    {
        public Label ScoreLabel { get; private set; }
        public Label TimeLabel { get; private set; }
        public Panel ClickableArea { get; private set; }
        public Panel[] LaneDividers { get; private set; }

        public GameForm()
        {
            InitializeComponents();
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
    }
}