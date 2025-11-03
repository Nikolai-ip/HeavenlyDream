namespace _Game.Source.Common.Validation
{
    public class NullPredicate: IPredicate
    {
        public bool Evaluate()
        {
            throw new System.NullReferenceException();
        }
    }
}