using _Game.Source.Application.TempStorages;
using Plugins.DIHelpers;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class TempGlobalStorageInstaller: SubInstaller
    {
        public override void InstallBindings(DiContainer Container)
        {
            Container.Bind<AircraftMadeResultStorage>().FromInstance(new AircraftMadeResultStorage()).AsSingle();
        }
    }
}