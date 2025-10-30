using System.Collections.Generic;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.Plane
{
    public class PlaneComponentContainer
    {
        public List<PlaneComponent> PlaneComponents { get; }

        public PlaneComponentContainer(List<PlaneComponent> planeComponents)
        {
            PlaneComponents = planeComponents;
        }
    }
}