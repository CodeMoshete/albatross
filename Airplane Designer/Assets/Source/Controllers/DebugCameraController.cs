using System;
using System.Collections.Generic;
using Controllers.Interfaces;
using Events;
using Cameras.ControlSystems;
using Services;
using UnityEngine;

namespace Controllers
{
	public class DebugCameraController: IUpdateObserver
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
		private Camera debugFarCamera;
		private DebugCameraControls debugControls;
		private DebugCameraControls debugFarControls;
		private Transform currentHoverObject;

		public DebugCameraController ()
		{
			// Near clip camera
			GameObject camObject = new GameObject(DEBUG_CAMERA_NAME);
			Camera = camObject.AddComponent<Camera>();
			Camera.clearFlags = CameraClearFlags.Nothing;
			Camera.depth = 1;
			//debugCamera.cullingMask = 1 << ;
			debugControls = new DebugCameraControls ();
			debugControls.Initialize (Camera);

			// Facr clip camera
			GameObject farCamObject = new GameObject(DEBUG_CAMERA_FAR_NAME);
			debugFarCamera = farCamObject.AddComponent<Camera>();
			debugFarCamera.nearClipPlane = 0.01f;
			debugFarControls = new DebugCameraControls ();
			debugFarControls.Initialize (debugFarCamera, 0.001f);

			Service.FrameUpdate.RegisterForUpdate (this);
		}

		public void Update(float dt)
		{
			debugControls.Update (dt);
			debugFarControls.Update (dt);
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
	}
}

