using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoWalkUI : MonoBehaviour, IGvrPointerHoverHandler // Implement Interface for Gaze hoover method
{
	public string walkText = "WALK";
	public string stopText = "STOP";
	[Range(0.1f, 5f)]
	public float secondsToActivate = 2f;
	Slider slider;
	GEN_CameraRigControl walker;
	Text texter; // Slider Text
	float valueForSlider;
	bool locked;
	GvrHead gvrHead;

	void Awake () {
		slider = GetComponent<Slider> ();
		walker = GetComponentInParent<GEN_CameraRigControl> ();
		texter = GetComponentInChildren<Text> ();
		slider.maxValue = secondsToActivate;
	}

	IEnumerator Start()
	{
		texter.color = Color.green;
		slider.value = 0f;
		valueForSlider = 0f;
		yield return gvrHead = GetComponentInParent<GvrHead> (); // Waits till the GVRHead is auto-implemented in the scene
		gvrHead.updateEarly = true; // ---IMPORTANT--- changed default Google VR Head behaviour to make sure 
		// that button location updates after the head movement (Check inside GVR script for more info
	}

	public void InsideButton ()
	{
		float timeStart = Time.fixedTime;
		while(Time.fixedTime - timeStart < secondsToActivate)
			print(Time.fixedTime - timeStart);
	}

	public void LeavesButton() // Called by Event Trigger placed on the inspector
	{
		slider.value = 0f;
		valueForSlider = 0f;
		locked = false;
		if (walker.ReadWalk ()) {
			texter.text = stopText;
			texter.color = Color.red;
		}
	}

	public void OnGvrPointerHover (PointerEventData eventData)
	{
		if (walker.ReadWalk() && locked == false) 
		{
			walker.ToggleautoWalk ();
			texter.text = walkText;
			texter.color = Color.green;
			//locked = true;
		}

		if (!locked) 
		{
			slider.value += Time.deltaTime;

			if (slider.value == secondsToActivate) {
				walker.ToggleautoWalk ();
				locked = true;
				slider.value = 0f;
			}
		} 
	}
}

