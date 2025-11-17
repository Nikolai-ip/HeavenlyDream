using _Game.Source.Application;
using Plugins.Pool;
using UnityEngine;
using Zenject;

namespace _Game.Source.Utilities
{
    public class AirplaneMaterialSpawner: MonoBehaviour
    {
        [Inject] private IInitablePool<AirplaneMaterial> _airplaneMaterialPool;
        [SerializeField] private Transform _spawnPosition;
        private AirplaneMaterial _airplaneMaterial;

        private void Start()
        {
            _airplaneMaterialPool.Init();
        }

        public void SpawnAirplaneMaterial()
        {
            if (_airplaneMaterial != null && _airplaneMaterial.gameObject.activeInHierarchy) return;
            _airplaneMaterial = _airplaneMaterialPool.GetElement();
            _airplaneMaterial.transform.position = _spawnPosition.position;
        }
        
    }
}