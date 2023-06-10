using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabController : MonoBehaviour
{
    private HandAnimatorManagerVR _handAnimatorManagerVR;
    [SerializeField] private float _crossFadeTimeToGrab;
    [SerializeField] private float _crossFadeTimeToCut;
    [SerializeField] private float _crossFadeTimeToOpen;

    [SerializeField] private string _grabAnimationName;
    [SerializeField] private string _cutAnimationName;

    [SerializeField] private XRController _hand;
    [SerializeField] private InputHelpers.Button _triggerButton = InputHelpers.Button.Trigger;
    [SerializeField] private InputHelpers.Button _grabButton = InputHelpers.Button.Grip;

    [SerializeField] private ScissorsBlade _scissorsBlade;

    private int _grabPosAnimationIndex;
    private int _scissorsCloseAnimationIndex;

    private bool _triggerPressed;
    private bool _grabPressed;

    private void Awake()
    {
        _handAnimatorManagerVR = GetComponent<HandAnimatorManagerVR>();
        _grabPosAnimationIndex = Animator.StringToHash(_grabAnimationName);
        _scissorsCloseAnimationIndex = Animator.StringToHash(_cutAnimationName);
    }

    void Update()
    {
        InputControl();
    }

    private void InputControl()
    {
        bool triggerPressed;
        bool grabPressed;

        _hand.inputDevice.IsPressed(_triggerButton, out triggerPressed);
        _hand.inputDevice.IsPressed(_grabButton, out grabPressed);

        if (_triggerPressed != triggerPressed) TriggerHandler(triggerPressed);
        if (_grabPressed != grabPressed) GrabHandler(grabPressed);
    }

    private void TriggerHandler(bool state)
    {
        _triggerPressed = state;
        Debug.Log("triggerPressed changed to " + (state));

        if (_triggerPressed) CloseScissors();
    }

    private void GrabHandler(bool state)
    {
        _grabPressed = state;
        Debug.Log("grabPressed changed to " + (state));

        if (_grabPressed) GrabScissors();
        else _handAnimatorManagerVR.TurnOnState(0);
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
        yield return new WaitForSeconds(_crossFadeTimeToOpen * 0.7f);
        StartCoroutine(_scissorsBlade.SetCutPosition());
        yield return new WaitForSeconds(_crossFadeTimeToOpen * 0.7f);
        _handAnimatorManagerVR.AnimatorCrossFade(_grabPosAnimationIndex, _crossFadeTimeToOpen);
    }
}
