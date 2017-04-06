using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraTest : MonoBehaviour {

	public GameObject canvas;
	public Transform canvasHolder;
	
	void Update () {

		if(Input.GetKeyDown(KeyCode.O)){
			if(canvas.activeSelf == false){
				canvas.transform.position = canvasHolder.position;
				canvas.transform.eulerAngles = new Vector3 (canvasHolder.eulerAngles.x,canvasHolder.eulerAngles.y,0);
			}
			canvas.SetActive(!canvas.activeSelf);
		}

		if(Input.GetKey(KeyCode.UpArrow)){
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x,gameObject.transform.position.y + Time.deltaTime,gameObject.transform.position.z);
		}
		if(Input.GetKey(KeyCode.DownArrow)){
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x,gameObject.transform.position.y - Time.deltaTime,gameObject.transform.position.z);
		}

		if(Input.GetKey(KeyCode.Y)){
			GameObject sbv = GameObject.Find ("Canvas").transform.FindChild ("Scroll View").FindChild ("Scrollbar Vertical").gameObject;
			Scrollbar sb = sbv.GetComponent<Scrollbar> ();

			if(1 > sb.value + 0.01f){
				sb.value = sb.value + 0.01f;
			}
		}
		if(Input.GetKey(KeyCode.H)){
			GameObject sbv = GameObject.Find ("Canvas").transform.FindChild ("Scroll View").FindChild ("Scrollbar Vertical").gameObject;
			Scrollbar sb = sbv.GetComponent<Scrollbar> ();

			if(0 < sb.value - 0.01f){
				sb.value = sb.value - 0.01f;
			}
		}
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast(ray, out hit);
		if (Input.GetMouseButton (0)) {
			if (hit.collider.gameObject.tag == "Handle") {
				float dist = Vector3.Distance (new Vector3 (0f, hit.point.y, 0f), new Vector3 (0f, -2.82f, 0f));
				hit.collider.gameObject.GetComponent<Scrollbar>().value = dist / .725f;
			}
		}
	}
}
