using UnityEngine;

namespace Plugins.Pool
{
    public interface IObjectInstantiator
    {
        T Instantiate<T>(T prefab, Transform container) where T : MonoBehaviour;
    }
}