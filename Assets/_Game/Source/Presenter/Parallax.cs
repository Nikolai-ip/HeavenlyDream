using System;
using UnityEngine;

[ExecuteAlways]
[ExecuteInEditMode]
public class Parallax : MonoBehaviour
{
    [SerializeField] private float _length;
    [Range(0,1)]
    [SerializeField] private float _parallaxScale;
    [SerializeField] private Transform _camera;
    private float _startPointX;

    private void Start()
    {
        _startPointX = transform.position.x;
    }

    private void FixedUpdate()
    {
        float dist = _camera.position.x * _parallaxScale;
        Vector3 p = transform.position;
        p.x = _startPointX + dist;
        transform.position = p;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _length/2);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.right * _length/2);
    }
}
