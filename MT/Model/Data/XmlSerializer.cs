using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model.Core;
using Model.Data;

namespace Model.Data
{
    public class XmlScoreSerializer : ScoreSerializer
    {
        private const string ScoresFileName = "scores.xml";
        private const string StateFileName = "gamestate.xml";

        public override string FileExtension => ".xml";

        public override void Serialize(List<int> scores)
        {
            var serializer = new XmlSerializer(typeof(List<int>));
            using (var writer = new StreamWriter(ScoresFileName))
            {
                serializer.Serialize(writer, scores);
            }
        }

        public override List<int> Deserialize()
        {
            if (File.Exists(ScoresFileName))
            {
                var serializer = new XmlSerializer(typeof(List<int>));
                using (var reader = new StreamReader(ScoresFileName))
                {
                    return Deserialize<List<int>>(ScoresFileName) ?? new List<int>();
                }
            }
            return new List<int>();
        }

        public override void SaveState(GameState state)
        {
            ((ISerializableState)this).ValidateState();

            var serializer = new XmlSerializer(typeof(GameState));
            using (var writer = new StreamWriter(StateFileName))
            {
                serializer.Serialize(writer, state);
            }
        }

        public override GameState LoadState()
        {
            return Deserialize<GameState>(StateFileName) ?? new GameState();
        }


        private T Deserialize<T>(string fileName) where T : new()
        {
            if (!File.Exists(fileName)) return new T();

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(fileName))
            {
                object result = serializer.Deserialize(reader);
                return result is T validResult ? validResult : new T();
            }
        }

        public void Serialize(object data, string fileName)
        {
            if (data == null) return;

            ((ScoreSerializer)this).ValidateBeforeSerialization(data);

            var type = data.GetType();
            var serializer = new XmlSerializer(type);
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}
