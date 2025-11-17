using System;
using System.Collections.Generic;

namespace _Game.Source.Application.TempStorages
{
    public class TempGlobalStorage
    {
        private readonly Dictionary<Type, object> _storage = new();

        public void Add<T>(T instance)
        {
            _storage.Add(typeof(T), instance);
        }

        public bool TryGet<T>(out  T instance) where T: class
        {
             _storage.TryGetValue(typeof(T), out var obj);
             instance = obj as T;
             return instance != null;
        }

    }
}