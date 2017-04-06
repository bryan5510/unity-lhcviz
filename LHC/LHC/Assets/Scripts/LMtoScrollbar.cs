using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
	[RequireComponent (typeof(Interactable))]
	[RequireComponent (typeof(LinearMapping))]
	public class LMtoScrollbar : MonoBehaviour
	{
		LinearMapping lm;
		Scrollbar sb;

		void Awake(){
			lm = this.GetComponent<LinearMapping> ();
			sb = this.GetComponentInParent<Scrollbar> ();
		}

		void FixedUpdate(){
			sb.value = 1 - lm.value;
		}

	}
}