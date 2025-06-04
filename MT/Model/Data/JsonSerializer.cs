using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MT
{
    public class JsonScoreSerializer : ScoreSerializer
    {
        private const string FileName = "scores.json";

        public override string FileExtension => ".json";

        public override void Serialize(List<int> scores)
        {
            string json = JsonConvert.SerializeObject(scores, Formatting.Indented);
            File.WriteAllText(FileName, json);
        }

        public override List<int> Deserialize()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                return JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>();
            }
            return new List<int>();
        }
    }
}