using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;

namespace _Game.Source.Domain.Plane
{
    public class PlaneComponentContainer
    {
        private readonly Dictionary<Guid, PlaneComponent> _planeComponents = new();
        public IReadOnlyList<PlaneComponent> PlaneComponents => _planeComponents.Values.ToList();
        
        public event Action OnPlaneComponentsChanged;
        public PlaneComponentContainer(List<PlaneComponent> planeComponents)
        {
            foreach (var planeComponent in planeComponents)
            {
                _planeComponents.Add(Guid.NewGuid(), planeComponent);
            }
        }

        public Guid AddComponent(PlaneComponent planeComponent)
        {
            var id = Guid.NewGuid();

            _planeComponents.Add(id, planeComponent);
            Debug.Log($"[PlaneComponentContainer] Added Component: {planeComponent.GetType().Name}, with multiple mode");
            OnPlaneComponentsChanged?.Invoke();
            return id;
        }

        public bool TryGetComponent<T>(Guid id, out T component)  where T : PlaneComponent
        {
            component = null;
            if (!_planeComponents.TryGetValue(id, out var planeComponent) ||
                planeComponent is not T planeComponentT) return false;
            
            component = planeComponentT;
            return true;

        }
        
        // public bool TryGetComponent(AirplaneComponentType type, out PlaneComponent component)
        // {
        //     component = _planeComponents.FirstOrDefault(c=>c.Type == type);
        //     return component != null;
        // }
        
    }

    public enum PlaneComponentQuantityMode
    {
        Single,
        Multiple
    }
}