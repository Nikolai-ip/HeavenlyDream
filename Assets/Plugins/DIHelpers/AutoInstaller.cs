using System.Linq;
using Zenject;

namespace Plugins.DIHelpers
{
    public class AutoInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            var installers = GetComponentsInChildren<AutoInstaller>().ToList();
            installers.ForEach(installer=>installer.InstallBindings());
        }
        
    }
}