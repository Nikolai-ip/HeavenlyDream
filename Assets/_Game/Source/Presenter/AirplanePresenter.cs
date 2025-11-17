using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;
using Zenject;

namespace _Game.Source.Presenter
{
    public class AirplanePresenter: MonoBehaviour
    {
        private PlaneComponentContainer _planeComponentContainer;
        private Dictionary<Type, Action<PlaneComponent>> _adjustViewComponentActions;
        [SerializeField] private PlaneComponentObjects _componentObjects;
        
        [Inject]
        public void Construct(PlaneComponentContainer planeComponentContainer)
        {
            _planeComponentContainer = planeComponentContainer;
            _adjustViewComponentActions = new()
            {
                { typeof(FuselageComponent), AdjustFuselageView },
                { typeof(WingComponent), AdjustWingView },
                { typeof(StabilizerComponent), AdjustStabilizerView },
                { typeof(AirplaneFinComponent), AdjustFinView},
                { typeof(BalancingWeightComponent), AdjustBalancingWeightView}
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
                var componentType = planeComponent.GetType();
                if (_adjustViewComponentActions.TryGetValue(componentType, out var adjustComponentView))
                {
                    adjustComponentView.Invoke(planeComponent);
                }
                else
                {
                    Debug.LogError($"Failed to adjust view component with type {componentType}");
                }
            }
        }
        private void AdjustFuselageView(PlaneComponent component)
        {
            var fuselage = (FuselageComponent)component;   
            _componentObjects.Fuselage.gameObject.SetActive(true);
            
        }
        private void AdjustWingView(PlaneComponent component)
        {
            var wing = (WingComponent)component;
            _componentObjects.Wing.gameObject.SetActive(true);
            _componentObjects.Wing.transform.localPosition = new Vector3(wing.CenterOffsetVertical, _componentObjects.Wing.transform.localPosition.y , wing.CenterOffsetHorizontal); 
        }
        private void AdjustStabilizerView(PlaneComponent component)
        {
            var stabilizerTr = _componentObjects.Stabilizer.transform;
            var stabilizer = (StabilizerComponent)component;
            stabilizerTr.gameObject.SetActive(true);
            stabilizerTr.localPosition = new Vector3(stabilizerTr.localPosition.x, stabilizerTr.localPosition.y, stabilizer.CenterOffsetHorizontal);
        }

        private void AdjustFinView(PlaneComponent component)
        {
            var fin = (AirplaneFinComponent)component;
            var finTr = _componentObjects.AirplaneFin;
            finTr.gameObject.SetActive(true);
            finTr.localPosition = new Vector3(finTr.localPosition.x, finTr.localPosition.y, fin.CenterOffsetVertical);
        }

        private void AdjustBalancingWeightView(PlaneComponent component)
        {
            var weightTr = _componentObjects.BalancingWeight;
            var weight = (BalancingWeightComponent)component;
            weightTr.gameObject.SetActive(true);
            weightTr.localScale = new Vector3(weightTr.localScale.x, weight.WeightAmount, weight.WeightAmount);
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