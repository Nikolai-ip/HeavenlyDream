using System.Collections.Generic;
using System.Linq;
using _Game.Source.Domain.Plane.Component;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;

namespace _Game.Source.Domain.Plane
{
    public class PlaneComponentContainer
    {
        private readonly List<PlaneComponent> _planeComponents;
        public IReadOnlyList<PlaneComponent> PlaneComponents => _planeComponents;
        public event Action OnPlaneComponentsChanged;

        public PlaneComponentContainer()
        {
            _planeComponents = new();
        }

        public PlaneComponentContainer(List<PlaneComponent> planeComponents)
        {
            _planeComponents = planeComponents;
        }

        public void AddComponent(PlaneComponent planeComponent, PlaneComponentQuantityMode mode)
        {
            switch (mode)
            {
                case PlaneComponentQuantityMode.Single:
                {
                    if (!_planeComponents.Any(c => c.GetType() == planeComponent.GetType()))
                    {
                        _planeComponents.Add(planeComponent);
                        Debug.Log($"[PlaneComponentContainer] Added Component: {planeComponent.GetType().Name}, with single mode");
                        OnPlaneComponentsChanged?.Invoke();
                    }
                    Debug.Log($"[PlaneComponentContainer] Failed add component: {planeComponent.GetType().Name} because there is component exist with same type");

                    break;
                }
                case PlaneComponentQuantityMode.Multiple:
                    _planeComponents.Add(planeComponent);
                    Debug.Log($"[PlaneComponentContainer] Added Component: {planeComponent.GetType().Name}, with multiple mode");
                    OnPlaneComponentsChanged?.Invoke();
                    break;
            }
        }

        public bool TryGetComponent<T>(out T component) where T : PlaneComponent
        {
            component = _planeComponents.FirstOrDefault(c=>c.GetType() == typeof(T)) as T;
            return component != null;
        }
    }

    public enum PlaneComponentQuantityMode
    {
        Single,
        Multiple
    }
}