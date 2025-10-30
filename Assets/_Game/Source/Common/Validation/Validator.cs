using System.Collections.Generic;
using System.Linq;

namespace _Game.Source.Common.Validation
{
    public class Validator
    {
        private readonly List<PredicateWithOperator> _predicates;

        public Validator(List<PredicateWithOperator> predicates)
        {
            _predicates = predicates;
        }

        public bool IsValid()
        {
            if (_predicates.Count == 0) return true;
            bool result = _predicates.Any(predicate=>predicate.OperatorType == BoolOperatorType.And);
            for (var i = 0; i < _predicates.Count; i++)
            {
                switch (_predicates[i].OperatorType)
                {
                    case BoolOperatorType.And: result = result && _predicates[i].Predicate.Evaluate();
                        break;
                    case BoolOperatorType.Or: result = result || _predicates[i].Predicate.Evaluate();
                        break;
                }
            }
            return result;
        }
    }

    public class PredicateWithOperator
    {
        public IPredicate Predicate { get; }
        public BoolOperatorType OperatorType { get; }

        public PredicateWithOperator(IPredicate predicate, BoolOperatorType operatorType)
        {
            Predicate = predicate;
            OperatorType = operatorType;
        }
    }
}