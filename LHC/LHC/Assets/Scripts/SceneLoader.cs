using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	void Update () {
		if(Input.GetKeyDown(KeyCode.R)){
			ReloadScene ();
		}
	}

	public void ReloadScene(){
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

}
