using UnityEngine;

namespace Plugins.Pool
{
    public class ObjectInstantiator: IObjectInstantiator
    {
        public T Instantiate<T>(T prefab, Transform container) where T : MonoBehaviour
        {
            return Object.Instantiate(prefab, container);
        }
    }
}