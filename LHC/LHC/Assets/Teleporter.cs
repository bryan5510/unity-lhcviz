using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class Teleporter : MonoBehaviour {

	GameObject camRig;
	SteamVR_TrackedObject trackedObj;
	LineRenderer lineRenderer;
	bool turnOnLazer = false;
	Vector3 startingPos;
	//Quaternion startingRot;

	void Awake () {
		camRig = GameObject.Find ("[CameraRig]");
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		lineRenderer = GetComponent<LineRenderer> ();
		startingPos = camRig.transform.position;
		//startingRot = camRig.transform.rotation;
	}

	void Update () {
		lineRenderer.SetPosition (0, transform.position);
		lineRenderer.SetPosition (1, transform.position);
		var device = SteamVR_Controller.Input((int)trackedObj.index);

		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			turnOnLazer = true;
		}
		if(device.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)){
			turnOnLazer = false;
		}
		if(device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)){
			ShowAppMenu ();
		}
		if(turnOnLazer){
			RaycastHit hit;
			bool wasHit = Physics.Raycast (transform.position, transform.TransformDirection(Vector3.forward), out hit);
			if(wasHit){
				if (device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)){
					if (hit.collider.gameObject.tag == "Warp") {
						Vector3 warpPoint = hit.point;
						camRig.transform.position = warpPoint;
						//camRig.transform.rotation = warpPoint.rotation;

						Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit);
					}
				}
				lineRenderer.SetPosition (0, transform.position);
				lineRenderer.SetPosition (1, hit.point);
			}
		}
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

	void ShowAppMenu(){
		GameObject canvas = GameObject.Find ("Canvas");
		canvas.SetActive(!canvas.activeSelf);
	}

}
