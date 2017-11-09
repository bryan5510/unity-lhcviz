using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	public class ButtonManager : MonoBehaviour
	{

		Button btn;
		//UIElement btn;
		EventSpawner EvSpa;
		igLister igLstr;
		//public bool isEvent;

		void Start ()
		{
			EvSpa = GameObject.Find ("EventSpawner").GetComponent<EventSpawner> ();
			igLstr = transform.GetComponentInParent<igLister> ();
			btn = gameObject.GetComponent<Button>();
			//btn = gameObject.GetComponent<UIElement> ();
			/*
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
			}*/
		}

		public void ChangeRun ()
		{
			//Debug.Log (btn.name);
			EvSpa.SetRun (btn.name);
			igLstr.ShowEvents (btn.name);
			btn.transform.parent.parent.parent.parent.Find ("Back").gameObject.SetActive (true);
			btn.transform.parent.parent.parent.parent.Find ("Refresh").gameObject.SetActive (false);
		}

		public void ChangeEvent ()
		{
			//Debug.Log (btn.name);
			EvSpa.SetEvent (btn.name);
		}

		public void GoBack ()
		{
			igLstr.ShowRuns ();
			btn.transform.parent.Find ("Refresh").gameObject.SetActive (true);
			gameObject.SetActive (false);
		}

		public void Refresh ()
		{
			igLstr.Refresh ();
		}

	}
}