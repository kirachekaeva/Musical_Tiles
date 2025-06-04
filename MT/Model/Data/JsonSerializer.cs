using Model.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MT
{
    public class JsonScoreSerializer : ScoreSerializer
    {
        private const string ScoresFileName = "scores.json";
        private const string StateFileName = "gamestate.json";

        public override string FileExtension => ".json";

        public override void Serialize(List<int> scores)
        {
            string json = JsonConvert.SerializeObject(scores, Formatting.Indented);
            File.WriteAllText(ScoresFileName, json);
        }

        public override List<int> Deserialize()
        {
            if (File.Exists(ScoresFileName))
            {
                string json = File.ReadAllText(ScoresFileName);
                return JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>();
            }
            return new List<int>();
        }

        public override void SaveState(GameState state)
        {
            string json = JsonConvert.SerializeObject(state, Formatting.Indented);
            File.WriteAllText(StateFileName, json);
        }

        public override GameState LoadState()
        {
            if (File.Exists(StateFileName))
            {
                string json = File.ReadAllText(StateFileName);
                return JsonConvert.DeserializeObject<GameState>(json) ?? new GameState();
            }
            return new GameState();
        }
    }
}