using UnityEngine;
using System.Collections;

public class DraftingCameraControls
{
	private Vector3 oldMousePos;
	private Transform transform;
	private bool isDragging;

	public void Initialize(Camera cam)
	{
		transform = cam.transform;
	}

	public void Update(float dt)
	{
		if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl))
		{
			Debug.Log ("Dragging");
		}
	}
}
