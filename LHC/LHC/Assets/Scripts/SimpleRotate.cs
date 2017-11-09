using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour {

	public float x;
	public float y;
	public float z;


	void FixedUpdate () {
		transform.Rotate (new Vector3(x,y,z) * 10);
	}
}
