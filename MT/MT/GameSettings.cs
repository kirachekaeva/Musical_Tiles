namespace MT
{
    public static class GameSettings
    {
        public static int Score { get; set; } = 0;
        public static int TimeLeft { get; set; } = 60;
        public static int TileSpeed { get; set; } = 5;
        public static int TileWidth { get; set; } = 80;
        public static int TileHeight { get; set; } = 30;
        public static int LanesCount { get; set; } = 4;
        public static int ClickableAreaHeight { get; set; } = 120;
        public static int MinTileGap { get; set; } = 30;
        public static string SelectedSerializer { get; set; } = "json";
    }
}