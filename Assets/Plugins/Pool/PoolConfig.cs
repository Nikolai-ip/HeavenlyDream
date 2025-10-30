using System;
using UnityEngine;

namespace Plugins.Pool
{
    [Serializable]
    public class PoolConfig
    {
        [field: SerializeField] public MonoBehaviour Prefab { get; private set; }
        [field: SerializeField] public int PoolSize { get; private set; }
        [field: SerializeField] public bool AutoExpand { get; private set; }
    }
}