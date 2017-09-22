using UnityEngine;
using Controllers;
using System.Collections;
using Services;
using Events;
using Utils;

public class DraftModeController : IEditorModeController 
{
	// Manages the creation on vertexes in the editor.
	private VertexCreationController vertexCreationController;

	private DraftPanel uiPanel;

	public void Activate()
	{
		Service.Cameras.SwitchToCamera (CameraType.Drafting);
		vertexCreationController = new VertexCreationController ();
		GameObject mainUI = GameObject.Find ("GUI_EditorInterface(Clone)");
		uiPanel = new DraftPanel(EditorUtils.FindGameObject (mainUI, "DraftPanel"));
	}

	public void Deactivate()
	{
		vertexCreationController.Dispose ();
		vertexCreationController = null;
	}
}
