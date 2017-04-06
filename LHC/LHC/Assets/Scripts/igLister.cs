using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class igLister : MonoBehaviour {

	EventSpawner EvSpa;
	GameObject UI;
	public Object runButtonPrefab;
	public Object eventButtonPrefab;
	GameObject content;
	//GameObject handle;
	//public Scrollbar scrollbar;

	void Start () {
		UI = GameObject.Find ("Canvas");
		//handle = UI.transform.FindChild ("Scroll View").FindChild ("Scrollbar Vertical").FindChild ("Sliding Area").FindChild ("Handle").gameObject;
		content = UI.transform.FindChild ("Scroll View").FindChild ("Viewport").FindChild ("Content").gameObject;
		content.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		EvSpa = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
	}
	/*
	void Update () {
		if(Input.GetKeyDown(KeyCode.P)){
			ShowRuns ();
		}
		//handle.transform.GetComponent<BoxCollider>().size = new Vector3(20f, 20f + (240f * scrollbar.size), 1f);
	}*/

	void PoputlateList(FileInfo[] files, Object buttonPrefab){
		Transform[] buttons = content.transform.GetComponentsInChildren<Transform> ();
		foreach(Transform button in buttons){
			if(content.transform != button){
				Destroy (button.gameObject);	
			}
		}
		float count = -20;
		foreach(FileInfo file in files){
			CreateButton(count, file.Name, buttonPrefab);
			count = count - 20;
		}
		content.GetComponent<RectTransform> ().sizeDelta = new Vector2(0,-count);
	}

	void CreateButton(float pos, string name, Object buttonPrefab){
		GameObject button = Instantiate (buttonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		button.transform.SetParent (content.transform);
		button.GetComponent<RectTransform> ().localScale = new Vector3(1,1,1);
		button.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3(0,pos,0);
		button.GetComponent<RectTransform> ().localRotation = Quaternion.identity;
		button.name = name;
		button.transform.FindChild("Text").GetComponent<Text>().text = name;
	}

	public void ShowEvents(string runName){
		content.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		FileInfo[] eventFiles = EvSpa.GetEventFiles ();
		PoputlateList (eventFiles, eventButtonPrefab);
	}

	public void Refresh(){
		EvSpa.Reset ();
		ShowRuns ();
	}

	public void ShowRuns(){
		content.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		FileInfo[] igFiles = EvSpa.GetIgFiles ();
		PoputlateList (igFiles, runButtonPrefab);
	}

}
