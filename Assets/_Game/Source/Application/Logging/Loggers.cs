using System;
using UnityEngine;

namespace _Game.Source.Application.Logging
{
    public class MiniGameLogger: Logger<MiniGameLogger>
    {
        protected override bool Enabled => DebugSettings.instance.MiniGamesLog;
    }
    public class PlaneLogger: Logger<PlaneLogger>
    {
        protected override bool Enabled => DebugSettings.instance.PlaneLog;
    }
    public class ErrorCalcLogger: Logger<PlaneLogger>
    {
        protected override bool Enabled => DebugSettings.instance.ErrorCalcLog;
    }
    
    public abstract class Logger<T> : Singleton<T> where T : new()
    {
        protected abstract bool Enabled { get; }
        
        public void Log(Func<string> message)
        {
            LogMessage(message, ColorUtility.ToHtmlStringRGB(Color.cyan));
        }

        public void LogError(Func<string> message)
        {
            LogMessage(message, ColorUtility.ToHtmlStringRGB(Color.red));
        }

        public void LogWarning(Func<string> message)
        {
            LogMessage(message, ColorUtility.ToHtmlStringRGB(Color.yellow));
        }

        private void LogMessage(Func<string> message, string color)
        {
            if (Enabled)
            {
                Debug.Log($"<color=#{color}>{message()}</color>");
            }        
        }
    }
    public class Singleton<T> where T: new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                _instance ??= new T();
                return _instance;
            }
        }
    }
    
}