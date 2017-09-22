using UnityEngine;
using System.Collections;
using Services;

public class MoveAction : IEditorAction
{
	private Vector3 startPosition;
	private Vector3 endPosition;
	private long transformId;

	public void SetArguments(Vector3 startPosition, Vector3 endPosition, long transformId)
	{
		this.startPosition = startPosition;
		this.endPosition = endPosition;
		this.transformId = transformId;
	}

	public void ApplyAction()
	{
		Service.EntityManager.GetEntity (transformId).BaseObject.transform.transform.position = endPosition;
	}

	public void ReverseAction()
	{
		Service.EntityManager.GetEntity (transformId).BaseObject.transform.transform.position = startPosition;
	}
}
