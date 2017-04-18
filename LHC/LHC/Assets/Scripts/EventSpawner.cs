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

	public Button autoLoopButton;

	void Start () {
		igFolder = new DirectoryInfo ("Data\\igFiles");
		Reset ();
		SwapRun (0);
		ColorBlock cb = autoLoopButton.colors;
		cb.normalColor = Color.red;
		cb.highlightedColor = new Color(1f,0.2f,0.2f);
		autoLoopButton.colors = cb;
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
		return ige.parseExtras (i);
	}

	void DeleteAllEvents(){
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("IgEvent");
		for(int i = 0; i < gos.Length; i++){
			Destroy (gos[i]);
		}
	}

	void FixedUpdate () {
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
		
	public float invokeDelay = 3;
	void AutoMoveToNextEvent(){
		if (IncEvent ()) {
			Invoke ("AutoMoveToNextEvent", invokeDelay);
		}else{
			Invoke ("AutoMoveToNextEvent", 0.5f);
		}
	}
		
	public void ToggleAutoMoveToNext(){
		if (IsInvoking ("AutoMoveToNextEvent")) {
			CancelInvoke ("AutoMoveToNextEvent");
			ColorBlock cb = autoLoopButton.colors;
			cb.normalColor = Color.red;
			cb.highlightedColor = new Color(1f,0.2f,0.2f);
			autoLoopButton.colors = cb;
		} else {
			Invoke ("AutoMoveToNextEvent",invokeDelay);
			ColorBlock cb = autoLoopButton.colors;
			cb.normalColor = Color.green;
			cb.highlightedColor = new Color(0.2f,1f,0.2f);
			autoLoopButton.colors = cb;
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
