using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSound : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayScissorsCutSound()
    {
        _audioSource.Play();
    }
}
