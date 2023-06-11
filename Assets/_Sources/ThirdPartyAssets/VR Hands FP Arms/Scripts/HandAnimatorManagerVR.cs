using UnityEngine;

public class HandAnimatorManagerVR : MonoBehaviour
{
    public StateModel[] stateModels;
	public GameObject[] _models;
    Animator handAnimator;

	public int currentState = 100;
	int lastState = -1;

	public bool action = false;
	public bool hold = false;

	//trackpad keys 8 or 9
	public string changeKey = "joystick button 9";
	//trigger keys 14 or 15
	public string actionKey = "joystick button 15";

	//grip axis 11 or 12
	public string holdKey = "Axis 12";

	public int numberOfAnimations = 8;

	// Use this for initialization
	void Start ()
	{
		string[] joys = UnityEngine.Input.GetJoystickNames ();
		foreach (var item in joys) {
			Debug.Log (item);
		}
		handAnimator = GetComponent<Animator> ();
	}

    public void TurnOnState (int stateNumber)
	{
        foreach (var item in stateModels)
        {
            if (item.stateNumber == stateNumber && !item.go.activeSelf)
                item.go.SetActive(true);
            else if (item.go.activeSelf)
                item.go.SetActive(false);
        }
    }

	public void AnimatorCrossFade(int stateIndex, float time)
    {
		handAnimator.SetBool("ScissorsPos", true);
		handAnimator.CrossFade(stateIndex, time);
	}

	public void IdleHandAnimation()
    {
		handAnimator.SetBool("ScissorsPos", false);
	}
}

