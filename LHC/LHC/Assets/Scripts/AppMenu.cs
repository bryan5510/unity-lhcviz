using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	[RequireComponent (typeof(Interactable))]
	public class AppMenu : MonoBehaviour
	{

		GameObject canvas;
		Transform canvasHolder;

		void Awake ()
		{
			canvas = GameObject.Find ("Canvas");
			canvasHolder = GameObject.Find ("CanvasHolder").transform;
		}

		void HandAttachedUpdate (Hand hand)
		{
			if (hand.controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) {
				ShowAppMenu ();
			}
		}

		void ShowAppMenu ()
		{
			if (canvas.activeSelf == false) {
				canvas.transform.position = canvasHolder.position;
				canvas.transform.eulerAngles = new Vector3 (canvasHolder.eulerAngles.x, canvasHolder.eulerAngles.y, 0);
			}
			canvas.SetActive (!canvas.activeSelf);
		}
	}
}