using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MT
{
    public partial class GameController
    {
        private void CheckMissedTiles()
        {
            foreach (var tile in tiles.ToList())
            {
                if (tile.Bottom >= gameForm.ClickableArea.Top && !tileObjects[tile].WasClicked)
                {
                    if (!tileObjects[tile].IsTrap)
                    {
                        EndGame(false);
                        return;
                    }
                }
            }
        }

        private void EndGame(bool timeRanOut)
        {
            gameTimer.Stop();
            tileTimer.Stop();
            animationTimer.Stop();

            foreach (var tile in tiles)
            {
                tile.Enabled = false;
            }

            gameOverLabel = new Label
            {
                Text = timeRanOut ? "TIME'S UP!" : "GAME OVER!",
                Font = new Font("Arial", 36, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(
                    gameForm.ClientSize.Width / 2 - 150,
                    gameForm.ClientSize.Height / 2 - 50)
            };
            gameForm.Controls.Add(gameOverLabel);
            gameOverLabel.BringToFront();

            var finalScoreLabel = new Label
            {
                Text = $"YOUR SCORE: {GameSettings.Score}",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(
                    gameForm.ClientSize.Width / 2 - 120,
                    gameForm.ClientSize.Height / 2 + 20)
            };
            gameForm.Controls.Add(finalScoreLabel);
            finalScoreLabel.BringToFront();

            var restartButton = new Button
            {
                Text = "PLAY AGAIN",
                Font = new Font("Arial", 14),
                Size = new Size(200, 50),
                Location = new Point(
                    gameForm.ClientSize.Width / 2 - 100,
                    gameForm.ClientSize.Height / 2 + 100),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            restartButton.FlatAppearance.BorderSize = 0;
            restartButton.Click += (s, e) =>
            {
                gameForm.Controls.Remove(gameOverLabel);
                gameForm.Controls.Remove(finalScoreLabel);
                gameForm.Controls.Remove(restartButton);
                ResetGame();
            };
            gameForm.Controls.Add(restartButton);
            restartButton.BringToFront();

            SaveScore();
        }


    }
}
