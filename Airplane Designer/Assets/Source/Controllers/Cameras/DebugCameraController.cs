using System;
using System.Collections.Generic;
using Controllers.Interfaces;
using Events;
using Cameras.ControlSystems;
using Services;
using UnityEngine;

namespace Controllers
{
	public class DebugCameraController: ICameraController, IUpdateObserver
	{
		public const string DEBUG_CAMERA_NAME = "DebugCamera";
		public const string DEBUG_CAMERA_FAR_NAME = "DebugCameraFar";

		private readonly List<string> GAME_CAMERA_LAYERS = new List<string> (new string[] { 
			"Default",
			"TransparentFX",
			"Ignore Raycast",
			"UI",
			"Weapons",
			"Collidable",
			"VRUI"
		});

		private readonly List<string> BACKGROUND_CAMERA_LAYERS = new List<string> (new string[] { 
			"BackgroundElements"
		});

		private bool debugCameraOn;
		public Camera Camera;
		private DebugCameraControls debugControls;
		private Transform currentHoverObject;

		public DebugCameraController()
		{
			// Near clip camera
			GameObject camObject = new GameObject(DEBUG_CAMERA_NAME);
			Camera = camObject.AddComponent<Camera>();
			Camera.depth = 1;
			debugControls = new DebugCameraControls ();
			debugControls.Initialize (Camera);
			Deactivate ();
		}

		public void Activate()
		{
			Service.FrameUpdate.RegisterForUpdate (this);
			Camera.gameObject.SetActive (true);
		}

		public void Deactivate()
		{
			Service.FrameUpdate.UnregisterForUpdate (this);
			Camera.gameObject.SetActive (false);
		}

		public void Update(float dt)
		{
			debugControls.Update (dt);
			Ray mouseRay = Camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			bool didRaycastHit = Physics.Raycast (mouseRay, out hit);
			if (didRaycastHit && hit.transform != currentHoverObject)
			{
				currentHoverObject = hit.transform;
			}
			else if(!didRaycastHit && currentHoverObject != null)
			{
				currentHoverObject = null;
			}
		}

		public Camera GetCamera()
		{
			return Camera;
		}
	}
}

