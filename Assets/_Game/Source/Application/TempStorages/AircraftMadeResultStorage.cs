using System;
using System.Collections.Generic;
using _Game.Source.Domain.ErrorCalculation;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Application.TempStorages
{
    public class AircraftMadeResultStorage
    {
        public ErrorEvaluateResult ErrorEvaluateResult { get; private set; }

        public AircraftMadeResultStorage(float commonError, float fuselageError, float wingError, float stabilizerError, float finError, float balancingError)
        {
            ErrorEvaluateResult = new()
            {
                CommonError = commonError,
                LocalErrors = new Dictionary<AirplaneComponentType, float>()
                {
                    {AirplaneComponentType.Fuselage, fuselageError},
                    {AirplaneComponentType.Wing, wingError},
                    {AirplaneComponentType.Stabilizer, stabilizerError},
                    {AirplaneComponentType.Fin, finError},
                    {AirplaneComponentType.Balancing, balancingError},
                }
            };
        }

        public AircraftMadeResultStorage()
        {
            ErrorEvaluateResult = new()
            {
                CommonError = 0,
                LocalErrors = new Dictionary<AirplaneComponentType, float>()
                {
                    {AirplaneComponentType.Fuselage, 0},
                    {AirplaneComponentType.Wing, 0},
                    {AirplaneComponentType.Stabilizer, 0},
                    {AirplaneComponentType.Fin, 0},
                    {AirplaneComponentType.Balancing, 0},
                }
            };
        }
    }
}