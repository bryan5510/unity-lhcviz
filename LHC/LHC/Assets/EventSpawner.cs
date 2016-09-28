using UnityEngine;
using System.Collections;
using System.IO;

public class EventSpawner : MonoBehaviour {

	FileInfo[] eventFiles;
	string path;
	int currentEvent = 0;

	public GameObject dotShape;
	public Material mat;

	void Start () {
		path = "Data\\Events\\Run_146644";
		DirectoryInfo eventFolder = new DirectoryInfo (path);
		eventFiles = eventFolder.GetFiles ();
		CreateEvent (eventFiles[currentEvent]);
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
		/*
	 	foreach(FileInfo eventFile in eventFiles){
			if(GameObject.Find (GetFileName(eventFile))){
				Destroy (GameObject.Find (GetFileName(eventFile)));
			}
		}
		*/
	}

	void Update () {
	
		if(Input.GetKeyDown(KeyCode.N) && eventFiles.Length != 0){
			if (currentEvent + 1 < eventFiles.Length) {
				currentEvent++;
			} else {
				currentEvent = 0;
			}
			CreateEvent (eventFiles[currentEvent]);
		}

	}
}
