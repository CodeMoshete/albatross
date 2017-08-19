using UnityEngine;
using Controllers;
using System.Collections;
using Services;

public class DraftModeController : IEditorModeController 
{
	public void Activate()
	{
		Service.Cameras.SwitchToCamera (CameraType.Drafting);
	}

	public void Deactivate()
	{
	}
}
