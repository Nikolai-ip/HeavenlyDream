using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Source.Application;
using _Game.Source.Application.MiniGameUseCases.Common;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;
using Zenject;

namespace _Game.Source.Presenter
{
    public class AirplaneModelView: MonoBehaviour
    {
        private PlaneComponentContainer _planeComponentContainer;
        private Dictionary<AirplaneComponentType, Action<PlaneComponent>> _adjustViewComponentActions;
        [SerializeField] private AirplaneRotator _airplaneRotator;
        [SerializeField] private PlaneComponentObjects _componentObjects;
        
        public Vector2 Center => transform.position;
        public Transform FuselageTr => _componentObjects.Fuselage;
        public float FuselageSize => _componentObjects.Fuselage.GetComponent<BoxCollider>().bounds.size.y;

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
        }
        
        private void OnEnable() => _planeComponentContainer.OnPlaneComponentsChanged += UpdateView;

        private void OnDisable() => _planeComponentContainer.OnPlaneComponentsChanged -= UpdateView;

        private void Start()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _componentObjects.DisableAll();
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
        private void AdjustFuselageView(PlaneComponent component)
        {
            var fuselage = (FuselageComponent)component;   
            _componentObjects.Fuselage.gameObject.SetActive(true);
            
        }
        private void AdjustComponentView(PlaneComponent component)
        {
            Transform tr = GetPlaneComponentTr(component.Type);
            tr.gameObject.SetActive(true);
            tr.localPosition = component.LocalPosition;
            tr.localRotation = component.LocalRotation;
        }

        private Transform GetPlaneComponentTr(AirplaneComponentType componentType)
        {
            switch (componentType)
            {
                case AirplaneComponentType.Wing:
                    return _componentObjects.Wing;
                case AirplaneComponentType.Stabilizer:
                    return _componentObjects.Stabilizer;
                case AirplaneComponentType.Fin:
                    return _componentObjects.AirplaneFin;
            }
            throw new ArgumentOutOfRangeException(nameof(componentType));
        }


        private void AdjustBalancingWeightView(PlaneComponent component)
        {
            var weightTr = _componentObjects.BalancingWeight;
            var weight = (BalancingWeightComponent)component;
            weightTr.gameObject.SetActive(true);
            weightTr.localScale = new Vector3(weightTr.localScale.x, weight.WeightAmount, weight.WeightAmount);
        }
        

        public void Rotate(RotationType rotation)
        {
            _airplaneRotator.Rotate(rotation);
        }
    }

    [Serializable]
    public struct PlaneComponentObjects
    {
        [SerializeField] public Transform Fuselage;
        [SerializeField] public Transform Wing;
        [SerializeField] public Transform Stabilizer;
        [SerializeField] public Transform AirplaneFin;
        [SerializeField] public Transform BalancingWeight;
        public void DisableAll()
        {
            Fuselage.gameObject.SetActive(false);
            Wing.gameObject.SetActive(false);
            Stabilizer.gameObject.SetActive(false);
            AirplaneFin.gameObject.SetActive(false);
            BalancingWeight.gameObject.SetActive(false);
        }
    }
}