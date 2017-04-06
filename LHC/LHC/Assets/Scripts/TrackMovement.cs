using UnityEngine;
using System.Collections;

public class TrackMovement : MonoBehaviour {

	Mesh mesh;
	Vector3[] verts;
	int vertCount;
	int trackCount;
	int currentFrame = 0;
	float scale = 0.07f;

	GameObject[] dots;

	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
		verts = mesh.vertices;
		vertCount = mesh.vertexCount;

		/*for(int i=0; i < vertCount; i++){
			GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			dot.transform.position = verts[i];
			dot.transform.localScale = new Vector3(scale/7f,scale/7f,scale/7f);
			dot.transform.SetParent (transform);
			dot.name = "basedot";
		}*/

		trackCount = (int) (vertCount / 33);

		dots = new GameObject[trackCount];

		for(int i=0; i < trackCount; i++){
			GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			dot.transform.position = verts[i * 33];
			dot.transform.localScale = new Vector3(scale,scale,scale);
			dot.GetComponent<Renderer> ().material.color = Color.yellow;
			dot.transform.SetParent (transform);
			dot.name = "Track " + i;
			dots [i] = dot;
		}

		//StartCoroutine (AnimateDots ());
	}

	void Update(){
		if(Input.GetKey(KeyCode.LeftArrow)){
			MoveDots (-1);
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			MoveDots (1);
		}
	}

	IEnumerator AnimateDots(){
		int direction = 1;
		while(true){
			if (!MoveDots (direction)) {
				direction *= -1;
			}
			yield return new WaitForSeconds(0.017f);
		}
	}

	bool MoveDots(int lr){
		if (currentFrame + lr >= 0 && currentFrame + lr <= 32) {
			currentFrame += lr;
			for (int i = 0; i < trackCount; i++) {//for each track per frame
				dots [i].transform.position = verts [(i * 33) + currentFrame];
			}
			return true;
		} else {
			return false;
		}
	}

}
