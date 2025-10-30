using System;
using UnityEngine;

namespace Plugins.Pool
{
    [CreateAssetMenu(fileName = "Pool", menuName = "StaticFactories/Pool", order = 0)]
    public class PoolFactory_SO: ScriptableObject
    {
        [SerializeField] private PoolConfig _config;

        public IInitablePool<T> GetPoolContainer<T>(Transform container, IObjectInstantiator instantiator) where T:MonoBehaviour
        {
            Type type = _config.Prefab.GetType(); 
            var poolContainerType = typeof(PoolContainer<>).MakeGenericType(type);
            var poolContainer = (IInitablePool<T>) Activator.CreateInstance(poolContainerType, _config, container, instantiator);
            return poolContainer;
        }
    }
}