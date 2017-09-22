using UnityEngine;
using System.Collections;
using Services;
using Controllers.Interfaces;
using Events;
using UnityEditor;
using Utils;

public class VertexCreationController : IUpdateObserver
{
	private bool isCreating;

	public VertexCreationController()
	{
		Service.FrameUpdate.RegisterForUpdate (this);
		Service.Events.AddListener (EventId.CreateButtonPressed, OnCreateButtonPressed);
	}

	public void Update(float dt)
	{
		if (isCreating)
		{
			if (Input.GetKey (KeyCode.Return))
			{
				Debug.Log ("Creation completed");
				isCreating = false;
				Service.Events.SendEvent (EventId.CreationComplete, null);
			}

			if (Input.GetMouseButtonDown(0))
			{
				PlaceVertexAction placementAction = new PlaceVertexAction ();

				Vector3 camPos = Service.Cameras.CurrentCamera.transform.position;
				float orthoSize = Service.Cameras.CurrentCamera.orthographicSize;
				float ratio = (2 * orthoSize) / Screen.height;
				float offset = ((Screen.width - Screen.height) / 2f) * ratio;
				Vector3 spawnPos = Vector3.zero;
				spawnPos.x = ratio * Input.mousePosition.x - orthoSize - offset + camPos.x;
				spawnPos.z = ratio * Input.mousePosition.y - orthoSize + camPos.z;

				placementAction.SetArguments (spawnPos);
				Service.ActionManager.ExecuteAction (placementAction);
			}
		}
	}

	private void OnCreateButtonPressed(object cookie)
	{
		isCreating = true;
	}

	public void Dispose()
	{
		Service.Events.RemoveListener (EventId.CreateButtonPressed, OnCreateButtonPressed);
	}
}
