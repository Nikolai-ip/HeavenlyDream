using UnityEngine;

namespace _Game.Source.Common
{
    public abstract class ICommandMono: MonoBehaviour
    {
        public abstract void Execute();
    }
}