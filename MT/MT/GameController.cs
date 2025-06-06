using Model;
using Model.Data;
using Model.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace MT
{
    public partial class GameController
    {
        private GameForm gameForm;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Timer tileTimer;
        private System.Windows.Forms.Timer animationTimer;

        private List<Button> tiles = new List<Button>();
        private Dictionary<Button, TileBase> tileObjects = new Dictionary<Button, TileBase>();
        private Dictionary<Button, DateTime> holdStartTimes = new Dictionary<Button, DateTime>();
        private Dictionary<Button, int> clickCounts = new Dictionary<Button, int>();
        private Random random = new Random();
        private TileGenerator tileGenerator;

        private Label gameOverLabel;

        public void StartGame()
        {
            tileGenerator = new TileGenerator(GameSettings.LanesCount);
            gameForm = new GameForm();
            gameForm.FormClosing += GameForm_FormClosing;
            InitializeTimers();
            gameForm.ShowDialog();
        }

        private void InitializeTimers()
        {
            gameTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;

            tileTimer = new System.Windows.Forms.Timer { Interval = 800 };
            tileTimer.Tick += TileTimer_Tick;

            animationTimer = new System.Windows.Forms.Timer { Interval = 16 };
            animationTimer.Tick += AnimationTimer_Tick;

            gameTimer.Start();
            tileTimer.Start();
            animationTimer.Start();
        }

        private void TileTimer_Tick(object sender, EventArgs e)
        {
            if (GameSettings.TimeLeft <= 0 || tiles.Count > 15) return;

            tileGenerator.UpdateScore(GameSettings.Score);

            int laneWidth = gameForm.ClientSize.Width / GameSettings.LanesCount;
            int lane = random.Next(0, GameSettings.LanesCount);
            int xPos = lane * laneWidth + (laneWidth - GameSettings.TileWidth) / 2;

            bool canPlace = true;
            foreach (var existingTile in tiles)
            {
                if (existingTile.Left == xPos &&
                    existingTile.Top + existingTile.Height + GameSettings.MinTileGap > 0)
                {
                    canPlace = false;
                    break;
                }
            }

            if (!canPlace) return;

            var tileObj = tileGenerator.GenerateTile();
            var tileButton = new Button
            {
                Size = new Size(GameSettings.TileWidth, GameSettings.TileHeight),
                Location = new Point(xPos, -GameSettings.TileHeight),
                BackColor = tileObj.TileColor,
                FlatStyle = FlatStyle.Flat,
                Tag = tileObj,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black
            };
            tileButton.FlatAppearance.BorderSize = 0;
            tileButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(tileObj.TileColor.R + 30, 255),
                Math.Min(tileObj.TileColor.G + 30, 255),
                Math.Min(tileObj.TileColor.B + 30, 255));

            if (tileObj.RequiresDoubleClick)
            {
                tileButton.Text = "DOUBLE";
            }
            else if (tileObj.RequiresHold)
            {
                tileButton.Text = "HOLD";
            }
            else if (tileObj.IsTrap)
            {
                tileButton.Text = "TRAP!";
            }
            else
            {
                tileButton.Text = $"+{tileObj.ScoreValue}";
            }

            tileButton.MouseDown += Tile_MouseDown;
            tileButton.MouseUp += Tile_MouseUp;
            tileButton.Click += Tile_Click;
            tileButton.DoubleClick += Tile_DoubleClick;

            tiles.Add(tileButton);
            tileObjects.Add(tileButton, tileObj);
            clickCounts.Add(tileButton, 0);
            gameForm.Controls.Add(tileButton);
            tileButton.BringToFront();
        }





        private void Tile_Click(object sender, EventArgs e)
        {
            var tile = sender as Button;
            if (tile == null || !tileObjects.ContainsKey(tile)) return;

            var tileObj = tileObjects[tile];
            clickCounts[tile]++;

            if (tileObj.RequiresDoubleClick)
            {
                if (clickCounts[tile] >= 2)
                {
                    ProcessTile(tile);
                }
            }
            else if (!tileObj.RequiresHold)
            {
                ProcessTile(tile, true);
            }
        }

        private void Tile_DoubleClick(object sender, EventArgs e)
        {
            var tile = sender as Button;
            if (tile == null || !tileObjects.ContainsKey(tile)) return;

            var tileObj = tileObjects[tile];
            if (tileObj.RequiresDoubleClick)
            {
                ProcessTile(tile);
            }
        }

        private void Tile_MouseUp(object sender, MouseEventArgs e)
        {
            var tile = sender as Button;
            if (tile == null || !tileObjects.ContainsKey(tile)) return;

            var tileObj = tileObjects[tile];
            if (tileObj.RequiresHold && holdStartTimes.ContainsKey(tile))
            {
                var holdTime = DateTime.Now - holdStartTimes[tile];
                if (holdTime.TotalMilliseconds >= 500)
                {
                    ProcessTile(tile, false);
                }
                else
                {
                    tile.BackColor = tileObj.TileColor;
                }
                holdStartTimes.Remove(tile);
            }
        }

        private void Tile_MouseDown(object sender, MouseEventArgs e)
        {
            var tile = sender as Button;
            if (tile == null || !tileObjects.ContainsKey(tile)) return;

            var tileObj = tileObjects[tile];
            if (tileObj.RequiresHold)
            {
                holdStartTimes[tile] = DateTime.Now;
                tile.BackColor = Color.FromArgb(
                    Math.Max(tileObj.TileColor.R - 50, 0),
                    Math.Max(tileObj.TileColor.G - 50, 0),
                    Math.Max(tileObj.TileColor.B - 50, 0));
            }
        }


 

        private void RemoveTile(Button tile)
        {
            gameForm.Controls.Remove(tile);
            tiles.Remove(tile);
            tileObjects.Remove(tile);
            clickCounts.Remove(tile);
            if (holdStartTimes.ContainsKey(tile))
                holdStartTimes.Remove(tile);
            tile.Dispose();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            GameSettings.TimeLeft--;
            gameForm.TimeLabel.Text = $"TIME: {GameSettings.TimeLeft}";

            if (GameSettings.TimeLeft <= 0)
            {
                EndGame(true);
            }
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameTimer?.Stop();
            tileTimer?.Stop();
            animationTimer?.Stop();

            foreach (var tile in tiles)
            {
                tile.Dispose();
            }
            tiles.Clear();
            tileObjects.Clear();
            holdStartTimes.Clear();
            clickCounts.Clear();

            SaveScore();
        }

        

        private void ResetGame()
        {
            foreach (var tile in tiles.ToList())
            {
                RemoveTile(tile);
            }

            GameSettings.Score = 0;
            GameSettings.TimeLeft = 60;
            GameSettings.TileSpeed = 5;
            gameForm.ScoreLabel.Text = $"SCORE: {GameSettings.Score}";
            gameForm.TimeLabel.Text = $"TIME: {GameSettings.TimeLeft}";

            gameTimer.Start();
            tileTimer.Start();
            animationTimer.Start();
        }

        private void SaveScore()
        {
            try
            {
                ScoreSerializer serializer = GameSettings.SelectedSerializer == "xml"
                    ? new XmlScoreSerializer()
                    : new JsonScoreSerializer();

                List<int> scores = serializer.Deserialize();
                if (!scores.Contains(GameSettings.Score))
                {
                    scores.Add(GameSettings.Score);
                }
                scores = scores.Distinct()            
                .OrderByDescending(s => s)    
                .Take(10)                     
                .ToList(); ;
                serializer.Serialize(scores);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving score: {ex.Message}");
            }
        }

        public static List<int> LoadScores()
        {
            try
            {
                ScoreSerializer serializer = GameSettings.SelectedSerializer == "xml"
                    ? new XmlScoreSerializer()
                    : new JsonScoreSerializer();

                return serializer.Deserialize();
            }
            catch
            {
                return new List<int>();
            }
        }

        public static void ConvertScores(string fromFormat, string toFormat)
        {
            try
            {
                ScoreSerializer sourceSerializer = fromFormat == "xml"
                    ? new XmlScoreSerializer()
                    : new JsonScoreSerializer();

                ScoreSerializer targetSerializer = toFormat == "xml"
                    ? new XmlScoreSerializer()
                    : new JsonScoreSerializer();

                var scores = sourceSerializer.Deserialize();
                targetSerializer.Serialize(scores);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting scores: {ex.Message}");
            }
        }
    }
}
