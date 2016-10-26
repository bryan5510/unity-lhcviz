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

	// Use this for initialization
	void Start () {
		EvSpa = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
		UI = GameObject.Find ("Canvas");
		content = UI.transform.FindChild ("Scroll View").FindChild ("Viewport").FindChild ("Content").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P)){
			ShowRuns ();
		}
	}

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
		button.GetComponent<RectTransform> ().localScale.Set (1,1,1);
		button.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3(0,pos,0);
		button.name = name;
		button.transform.FindChild("Text").GetComponent<Text>().text = name;
	}

	public void ShowEvents(string runName){
		FileInfo[] eventFiles = EvSpa.GetEventFiles ();
		PoputlateList (eventFiles, eventButtonPrefab);
	}

	public void Refresh(){
		EvSpa.Reset ();
		ShowRuns ();
	}

	public void ShowRuns(){
		FileInfo[] igFiles = EvSpa.GetIgFiles ();
		PoputlateList (igFiles, runButtonPrefab);
	}

}
