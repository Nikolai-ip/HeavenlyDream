using UnityEngine;
using UnityEngine.Events;

namespace _Game.Source.Application.Utilities
{
    public class AnimationEvent: MonoBehaviour
    {
        public UnityEvent _chanel1;
        public UnityEvent _chanel2;
        public UnityEvent _chanel3;
        public UnityEvent _chanel4;

        public void OnAnimChanel1()
        {
            _chanel1.Invoke();
        }

        public void OnAnimChanel2()
        {
            _chanel2.Invoke();
        }

        public void OnAnimChanel3()
        {
            _chanel3.Invoke();
        }

        public void OnAnimChanel4()
        {
            _chanel4.Invoke();
        }
    }
}