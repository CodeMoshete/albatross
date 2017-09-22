using UnityEngine;
using System.Collections;
using Services;
using Events;
using Controllers.Interfaces;

public class SelectionController : IUpdateObserver
{
	private const float PRESS_SELECT_THRESHOLD = 2.1f;

	public EntityBase CurrentEntity { get; private set; }
	private bool isCreating;
	private bool isPressed;
	private float pressTime;

	public SelectionController()
	{
		Service.Events.AddListener (EventId.CreateButtonPressed, OnCreateButtonPressed);
		Service.Events.AddListener (EventId.CreationComplete, OnCreationComplete);
		Service.FrameUpdate.RegisterForUpdate (this);
	}

	public void OnCreationComplete(object cookie)
	{
		isCreating = false;
	}

	public void OnCreateButtonPressed(object cookie)
	{
		isCreating = true;
		isPressed = false;
		pressTime = 0f;
	}

	public void Update(float dt)
	{
		// If in create mode we take an early out here.
		if (isCreating)
			return;

		if (isPressed)
		{
			pressTime += dt;
		}

		if (Input.GetMouseButtonDown (0))
		{
			isPressed = true;
		}
		else if (Input.GetMouseButtonUp (0))
		{
			isPressed = false;

			if (pressTime <= PRESS_SELECT_THRESHOLD)
			{
				TrySelect ();
				pressTime = 0f;
			}
		}
	}

	private void TrySelect()
	{
		SelectAction selectAction = new SelectAction ();
		RaycastHit hit;
		Camera currentCam = Service.Cameras.CurrentCamera;
		Ray mouseRay = Service.Cameras.CurrentCamera.ScreenPointToRay (Input.mousePosition);
		bool entitySelected = false;
		if (Physics.Raycast (mouseRay, out hit) && hit.transform != null)
		{
			EntityRef entRef = hit.transform.GetComponent<EntityRef> ();
			if (entRef != null && entRef != CurrentEntity)
			{
				selectAction.SetArguments (CurrentEntity, entRef);
				Service.ActionManager.ExecuteAction (selectAction);
				CurrentEntity = entRef.GetEntity ();
				Debug.Log ("Selected entity: " + CurrentEntity.EntityId);
				entitySelected = true;
			}
		}

		if(!entitySelected && CurrentEntity != null)
		{
			selectAction.SetArguments (CurrentEntity, null);
			Service.ActionManager.ExecuteAction (selectAction);
			CurrentEntity = null;
			Debug.Log ("No entity selected");
		}
	}

	public void Unload()
	{
		Service.Events.RemoveListener (EventId.CreateButtonPressed, OnCreateButtonPressed);
		Service.Events.RemoveListener (EventId.CreationComplete, OnCreationComplete);
		Service.FrameUpdate.UnregisterForUpdate (this);
	}
}
