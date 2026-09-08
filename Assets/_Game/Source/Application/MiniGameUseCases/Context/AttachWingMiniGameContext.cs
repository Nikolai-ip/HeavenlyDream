using _Game.Source.Application.MiniGameUseCases.AttackWingMiniGame;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Context
{
    public class AttachWingMiniGameContext: MiniGameUnityContext
    {
        public Transform WingSpawnPoint;
        public ShakableComponentAttacher WingPrefab;
    }
}