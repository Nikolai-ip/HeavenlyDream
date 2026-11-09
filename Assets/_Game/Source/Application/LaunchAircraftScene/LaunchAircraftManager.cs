using UnityEngine;

namespace _Game.Source.Application.LaunchAircraftScene
{
    public class LaunchAircraftManager: MonoBehaviour
    {
        [SerializeField] private BoyController _boyController;
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _boyController.LaunchPlane();
            }
        }
    }
}