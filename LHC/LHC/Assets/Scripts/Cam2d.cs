using UnityEngine;
using System.Collections;

public class Cam2d : MonoBehaviour {

	public GameObject canvas;
	float speed = 10;
	float speedMode = 10;
	//Vector3 startingRot;
	bool isAutoScrolling = true;

	void Start () {
		canvas.SetActive (true);
		//startingRot = transform.eulerAngles;
	}

	void autoScrollToggle(bool shouldScroll){
		isAutoScrolling = shouldScroll;
		if (isAutoScrolling) {
			transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
			speed = speedMode;
		} else {
			speed = 40;
		}
	}

	public Camera c;
	Vector3 mpOld = Vector3.zero;
	Vector3 mpNew = Vector3.zero;
	void FixedUpdate(){
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray r = c.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
			if (Physics.Raycast(r,out hit)) {
				if (hit.collider.name == "turningSphere") {
					autoScrollToggle (false);
					mpOld = Input.mousePosition;
				}
			}
		}if(Input.GetMouseButton(0)){
			RaycastHit hit;
			Ray r = c.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0));
			if (Physics.Raycast(r,out hit)) {
				if (hit.collider.name == "turningSphere") {
					mpNew = Input.mousePosition;
					float x = mpNew.x - mpOld.x;
					float y = mpNew.y - mpOld.y;
					float z = mpNew.z - mpOld.z;
					transform.Rotate (0,x,y);
				}
			}
			mpOld = mpNew;
		}

		//Debug.DrawRay (r.origin, r.direction * 10, Color.yellow);
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

		if(Input.GetKeyDown(KeyCode.Alpha1)){
			speedMode = 5;
			autoScrollToggle (true);
		}if(Input.GetKeyDown(KeyCode.Alpha2)){
			speedMode = 10;
			autoScrollToggle (true);
		}if(Input.GetKeyDown(KeyCode.Alpha3)){
			speedMode = 20;
			autoScrollToggle (true);
		}if(Input.GetKeyDown(KeyCode.Alpha4)){
			speedMode = 30;
			autoScrollToggle (true);
		}

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
