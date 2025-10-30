using UnityEngine;

namespace Plugins.Pool
{
    public class PoolContainer<T>:IPool<T> where T : MonoBehaviour
    {
        private readonly PoolMono<T> _poolMono;

        public PoolContainer(PoolConfig config, Transform container)
        {
            _poolMono = new PoolMono<T>(config.Prefab as T, config.PoolSize, container, config.AutoExpand);
        }

        public T GetElement()
        {
            return _poolMono.GetFreeElement();
        }
    }

    public interface IPool<out T> where T : MonoBehaviour
    {
        T GetElement();
    }
    
}