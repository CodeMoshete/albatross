using UnityEngine;
using System.Collections;
using Services;
using System.Collections.Generic;

public class DeselectAction: IEditorAction 
{
	private EntityBase deselectedEntity;

	public void SetArguments(EntityBase deselectedEntity)
	{
		this.deselectedEntity = deselectedEntity;
	}

	public void ApplyAction()
	{
		Service.SelectionController.DeselectEntity (deselectedEntity, null);
	}

	public void ReverseAction()
	{
		// This action only happens when an entity is deselected from a group
		Service.SelectionController.SelectEntity (deselectedEntity, true);
	}
}
