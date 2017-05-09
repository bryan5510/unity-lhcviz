using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using Ionic.Zip;

public class EventSpawner : MonoBehaviour {

	DirectoryInfo igFolder;
	DirectoryInfo[] runFolders;
	FileInfo[] eventFiles;
	FileInfo[] igFiles;
	string dataEventPath = "Data\\Events";
	int currentEvent = 0;
	int currentRun = 0;

	public GameObject dotShape;
	public Material mat;
	public Material proton;
	public Material electron;

	void Start () {
		igFolder = new DirectoryInfo ("Data\\igFiles");
		Reset ();
		SwapRun (0);
	}

	public void Reset(){
		currentEvent = 0;
		currentRun = 0;

		igFiles = igFolder.GetFiles ();
		UnzipAll ();

		DirectoryInfo eventFolder = new DirectoryInfo (dataEventPath);
		runFolders = eventFolder.GetDirectories ();
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
		//Directory.CreateDirectory ("Data");
		using (ZipFile zip = ZipFile.Read(fileToUnzip.FullName)){
			zip.ExtractAll ("Data", ExtractExistingFileAction.OverwriteSilently);
		}

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
		ige.mat = mat;
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

	void Update () {
		if (Input.GetKeyDown (KeyCode.N) && eventFiles.Length > 1) {
			IncEvent ();
		}
		if(Input.GetKeyDown(KeyCode.M) && runFolders.Length > 1){
			IncRun ();
		}
		if(Input.GetKeyDown(KeyCode.R)){
			Reset ();
		}
	}

	public bool IncEvent(){
		if (currentEvent + 1 < eventFiles.Length) {
			currentEvent++;
		} else {
			currentEvent = 0;
		}
		return SwapEvent (currentEvent);
	}

	public bool IncRun(){
		if (currentRun + 1 < runFolders.Length) {
			currentRun++;
		} else {
			currentRun = 0;
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
		if(i >= 0 && i < runFolders.Length){
			currentRun = i;
			currentEvent = 0;
			eventFiles = runFolders[i].GetFiles();
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
