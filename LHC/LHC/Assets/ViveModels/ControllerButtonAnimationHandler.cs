using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerButtonAnimationHandler : MonoBehaviour {/*
	uint index;
	Dictionary<int, string> nameCache;

	void HandAttachedUpdate(Hand hand){
		index = hand.controller.index;
		UpdateCustomComponents ();
	}

	void UpdateCustomComponents()
	{
		var t = transform;
		if (t.childCount == 0)
			return;

		var controllerState = (index != SteamVR_TrackedObject.EIndex.None) ?
			SteamVR_Controller.Input((int)index).GetState() : new VRControllerState_t();

		if (nameCache == null)
			nameCache = new Dictionary<int, string>();

		for (int i = 0; i < t.childCount; i++)
		{
			var child = t.GetChild(i);

			// Cache names since accessing an object's name allocate memory.
			string name;
			if (!nameCache.TryGetValue(child.GetInstanceID(), out name))
			{
				name = child.name;
				nameCache.Add(child.GetInstanceID(), name);
			}

			var componentState = new RenderModel_ComponentState_t();

			var componentTransform = new SteamVR_Utils.RigidTransform(componentState.mTrackingToComponentRenderModel);
			child.localPosition = componentTransform.pos;
			child.localRotation = componentTransform.rot;

			var attach = child.FindChild("attach");
			if (attach != null)
			{
				var attachTransform = new SteamVR_Utils.RigidTransform(componentState.mTrackingToComponentLocal);
				attach.position = t.TransformPoint(attachTransform.pos);
				attach.rotation = t.rotation * attachTransform.rot;
			}

			bool visible = (componentState.uProperties & (uint)EVRComponentProperty.IsVisible) != 0;
			if (visible != child.gameObject.activeSelf)
			{
				child.gameObject.SetActive(visible);
			}
		}
	}*/
}
