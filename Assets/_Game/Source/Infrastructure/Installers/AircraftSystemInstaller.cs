using _Game.Source.Domain.Plane;
using _Game.Source.Presenter;
using Plugins.DIHelpers;
using UnityEngine;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class AircraftSystemInstaller: SubInstaller
    {
        [SerializeField] private AirplaneModelView _aircraftModel;
        public override void InstallBindings(DiContainer Container)
        {
            Container.Bind<PlaneComponentContainer>().AsSingle();
            Container.Bind<AirplaneModelView>().FromInstance(_aircraftModel).AsSingle();
        }
    }
}