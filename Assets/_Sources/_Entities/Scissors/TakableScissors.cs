using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakableScissors : MonoBehaviour
{
    private BoxCollider _collider;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private MeshRenderer[] _meshRenderers;
    private Outline[] _outlines;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _outlines = GetComponentsInChildren<Outline>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _audioSource.Play();
    }

    public void Take()
    {
        _collider.enabled = false;
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;

        foreach (var item in _meshRenderers)
        {
            item.enabled = false;
        }

        foreach (var item in _outlines)
        {
            item.enabled = false;
        }
    }

    public void Drop()
    {
        _collider.enabled = true;
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;

        foreach (var item in _meshRenderers)
        {
            item.enabled = true;
        }

        foreach (var item in _outlines)
        {
            item.enabled = true;
        }
    }
}
