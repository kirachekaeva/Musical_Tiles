using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Model.Core
{
    [Serializable]
    [XmlRoot("GameState")]
    public class GameState
    {
        [XmlElement("Score")]
        public int Score { get; set; }

        [XmlElement("TimeLeft")]
        public int TimeLeft { get; set; }

        [XmlElement("TileSpeed")]
        public int TileSpeed { get; set; }

        [XmlElement("LanesCount")]
        public int LanesCount { get; set; }

        [XmlArray("HighScores")]
        [XmlArrayItem("Score")]
        public List<int> HighScores { get; set; } = new List<int>();
    }
}