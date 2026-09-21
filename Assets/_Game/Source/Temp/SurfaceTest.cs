
using System;
using System.Collections.Generic;
using _Game.Source.Application.MiniGameUseCases.Common;
using UnityEngine;

namespace _Game.Source.Temp
{
    public class SurfaceTest: MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private Transform _movable;
        [SerializeField] private BoxCollider _surface;
        [SerializeField] private List<int> _axis = new(3);
        [SerializeField] private float _yaw;
        
        
        private Vector3 _a;
        private Vector3 _b;
        //Цель1: размещать объект на поверхности 3д колайдера, в точке пересечения мыши с поверхностью
        //Цель2: объект должен поворачиваться по желаемой оси коллайдера
        private void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            // p(t) = origin + dir * t
            // (p - surfaceCenter) dot surfaceNormal = 0
            // (origin + dir * t - surfaceCenter) dot surfaceNormal = 0
            // origin dot surfaceNormal + dir*t dot surfaceNormal - surfaceCenter dot surfaceNormal = 0
            // dir*t dot surfaceNormal = surfaceCenter dot surfaceNormal - origin dot surfaceNormal 
            // t * (dir dot surfaceNormal) = surfaceCenter dot surfaceNormal
            // t = (surfaceCenter dot surfaceNormal - origin dot surfaceNormal)/ (dir dot surfaceNormal)

            // float t = (Vector3.Dot(_surface.transform.position, _surface.transform.up) - Vector3.Dot(ray.origin, _surface.transform.up)) /
            //     Vector3.Dot(ray.direction, _surface.transform.up);
            // _movable.position = ray.origin + ray.direction * t;
            Vector3 size = _surface.size;
            Vector3 center = _surface.center;
            var lowCorner = _surface.transform.TransformPoint(center - new Vector3(0, size.y) / 2);
            var upCorner = _surface.transform.TransformPoint(center + new Vector3(0, size.y) / 2);      
            float h = (upCorner - lowCorner).magnitude;
            
            Plane plane = new Plane(_surface.transform.up, _surface.transform.position + _surface.transform.up * h);
            plane.Raycast(ray, out float t);
            var resultPoint = _surface.ClosestPoint(ray.origin +  ray.direction * t);
            _movable.position =  resultPoint;

            List<Action<Vector3>> setters = new()
            {
                (v) => _movable.forward = v,
                (v) => _movable.up = v,
            };
            for (var i = 0; i < setters.Count; i++)
            {
                switch (_axis[i])
                {
                    case 0:
                        setters[i](_surface.transform.forward);
                        Quaternion rotationF = Quaternion.AngleAxis(_yaw, _movable.forward) * _movable.rotation;
                        _movable.rotation = rotationF;
                        
                        break;
                    case 1:
                        setters[i](_surface.transform.up);
                        Quaternion rotationU = Quaternion.AngleAxis(_yaw, _movable.up) * _movable.rotation;
                        _movable.rotation = rotationU;

                        break;
                }
            }

            if (Input.GetMouseButton(0))
            {
                _yaw += 15 * Time.deltaTime;
                _yaw %= 360;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_b, _radius);
            Vector3 size = _surface.size;
            Vector3 center = _surface.center;
            
            var lowCorner = _surface.transform.TransformPoint(center - new Vector3(0, size.y) / 2);
            var upCorner = _surface.transform.TransformPoint(center + new Vector3(0, size.y) / 2);        
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(lowCorner, _radius);
            Gizmos.DrawSphere(upCorner, _radius);


        }
    }
}