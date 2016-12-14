using UnityEngine;
using System.Collections;

public class Cam2d : MonoBehaviour {

	public GameObject canvas;
	float speed = 10;
	Vector3 startingRot;
	bool isAutoScrolling = true;

	void Start () {
		canvas.SetActive (true);
		startingRot = transform.eulerAngles;
	}

	void autoScrollToggle(bool shouldScroll){
		isAutoScrolling = shouldScroll;
		if (isAutoScrolling) {
			transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
			speed = 10;
		} else {
			speed = 40;
		}
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if (!isAutoScrolling) {
				autoScrollToggle (true);
			} else {
				autoScrollToggle (false);
			}
		}
		if(Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Escape)){
			canvas.SetActive (!canvas.activeSelf);
		}
		float y = 0;
		float z = 0;

		if(Input.GetKey(KeyCode.W)){
			z--;
			autoScrollToggle (false);
		}
		if(Input.GetKey(KeyCode.A)){
			y++;
			autoScrollToggle (false);
		}
		if(Input.GetKey(KeyCode.S)){
			z++;
			autoScrollToggle (false);
		}
		if(Input.GetKey(KeyCode.D)){
			y--;
			autoScrollToggle (false);
		}

		if (isAutoScrolling) {
			y = 1;
		}

		transform.Rotate (new Vector3(0,y,z) * Time.deltaTime * speed);
		transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,transform.eulerAngles.z);
	}
}
