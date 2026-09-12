using UnityEditor;
using UnityEngine;

namespace _Game.Source.Application
{
    [CreateAssetMenu(fileName = "DebugSettings", menuName = "StaticData/DebugSettings")]
    public class DebugSettings: ScriptableSingleton<DebugSettings>
    {
        [Header("Logs")] 
        public bool MiniGamesLog;
        public bool PlaneLog;
        public bool ErrorCalcLog;
    }
}