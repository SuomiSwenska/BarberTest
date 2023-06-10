using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    private HandAnimatorManagerVR _handAnimatorManagerVR;
    [SerializeField] private float _crossFadeTimeToGrab;
    [SerializeField] private float _crossFadeTimeToCut;
    [SerializeField] private float _crossFadeTimeToOpen;

    [SerializeField] private string _grabAnimationName;
    [SerializeField] private string _cutAnimationName;

    private int _grabPosAnimationIndex;
    private int _scissorsCloseAnimationIndex;

    private void Awake()
    {
        _handAnimatorManagerVR = GetComponent<HandAnimatorManagerVR>();
        _grabPosAnimationIndex = Animator.StringToHash(_grabAnimationName);
        _scissorsCloseAnimationIndex = Animator.StringToHash(_cutAnimationName);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) GrabScissors();
        else if (Input.GetKey(KeyCode.C)) CloseScissors();
    }

    private void GrabScissors()
    {
        _handAnimatorManagerVR.TurnOnState(1);
        _handAnimatorManagerVR.AnimatorCrossFade(_grabPosAnimationIndex, _crossFadeTimeToGrab);
    }

    private void CloseScissors()
    {
        _handAnimatorManagerVR.AnimatorCrossFade(_scissorsCloseAnimationIndex, _crossFadeTimeToCut);
        StartCoroutine(ScissorsOpeneCoroutine());
    }

    private IEnumerator ScissorsOpeneCoroutine()
    {
        yield return new WaitForSeconds(_crossFadeTimeToOpen * 3);
        _handAnimatorManagerVR.AnimatorCrossFade(_grabPosAnimationIndex, _crossFadeTimeToOpen);
    }
}
