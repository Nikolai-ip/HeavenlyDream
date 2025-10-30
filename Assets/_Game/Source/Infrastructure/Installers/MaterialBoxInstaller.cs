using _Game.Source.Application;
using _Game.Source.Infrastructure.Factories;
using Plugins.Pool;
using UnityEngine;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class MaterialBoxInstaller : MonoInstaller
    {
        [SerializeField] private PoolFactory_SO _airplaneMaterialPoolFactory_SO;
        [SerializeField] private Transform _airplaneMaterialContainer;
        public override void InstallBindings()
        {
            var pool = _airplaneMaterialPoolFactory_SO.GetPoolContainer<AirplaneMaterial>(_airplaneMaterialContainer,
                new DIInstantiator(Container));
            Container.Bind<IInitablePool<AirplaneMaterial>>()
                .FromInstance(pool)
                .AsSingle();
        }
    }
}