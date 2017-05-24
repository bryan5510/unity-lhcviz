using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

	public int fps = 270;
	public bool showJets = true;
	public bool showHits = true;

	public bool isAutoLoopOn = true;

	void Awake()
	{
		DontDestroyOnLoad(this);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}		
}
