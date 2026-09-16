using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Source.Application;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using _Game.Source.Infrastructure;
using UnityEngine;
using Zenject;

namespace _Game.Source.Presenter
{
    public class AirplaneModelView: MonoBehaviour
    {
        private PlaneComponentContainer _planeComponentContainer;
        private Dictionary<AirplaneComponentType, Action<PlaneComponent>> _adjustViewComponentActions;
        [SerializeField] private AirplaneRotator _airplaneRotator;
        [SerializeField] private DictionaryInspector<AirplaneComponentType, AirplaneSurface> _componentObjectsInspector;
        private Dictionary<AirplaneComponentType, AirplaneSurface> _componentObjects;
        
        [Inject]
        public void Construct(PlaneComponentContainer planeComponentContainer)
        {
            _planeComponentContainer = planeComponentContainer;
            _adjustViewComponentActions = new()
            {
                {AirplaneComponentType.Fuselage, AdjustFuselageView },
                {AirplaneComponentType.Wing, AdjustComponentView },
                {AirplaneComponentType.Stabilizer, AdjustComponentView },
                {AirplaneComponentType.Fin, AdjustComponentView},
                {AirplaneComponentType.Balancing, AdjustBalancingWeightView}
            };
            _componentObjects = _componentObjectsInspector.GetDictionary;
        }

        public AirplaneSurface GetSurface(AirplaneComponentType componentType)
        {
            return _componentObjects[componentType];
        }
        
        private void OnEnable() => _planeComponentContainer.OnPlaneComponentsChanged += UpdateView;

        private void OnDisable() => _planeComponentContainer.OnPlaneComponentsChanged -= UpdateView;

        private void Start()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            DisableAllComponents();

            foreach (var planeComponent in _planeComponentContainer.PlaneComponents)
            {
                if (_adjustViewComponentActions.TryGetValue( planeComponent.Type, out var adjustComponentView))
                {
                    adjustComponentView.Invoke(planeComponent);
                }
                else
                {
                    Debug.LogError($"Failed to adjust view component with type { planeComponent.Type}");
                }
            }
        }

        private void DisableAllComponents()
        {
            _componentObjects.Values.ToList().ForEach(t => t.gameObject.SetActive(false));
        }

        private void AdjustFuselageView(PlaneComponent component)
        {
            _componentObjects[component.Type].gameObject.SetActive(true);
            
        }
        private void AdjustComponentView(PlaneComponent component)
        {
            Transform tr = _componentObjects[component.Type].transform;
            tr.gameObject.SetActive(true);
            tr.localPosition = component.LocalPosition;
            tr.localRotation = component.LocalRotation;
        }
        
        private void AdjustBalancingWeightView(PlaneComponent component)
        {
            var weight = (BalancingWeightComponent)component;
            var weightTr = _componentObjects[AirplaneComponentType.Balancing].transform;
            weightTr.gameObject.SetActive(true);
            weightTr.localScale = new Vector3(weightTr.localScale.x, weight.WeightAmount, weight.WeightAmount);
        }

        public void Rotate(RotationType rotation)
        {
            _airplaneRotator.Rotate(rotation);
        }
    }
}