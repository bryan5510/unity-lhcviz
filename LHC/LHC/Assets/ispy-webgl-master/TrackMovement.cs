using UnityEngine;
using System.Collections;

public class TrackMovement : MonoBehaviour {

	Mesh mesh;
	Vector3[] verts;
	int vertCount;
	int trackCount;
	int currentFrame = 0;

	GameObject[] dots;

	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
		verts = mesh.vertices;
		vertCount = mesh.vertexCount;
		trackCount = (int) (vertCount / 33);

		dots = new GameObject[trackCount];

		for(int i=0; i < trackCount; i++){
			GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			dot.transform.position = verts[i * 33];
			dot.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
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
	/*
	IEnumerator AnimateDots(){
		while(true){
			for(int j=0; j < 33; j++){//for each animation frame
				for(int i=0; i < trackCount; i++){//for each track per frame
					dots [i].transform.position = verts[(i * 33) + j];
				}
				yield return new WaitForSeconds (1f);
			}
		}
	}*/

	void MoveDots(int lr){
		if(currentFrame + lr >= 0 && currentFrame + lr <= 32){
			currentFrame += lr;
			for(int i=0; i < trackCount; i++){//for each track per frame
				dots [i].transform.position = verts[(i * 33) + currentFrame];
			}
		}
	}

}
