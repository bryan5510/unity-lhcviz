using UnityEngine;
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

	void SwapRun(int i){
		if(i >= 0 && i < runFolders.Length){
			currentRun = i;
			currentEvent = 0;
			eventFiles = runFolders[i].GetFiles();
			SwapEvent (currentEvent);
		}
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

	void CreateEvent(FileInfo i){
		DeleteAllEvents ();
		GameObject igEventObj = new GameObject ();
		igEventObj.name = "IgEvent";
		IgEvent ige = igEventObj.AddComponent<IgEvent> ();
		ige.dotShape = dotShape;
		ige.mat = mat;
		ige.parseExtras (i);
	}

	void DeleteAllEvents(){
		if(GameObject.Find ("IgEvent")){
			Destroy (GameObject.Find ("IgEvent"));
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

	public void IncEvent(){
		if (currentEvent + 1 < eventFiles.Length) {
			currentEvent++;
		} else {
			currentEvent = 0;
		}
		SwapEvent (currentEvent);
	}

	public void IncRun(){
		if (currentRun + 1 < runFolders.Length) {
			currentRun++;
		} else {
			currentRun = 0;
		}
		SwapRun (currentRun);
	}

	public void SwapEvent(int eventNum){
		if(eventNum < eventFiles.Length && eventNum >= 0){
			CreateEvent (eventFiles [eventNum]);
		}
	}

	public void SetEvent(String buttonName){
		for(int i = 0; i < eventFiles.Length; i++){
			if(eventFiles[i].Name.Equals(buttonName)){
				currentEvent = i;
				SwapEvent (currentEvent);
				return;
			}
		}
	}

	public void SetRun(String buttonName){
		for(int i = 0; i < igFiles.Length; i++){
			if(igFiles[i].Name.Equals(buttonName)){
				SwapRun (i);
				return;
			}
		}
	}

}
