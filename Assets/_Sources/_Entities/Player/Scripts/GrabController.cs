using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabController : MonoBehaviour
{
    private HandAnimatorManagerVR _handAnimatorManagerVR;
    private HandSound _handSound;
    [SerializeField] private float _crossFadeTimeToGrab;
    [SerializeField] private float _crossFadeTimeToCut;
    [SerializeField] private float _crossFadeTimeToOpen;

    [SerializeField] private string _grabAnimationName;
    [SerializeField] private string _cutAnimationName;

    [SerializeField] private XRController _hand;
    [SerializeField] private InputHelpers.Button _triggerButton = InputHelpers.Button.Trigger;
    [SerializeField] private InputHelpers.Button _grabButton = InputHelpers.Button.Grip;

    [SerializeField] private Transform _scissorsTransform;
    [SerializeField] private ScissorsBlade _scissorsBlade;
    [SerializeField] private TakableScissors _takableScissors;

    private int _grabPosAnimationIndex;
    private int _scissorsCloseAnimationIndex;

    private bool _triggerPressed;
    private bool _grabPressed;

    private bool _isTakePossible;
    private bool _isHaveScissors;

    private void Awake()
    {
        _handAnimatorManagerVR = GetComponent<HandAnimatorManagerVR>();
        _grabPosAnimationIndex = Animator.StringToHash(_grabAnimationName);
        _scissorsCloseAnimationIndex = Animator.StringToHash(_cutAnimationName);
        _handSound = GetComponent<HandSound>();
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

        if (_triggerPressed && _isHaveScissors) CloseScissors();
    }

    private void GrabHandler(bool state)
    {
        _grabPressed = state;
        Debug.Log("grabPressed changed to " + (state));

        if (_grabPressed && _isTakePossible) GrabScissors();
        else if (!_grabPressed && _isHaveScissors) DropScissors();
    }


    private void GrabScissors()
    {
        _handAnimatorManagerVR.TurnOnState(1);
        _handAnimatorManagerVR.AnimatorCrossFade(_grabPosAnimationIndex, _crossFadeTimeToGrab);
        _takableScissors.Take();
        _isHaveScissors = true;
    }

    private void CloseScissors()
    {
        _handSound.PlayScissorsCutSound();
        _handAnimatorManagerVR.AnimatorCrossFade(_scissorsCloseAnimationIndex, _crossFadeTimeToCut);
        StartCoroutine(ScissorsOpeneCoroutine());
    }

    private void DropScissors()
    {
        _handAnimatorManagerVR.TurnOnState(0);
        _isHaveScissors = false;
        _takableScissors.transform.position = _scissorsTransform.position;
        _takableScissors.Drop();
        _takableScissors = null;
        _isTakePossible = false;
    }

    private IEnumerator ScissorsOpeneCoroutine()
    {
        yield return new WaitForSeconds(_crossFadeTimeToOpen * 0.7f);
        StartCoroutine(_scissorsBlade.SetCutPosition());
        yield return new WaitForSeconds(_crossFadeTimeToOpen * 0.7f);
        _handAnimatorManagerVR.AnimatorCrossFade(_grabPosAnimationIndex, _crossFadeTimeToOpen);
    }

    private void OnTriggerEnter(Collider other)
    {
        TakableScissors takableScissors = other.GetComponent<TakableScissors>();
        if (takableScissors is TakableScissors)
        {
            _isTakePossible = true;
            _takableScissors = takableScissors;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TakableScissors takableScissors = other.GetComponent<TakableScissors>();
        if (takableScissors is TakableScissors)
        {
            _isTakePossible = false;
            _takableScissors = null;
        }
    }
}
