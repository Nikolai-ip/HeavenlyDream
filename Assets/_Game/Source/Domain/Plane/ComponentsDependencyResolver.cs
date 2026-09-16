using System.Collections.Generic;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.Plane
{
    public static class ComponentsDependencyResolver
    {
        public static List<AirplaneComponentType> GetRequirementsFor(AirplaneComponentType airplaneComponentType)
        {
            switch (airplaneComponentType)
            {
                case AirplaneComponentType.Fuselage: return new List<AirplaneComponentType>();
                case AirplaneComponentType.Wing: return new() { AirplaneComponentType.Fuselage };
                case AirplaneComponentType.Stabilizer: return new() { AirplaneComponentType.Fuselage };
                case AirplaneComponentType.Fin: return new() { AirplaneComponentType.Fuselage,AirplaneComponentType.Stabilizer };
                case AirplaneComponentType.Balancing: return new (){ AirplaneComponentType.Fuselage };
                default: return new List<AirplaneComponentType>();
            }
        }
        
        public static AirplaneComponentType GetAirplaneSurfaceFor(AirplaneComponentType airplaneComponentType)
        {
            switch (airplaneComponentType)
            {
                case AirplaneComponentType.Wing: return AirplaneComponentType.Fuselage;
                case AirplaneComponentType.Stabilizer: return AirplaneComponentType.Fuselage;
                case AirplaneComponentType.Fin: return AirplaneComponentType.Stabilizer;
                default: return AirplaneComponentType.None;
            }
        }
    }
}