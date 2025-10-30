using UnityEngine;
using Zenject;

namespace Plugins.DIHelpers
{
    public abstract class SubInstaller:MonoBehaviour
    {
        public abstract void InstallBindings(DiContainer Container);
    }
}