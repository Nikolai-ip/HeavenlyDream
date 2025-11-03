using _Game.Source.Application.Airplane.Adders;
using _Game.Source.Common;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Commands
{
    public class AdjustPlaneComponentParamsCommand: ICommandMono
    {
        [SerializeField] private PlaneComponentController planeComponentController;
        public override void Execute()
        {
            planeComponentController.TryAdjustComponentParams();
        }
    }
}