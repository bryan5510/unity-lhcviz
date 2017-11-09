using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JetMovement : MonoBehaviour {
	IgEvent ig;
	public Vector3 startingScale = Vector3.zero;
	public Vector3 targetScale;
	public bool isJet = false;

	private UnityAction updateLineListener;

	void Awake(){
		updateLineListener = new UnityAction (UpdateLine);
	}

	//EventSpawner es;
	SettingsManager sm;
	float r;
	void Start(){
		r = UnityEngine.Random.value;
		//es = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
		sm = GameObject.Find ("SettingsManager").GetComponent<SettingsManager> ();
		ig = gameObject.GetComponentInParent<IgEvent> ();
		EventManager.StartListening ("UpdateLine", updateLineListener);
		UpdateLine ();
	}

	public void UpdateLine(){
		float t;
		if(isJet){
			t = (ig.currentFrame*1f) / (sm.fps*1f);
		}else{
			t = (ig.currentFrame*(2f+r)) / (sm.fps*1f);
			if(t > 1){
				t = 1;
			}
		}
		transform.localScale = Vector3.Lerp (startingScale, targetScale, t);
	}
}