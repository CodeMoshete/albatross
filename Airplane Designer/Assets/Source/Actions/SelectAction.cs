using UnityEngine;
using System.Collections;

public class SelectAction: IEditorAction 
{
	private EntityBase previousEntity;
	private EntityBase selectedEntity;

	public void SetArguments(EntityBase previousEntity, EntityBase selectedEntity)
	{
		this.previousEntity = previousEntity;
		this.selectedEntity = selectedEntity;
	}

	public void ApplyAction()
	{

	}

	public void ReverseAction()
	{

	}
}
