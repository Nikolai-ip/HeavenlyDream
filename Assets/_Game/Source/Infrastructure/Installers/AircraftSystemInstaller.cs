using _Game.Source.Domain.Plane;
using Plugins.DIHelpers;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class AircraftSystemInstaller: SubInstaller
    {
        public override void InstallBindings(DiContainer Container)
        {
            Container.Bind<PlaneComponentContainer>().AsSingle();
        }
    }
}