using _Game.Source.Application.Airplane.Adders;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.ConcreteMiniGames.PolishRail
{
    public class FuselageMiniGameController: MiniGameController
    {
        [SerializeField] private FuselageComponentAdjuster _fuselageComponentAdjuster;
        // todo: Здесь написать логику передачи значения PolishDegree в аджастер

        private void OnEnable()
        {
            StateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(MiniGameState state)
        {
            if (state == MiniGameState.Completed)
                _fuselageComponentAdjuster.TryAdjustComponentParams();
        }
    }
}