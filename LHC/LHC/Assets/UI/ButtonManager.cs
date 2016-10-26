using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour {

	Button btn;
	EventSpawner EvSpa;
	igLister igLstr;
	public bool isEvent;

	void Start () {
		EvSpa = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
		igLstr = transform.GetComponentInParent<igLister> ();
		btn = gameObject.GetComponent<Button>();

		if (btn.name.Equals ("Back")) {
			btn.onClick.AddListener (GoBack);
		} else if (btn.name.Equals ("Refresh")) {
			btn.onClick.AddListener (Refresh);
		} else {
			if (isEvent) {
				btn.onClick.AddListener (ChangeEvent);
			} else {
				btn.onClick.AddListener (ChangeRun);
			}
		}
	}

	void ChangeRun () {
		Debug.Log (btn.name);
		EvSpa.SetRun (btn.name);
		igLstr.ShowEvents (btn.name);
		btn.transform.parent.parent.parent.parent.FindChild ("Back").gameObject.SetActive(true);
		btn.transform.parent.parent.parent.parent.FindChild ("Refresh").gameObject.SetActive (false);
	}

	void ChangeEvent () {
		Debug.Log (btn.name);
		EvSpa.SetEvent (btn.name);
	}

	void GoBack () {
		igLstr.ShowRuns ();
		btn.transform.parent.FindChild ("Refresh").gameObject.SetActive(true);
		gameObject.SetActive (false);
	}

	void Refresh(){
		EvSpa.Reset ();
		igLstr.ShowRuns ();
	}

}
