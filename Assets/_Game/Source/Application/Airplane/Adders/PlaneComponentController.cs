using System;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.Airplane.Adders
{
    public class PlaneComponentAdjuster: MonoBehaviour, IPlaneComponentAdder, IPlaneComponentAdjustment
    {
        [Inject] protected PlaneComponentContainer PlaneComponentContainer;
        [SerializeField] private AirplaneComponentType _ajustedComponentType;
        [field: SerializeField] [field:ReadOnly] public Vector3 FinalLocalPos { private get; set; }
        [field: SerializeField] [field:ReadOnly] public Quaternion FinalRotation { private get; set; }
        public AirplaneComponentType AirplaneComponentType => _ajustedComponentType;
        protected Guid PlaneComponentId; 
        public virtual void TryAdd()
        {
            PlaneComponentId = PlaneComponentContainer.AddComponent(new PlaneComponent(FinalLocalPos, FinalRotation, _ajustedComponentType));
        }

        public virtual void TryAdjustComponentParams()
        {
            if (PlaneComponentId.Equals(Guid.Empty))
            {
                Debug.LogException(new OperationCanceledException("Failed to adjust parameters to not existing plane component"));
                return;
            }
            
            PlaneComponentContainer.TryGetComponent(PlaneComponentId, out PlaneComponent planeComponent);
            
            planeComponent.LocalPosition = FinalLocalPos;
            planeComponent.LocalRotation = FinalRotation;
        }
    }
}