using System;
using System.Collections.Generic;
using _Game.Source.Domain.ErrorCalculation;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Application.TempStorages
{
    public class AircraftMadeResultStorage
    {
        public ErrorEvaluateResult ErrorEvaluateResult { get; private set; }

        public AircraftMadeResultStorage(float commonError, float fuselageError, float wingError, float stabilizerError, float finError, float balancingWeight)
        {
            ErrorEvaluateResult = new()
            {
                CommonError = commonError,
                LocalErrors = new Dictionary<Type, float>()
                {
                    {typeof(FuselageComponent), fuselageError},
                    {typeof(WingComponent), wingError},
                    {typeof(StabilizerComponent), stabilizerError},
                    {typeof(AirplaneFinComponent), finError},
                    {typeof(BalancingWeightComponent), balancingWeight},
                }
            };
        }

        public AircraftMadeResultStorage()
        {
            ErrorEvaluateResult = new()
            {
                CommonError = 0,
                LocalErrors = new Dictionary<Type, float>()
                {
                    { typeof(FuselageComponent), 0 },
                    { typeof(WingComponent), 0 },
                    { typeof(StabilizerComponent), 0 },
                    { typeof(AirplaneFinComponent), 0 },
                    { typeof(BalancingWeightComponent), 0 },
                }
            };
        }
    }
}