using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCycler : MonoBehaviour {

	public Camera[] cams;
	int currentCam = 0;
	EventSpawner es;
	SettingsManager sm;

	void Awake(){
		es = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
		sm = GameObject.Find ("SettingsManager").GetComponent<SettingsManager> ();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.C)){
			CycleOnce ();
		}
	}

	public void SetCamera(int cam){
		cams [currentCam].enabled = false;
		currentCam = cam;
		//CheckEventsToShow ();
		cams [currentCam].enabled = true;
	}

	//float cameraDelay = 15f;
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
		//CheckEventsToShow ();
	}

	void CheckEventsToShow(){
		if(currentCam == 0){
			sm.showJets = false;
			GameObject.Find ("IgEvent").GetComponent<IgEvent> ().HideJets (sm.showJets);
			sm.showHits = false;
			GameObject.Find ("IgEvent").GetComponent<IgEvent> ().HideHits (sm.showHits);
		}else if(currentCam == 1){
			sm.showJets = true;
			GameObject.Find ("IgEvent").GetComponent<IgEvent> ().HideJets (sm.showJets);
			sm.showHits = false;
			GameObject.Find ("IgEvent").GetComponent<IgEvent> ().HideHits (sm.showHits);
		}else if(currentCam == 2){
			sm.showJets = false;
			GameObject.Find ("IgEvent").GetComponent<IgEvent> ().HideJets (sm.showJets);
			sm.showHits = true;
			GameObject.Find ("IgEvent").GetComponent<IgEvent> ().HideHits (sm.showHits);
		}else if(currentCam == 3){
			sm.showJets = true;
			GameObject.Find ("IgEvent").GetComponent<IgEvent> ().HideJets (sm.showJets);
			sm.showHits = true;
			GameObject.Find ("IgEvent").GetComponent<IgEvent> ().HideHits (sm.showHits);
		}
	}

	void ShowNextEvent(){
		if(!es.IncEvent ()){
			Invoke ("ShowNextEvent",0.1f);
		}
	}
}
