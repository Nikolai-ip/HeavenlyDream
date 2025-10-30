using System.Linq;
using Zenject;

namespace Plugins.DIHelpers
{
    public class AutoInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            var installers = GetComponentsInChildren<SubInstaller>().ToList();
            installers.ForEach(installer=>installer.InstallBindings(Container));
        }
        
    }
}