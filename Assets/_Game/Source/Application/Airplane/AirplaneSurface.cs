using UnityEngine;

namespace _Game.Source.Application
{
    public class AirplaneSurface
    {
        private Transform _tr;
        public Vector3 Center => _tr.position;

        public Vector3 Project(Vector3 p)
        {
            Vector3 localP = _tr.InverseTransformPoint(p);
            localP.z = 0;
            return localP;
        }
    }
}