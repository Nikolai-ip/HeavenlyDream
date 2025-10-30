namespace _Game.Source.Domain.Plane.Component
{
    public class BalancingWeightComponent: PlaneComponent
    {
        public float WeightAmount { get; }

        public BalancingWeightComponent(float weightAmount)
        {
            WeightAmount = weightAmount;
        }
    }
}