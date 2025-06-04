using Model.Core;
using Model.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Model.Data
{
    public abstract class ScoreSerializer : ISerializableState
    {
        public abstract string FileExtension { get; }

        public abstract void Serialize(List<int> scores);
        public abstract List<int> Deserialize();
        public abstract void SaveState(GameState state);
        public abstract GameState LoadState();

        public virtual void ValidateState()
        {
            if (!File.Exists(GetStateFilePath()))
            {
                Console.WriteLine("Файл состояния не найден. Будет создан новый при сохранении.");
                return;
            }

            var fileInfo = new FileInfo(GetStateFilePath());
            if (fileInfo.Length == 0)
            {
                Console.WriteLine("Файл состояния пуст. Будет создан новый.");
                File.Delete(GetStateFilePath());
                return;
            }

            if ((DateTime.Now - fileInfo.LastWriteTime).TotalDays > 30)
            {
                Console.WriteLine("Внимание: файл состояния не обновлялся более 30 дней");
            }
        }

        public virtual void ValidateBeforeSerialization(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data),
                    "Не могу сериализовать null-объект");
            }

            if (!(data is GameState) && !(data is List<int>))
            {
                throw new InvalidOperationException(
                    $"Неподдерживаемый тип данных: {data.GetType().Name}. " +
                    "Поддерживаются только GameState и List<int>");
            }

            if (data is GameState state)
            {
                if (state.Score < 0)
                    throw new InvalidDataException("Счет не может быть отрицательным");

                if (state.TimeLeft < 0)
                    throw new InvalidDataException("Оставшееся время не может быть отрицательным");

                if (state.LanesCount < 1 || state.LanesCount > 10)
                    throw new InvalidDataException("Недопустимое количество дорожек (допустимо 1-10)");
            }

            if (data is List<int> scores)
            {
                if (scores.Count > 100)
                    throw new InvalidDataException("Слишком много результатов (максимум 100)");

                if (scores.Any(score => score < 0))
                    throw new InvalidDataException("Таблица рекордов содержит отрицательные значения");
            }
        }

        protected virtual string GetScoresFilePath()
        {
            return $"scores{FileExtension}";
        }

        protected virtual string GetStateFilePath()
        {
            return $"gamestate{FileExtension}";
        }

        public void Serialize(object data)
        {
            ValidateBeforeSerialization(data);

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

            throw new NotSupportedException($"Тип {type.Name} не поддерживается для десериализации");
        }

        public virtual string GetDefaultScoresFileName()
        {
            return $"scores{FileExtension}";
        }

        public virtual string GetDefaultStateFileName()
        {
            return $"gamestate{FileExtension}";
        }
    }
}
