using UnityEngine;
using System.Collections;
using Services;
using System.Collections.Generic;

public class ChangeEditorModeAction: IEditorAction 
{
	private EditorMode newMode;
	private EditorMode previousMode;
	private List<EntityBase> previouslySelectedEntities;

	public void SetArguments(EditorMode newMode, EditorMode previousMode)
	{
		this.newMode = newMode;
		this.previousMode = previousMode;
	}

	public void ApplyAction()
	{
		Service.EditorModeManager.SwitchToMode (newMode);
		previouslySelectedEntities = Service.SelectionController.CurrentEntities;
		Service.SelectionController.DeselectAll ();
	}

	public void ReverseAction()
	{
		Service.EditorModeManager.SwitchToMode (previousMode);
	}
}
