using _Game.Source.Domain.Plane;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.Airplane.Adders
{
    public abstract class PlaneComponentController: MonoBehaviour, IPlaneComponentAdder, IPlaneComponentAdjustment
    {
        [Inject] protected PlaneComponentContainer PlaneComponentContainer;
        public abstract void TryAdd();
        public virtual void TryAdjustComponentParams()
        { }
    }
}