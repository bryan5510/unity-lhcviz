using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

public class IgEvent : MonoBehaviour{

	public GameObject dotShape;
	public Material mat;
	public Material proton;
	public Material electron;

	GameObject[] tracks;
	public int currentFrame = 0;
	public int fps = 270;
	public Vector3[,] LRpoints;
	public bool stopAnim = false;

	public void StartAnim(){
		stopAnim = false;
	}
	public void StopAnim(){
		stopAnim = true;
	}

	public void SetCurrentFrame(int t){
		currentFrame = t;
		EventManager.TriggerEvent ("UpdateDots");
		EventManager.TriggerEvent ("UpdateLine");
	}

	public int GetFPS(){
		return fps;
	}

	CameraCycler cycler;
	void Awake(){
		cycler = GameObject.Find ("CameraCycle").GetComponent<CameraCycler> ();
	}

	int direction = 1;
	void FixedUpdate(){
		if(Input.GetKey(KeyCode.LeftArrow)){
			stopAnim = true;
			MoveDots (-1);
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			stopAnim = true;
			MoveDots (1);
		}
		if(Input.GetKeyUp(KeyCode.LeftArrow)||Input.GetKeyUp(KeyCode.RightArrow)){
			stopAnim = false;
		}

		if (!stopAnim) {
			if (!MoveDots (direction)) {
				if(cycler.GetIsOn()){
					cycler.CycleOnce();
				}
				SetCurrentFrame (0);
			}
		}

	}

	bool MoveDots(int c){
		if (currentFrame + c >= 0 && currentFrame + c <= fps) {
			currentFrame += c;
			EventManager.TriggerEvent ("UpdateDots");
			EventManager.TriggerEvent ("UpdateLine");
			return true;
		} else {
			return false;
		}
	}


	Vector3[] GetTrack(int i, int size){
		Vector3[] LRthisTrack = new Vector3[size];
		for(int j = 0; j < size; j++){
			LRthisTrack[j] = LRpoints [i, j];
		}
		return LRthisTrack;
	}

	int[] ParseTracks(string eventFile, string trkLocName, int trkIndex){
		int tracksLoc = eventFile.IndexOf (trkLocName);
		eventFile = eventFile.Substring (tracksLoc);
		tracksLoc = eventFile.IndexOf (":");
		eventFile = eventFile.Substring (tracksLoc);
		int tracksEnd = eventFile.IndexOf ("\"");
		eventFile = eventFile.Substring (3,tracksEnd-10);
		string[] lines = eventFile.Split ("\n"[0]);
		int[] charge = new int[lines.Length];
		for(int i = 0; i < lines.Length; i++){
			// "Tracks_V3": [["pos", "v3d"],["dir", "v3d"],["pt", "double"],["phi", "double"],["eta", "double"],["charge", "int"],["chi2", "double"],["ndof", "double"]]
			// "Tracks_V3": [[[0.000924736, 0.000185603, -0.0215063], [-0.426364, -0.346344, -0.835619], 0.962035, -2.45938, -1.20648, 1, 15.2759, 11], 
			string[] values = lines [i].Split (","[0]);
			values[trkIndex] = values[trkIndex].Substring(1);
			charge[i] = int.Parse(values [trkIndex]);

		}
		return charge;
	}

	int[] ParseCharge(string eventFile){
		int[] x;
		try{
			x = ParseTracks(eventFile, "\"Tracks_V", 9);
		}catch{
			Debug.Log ("No charge in event.");
			return null;
		}
		return x;

	}

	public void ParseEventInfo (string eventFile){
		//"Event_V2": [["run", "int"],["event", "int"],["ls", "int"],["orbit", "int"],["bx", "int"],["time", "string"],["localtime", "string"]],
		//"Collections": {"Event_V2": [[146436, 90625265, 322, 84148692, 910, "2010-Sep-22 21:25:46.221672 GMT", "Wed Sep 22 16:25:46 2010 CDT"]],
		int tracksLoc = eventFile.IndexOf ("Event_V");
		eventFile = eventFile.Substring (tracksLoc);
		tracksLoc = eventFile.IndexOf (":");
		eventFile = eventFile.Substring (tracksLoc);
		int tracksEnd = eventFile.IndexOf ("\"]");
		eventFile = eventFile.Substring (4,tracksEnd-3);
		//Debug.Log (eventFile);
		string[] eventParts = eventFile.Split (new char[]{','});

		int runNumber = int.Parse(eventParts [0]);
		int eventNumber = int.Parse(eventParts [1].Substring(1));
		int lsNumber = int.Parse(eventParts [2].Substring(1));
		int orbitNumber = int.Parse(eventParts [3].Substring(1));
		int bxNumber = int.Parse(eventParts [4].Substring(1));
		string eventTime = eventParts[5].Substring(2,eventParts[5].LastIndexOf("\"")-2);
		string eventLocalTime = eventParts[6].Substring(2,eventParts[6].LastIndexOf("\"")-2);

		PushInfoToCanvas (runNumber,eventNumber,lsNumber,orbitNumber,bxNumber,eventTime,eventLocalTime);

	}

	public void PushInfoToCanvas (int runNumber,int eventNumber,int lsNumber,int orbitNumber,int bxNumber,string eventTime,string eventLocalTime){
		try{
			GameObject eventInformationCanvas = GameObject.Find ("EventInformationCanvas");

			eventInformationCanvas.transform.FindChild ("RunNumber").GetComponent<Text> ().text = runNumber.ToString();
			eventInformationCanvas.transform.FindChild ("EventNumber").GetComponent<Text> ().text = eventNumber.ToString();
			eventInformationCanvas.transform.FindChild ("EventTime").GetComponent<Text> ().text = eventTime;
			//eventInformationCanvas.transform.FindChild ("EventLocalTime").GetComponent<Text> ().text = eventLocalTime;
			//eventInformationCanvas.transform.FindChild ("LsNumber").GetComponent<Text> ().text = lsNumber.ToString();
			//eventInformationCanvas.transform.FindChild ("OrbitNumber").GetComponent<Text> ().text = orbitNumber.ToString();
			//eventInformationCanvas.transform.FindChild ("BxNumber").GetComponent<Text> ().text = bxNumber.ToString();
		}
		catch{}
	}

	public bool parseExtras(FileInfo eventInfo){
		string eventFile = File.ReadAllText(eventInfo.FullName);
		int tracksLoc = eventFile.IndexOf ("Collections");
		eventFile = eventFile.Substring (tracksLoc);
		
		int[] charge = ParseCharge (eventFile);
		ParseEventInfo (eventFile);

		//"Extras_V1": [["pos_1", "v3d"],["dir_1", "v3d"],["pos_2", "v3d"],["dir_2", "v3d"]]
		// [[[0.000924736, 0.000185603, -0.0215063], [-0.746714, -0.606572, -1.46347], [-1.2536, 0.236426, -2.22576], [-0.451694, 0.799546, -1.39922]], 
		tracksLoc = eventFile.IndexOf ("Extras_V");
		eventFile = eventFile.Substring (tracksLoc);
		tracksLoc = eventFile.IndexOf (":");
		eventFile = eventFile.Substring (tracksLoc);
		int tracksEnd = eventFile.IndexOf ("\"");
		try{
			eventFile = eventFile.Substring (3,tracksEnd-10);}
		catch{
			Debug.Log ("No tracks in event "+eventInfo.Name);
			return false;
		}
		string[] lines = eventFile.Split ("\n"[0]);

		GameObject[] trks = new GameObject[lines.Length];


		LRpoints = new Vector3[lines.Length,fps+1];

		for(int i = 0; i < lines.Length; i++){
			//Debug.Log (lines[i]);
			lines[i] = lines[i].Substring(2);
			tracksEnd = lines[i].IndexOf ("]");
			string pos1 = lines[i].Substring (0,tracksEnd);
			tracksEnd = lines[i].IndexOf ("[");
			lines[i] = lines[i].Substring(tracksEnd);
			tracksEnd = lines[i].IndexOf ("]");
			string dir1 = lines[i].Substring (1,tracksEnd-1);
			lines[i] = lines[i].Substring(tracksEnd);
			tracksEnd = lines[i].IndexOf ("[");
			lines[i] = lines[i].Substring(tracksEnd);
			tracksEnd = lines[i].IndexOf ("]");
			string pos2 = lines[i].Substring (1,tracksEnd-1);
			lines[i] = lines[i].Substring(tracksEnd);
			tracksEnd = lines[i].IndexOf ("[");
			lines[i] = lines[i].Substring(tracksEnd);
			lines[i] = lines[i] + "]],";
			tracksEnd = lines[i].IndexOf ("]");
			string dir2 = lines[i].Substring (1,tracksEnd-1);
			string text = pos1 + "," + dir1 + "," + pos2 + "," + dir2;

			string[] remaining = text.Split (","[0]);
			float[] values = new float[remaining.Length];
			for(int j=0; j < remaining.Length; j++){
				values [j] = float.Parse (remaining[j]);
				//Debug.Log (values[j]);
			}

			Vector3 p1 = new Vector3 (values[0],values[1],values[2]);
			Vector3 d1 = new Vector3 (values[3],values[4],values[5]);
			d1.Normalize();
			Vector3 p2 = new Vector3 (values[6],values[7],values[8]);
			Vector3 d2 = new Vector3 (values[9],values[10],values[11]);
			d2.Normalize();



			float distance = Vector3.Distance(p1,p2);
			float scale = distance*0.25f;

			Vector3 p3 = new Vector3(p1.x+scale*d1.x, p1.y+scale*d1.y, p1.z+scale*d1.z);
			Vector3 p4 = new Vector3(p2.x-scale*d2.x, p2.y-scale*d2.y, p2.z-scale*d2.z);

			GameObject curve = new GameObject();
			curve.name = "track " + i;
			BezierSpline bs = curve.AddComponent<BezierSpline> ();
			bs.SetAllControlPoints (p1,p3,p4,p2);
			bs.MakeSpline(1,new Transform[1] {dotShape.transform});

			for(int j = 0; j < fps + 1; j++){
				LRpoints [i,j] = bs.GetPoint ((j*1f)/(fps*1f));
			}

			LineRenderer lr = curve.AddComponent<LineRenderer>();
			lr.numPositions = fps + 1;
			Vector3[] LRthisTrack = GetTrack (i,fps+1);
			lr.SetPositions (LRthisTrack);
			lr.startWidth = 0.01f;
			lr.endWidth = 0.01f;
			lr.material = mat;

			if (charge != null) {
				try{
					if (charge [i] > 0) {
						curve.transform.GetChild (0).GetComponent<MeshRenderer> ().material = proton;
					} else if (charge [i] < 0) {
						curve.transform.GetChild (0).GetComponent<MeshRenderer> ().material = electron;
						curve.transform.GetChild (0).localScale = new Vector3(0.05f,0.05f,0.05f);
						//curve.transform.GetChild (0).localScale = new Vector3(0.000054f,0.000054f,0.000054f);
					}
				}catch{
				}
			}

			curve.AddComponent<TrackMovement> ();

			trks [i] = curve;
		}
		tracks = trks;

		foreach(GameObject track in tracks){
			track.transform.SetParent (transform);
		}
		return true;
	}


}
