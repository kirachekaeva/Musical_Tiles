using Model.Core;
using System;
using System.Windows.Forms;

namespace MT
{
    public partial class GameController
    {
        private void ProcessTile(Button tile)
        {
            if (tileObjects.TryGetValue(tile, out TileBase tileObj))
            {
                if (tileObj.IsTrap)
                {
                    EndGame(false);
                    return;
                }

                tileObj.WasClicked = true;
                AddScore(tileObj.ScoreValue);
                RemoveTile(tile);
            }
        }

        private void ProcessTile(Button tile, bool ignoreTrap)
        {
            if (tileObjects.TryGetValue(tile, out TileBase tileObj))
            {
                if (tileObj.IsTrap && !ignoreTrap)
                {
                    EndGame(false);
                    return;
                }

                tileObj.WasClicked = true;
                AddScore(tileObj.ScoreValue);
                RemoveTile(tile);
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            foreach (var tile in tiles.ToList())
            {
                tile.Top += GameSettings.TileSpeed / 3;

                if (tile.Top > gameForm.ClickableArea.Bottom && !tileObjects[tile].WasClicked && !tileObjects[tile].IsTrap)
                {
                    AddScore(tileObjects[tile].ScoreValue / 2); 
                    RemoveTile(tile);
                    continue;
                }

                CheckMissedTiles();

                if (tile.Top > gameForm.ClientSize.Height)
                {
                    RemoveTile(tile);
                }
            }


        }

        private void UpdateGameDifficulty()
        {
            if (GameSettings.Score > 50)
            {
                GameSettings.TileSpeed = 7;
                tileTimer.Interval = 600;
            }
            if (GameSettings.Score > 100)
            {
                GameSettings.TileSpeed = 9;
                tileTimer.Interval = 400;
            }
            if (GameSettings.Score > 200)
            {
                GameSettings.TileSpeed = 11;
                tileTimer.Interval = 300;
            }
            if (GameSettings.Score > 500)
            {
                GameSettings.TileSpeed = 13;
                tileTimer.Interval = 200;
            }
        }

        private void AddScore(int points)
        {
            GameSettings.Score += points;
            gameForm.ScoreLabel.Text = $"SCORE: {GameSettings.Score}";
            UpdateGameDifficulty();
        }
    }
}
