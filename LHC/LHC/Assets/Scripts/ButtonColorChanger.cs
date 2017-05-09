using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour {

	Button autoLoopButton;
	bool isOn = false;

	void Awake(){
		autoLoopButton = this.GetComponent<Button> ();

		ColorBlock cb = autoLoopButton.colors;
		cb.normalColor = Color.red;
		cb.highlightedColor = new Color(1f,0.2f,0.2f);
		autoLoopButton.colors = cb;
	}

	public void ToggleAutoMoveToNext(){
		isOn = !isOn;
		if (!isOn) {
			ColorBlock cb = autoLoopButton.colors;
			cb.normalColor = Color.red;
			cb.highlightedColor = new Color(1f,0.2f,0.2f);
			autoLoopButton.colors = cb;
		} else {
			ColorBlock cb = autoLoopButton.colors;
			cb.normalColor = Color.green;
			cb.highlightedColor = new Color(0.2f,1f,0.2f);
			autoLoopButton.colors = cb;
		}
	}

}
