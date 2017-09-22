using UnityEngine;
using System.Collections;
using Services;

public class PlaceVertexAction : IEditorAction
{
	private Vector3 spawnPosition;
	private EntityVertex entity;
	private string transformId;

	public void SetArguments(Vector3 spawnPosition)
	{
		this.spawnPosition = spawnPosition;
	}

	public void ApplyAction()
	{
		entity = new EntityVertex (spawnPosition);
	}

	public void ReverseAction()
	{
		if (entity != null)
		{
			entity.Destroy ();
		}
	}
}
