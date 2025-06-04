using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MT
{
    public class XmlScoreSerializer : ScoreSerializer
    {
        private const string FileName = "scores.xml";

        public override string FileExtension => ".xml";

        public override void Serialize(List<int> scores)
        {
            var serializer = new XmlSerializer(typeof(List<int>));
            using (var writer = new StreamWriter(FileName))
            {
                serializer.Serialize(writer, scores);
            }
        }

        public override List<int> Deserialize()
        {
            if (File.Exists(FileName))
            {
                var serializer = new XmlSerializer(typeof(List<int>));
                using (var reader = new StreamReader(FileName))
                {
                    return (List<int>)serializer.Deserialize(reader);
                }
            }
            return new List<int>();
        }
    }
}