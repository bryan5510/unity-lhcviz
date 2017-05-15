using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour {

	public float timer = 3;
	void Start () {
		Invoke ("ReloadAndGC", timer);
	}

	void ReloadAndGC () {
		Resources.UnloadUnusedAssets ();
		System.GC.Collect();
		Invoke ("ReloadAndGC", timer);
	}
}
