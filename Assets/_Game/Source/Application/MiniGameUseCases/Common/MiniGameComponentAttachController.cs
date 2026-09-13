using _Game.Source.Application.Airplane.Adders;
using _Game.Source.Application.MiniGameUseCases.AttackWingMiniGame;
using _Game.Source.Presenter;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.MiniGameUseCases
{
    public class MiniGameComponentAttachController: MiniGameController
    {
        [SerializeField] private PlaneComponentAdjuster _planeComponentAdjuster;
        [SerializeField] private ComponentAttacher _componentAttacherPref;
        [SerializeField] private AirplaneModelView _airplaneModelView;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _space;
        private ComponentAttacher _componentAttacher;
        public override void OnMiniGameStart()
        {
            base.OnMiniGameStart();
            _componentAttacher = Instantiate(_componentAttacherPref, _space);
            _componentAttacher.transform.position = _spawnPoint.position;
            _componentAttacher.transform.localRotation = _spawnPoint.localRotation;
            _componentAttacher.Construct(_airplaneModelView);
            _componentAttacher.OnComponentAttached += OnComponentAttached;
        }

        public override void OnMiniGameEnd()
        {
            _componentAttacher.OnComponentAttached -= OnComponentAttached;
        }

        private void OnComponentAttached(Vector3 localPos, Quaternion localRot)
        {
            base.OnMiniGameEnd();
            _planeComponentAdjuster.FinalLocalPos =  localPos;
            _planeComponentAdjuster.FinalRotation = localRot;
            _planeComponentAdjuster.TryAdd();
            Destroy(_componentAttacher);
            _componentAttacher = null;
        }

        private void OnDisable()
        {
            if (_componentAttacher != null)
                _componentAttacher.OnComponentAttached -= OnComponentAttached;
        }

    }
}