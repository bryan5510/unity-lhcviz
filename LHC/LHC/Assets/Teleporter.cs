using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//[RequireComponent(typeof(SteamVR_TrackedObject))]
namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent (typeof(Interactable))]
	public class Teleporter : MonoBehaviour
	{

		GameObject canvas;
		Transform canvasHolder;
		//GameObject camRig;
		//SteamVR_TrackedObject trackedObj;
		//LineRenderer lineRenderer;
		//bool turnOnLazer = false;
		//Vector3 startingPos;
		//Quaternion startingRot;

		void Awake ()
		{
			canvasHolder = GameObject.Find ("Player").transform.FindChild("FollowHead").transform.FindChild("CanvasHolder");
			canvas = GameObject.Find ("Canvas");
			//camRig = GameObject.Find ("[CameraRig]");
			//trackedObj = GetComponent<SteamVR_TrackedObject> ();
			//lineRenderer = GetComponent<LineRenderer> ();
			//startingPos = camRig.transform.position;
			//startingRot = camRig.transform.rotation;
		}

		void HandAttachedUpdate (Hand hand)
		{
			//lineRenderer.SetPosition (0, transform.position);
			//lineRenderer.SetPosition (1, transform.position);
			//var device = SteamVR_Controller.Input((int)trackedObj.index);
			/*
			if (hand.controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
				turnOnLazer = true;
			}
			if (hand.controller.GetPressUp (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
				turnOnLazer = false;
			}*/
			if (hand.controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) {
				ShowAppMenu ();
			}
			/*
			if (turnOnLazer) {
				RaycastHit hit;
				bool wasHit = Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit);
				if (wasHit) {
					if (hand.controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
						if (hit.collider.gameObject.tag == "Warp") {
							Vector3 warpPoint = hit.point;
							camRig.transform.position = warpPoint;
							//camRig.transform.rotation = warpPoint.rotation;

							Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit);
						} else if (hit.collider.gameObject.tag == "Button") {
							ButtonManager btn = hit.collider.gameObject.GetComponent<ButtonManager> ();
							if (btn.name.Equals ("Back")) {
								btn.GoBack ();
							} else if (btn.name.Equals ("Refresh")) {
								btn.Refresh ();
							} else {
								if (btn.isEvent) {
									btn.ChangeEvent ();
								} else {
									btn.ChangeRun ();
								}
							}
						}
					} else if (hand.controller.GetPress (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
						if (hit.collider.gameObject.tag == "Handle") {
							float dist = Vector3.Distance (new Vector3 (0f, hit.point.y, 0f), new Vector3 (0f, -2.82f, 0f));
							hit.collider.gameObject.GetComponent<Scrollbar> ().value = dist / .725f;
						}
					}
					lineRenderer.SetPosition (0, transform.position);
					lineRenderer.SetPosition (1, hit.point);
				}
			}
			*/
			/*
		if(device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad) || device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)){
			if (camRig.transform.position == startingPos) {
				camRig.transform.position = GameObject.Find ("Warp1").transform.position;
			} else {
				camRig.transform.position = startingPos;
				//camRig.transform.rotation = startingRot;
			}
		}*/
		}

		void ShowAppMenu ()
		{
			if (canvas.activeSelf == false) {
				canvas.transform.position = canvasHolder.position;
				canvas.transform.eulerAngles = new Vector3 (canvasHolder.eulerAngles.x, canvasHolder.eulerAngles.y, 0);
			}
			canvas.SetActive (!canvas.activeSelf);
		}

	}
}