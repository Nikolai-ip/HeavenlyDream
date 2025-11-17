using System;
using System.Collections.Generic;
using _Game.Source.Domain.ErrorCalculation;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Application.TempStorages
{
    public class AircraftMadeResultStorage
    {
        public ErrorEvaluateResult ErrorEvaluateResult { get; private set; }

        public AircraftMadeResultStorage()
        {
            ErrorEvaluateResult = new()
            {
                CommonError = 0.3f,
                LocalErrors = new Dictionary<Type, float>()
                {
                    {typeof(FuselageComponent), 0f},
                    {typeof(WingComponent), 0.1f},
                    {typeof(StabilizerComponent), 0f},
                    {typeof(AirplaneFinComponent), 0},
                    {typeof(BalancingWeightComponent), 0.2f},
                }
            };
        }
    }
}