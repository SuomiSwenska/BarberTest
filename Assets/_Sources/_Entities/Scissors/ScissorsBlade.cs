using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsBlade : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private Transform _bladePivotTransform;
    [SerializeField] private Transform _bladePivotFarTransform;

    private Transform _targetTransform;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _targetTransform = _bladePivotFarTransform;
    }

    private void LateUpdate()
    {
        _rigidbody.MovePosition(_targetTransform.TransformPoint(_targetTransform.localPosition));
    }

    public IEnumerator SetCutPosition()
    {
        _targetTransform = _bladePivotTransform;
        yield return new WaitForSeconds(0.3f);
        _targetTransform = _bladePivotFarTransform;
    }
}
