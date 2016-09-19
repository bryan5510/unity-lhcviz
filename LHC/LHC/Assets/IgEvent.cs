using UnityEngine;
using System.Collections;
using System.IO;

//[System.Serializable]
public class IgEvent : MonoBehaviour{

	public GameObject dotShape;
	GameObject[] tracks;
	int currentFrame = 0;
	float fps = 60f;
	 
	void Start(){
//		parseTracks ();
		tracks = parseExtras ();
		//Debug.Log (eventFile);
		foreach(GameObject track in tracks){
			track.transform.SetParent (transform);
		}
		StartCoroutine (AnimateDots ());
	}


	IEnumerator AnimateDots(){
		int direction = 1;
		while(true){
			if (!MoveDots (direction)) {
				direction *= -1;
			}
			yield return new WaitForSeconds(0.017f);
		}
	}

	bool MoveDots(int lr){
		if (currentFrame + lr >= 0 && currentFrame + lr <= fps) {
			currentFrame += lr;
			for (int i = 0; i < tracks.Length; i++) {//for each track per frame
				tracks [i].transform.GetChild(0).position = tracks[i].GetComponent<BezierSpline>().GetPoint(currentFrame/fps);
			}
			return true;
		} else {
			return false;
		}
	}

	void parseTracks(){
		string eventFile = File.ReadAllText("Assets\\ispy-webgl-master\\data\\Electron\\Events\\Run_146644\\Event_719109566");
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
			Debug.Log (lines[i]);
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
				Debug.Log (values[j]);
			}
		}
	}

	GameObject[] parseExtras(){
		//"Extras_V1": [["pos_1", "v3d"],["dir_1", "v3d"],["pos_2", "v3d"],["dir_2", "v3d"]]
		// [[[0.000924736, 0.000185603, -0.0215063], [-0.746714, -0.606572, -1.46347], [-1.2536, 0.236426, -2.22576], [-0.451694, 0.799546, -1.39922]], 
		string eventFile = File.ReadAllText("Assets\\ispy-webgl-master\\data\\Electron\\Events\\Run_146644\\Event_719109566");
		int tracksLoc = eventFile.IndexOf ("Collections");
		eventFile = eventFile.Substring (tracksLoc);
		tracksLoc = eventFile.IndexOf ("Extras_V");
		eventFile = eventFile.Substring (tracksLoc);
		tracksLoc = eventFile.IndexOf (":");
		eventFile = eventFile.Substring (tracksLoc);
		int tracksEnd = eventFile.IndexOf ("\"");
		eventFile = eventFile.Substring (3,tracksEnd-10);
		string[] lines = eventFile.Split ("\n"[0]);

		GameObject[] trks = new GameObject[lines.Length];

		for(int i = 0; i < lines.Length; i++){
			Debug.Log (lines[i]);
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
			tracksEnd = lines[i].IndexOf ("]");
			string dir2 = lines[i].Substring (1,tracksEnd-1);
			string text = pos1 + "," + dir1 + "," + pos2 + "," + dir2;

			string[] remaining = text.Split (","[0]);
			float[] values = new float[remaining.Length];
			for(int j=0; j < remaining.Length; j++){
				values [j] = float.Parse (remaining[j]);
				Debug.Log (values[j]);
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
			curve.AddComponent<BezierSpline> ();
			BezierSpline bs = curve.GetComponent<BezierSpline> ();
			bs.SetAllControlPoints (p1,p3,p4,p2);
			bs.SetAllControlPointMode (BezierControlPointMode.Free);
			SplineDecorator sd = curve.AddComponent<SplineDecorator> ();
			sd.spline = bs;
			sd.frequency = 1;
			Transform[] dotShapes = new Transform[1] {dotShape.transform};
			sd.items = dotShapes;
			sd.MakeSpline();

			trks [i] = sd.gameObject;


			/*
			scale = 0.1f;
			GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			dot.transform.position = p1;
			dot.GetComponent<Renderer> ().material.color = Color.blue;
			dot.transform.localScale = new Vector3(scale,scale,scale);
			GameObject dot2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			dot2.transform.position = p2;
			dot2.GetComponent<Renderer> ().material.color = Color.red;
			dot2.transform.localScale = new Vector3(scale,scale,scale);
			*/
		}
		return trks;
	}


	/*


  	collectionNames = [ x.split("\":")[0].strip("\"")
                      for x in text.split("\'Types\': {")[1]
                                   .split("\'Collections\'")[0]
                                   .split("\n")
	if '":' in x]

	*/

	/*
	public int[][] numbs;
	void Start (){
		numbs = new int[][] { new int[] {1,2,3,4,5,6,7,8,9}, new int[] {1,2,3,4,5,6,7,8,9}, new int[] {1,2,3,4,5,6,7,8,9} };
		Debug.Log (ToJsonString ());
		Debug.Log (numbs[2][5]);
		OverwriteFromJSON ("{\"numbs\":[[12,1],[13,2],[14,3]]}");
		Debug.Log (ToJsonString ());
	}

	public static IgEvent CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<IgEvent> (jsonString);
	}

	public void OverwriteFromJSON(string jsonString){
		JsonUtility.FromJsonOverwrite (jsonString, this);
	}

	public string ToJsonString(){
		return JsonUtility.ToJson (this);
	}
	
	*/

	/*
	string[,] Event_V2 = "Event_V2": [["run", "int"],["event", "int"],["ls", "int"],["orbit", "int"],["bx", "int"],["time", "string"],["localtime", "string"]];

		"Products_V1": [["Product", "string"]]
	;
		"BeamSpot_V1": [["pos", "v3d"],["xError", "double"],["yError", "double"],["zError", "double"]]
	;	
		"CaloClusters_V1": [["energy", "double"],["pos", "v3d"],["eta", "double"],["phi", "double"],["algo", "string"]]
	;	
		"RecHitFractions_V1": [["detid", "int"],["fraction", "double"],["front_1", "v3d"],["front_2", "v3d"],["front_3", "v3d"],["front_4", "v3d"],["back_1", "v3d"],["back_2", "v3d"],["back_3", "v3d"],["back_4", "v3d"]]
	;	
		"CaloTowers_V2": [["et", "double"],["eta", "double"],["phi", "double"],["iphi", "double"],["hadEnergy", "double"],["emEnergy", "double"],["outerEnergy", "double"],["ecalTime", "double"],["hcalTime", "double"],["emPosition", "v3d"],["hadPosition", "v3d"],["front_1", "v3d"],["front_2", "v3d"],["front_3", "v3d"],["front_4", "v3d"],["back_1", "v3d"],["back_2", "v3d"],["back_3", "v3d"],["back_4", "v3d"]]
	;	
		"CaloMETs_V1": [["phi", "double"],["pt", "double"],["px", "double"],["py", "double"],["pz", "double"]]
	;	
		"CaloTaus_V1": [["eta", "double"],["phi", "double"],["pt", "double"],["charge", "double"],["leadTrackSignedSiPt", "double"],["leadTrackHCAL3x3HitsEtSum", "double"],["leadTrackHCAL3x3HottestHitDEta", "double"],["signalTracksInvariantMass", "double"],["TracksInvariantMass", "double"],["isolationTracksPtSum", "double"],["isolationECALHitsEtSum", "double"],["maximumHCALHitEt", "double"]]
	;	
		"EBRecHits_V2": [["energy", "double"],["eta", "double"],["phi", "double"],["time", "double"],["detid", "int"],["front_1", "v3d"],["front_2", "v3d"],["front_3", "v3d"],["front_4", "v3d"],["back_1", "v3d"],["back_2", "v3d"],["back_3", "v3d"],["back_4", "v3d"]]
	;	
		"EERecHits_V2": [["energy", "double"],["eta", "double"],["phi", "double"],["time", "double"],["detid", "int"],["front_1", "v3d"],["front_2", "v3d"],["front_3", "v3d"],["front_4", "v3d"],["back_1", "v3d"],["back_2", "v3d"],["back_3", "v3d"],["back_4", "v3d"]]
	;	
		"ESRecHits_V2": [["energy", "double"],["eta", "double"],["phi", "double"],["time", "double"],["detid", "int"],["front_1", "v3d"],["front_2", "v3d"],["front_3", "v3d"],["front_4", "v3d"],["back_1", "v3d"],["back_2", "v3d"],["back_3", "v3d"],["back_4", "v3d"]]
	;	
		"HBRecHits_V2": [["energy", "double"],["eta", "double"],["phi", "double"],["time", "double"],["detid", "int"],["front_1", "v3d"],["front_2", "v3d"],["front_3", "v3d"],["front_4", "v3d"],["back_1", "v3d"],["back_2", "v3d"],["back_3", "v3d"],["back_4", "v3d"]]
	;	
		"HERecHits_V2": [["energy", "double"],["eta", "double"],["phi", "double"],["time", "double"],["detid", "int"],["front_1", "v3d"],["front_2", "v3d"],["front_3", "v3d"],["front_4", "v3d"],["back_1", "v3d"],["back_2", "v3d"],["back_3", "v3d"],["back_4", "v3d"]]
	;	
		"HFRecHits_V2": [["energy", "double"],["eta", "double"],["phi", "double"],["time", "double"],["detid", "int"],["front_1", "v3d"],["front_2", "v3d"],["front_3", "v3d"],["front_4", "v3d"],["back_1", "v3d"],["back_2", "v3d"],["back_3", "v3d"],["back_4", "v3d"]]
	;	
		"HORecHits_V2": [["energy", "double"],["eta", "double"],["phi", "double"],["time", "double"],["detid", "int"],["front_1", "v3d"],["front_2", "v3d"],["front_3", "v3d"],["front_4", "v3d"],["back_1", "v3d"],["back_2", "v3d"],["back_3", "v3d"],["back_4", "v3d"]]
	;	
		"Jets_V1": [["et", "double"],["eta", "double"],["theta", "double"],["phi", "double"]]
	;	
		"METs_V1": [["phi", "double"],["pt", "double"],["px", "double"],["py", "double"],["pz", "double"]]
	;	
		"StandaloneMuons_V2": [["pt", "double"],["charge", "int"],["pos", "v3d"],["phi", "double"],["eta", "double"],["calo_energy", "double"]]
	;	
		"GlobalMuons_V1": [["pt", "double"],["charge", "int"],["rp", "v3d"],["phi", "double"],["eta", "double"],["calo_energy", "double"]]
	;	
		"Extras_V1": [["pos_1", "v3d"],["dir_1", "v3d"],["pos_2", "v3d"],["dir_2", "v3d"]]
	;
		"Points_V1": [["pos", "v3d"]]
	;
		"Photons_V1": [["energy", "double"],["et", "double"],["eta", "double"],["phi", "double"],["pos", "v3d"],["hadronicOverEm", "double"],["hadronicDepth1OverEcal", "double"],["hadronicDepth2OverEcal", "double"],["e1x5", "double"],["e2x5", "double"],["e3x3", "double"],["e5x5", "double"],["maxEnergyXtal", "double"],["sigmaEtaEta", "double"],["sigmaIetaIeta", "double"],["r1x5", "double"],["r2x5", "double"],["r9", "double"],["ecalRecHitSumEtConeDR04", "double"],["hcalTowerSumEtConeDR04", "double"],["hcalDepth1TowerSumEtConeDR04", "double"],["hcalDepth2TowerSumEtConeDR04", "double"],["trkSumPtSolidConeConeDR04", "double"],["trkSumPtHollowConeDR04", "double"],["nTrkSolidConeDR04", "int"],["nTrkHollowConeDR04", "int"],["ecalRecHitSumEtDR03", "double"],["hcalTowerSumEtDR03", "double"],["hcalDepth1TowerSumEtDR03", "double"],["hcalDepth2TowerSumEtDR03", "double"],["trkSumPtSolidConeDR03", "double"],["trkSumPtHollowConeDR03", "double"],["nTrkSolidConeDR03", "int"],["nTrkHollowConeDR03", "int"]]
	;
		"SuperClusters_V1": [["energy", "double"],["pos", "v3d"],["eta", "double"],["phi", "double"],["algo", "string"],["etaWidth", "double"],["phiWidth", "double"],["rawEnergy", "double"],["preshowerEnergy", "double"]]
	;
		"Tracks_V3": [["pos", "v3d"],["dir", "v3d"],["pt", "double"],["phi", "double"],["eta", "double"],["charge", "int"],["chi2", "double"],["ndof", "double"]]
	;
		"GsfElectrons_V2": [["pt", "double"],["eta", "double"],["phi", "double"],["charge", "int"],["pos", "v3d"],["dir", "v3d"]]
	;
		"TrackerMuons_V2": [["pt", "double"],["charge", "int"],["pos", "v3d"],["phi", "double"],["eta", "double"]]
	;
		"TriggerPaths_V1": [["Name", "string"],["Index", "int"],["WasRun", "int"],["Accept", "int"],["Error", "int"],["Objects", "string"]]
	;
		"TriggerObjects_V1": [["path", "string"],["slot", "int"],["moduleLabel", "string"],["moduleType", "string"],["VID", "int"],["KEY", "int"],["id", "int"],["pt", "double"],["eta", "double"],["phi", "double"],["mass", "double"]]
	;
		"Vertices_V1": [["isValid", "int"],["isFake", "int"],["pos", "v3d"],["xError", "double"],["yError", "double"],["zError", "double"],["chi2", "double"],["ndof", "double"]]
*/



}
