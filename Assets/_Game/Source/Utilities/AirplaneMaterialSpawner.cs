using _Game.Source.Application;
using Plugins.Pool;
using UnityEngine;
using Zenject;

namespace _Game.Source.Utilities
{
    public class AirplaneMaterialSpawner: MonoBehaviour
    {
        [Inject] private IInitablePool<AirplaneMaterial> _airplaneMaterialPool;
        [SerializeField] private Vector3 _spawnPosition;

        private void Start()
        {
            _airplaneMaterialPool.Init();
        }

        public void SpawnAirplaneMaterial()
        {
            var airplaneMaterial = _airplaneMaterialPool.GetElement();
            airplaneMaterial.transform.position = _spawnPosition;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_spawnPosition, 0.2f);
        }
    }
}