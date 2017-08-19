using UnityEngine;
using System.Collections;

public class MoveAction : IEditorAction
{
	private Vector3 startPosition;
	private Vector3 endPosition;
	private string transformId;

	public void SetArguments(Vector3 startPosition, Vector3 endPosition, string transformId)
	{
		this.startPosition = startPosition;
		this.endPosition = endPosition;
		this.transformId = transformId;
	}

	public void ApplyAction()
	{

	}

	public void ReverseAction()
	{

	}
}
