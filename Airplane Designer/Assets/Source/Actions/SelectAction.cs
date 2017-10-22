using UnityEngine;
using System.Collections;
using Services;
using System.Collections.Generic;

public class SelectAction: IEditorAction 
{
	private List<EntityBase> previousEntities;
	private EntityBase selectedEntity;
	private bool applyToCurrent;

	public SelectAction()
	{
		previousEntities = new List<EntityBase> ();
	}

	public void SetArguments(List<EntityBase> previousEntities, EntityBase selectedEntity, bool applyToCurrent)
	{
		this.previousEntities = previousEntities;
		this.selectedEntity = selectedEntity;
		this.applyToCurrent = applyToCurrent;
	}

	public void ApplyAction()
	{
		Service.SelectionController.SelectEntity (selectedEntity, applyToCurrent);
	}

	public void ReverseAction()
	{
		Service.SelectionController.DeselectEntity (selectedEntity, previousEntities);
	}
}
