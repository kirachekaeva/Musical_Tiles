using Model.Core;
using Model.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Model.Data
{
    public class JsonScoreSerializer : ScoreSerializer
    {
        public override string FileExtension => ".json";

        public override void Serialize(List<int> scores)
        {
            ScoreSerializer baseSerializer = this;
            baseSerializer.ValidateBeforeSerialization(scores);

            string json = JsonConvert.SerializeObject(scores, Formatting.Indented);
            File.WriteAllText(GetScoresFilePath(), json);
        }

        public override List<int> Deserialize()
        {
            string filePath = GetScoresFilePath();
            if (!File.Exists(filePath))
                return new List<int>();

            ISerializableState serializable = this;
            serializable.ValidateState();

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>();
        }

        public override void SaveState(GameState state)
        {
            ((ISerializableState)this).ValidateBeforeSerialization(state);

            string json = JsonConvert.SerializeObject(state, Formatting.Indented);
            File.WriteAllText(GetStateFilePath(), json);
        }

        public override GameState LoadState()
        {
            string filePath = GetStateFilePath();
            if (!File.Exists(filePath))
                return new GameState();

            ScoreSerializer serializer = this;
            serializer.ValidateState();

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GameState>(json) ?? new GameState();
        }

        public new void Serialize(object data)
        { 
            ((ScoreSerializer)this).Serialize(data);
        }

        public new object Deserialize(Type type)
        {
            ISerializableState serializable = this;
            serializable.ValidateState();

            return ((ScoreSerializer)this).Deserialize(type);
        }

        public string SerializeToString(object data)
        {
            if (data is GameState state)
            {
                ((ISerializableState)this).ValidateBeforeSerialization(state);
                return JsonConvert.SerializeObject(state);
            }

            if (data is List<int> scores)
            {
                ScoreSerializer serializer = this;
                serializer.ValidateBeforeSerialization(scores);
                return JsonConvert.SerializeObject(scores);
            }

            throw new InvalidOperationException("Unsupported data type");
        }
    }
}
