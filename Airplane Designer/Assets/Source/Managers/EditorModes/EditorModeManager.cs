using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EditorMode
{
	None,
	Build,
	Draft
}

public class EditorModeManager 
{
	private EditorMode currentMode;
	private IDictionary<EditorMode, IEditorModeController> modeControllers;
	private IEditorModeController currentController;

	public EditorModeManager()
	{
		modeControllers = new Dictionary<EditorMode, IEditorModeController> ();
		modeControllers.Add (EditorMode.Build, new BuildModeController ());
		modeControllers.Add (EditorMode.Draft, new DraftModeController ());
	}

	public void SwitchToMode(EditorMode editorType)
	{
		if (editorType != currentMode)
		{
			if (currentController != null)
			{
				currentController.Deactivate ();
			}
			currentController = modeControllers [editorType];
			currentController.Activate ();
			currentMode = editorType;
		}
	}
}
