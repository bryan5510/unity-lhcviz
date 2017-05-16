using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JetMovement : MonoBehaviour {
	IgEvent ig;
	public Vector3 targetScale;

	private UnityAction updateLineListener;

	void Awake(){
		updateLineListener = new UnityAction (UpdateLine);
	}

	//EventSpawner es;
	SettingsManager sm;
	void Start(){
		//es = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
		sm = GameObject.Find ("SettingsManager").GetComponent<SettingsManager> ();
		ig = gameObject.GetComponentInParent<IgEvent> ();
		EventManager.StartListening ("UpdateLine", updateLineListener);
		UpdateLine ();
	}

	public void UpdateLine(){
		float t = (ig.currentFrame*1f) / (sm.fps*1f);
		transform.localScale = Vector3.Lerp (Vector3.zero, targetScale, t);
	}
}