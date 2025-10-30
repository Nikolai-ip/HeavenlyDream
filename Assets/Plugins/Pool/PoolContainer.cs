using UnityEngine;

namespace Plugins.Pool
{
    public class PoolContainer<T>:IInitablePool<T> where T : MonoBehaviour
    {
        private readonly PoolConfig _config;
        private readonly Transform _container;
        private readonly IObjectInstantiator _instantiator;
        private PoolMono<T> _poolMono;
        private bool _init;

        public PoolContainer(PoolConfig config, Transform container, IObjectInstantiator instantiator)
        {
            _config = config;
            _container = container;
            _instantiator = instantiator;
        }

        public T GetElement()
        {
            if (!_init) Init();
            return _poolMono.GetFreeElement();
        }

        public void Init()
        {
            _poolMono = new PoolMono<T>(_config.Prefab as T, _config.PoolSize, _container, _config.AutoExpand, _instantiator);
            _init = true;
        }
    }

    public interface IInitablePool<out T> where T : MonoBehaviour
    {
        T GetElement();
        void Init();
    }
    
}