using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class GenericLoader<TValue> : ILoader<int, TValue> where TValue : IConvertRowData<TValue>
    {
        public List<TValue> rows = new();

        Dictionary<int, TValue> ILoader<int, TValue>.MakeDict()
        {
            Dictionary<int, TValue> dict = new();

            foreach(TValue row in rows)
            {
                int key = row.Id;
                dict[key] = row;
            }
            return dict;
        }
    }

    [Serializable]
    public class WaveData : IConvertRowData<WaveData>
    {
        public int Id => id;

        public int id;
        public int wave;
        public string zombieKey;
        public float waitTime;
        public float spawnInterval;
        public int spawnCount;

        public WaveData ConvertRow(List<string> row)
        {
            return new WaveData
            {
                id = int.Parse(row[0]),
                wave = int.Parse(row[1]),
                zombieKey = row[2],
                waitTime = float.Parse(row[3]),
                spawnInterval = float.Parse(row[4]),
                spawnCount = int.Parse(row[5])
            };
        }
    }
}

