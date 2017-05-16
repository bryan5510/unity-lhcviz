using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent (typeof(Interactable))]
	public class ParticalTimeManipulator : MonoBehaviour
	{
		Vector2 touchpad;
		int mode = 0;
		float buttonSize = 0.2f;
		int currentFrame = 0;
		bool isPaused = false;
		IgEvent IE;

		//EventSpawner es;
		SettingsManager sm;
		void Awake(){
			//es = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
			sm = GameObject.Find ("SettingsManager").GetComponent<SettingsManager> ();
		}

		void HandAttachedUpdate (Hand hand)
		{
			IE = GameObject.Find ("IgEvent").GetComponent<IgEvent> ();
			//int fps = IE.GetFPS ();
			touchpad = hand.controller.GetAxis ();
			//float midDist = Vector2.Distance (Vector2.zero, touchpad);
			float topDist = Vector2.Distance (new Vector2(0,1-buttonSize), touchpad);
			float botDist = Vector2.Distance (new Vector2(0,-1+buttonSize), touchpad);
			//float rightDist = Vector2.Distance (new Vector2(0,1-buttonSize), touchpad);
			//float leftDist = Vector2.Distance (new Vector2(0,-1+buttonSize), touchpad);

			//switch mode by pressing the top button
			if (hand.controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
				if (-buttonSize < topDist && topDist < buttonSize) {
					if (mode < 1) {
						mode++;
					} else {
						mode = 0;
					}
				}
			}

			if (mode == 0) {
				if (hand.controller.GetPress (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
					if (-buttonSize > touchpad.x) {
						if (currentFrame - 1 >= 0) {
							currentFrame -= 1;
							int t = currentFrame;
							IE.SetCurrentFrame (t);
							if(!isPaused){
								TogglePause ();
							}
						}
					}else if (touchpad.x > buttonSize) {
						if (currentFrame + 1 <= sm.fps) {
							currentFrame += 1;
							int t = currentFrame;
							IE.SetCurrentFrame (t);
							if(!isPaused){
								TogglePause ();
							}
						}
					}
				}
			}else if (mode == 1) {
				if (hand.controller.GetTouch (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
					if(-1+buttonSize+buttonSize < touchpad.y && touchpad.y < 1-buttonSize-buttonSize){
						int t = (int)(((touchpad.x + 1f) / 2f) * sm.fps);
						IE.SetCurrentFrame (t);
						//Debug.Log(t);
					}
				}
			}
				
			//on pressing the pause button, toggle pause
			if (hand.controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
				if (-buttonSize < botDist && botDist < buttonSize) {
					TogglePause ();
				}
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

		void TogglePause(){
			isPaused = !isPaused;
			if (isPaused) {
				IE.StopAnim ();
			} else {
				IE.StartAnim ();
			}
		}


	}
}