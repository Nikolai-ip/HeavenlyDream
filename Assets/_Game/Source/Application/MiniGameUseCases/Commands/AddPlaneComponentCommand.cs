using _Game.Source.Application.Airplane.Adders;
using _Game.Source.Common;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Commands
{
    public class AddPlaneComponentCommand: ICommandMono
    {
        [SerializeField] private PlaneComponentAdjuster planeComponentAdjuster;
        public override void Execute()
        {
            planeComponentAdjuster.TryAdd();
        }
    }
}