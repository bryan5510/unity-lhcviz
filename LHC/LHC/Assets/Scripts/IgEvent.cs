using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

//[System.Serializable]
public class IgEvent : MonoBehaviour{

	public GameObject dotShape;
	public Material mat;

	GameObject[] tracks;
	int currentFrame = 0;
	int fps = 90;
	Vector3[,] LRpoints;
	bool stopAnim = false;

	public void StartAnim(){
		stopAnim = false;
	}
	public void StopAnim(){
		stopAnim = true;
	}

	public void SetCurrentFrame(int t){
		currentFrame = t;
		MoveDots (0);
	}

	public int GetFPS(){
		return fps;
	}

	void Update(){
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
	}


	IEnumerator AnimateDots(){
		int direction = 1;
		while (true) {
			if (!stopAnim) {
				if (!MoveDots (direction)) {
					direction *= -1;
				}
			}
			yield return new WaitForSeconds (Time.deltaTime/2);
		}
	}

	bool MoveDots(int c){
		if (currentFrame + c >= 0 && currentFrame + c <= fps) {
			currentFrame += c;
			for (int i = 0; i < tracks.Length; i++) {//for each track per frame
				tracks [i].transform.GetChild(0).position = tracks[i].GetComponent<BezierSpline>().GetPoint((currentFrame*1f)/(fps*1f));
				UpdateLine (i);
			}
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

	void UpdateLine(int i){
		LineRenderer lr = tracks[i].GetComponent<LineRenderer>();
		lr.SetVertexCount (currentFrame);
		Vector3[] LRthisTrack = GetTrack (i,currentFrame);
		lr.SetPositions (LRthisTrack);
	}
	/*
	void parseTracks(string eventName){
		string eventFile = File.ReadAllText("Assets\\ispy-webgl-master\\data\\Electron\\Events\\Run_146644\\" + eventName);
		int tracksLoc = eventFile.IndexOf ("Collections");
		eventFile = eventFile.Substring (tracksLoc);
		tracksLoc = eventFile.IndexOf ("Tracks_V");
		eventFile = eventFile.Substring (tracksLoc);
		tracksLoc = eventFile.IndexOf (":");
		eventFile = eventFile.Substring (tracksLoc);
		int tracksEnd = eventFile.IndexOf ("\"");
		eventFile = eventFile.Substring (3,tracksEnd-10);
		string[] lines = eventFile.Split ("\n"[0]);
		for(int i = 0; i < lines.Length; i++){
			//[[0.000924736, 0.000185603, -0.0215063], [-0.426364, -0.346344, -0.835619], 0.962035, -2.45938, -1.20648, 1, 15.2759, 11], 
			//Debug.Log (lines[i]);
			lines[i] = lines[i].Substring(2);
			tracksEnd = lines[i].IndexOf ("]");
			string pos = lines[i].Substring (0,tracksEnd);
			tracksEnd = lines[i].IndexOf ("[");
			lines[i] = lines[i].Substring(tracksEnd);
			tracksEnd = lines[i].IndexOf ("]");
			string dir = lines[i].Substring (1,tracksEnd-1);
			lines[i] = lines[i].Substring(tracksEnd+2);
			tracksEnd = lines[i].IndexOf ("]");
			lines [i] = lines [i].Substring (0, tracksEnd);
			string text = pos + "," + dir + "," + lines [i];
			string[] remaining = text.Split (","[0]);
			float[] values = new float[remaining.Length];
			for(int j=0; j < remaining.Length; j++){
				values [j] = float.Parse (remaining[j]);
				//Debug.Log (values[j]);
			}
		}
	}*/

	public void ParseEventInfo (FileInfo eventInfo){
		string eventFile = File.ReadAllText(eventInfo.FullName);
		int tracksLoc = eventFile.IndexOf ("Collections");
		eventFile = eventFile.Substring (tracksLoc);
		//"Event_V2": [["run", "int"],["event", "int"],["ls", "int"],["orbit", "int"],["bx", "int"],["time", "string"],["localtime", "string"]],
		//"Collections": {"Event_V2": [[146436, 90625265, 322, 84148692, 910, "2010-Sep-22 21:25:46.221672 GMT", "Wed Sep 22 16:25:46 2010 CDT"]],
		tracksLoc = eventFile.IndexOf ("Event_V");
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
		catch(NullReferenceException e){}
	}

	public bool parseExtras(FileInfo eventInfo){

		ParseEventInfo (eventInfo);

		//"Extras_V1": [["pos_1", "v3d"],["dir_1", "v3d"],["pos_2", "v3d"],["dir_2", "v3d"]]
		// [[[0.000924736, 0.000185603, -0.0215063], [-0.746714, -0.606572, -1.46347], [-1.2536, 0.236426, -2.22576], [-0.451694, 0.799546, -1.39922]], 
		string eventFile = File.ReadAllText(eventInfo.FullName);
		int tracksLoc = eventFile.IndexOf ("Collections");
		eventFile = eventFile.Substring (tracksLoc);
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

			//Vector3[] LRpoints = new Vector3[fps + 1];
			for(int j = 0; j < fps + 1; j++){
				LRpoints [i,j] = bs.GetPoint ((j*1f)/(fps*1f));
			}

			LineRenderer lr = curve.AddComponent<LineRenderer>();
			lr.SetVertexCount (fps+1);
			Vector3[] LRthisTrack = GetTrack (i,fps+1);
			lr.SetPositions (LRthisTrack);
			lr.SetWidth (0.01f, 0.01f);
			lr.material = mat;

			trks [i] = curve;
		}
		tracks = trks;

		foreach(GameObject track in tracks){
			track.transform.SetParent (transform);
		}
		StartCoroutine (AnimateDots ());
		return true;
	}


}
