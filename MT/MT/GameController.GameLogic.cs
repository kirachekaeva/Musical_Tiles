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

            if (timeRanOut)
            {
                gameForm.ShowGameOver(true); 
            }
            else
            {
                gameForm.ShowGameOver("GAME OVER!", Color.Red); 
            }

            gameForm.ShowRestartButton(() =>
            {
                gameForm.ClearGameOverUI();
                ResetGame();
            });

            SaveScore();
        }


    }
}
