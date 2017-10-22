using UnityEngine;
using System.Collections;
using Services;
using Events;
using Controllers.Interfaces;
using System.Collections.Generic;

public class SelectionController : IUpdateObserver
{
	private const float PRESS_SELECT_THRESHOLD = 2.1f;

	public List<EntityBase> CurrentEntities { get; private set; }
	private bool isCreating;
	private bool isPressed;
	private float pressTime;

	public SelectionController()
	{
		CurrentEntities = new List<EntityBase> ();
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
				bool applyToCurrent = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
				TrySelect (applyToCurrent);
				pressTime = 0f;
			}
		}
	}

	private void TrySelect(bool applyToCurrent)
	{
		RaycastHit hit;
		Camera currentCam = Service.Cameras.CurrentCamera;
		Ray mouseRay = Service.Cameras.CurrentCamera.ScreenPointToRay (Input.mousePosition);
		bool entitySelected = false;
		if (Physics.Raycast (mouseRay, out hit) && hit.transform != null)
		{
			EntityRef entRef = hit.transform.GetComponent<EntityRef> ();
			if (entRef != null && !CurrentEntities.Contains (entRef.GetEntity ()))
			{
				SelectAction selectAction = new SelectAction ();
				selectAction.SetArguments (CurrentEntities, entRef.GetEntity (), applyToCurrent);
				Service.ActionManager.ExecuteAction (selectAction);
				entitySelected = true;
			}
			else if (applyToCurrent &&
				entRef != null && 
				CurrentEntities.Count > 1 && 
				CurrentEntities.Contains (entRef.GetEntity ()))
			{
				DeselectAction deselectAction = new DeselectAction ();
				deselectAction.SetArguments (entRef.GetEntity ());
				Service.ActionManager.ExecuteAction (deselectAction);
				entitySelected = true;
			}
			else if(entRef != null && CurrentEntities.Count > 1 && CurrentEntities.Contains (entRef.GetEntity ()))
			{
				// If the user selects a single element that is already selected as part of a group, do nothing.
				entitySelected = true;
			}
		}

		// No entity clicked on. Deselect all entities.
		if(!entitySelected && CurrentEntities.Count > 0)
		{
			SelectAction selectAction = new SelectAction ();
			selectAction.SetArguments (CurrentEntities, null, false);
			Service.ActionManager.ExecuteAction (selectAction);
			CurrentEntities.Clear();
		}
	}

	public void SelectEntity(EntityBase entity, bool applyToCurrent)
	{
		// We're just selecting a new object
		if (!applyToCurrent)
		{
			CurrentEntities.Clear ();
		}

		if (entity != null)
		{
			CurrentEntities.Add (entity);
		}

		LogCurrentEntities ();
	}

	public void DeselectEntity(EntityBase entity, List<EntityBase> previouslySelected)
	{
		if (CurrentEntities.Contains (entity))
		{
			CurrentEntities.Remove (entity);
		}

		if (previouslySelected != null)
		{
			CurrentEntities.AddRange (previouslySelected);
		}

		LogCurrentEntities ();
	}

	public void DeselectAll()
	{
		CurrentEntities.Clear ();
	}

	public void SetSelectedGroup(List<EntityBase> selectedEntities)
	{
		CurrentEntities.Clear ();
		CurrentEntities = selectedEntities;
	}

	private void LogCurrentEntities()
	{
		string entityNames = "";
		for (int i = 0, ct = CurrentEntities.Count; i < ct; i++)
		{
			entityNames += ": EntId_" + CurrentEntities [i].EntityId + ", ";
		}
		Debug.Log ("Selected entities: " + entityNames);
	}

	public void Unload()
	{
		Service.Events.RemoveListener (EventId.CreateButtonPressed, OnCreateButtonPressed);
		Service.Events.RemoveListener (EventId.CreationComplete, OnCreationComplete);
		Service.FrameUpdate.UnregisterForUpdate (this);
	}
}
