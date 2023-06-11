using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSoundController : MonoBehaviour
{
    private AudioSource _footstepAudioSource;
    private CharacterController _characterController;

    [SerializeField] private float _movingSensitivityLimit;
    [SerializeField] private float _movingMagnitude;

    private bool _isSteps;

    private void Awake()
    {
        _footstepAudioSource = GetComponent<AudioSource>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _movingMagnitude = _characterController.velocity.magnitude;

        if (_movingMagnitude >= _movingSensitivityLimit)
        {
            if (_isSteps) return;
            _isSteps = true;
            _footstepAudioSource.Play();
        }
        else if (_isSteps)
        {
            _isSteps = false;
            _footstepAudioSource.Stop();
        }
    }
}
