using _Game.Source.Application.Airplane.Adders;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.ConcreteMiniGames.PolishRail
{
    public class FuselageMiniGameController: MiniGameController
    {
        [SerializeField] private FuselageComponentAdjuster _fuselageComponentAdjuster;
        // todo: Здесь написать логику передачи значения PolishDegree в аджастер

        public override void OnMiniGameEnd()
        {
            base.OnMiniGameEnd();
            _fuselageComponentAdjuster.TryAdjustComponentParams();
        }
    }
}