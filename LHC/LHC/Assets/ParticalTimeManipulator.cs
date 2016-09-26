using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ParticalTimeManipulator : MonoBehaviour {

	IgEvent IE;
	//GameObject camRig;
	SteamVR_TrackedObject trackedObj;
	Vector2 touchpad;

	void Awake () {
		IE = GameObject.Find ("IgEventHolder").GetComponent<IgEvent> ();
		//camRig = GameObject.Find ("[CameraRig]");
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	void Update () {
		var device = SteamVR_Controller.Input((int)trackedObj.index);

		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			IE.StartAnim ();
		}

		if (device.GetTouch (SteamVR_Controller.ButtonMask.Touchpad)) {
			IE.StopAnim ();
			touchpad = device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
			int fps = IE.GetFPS ();
			int t = (int)(((touchpad.x+1f)/2f)*fps);
			IE.SetCurrentFrame (t);
			//Debug.Log(t);
		}
	}
}
