using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasActiveController : MonoBehaviour {

	public GameObject canvas;

	void Start () {
		canvas.SetActive (true);
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Escape)){
			canvas.SetActive (!canvas.activeSelf);
		}
	}
}
