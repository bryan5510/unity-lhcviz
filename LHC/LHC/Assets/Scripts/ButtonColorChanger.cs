using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour {

	Button autoLoopButton;

	SettingsManager sm;
	void Start(){
		sm = GameObject.Find ("SettingsManager").GetComponent<SettingsManager> ();
		autoLoopButton = this.GetComponent<Button> ();

		SetCorrectColor ();
	}

	public void ToggleAutoMoveToNext(){
		sm.isAutoLoopOn = !sm.isAutoLoopOn;
		SetCorrectColor ();
	}

	void SetCorrectColor(){
		if (!sm.isAutoLoopOn) {
			SetRed ();
		} else {
			SetGreen ();
		}
	}

	void SetRed(){
		ColorBlock cb = autoLoopButton.colors;
		cb.normalColor = Color.red;
		cb.highlightedColor = new Color(1f,0.2f,0.2f);
		autoLoopButton.colors = cb;
	}

	void SetGreen(){
		ColorBlock cb = autoLoopButton.colors;
		cb.normalColor = Color.green;
		cb.highlightedColor = new Color(0.2f,1f,0.2f);
		autoLoopButton.colors = cb;
	}

}
