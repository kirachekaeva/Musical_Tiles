using System.Collections.Generic;

namespace MT
{
    public abstract class ScoreSerializer
    {
        public abstract void Serialize(List<int> scores);
        public abstract List<int> Deserialize();
        public abstract string FileExtension { get; }
    }
}