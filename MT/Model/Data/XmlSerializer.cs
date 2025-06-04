using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model.Core;

namespace MT
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
                    return (List<int>)serializer.Deserialize(reader) ?? new List<int>();
                }
            }
            return new List<int>();
        }

        public override void SaveState(GameState state)
        {
            var serializer = new XmlSerializer(typeof(GameState));
            using (var writer = new StreamWriter(StateFileName))
            {
                serializer.Serialize(writer, state);
            }
        }

        public override GameState LoadState()
        {
            if (File.Exists(StateFileName))
            {
                var serializer = new XmlSerializer(typeof(GameState));
                using (var reader = new StreamReader(StateFileName))
                {
                    return (GameState)serializer.Deserialize(reader) ?? new GameState();
                }
            }
            return new GameState();
        }

        public void Serialize(object data, string fileName)
        {
            if (data == null) return;

            var type = data.GetType();
            var serializer = new XmlSerializer(type);
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, data);
            }
        }

        public object Deserialize(string fileName, Type type)
        {
            if (File.Exists(fileName))
            {
                var serializer = new XmlSerializer(type);
                using (var reader = new StreamReader(fileName))
                {
                    return serializer.Deserialize(reader);
                }
            }
            return Activator.CreateInstance(type);
        }
    }
}