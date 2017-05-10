using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TrackMovement : MonoBehaviour {

	IgEvent ig;
	int id;
	//int currentFrame = 0;

	//private UnityAction moveForwardListener;
	//private UnityAction moveBackListener;
	private UnityAction updateDotsListener;
	private UnityAction updateLineListener;

	void Awake(){
		//moveForwardListener = new UnityAction (MoveDotsForward);
		//moveBackListener = new UnityAction (MoveDotsBack);
		updateDotsListener = new UnityAction (UpdateDots);
		updateLineListener = new UnityAction (UpdateLine);
	}

	EventSpawner es;
	void Start(){
		id = int.Parse(gameObject.name.Substring (6));
		ig = gameObject.GetComponentInParent<IgEvent> ();
		es = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
		//EventManager.StartListening ("MoveDotsForward", moveForwardListener);
		//EventManager.StartListening ("MoveDotsBack", moveBackListener);
		EventManager.StartListening ("UpdateDots", updateDotsListener);
		EventManager.StartListening ("UpdateLine", updateLineListener);
		UpdateDots ();
		UpdateLine ();
	}
	/*
	public void MoveDotsForward(){
		MoveDots (1);
	}

	public void MoveDotsBack(){
		MoveDots (-1);
	}
*/
	public void UpdateDots(){
		gameObject.transform.GetChild(0).position = gameObject.GetComponent<BezierSpline>().GetPoint((ig.currentFrame*1f)/(es.fps*1f));
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
		lr.numPositions = ig.currentFrame;
		Vector3[] LRthisTrack = GetTrack (id,ig.currentFrame);
		lr.SetPositions (LRthisTrack);
	}
}