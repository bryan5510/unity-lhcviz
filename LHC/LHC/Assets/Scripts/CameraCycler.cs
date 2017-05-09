using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCycler : MonoBehaviour {

	public Camera[] cams;
	int currentCam = 0;
	EventSpawner es;
	Rect[] rects;

	void Awake(){
		rects = new Rect[6];
		rects [0] = new Rect (0, 0.2f, 1, 0.8f);
		rects [1] = new Rect (0, 0, 0.2f, 0.2f);
		rects [2] = new Rect (0.2f, 0, 0.2f, 0.2f);
		rects [3] = new Rect (0.4f, 0, 0.2f, 0.2f);
		rects [4] = new Rect (0.6f, 0, 0.2f, 0.2f);
		rects [5] = new Rect (0.8f, 0, 0.2f, 0.2f);
		es = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.C)){
			CycleOnce ();
		}
	}

	public void CycleOnce(){
		if (IsInvoking ("Cycle")) {
			CancelInvoke ("Cycle");
			Cycle ();
		} else {
			Cycle ();
			CancelInvoke ("Cycle");
		}
	}

	float cameraDelay = 15f;
	void Cycle(){
		//switch camera angle
		cams [currentCam].depth = 2;
		cams [currentCam].rect = rects [currentCam + 1];
		currentCam+=1;
		if(currentCam >= cams.Length){
			currentCam = 0;
		}
		if(currentCam == 0){
			ShowNextEvent ();
		}
		cams [currentCam].depth = 1;
		cams [currentCam].rect = rects [0];
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
