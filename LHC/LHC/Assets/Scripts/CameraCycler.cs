using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCycler : MonoBehaviour {

	public Camera[] cams;
	int currentCam = 0;
	EventSpawner es;

	void Awake(){
		es = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
	}

	float cameraDelay = 15f;
	public void Cycle(){
		//switch camera angle
		cams [currentCam].depth = 2;
		currentCam+=1;
		if(currentCam >= cams.Length){
			currentCam = 0;
		}
		if(currentCam == 0){
			ShowNextEvent ();
		}
		cams [currentCam].depth = 4;
		Invoke ("Cycle", cameraDelay);
	}

	void ShowNextEvent(){
		if(!es.IncEvent ()){
			Invoke ("ShowNextEvent",0.1f);
		}
	}

	public void ToggleAutoMoveToNext(){
		if (IsInvoking ("Cycle")) {
			CancelInvoke ("Cycle");
			if (IsInvoking ("ShowNextEvent")) {
				CancelInvoke ("ShowNextEvent");
			}
		} else {
			Invoke ("Cycle",cameraDelay);
		}
	}

}
