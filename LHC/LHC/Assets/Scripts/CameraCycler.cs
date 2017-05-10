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

	void Update(){
		if(Input.GetKeyDown(KeyCode.C)){
			CycleOnce ();
		}
	}

	public void SetCamera(int cam){
		cams [currentCam].enabled = false;
		currentCam = cam;
		cams [currentCam].enabled = true;
	}

	float cameraDelay = 15f;
	public void CycleOnce(){
		//switch camera angle
		cams [currentCam].enabled = false;
		currentCam+=1;
		if(currentCam >= cams.Length){
			currentCam = 0;
		}
		if(currentCam == 0){
			ShowNextEvent ();
		}
		cams [currentCam].enabled = true;
	}

	void ShowNextEvent(){
		if(!es.IncEvent ()){
			Invoke ("ShowNextEvent",0.1f);
		}
	}

	bool isOn = false;
	public bool GetIsOn(){
		return isOn;
	}

	public void ToggleAutoMoveToNext(){
		isOn = !isOn;
	}

}
