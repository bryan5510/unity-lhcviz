using UnityEngine;
using System.Collections;

public class Cam2d : MonoBehaviour {

	float speed = 0.25f;
	float speedMode = 0.25f;
	bool isAutoScrolling = true;

	void autoScrollToggle(bool shouldScroll){
		isAutoScrolling = shouldScroll;
		if (isAutoScrolling) {
			transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
			speed = speedMode;
		} else {
			speed = 1;
		}
	}

	//public Camera c;
	//Vector3 mpOld = Vector3.zero;
	//Vector3 mpNew = Vector3.zero;
	void Update(){
		/*
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
					float xAngle = mpNew.x - mpOld.x;
					float yAngle = mpNew.y - mpOld.y;
					transform.Rotate (0,xAngle,yAngle);
				}
			}
			mpOld = mpNew;
		}*/

		/*if(50 < c.fieldOfView-Input.mouseScrollDelta.y && c.fieldOfView-Input.mouseScrollDelta.y < 110){
			c.fieldOfView -= Input.mouseScrollDelta.y;
		}*/

		if(Input.GetKeyDown(KeyCode.Space)){
			if (!isAutoScrolling) {
				autoScrollToggle (true);
			} else {
				autoScrollToggle (false);
			}
		}

		float y = 0;
		float z = 0;

		if(Input.GetKeyDown(KeyCode.Alpha1)){
			speedMode = 0.1f;
			autoScrollToggle (true);
		}if(Input.GetKeyDown(KeyCode.Alpha2)){
			speedMode = 0.25f;
			autoScrollToggle (true);
		}if(Input.GetKeyDown(KeyCode.Alpha3)){
			speedMode = 0.5f;
			autoScrollToggle (true);
		}if(Input.GetKeyDown(KeyCode.Alpha4)){
			speedMode = 0.75f;
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

		transform.Rotate (new Vector3(0,y,z) * speed * 10 * Time.deltaTime);
		transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,transform.eulerAngles.z);
	}
}
