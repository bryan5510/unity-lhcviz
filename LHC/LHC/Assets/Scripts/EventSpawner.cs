using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using Ionic.Zip;

public class EventSpawner : MonoBehaviour {

	DirectoryInfo igZippedFolder;
	//DirectoryInfo[] runFolders;
	FileInfo[] eventFiles;
	DirectoryInfo[] igsUnpackedFolder;
	FileInfo[] igFiles;
	string dataEventPath = "Data\\UnZippedIgs";
	int currentEvent = 0;
	int currentRun = 0;

	public GameObject dotShape;
	public GameObject cone;
	public Material mat;
	public Material met;
	public Material proton;
	public Material electron;

	SettingsManager sm;
	void Start () {
		igZippedFolder = new DirectoryInfo ("Data\\igFiles");
		sm = GameObject.Find ("SettingsManager").GetComponent<SettingsManager> ();
		Reset ();
		//SwapRun (0);
	}

	public void Reset(){
		currentEvent = 0;
		currentRun = 0;

		igFiles = igZippedFolder.GetFiles ();
		UnzipAll ();

		DirectoryInfo iuf = new DirectoryInfo (dataEventPath);
		igsUnpackedFolder = iuf.GetDirectories ();

		SwapRun (0);
	}

	public FileInfo[] GetIgFiles(){
		return igFiles;
	}

	public FileInfo[] GetEventFiles(){
		return eventFiles;
	}

	void UnzipAll(){
		foreach(FileInfo ig in igFiles){
			Unzip (ig);
		}
	}

	void Unzip(FileInfo fileToUnzip){
		//try{
		Directory.CreateDirectory (dataEventPath+"\\"+fileToUnzip.Name);
		//}catch{}
		using (ZipFile zip = ZipFile.Read(fileToUnzip.FullName)){
			zip.ExtractAll (dataEventPath+"\\"+fileToUnzip.Name, ExtractExistingFileAction.OverwriteSilently);
		}

		DirectoryInfo igfolder = new DirectoryInfo (dataEventPath+"\\"+fileToUnzip.Name+"\\Events");
		DirectoryInfo[] allruns = igfolder.GetDirectories ();
		foreach(DirectoryInfo run in allruns){
			FileInfo[] allfiles = run.GetFiles ();
			foreach(FileInfo file in allfiles){
				try{
					file.MoveTo (dataEventPath+"\\"+fileToUnzip.Name+"\\Events\\"+file.Name);
				}catch{}
			}
			run.Delete (true);
		}
	}

	void fileCleanUp(){
		Directory.Delete (dataEventPath, true);
	}

	void OnApplicationQuit(){
		fileCleanUp ();
	}

	string GetFileName(FileInfo i){
		return Path.GetFileNameWithoutExtension (i.ToString());
	}

	bool CreateEvent(FileInfo i){
		DeleteAllEvents ();
		GameObject igEventObj = new GameObject ();
		igEventObj.name = "IgEvent";
		igEventObj.tag = "IgEvent";
		IgEvent ige = igEventObj.AddComponent<IgEvent> ();
		ige.dotShape = dotShape;
		ige.cone = cone;
		ige.mat = mat;
		ige.met = met;
		ige.proton = proton;
		ige.electron = electron;
		return ige.parseExtras (i);
	}

	void DeleteAllEvents(){
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("IgEvent");
		for(int i = 0; i < gos.Length; i++){
			Destroy (gos[i]);
		}
	}

	void ToggleJets(){
		sm.showJets = !sm.showJets;
		GameObject.Find ("IgEvent").GetComponent<IgEvent> ().HideJets (sm.showJets);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.N) && eventFiles.Length > 1) {
			IncEvent ();
		}
		if(Input.GetKeyDown(KeyCode.M) && igsUnpackedFolder.Length > 1){
			IncRun ();
		}
		if(Input.GetKeyDown(KeyCode.J)){
			ToggleJets ();
		}
		/*if(Input.GetKeyDown(KeyCode.R)){
			Reset ();
		}*/

		if(Input.GetKeyDown(KeyCode.Alpha1)){
			sm.fps = 90;
			SwapEvent (currentEvent);
		}if(Input.GetKeyDown(KeyCode.Alpha2)){
			sm.fps = 180;
			SwapEvent (currentEvent);
		}if(Input.GetKeyDown(KeyCode.Alpha3)){
			sm.fps = 270;
			SwapEvent (currentEvent);
		}if(Input.GetKeyDown(KeyCode.Alpha4)){
			sm.fps = 360;
			SwapEvent (currentEvent);
		}if(Input.GetKeyDown(KeyCode.Alpha5)){
			sm.fps = 450;
			SwapEvent (currentEvent);
		}

	}

	public bool IncEvent(bool canMoveOntoNextRun = true){
		if (currentEvent + 1 < eventFiles.Length) {
			currentEvent++;
		} else if(canMoveOntoNextRun){
			return IncRun ();
		} else {
			currentEvent = 0;
		}
		return SwapEvent (currentEvent);
	}

	public bool IncRun(){
		if (currentRun + 1 < igsUnpackedFolder.Length) {
			currentRun++;
		} else {
			GameObject.Find ("SceneLoader").GetComponent<SceneLoader> ().ReloadScene ();
			return true;
		}
		return SwapRun (currentRun);
	}

	public bool SwapEvent(int eventNum){
		if(eventNum < eventFiles.Length && eventNum >= 0){
			return CreateEvent (eventFiles [eventNum]);
		}
		return false;
	}

	public bool SwapRun(int i){
		if(i >= 0 && i < igsUnpackedFolder.Length){
			currentRun = i;
			currentEvent = 0;
			eventFiles = igsUnpackedFolder[i].GetDirectories()[0].GetFiles();
			return SwapEvent (currentEvent);
		}
		return false;
	}

	public bool SetEvent(String buttonName){
		for(int i = 0; i < eventFiles.Length; i++){
			if(eventFiles[i].Name.Equals(buttonName)){
				currentEvent = i;
				return SwapEvent (currentEvent);
			}
		}
		return false;
	}

	public bool SetRun(String buttonName){
		for(int i = 0; i < igFiles.Length; i++){
			if(igFiles[i].Name.Equals(buttonName)){
				return SwapRun (i);
			}
		}
		return false;
	}

}
