using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsBlade : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private Transform _bladePivotTransform;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rigidbody.MovePosition(_bladePivotTransform.TransformPoint(_bladePivotTransform.localPosition));
    }
}
