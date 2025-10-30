using System.Linq;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.Plane.Validation.Predicates
{
    public class PlaneContainComponentPredicate<TPlaneComponent>: IPredicate where TPlaneComponent: PlaneComponent
    {
		private readonly PlaneComponentContainer _plane;

		public PlaneContainComponentPredicate(PlaneComponentContainer plane)
		{
			_plane = plane;
		}
        public bool Evaluate()
        {
            return _plane.PlaneComponents
	            .FirstOrDefault(component => component.GetType() == typeof(TPlaneComponent)) != null;
        }
    }
}