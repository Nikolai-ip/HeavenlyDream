using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.Pool
{
    public class PoolMono<T>:IEnumerable<T> where T:MonoBehaviour
    {
        private List<T> _pool;
        private readonly T _prefab;
        private readonly bool _autoExpand;
        private readonly Transform _container;
        private readonly IObjectInstantiator _instantiator;
        
        public PoolMono(T prefab, int poolCapacity, IObjectInstantiator instantiator)
        {
            _prefab = prefab;
            _instantiator = instantiator;
            _container = null;
            CreatePool(poolCapacity);
        }
        
        public PoolMono(T prefab, int poolCapacity, Transform container, bool autoExpand, IObjectInstantiator instantiator)
        {
            _prefab = prefab;
            _container = container;
            _autoExpand = autoExpand;
            _instantiator = instantiator;
            CreatePool(poolCapacity);
        }
        
        private void CreatePool(int capacity)
        {
            _pool = new List<T>();
            for (int i = 0; i < capacity; i++)
            {
                _pool.Add(CreateObject());
            }
        }
        
        private T CreateObject(bool isActiveByDefault = false)
        {
            var instance = _instantiator.Instantiate(_prefab, _container);
            instance.gameObject.SetActive(isActiveByDefault);
            return instance;
        }
        
        public bool HasFreeElement(out T element)
        {
            foreach (var obj in _pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    element = obj;
                    obj.gameObject.SetActive(true);
                    return true;
                }
            }
            element = null;
            return false;
        }
        
        public T GetFreeElement()
        {
            if (HasFreeElement(out T element))
            {
                return element;
            }
            if (_autoExpand)
                return CreateObject(true);
            throw new System.Exception($"Pool<{typeof(T)}is overflow");
        }
        
        public T[] GetFreeElements(int count)
        {
            T[] elements = new T[count];    
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = GetFreeElement();
            }
            return elements;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _pool.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _pool.GetEnumerator();
        }
    }
}