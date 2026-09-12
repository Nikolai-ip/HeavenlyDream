using System.Linq;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.Plane.Validation.Predicates
{
    public class PlaneContainComponentPredicate: IPredicate
    {
	    private readonly PlaneComponentContainer _plane;
	    private readonly AirplaneComponentType _componentType;
	    public AirplaneComponentType ComponentType => _componentType;
	    
	    public PlaneContainComponentPredicate(PlaneComponentContainer plane, AirplaneComponentType componentType)
	    {
		    _plane = plane;
		    _componentType = componentType;
	    }
        public virtual bool Evaluate() => PlaneContainComponent();

        protected bool PlaneContainComponent() => _plane.PlaneComponents
	        .FirstOrDefault(component => component.Type == _componentType) != null;
    }
}