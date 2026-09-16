using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source.Temp
{
    public class SurfaceTest: MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _layerMask;
        
        private Vector3 _target;

        private void Update()
        {
            var mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit,  Mathf.Infinity, _layerMask))
            {
                Debug.Log(hit.collider.name);
                _target = hit.point;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_meshRenderer.localBounds.center, _radius);
            
            Vector3 min = _meshRenderer.localBounds.min;
            Vector3 max = _meshRenderer.localBounds.max;
            List<Vector3> vertices = new()
            {
                min,
                new Vector3(max.x, min.y, max.z),
                new Vector3(min.x, min.y, max.z),
                new Vector3(max.x, min.y, min.z),
                
                max,
                new Vector3(max.x, max.y, max.z),
                new Vector3(min.x, max.y, max.z),
                new Vector3(max.x, max.y, min.z),
            };
            
            for (int i = 0; i < vertices.Count; i++)
            {
                Gizmos.DrawSphere(
                    _meshRenderer.transform.TransformPoint(vertices[i]), _radius);
            }
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_target, _radius);
        }
    }
}