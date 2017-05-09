using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TrackMovement : MonoBehaviour {

	IgEvent ig;
	int id;
	//int currentFrame = 0;

	private UnityAction moveForwardListener;
	private UnityAction moveBackListener;
	private UnityAction updateListener;

	void Awake(){
		//moveForwardListener = new UnityAction (MoveDotsForward);
		//moveBackListener = new UnityAction (MoveDotsBack);
		updateListener = new UnityAction (UpdateDots);
	}

	void Start(){
		id = int.Parse(gameObject.name.Substring (6));
		ig = gameObject.GetComponentInParent<IgEvent> ();
		//EventManager.StartListening ("MoveDotsForward", moveForwardListener);
		//EventManager.StartListening ("MoveDotsBack", moveBackListener);
		EventManager.StartListening ("UpdateDots", updateListener);
		UpdateDots ();
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
		MoveDots ();
	}

	public void MoveDots(){
		gameObject.transform.GetChild(0).position = gameObject.GetComponent<BezierSpline>().GetPoint((ig.currentFrame*1f)/(ig.fps*1f));
		UpdateLine (id);
	}

	Vector3[] GetTrack(int id, int size){
		Vector3[] LRthisTrack = new Vector3[size];
		for(int j = 0; j < size; j++){
			LRthisTrack[j] = ig.LRpoints [id, j];
		}
		return LRthisTrack;
	}

	void UpdateLine(int id){
		LineRenderer lr = gameObject.GetComponent<LineRenderer>();
		lr.numPositions = ig.currentFrame;
		Vector3[] LRthisTrack = GetTrack (id,ig.currentFrame);
		lr.SetPositions (LRthisTrack);
	}
}