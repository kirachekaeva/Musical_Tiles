using Model.Data;
using Model.Core;
using System.Collections.Generic;
using System;

public abstract class ScoreSerializer : ISerializableState
{
    public abstract string FileExtension { get; }
    public abstract void Serialize(List<int> scores);
    public abstract List<int> Deserialize();

    public abstract void SaveState(GameState state);
    public abstract GameState LoadState();

    public void Serialize(object data)
    {
        if (data is List<int> scores)
            Serialize(scores);
        else if (data is GameState state)
            SaveState(state);
    }

    public object Deserialize(Type type)
    {
        if (type == typeof(List<int>))
            return Deserialize();
        else if (type == typeof(GameState))
            return LoadState();
        return null;
    }
}