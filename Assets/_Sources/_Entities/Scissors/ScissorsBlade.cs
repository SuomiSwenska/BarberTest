using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsBlade : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private Transform _bladePivotTransform;
    private BoxCollider _boxCollider;

    private bool isMove = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void LateUpdate()
    {
        _rigidbody.MovePosition(_bladePivotTransform.TransformPoint(_bladePivotTransform.localPosition));
    }

    public IEnumerator SetCutPosition()
    {
        _boxCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _boxCollider.enabled = false;
    }
}
