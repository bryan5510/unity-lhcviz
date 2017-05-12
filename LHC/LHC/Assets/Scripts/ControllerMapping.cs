using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerMapping : MonoBehaviour {

	public Transform body;
	public Transform led;

	public Transform button;
	public Transform l_grip;
	public Transform r_grip;
	public Transform sys_button;

	public Transform trackpad;
	public Transform trackpad_touch;
	public Transform trigger;


	float offset = 100;
	void HandAttachedUpdate (Hand hand){
		if(Input.GetKeyDown(KeyCode.A)){
			button.localPosition = new Vector3(button.localPosition.x, button.localPosition.y - 0.001f, button.localPosition.z);
			l_grip.localPosition = new Vector3(l_grip.localPosition.x, l_grip.localPosition.y - 0.001f, l_grip.localPosition.z);
			r_grip.localPosition = new Vector3(r_grip.localPosition.x, r_grip.localPosition.y - 0.001f, r_grip.localPosition.z);
			sys_button.localPosition = new Vector3(sys_button.localPosition.x, sys_button.localPosition.y - 0.001f, sys_button.localPosition.z);
		}
		if(Input.GetKeyUp(KeyCode.A)){
			button.localPosition = new Vector3(button.localPosition.x, button.localPosition.y + 0.001f, button.localPosition.z);
			l_grip.localPosition = new Vector3(l_grip.localPosition.x, l_grip.localPosition.y + 0.001f, l_grip.localPosition.z);
			r_grip.localPosition = new Vector3(r_grip.localPosition.x, r_grip.localPosition.y + 0.001f, r_grip.localPosition.z);
			sys_button.localPosition = new Vector3(sys_button.localPosition.x, sys_button.localPosition.y + 0.001f, sys_button.localPosition.z);
		}

		// pip limits at 0.02 from mid
		trackpad.localEulerAngles = new Vector3 (trackpad_touch.localPosition.z * offset, 0, -trackpad_touch.localPosition.x * offset);

		//trigger rot should be between 0 and -18 on the x

		//hand.controller.hairTriggerDelta;

	}

}
