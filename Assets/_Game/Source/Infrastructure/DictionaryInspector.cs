using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Source.Infrastructure
{
    [Serializable]
    public class DictionaryInspector<TKey, TValue>
    {
        [SerializeField] private List<KeyValue<TKey, TValue>> _dict;
        public Dictionary<TKey, TValue> GetDictionary 
            => _dict.ToDictionary(k=>k.Key,v=>v.Value);
    }

    [Serializable]
    internal class KeyValue<T, T1>
    {
        public T Key;
        public T1 Value;
    }
}