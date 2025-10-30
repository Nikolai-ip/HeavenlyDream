using Plugins.Pool;
using UnityEngine;
using Zenject;

namespace _Game.Source.Infrastructure.Factories
{
    public class DIInstantiator: IObjectInstantiator
    {
        private readonly DiContainer _container;

        public DIInstantiator(DiContainer container)
        {
            _container = container;
        }

        public T Instantiate<T>(T prefab, Transform container) where T : MonoBehaviour
        {
            return _container.InstantiatePrefab(prefab,container).GetComponent<T>();
        }
    }
}