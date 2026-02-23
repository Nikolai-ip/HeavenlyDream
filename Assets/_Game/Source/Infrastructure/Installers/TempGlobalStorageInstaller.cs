using _Game.Source.Application.TempStorages;
using NaughtyAttributes;
using Plugins.DIHelpers;
using UnityEngine;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class TempGlobalStorageInstaller: SubInstaller
    {
        [SerializeField] private bool _useTestValues;
        [SerializeField, ShowIf(nameof(_useTestValues))] private float CommonError;
        [SerializeField, ShowIf(nameof(_useTestValues))] private float FuselageError;
        [SerializeField, ShowIf(nameof(_useTestValues))] private float WingError;
        [SerializeField, ShowIf(nameof(_useTestValues))] private float StabilizerError;
        [SerializeField, ShowIf(nameof(_useTestValues))] private float AirplaneFinError;
        [SerializeField, ShowIf(nameof(_useTestValues))] private float BalancingWeightError;
        
        public override void InstallBindings(DiContainer Container)
        {
            var aircraftMadeResultStorage = _useTestValues? 
                new AircraftMadeResultStorage(CommonError, FuselageError, WingError, StabilizerError, AirplaneFinError, BalancingWeightError)
                : new AircraftMadeResultStorage();
            
            Container.Bind<AircraftMadeResultStorage>().FromInstance(aircraftMadeResultStorage).AsSingle();
        }
    }
}