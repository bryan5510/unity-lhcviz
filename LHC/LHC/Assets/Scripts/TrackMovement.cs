using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TrackMovement : MonoBehaviour {

	IgEvent ig;
	int id;
	private UnityAction updateDotsListener;
	private UnityAction updateLineListener;

	void Awake(){
		updateDotsListener = new UnityAction (UpdateDots);
		updateLineListener = new UnityAction (UpdateLine);
	}

	//EventSpawner es;
	SettingsManager sm;
	void Start(){
		//es = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
		sm = GameObject.Find ("SettingsManager").GetComponent<SettingsManager> ();
		id = int.Parse(gameObject.name.Substring (6));
		ig = gameObject.GetComponentInParent<IgEvent> ();
		EventManager.StartListening ("UpdateDots", updateDotsListener);
		EventManager.StartListening ("UpdateLine", updateLineListener);
		UpdateDots ();
		UpdateLine ();
	}

	public void UpdateDots(){
		try{
			gameObject.transform.GetChild(0).position = gameObject.GetComponent<BezierSpline>().GetPoint((ig.currentFrame*1f)/(sm.fps*1f));
		}catch{}
		//UpdateLine ();
	}

	Vector3[] GetTrack(int id, int size){
		Vector3[] LRthisTrack = new Vector3[size];
		for(int j = 0; j < size; j++){
			LRthisTrack[j] = ig.LRpoints [id, j];
		}
		return LRthisTrack;
	}

	public void UpdateLine(){
		LineRenderer lr = gameObject.GetComponent<LineRenderer>();
		lr.positionCount = ig.currentFrame;
		Vector3[] LRthisTrack = GetTrack (id,ig.currentFrame);
		lr.SetPositions (LRthisTrack);
	}
}