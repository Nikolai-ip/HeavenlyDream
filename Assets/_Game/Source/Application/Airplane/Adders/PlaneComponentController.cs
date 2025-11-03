using UnityEngine;

namespace _Game.Source.Application.Airplane.Adders
{
    public abstract class PlaneComponentController : MonoBehaviour
    {
        public abstract void TryAdd();
        public abstract void TryAdjustComponentParams();
    }
}