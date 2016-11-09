using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ParticalTimeManipulator : MonoBehaviour {

	public GameObject canvas;
	public Transform canvasHolder;
	SteamVR_TrackedObject trackedObj;
	Vector2 touchpad;

	void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	void Update () {
		var device = SteamVR_Controller.Input((int)trackedObj.index);

		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			IgEvent IE = GameObject.Find ("IgEvent").GetComponent<IgEvent> ();
			IE.StartAnim ();
		}

		if (device.GetTouch (SteamVR_Controller.ButtonMask.Touchpad)) {
			IgEvent IE = GameObject.Find ("IgEvent").GetComponent<IgEvent> ();
			IE.StopAnim ();
			touchpad = device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
			int fps = IE.GetFPS ();
			int t = (int)(((touchpad.x+1f)/2f)*fps);
			IE.SetCurrentFrame (t);
			//Debug.Log(t);
		}
		if(device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)){
			ShowAppMenu ();
		}
		/*
		*if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)){
		*	EventSpawner ES = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
		*	ES.IncRun ();
		*}
		*if (device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)){
		*	EventSpawner ES = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
		*	ES.IncEvent ();
		*}
		*/
	}

	void ShowAppMenu(){
		if(canvas.activeSelf == false){
			canvas.transform.position = canvasHolder.position;
			canvas.transform.eulerAngles = new Vector3 (canvasHolder.eulerAngles.x,canvasHolder.eulerAngles.y,0);
		}
		canvas.SetActive(!canvas.activeSelf);
	}
}
