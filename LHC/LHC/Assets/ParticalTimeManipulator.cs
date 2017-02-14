using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent (typeof(Interactable))]
	//[RequireComponent(typeof(SteamVR_TrackedObject))]
	public class ParticalTimeManipulator : MonoBehaviour
	{

		//SteamVR_TrackedObject trackedObj;
		Vector2 touchpad;

		//void Update () {
		void HandAttachedUpdate (Hand hand)
		{
			//var device = SteamVR_Controller.Input((int)trackedObj.index);

			//if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {

			if (hand.controller.GetTouch (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
				IgEvent IE = GameObject.Find ("IgEvent").GetComponent<IgEvent> ();
				IE.StopAnim ();
				touchpad = hand.controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
				int fps = IE.GetFPS ();
				int t = (int)(((touchpad.x + 1f) / 2f) * fps);
				IE.SetCurrentFrame (t);
				//Debug.Log(t);
			}
			if (hand.controller.GetTouchUp (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
				IgEvent IE = GameObject.Find ("IgEvent").GetComponent<IgEvent> ();
				IE.StartAnim ();
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
	}
}