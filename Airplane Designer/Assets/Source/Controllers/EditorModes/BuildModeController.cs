using UnityEngine;
using System.Collections;
using Controllers;
using Services;

public class BuildModeController : IEditorModeController
{
	public void Activate()
	{
		Service.Cameras.SwitchToCamera (CameraType.Building);
	}

	public void Deactivate()
	{
	}
}
