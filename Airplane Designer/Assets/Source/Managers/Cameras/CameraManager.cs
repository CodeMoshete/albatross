using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Controllers;

public enum CameraType
{
	Building,
	Drafting
}

public class CameraManager
{
	private IDictionary<CameraType, ICameraController> Controllers;
	private ICameraController currentController;

	public CameraManager()
	{
		Controllers = new Dictionary<CameraType, ICameraController> ();
		Controllers.Add (CameraType.Building, new DebugCameraController ());
		Controllers.Add (CameraType.Drafting, new DraftingCameraController ());
	}

	public void SwitchToCamera(CameraType cameraType)
	{
		if (currentController != null)
		{
			currentController.Deactivate ();
		}
		currentController = Controllers [cameraType];
		currentController.Activate ();
	}

	public Camera CurrentCamera
	{
		get
		{
			Camera currentCamera = null;
			if (currentController != null)
			{
				currentCamera = currentController.GetCamera ();
			}
			return currentCamera;
		}
	}
}
