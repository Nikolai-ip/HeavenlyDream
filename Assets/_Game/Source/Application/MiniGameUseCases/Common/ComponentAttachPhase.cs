using _Game.Source.Application.Airplane.Adders;
using _Game.Source.Presenter;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Common
{
    public class ComponentAttachPhase : MiniGamePhase
    {
        [SerializeField] private PlaneComponentAdjuster _planeComponentAdjuster;
        [SerializeField] private ComponentAttacher _componentAttacherPrefab;
        
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _space;
        private ComponentAttacher _componentAttacher;

        protected override void OnEnter()
        {
            _componentAttacher = Instantiate(_componentAttacherPrefab, _space);
            _componentAttacher.transform.position = _spawnPoint.position;
            _componentAttacher.transform.localRotation = _spawnPoint.localRotation;
            _componentAttacher.Construct(
                AirplaneModelView,
                _planeComponentAdjuster.AirplaneComponentType);
            _componentAttacher.OnComponentAttached += OnComponentAttached;
        }

        protected override void OnComplete(MiniGameResult result)
        {
            if (result.Status != MiniGameResultStatus.Completed)
                return;

            _planeComponentAdjuster.FinalLocalPos = result.LocalPosition;
            _planeComponentAdjuster.FinalRotation = result.LocalRotation;
            _planeComponentAdjuster.TryAdd();
        }

        protected override void OnExit()
        {
            if (_componentAttacher == null)
                return;

            _componentAttacher.OnComponentAttached -= OnComponentAttached;
            Destroy(_componentAttacher.gameObject);
            _componentAttacher = null;
        }

        private void OnComponentAttached(Vector3 localPosition, Quaternion localRotation)
        {
            CompletePhase(MiniGameResult.Completed(localPosition, localRotation));
        }
    }
}
