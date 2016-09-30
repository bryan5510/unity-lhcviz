using UnityEngine;
using System;
using System.Collections;
using System.IO;
using Ionic.Zip;

public class EventSpawner : MonoBehaviour {

	DirectoryInfo[] runFolders;
	FileInfo[] eventFiles;
	FileInfo[] igFiles;
	string dataEventPath = "Data\\Events";
	int currentEvent = 0;
	int currentRun = 0;

	public GameObject dotShape;
	public Material mat;

	void Start () {
		Reset ();
	}

	void Reset(){
		currentEvent = 0;
		currentRun = 0;

		UnzipAll ();

		DirectoryInfo eventFolder = new DirectoryInfo (dataEventPath);
		runFolders = eventFolder.GetDirectories ();

		SwapEvent (0);
	}

	void SwapEvent(int i){
		if(i >= 0 && i < runFolders.Length){
			currentEvent = 0;
			eventFiles = runFolders[i].GetFiles();
			CreateEvent (eventFiles[currentEvent]);
		}
	}

	void UnzipAll(){

		DirectoryInfo igFolder = new DirectoryInfo ("Data\\igFiles");
		igFiles = igFolder.GetFiles ();

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
	
		if(Input.GetKeyDown(KeyCode.N) && eventFiles.Length > 1){
			if (currentEvent + 1 < eventFiles.Length) {
				currentEvent++;
			} else {
				currentEvent = 0;
			}
			CreateEvent (eventFiles[currentEvent]);
		}
		if(Input.GetKeyDown(KeyCode.M) && runFolders.Length > 1){
			if (currentRun + 1 < runFolders.Length) {
				currentRun++;
			} else {
				currentRun = 0;
			}
			SwapEvent (currentRun);
		}
		if(Input.GetKeyDown(KeyCode.R)){
			Reset ();
		}

	}
}
