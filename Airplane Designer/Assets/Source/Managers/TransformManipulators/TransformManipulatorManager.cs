using UnityEngine;
using System.Collections;
using Services;
using Events;
using System.Collections.Generic;
using Components;
using Controllers.Interfaces;

public class TransformManipulatorManager : IUpdateObserver
{
	private RotationHandle rotationHandle;
	private TranslationHandle translationHandle;
	private IHandleGizmo currentGizmo;
	private Transform multiHolder;

	public TransformManipulatorManager()
	{
		Service.Events.AddListener (EventId.EntitiesSelected, OnEntitiesSelected);
		rotationHandle = new RotationHandle ();
		rotationHandle.Start ();
		translationHandle = new TranslationHandle ();
		translationHandle.Start ();
		Service.FrameUpdate.RegisterForUpdate (this);
		GameObject multiHolderGameObject = new GameObject ();
		multiHolder = multiHolderGameObject.transform;
		currentGizmo = translationHandle;
	}

	public void OnEntitiesSelected(object cookie)
	{
		List<EntityBase> selectedEntities = (List<EntityBase>)cookie;
		multiHolder.position = GetEntityCenterpoint (selectedEntities);
		bool wasActive = selectedEntities.Count > 0;
		currentGizmo.SetGizmoOnTarget (multiHolder);
		currentGizmo.SetActive (wasActive);
	}

	private Vector3 GetEntityCenterpoint(List<EntityBase> entities)
	{
		Vector3 retVal = Vector3.zero;
		int numEntities = entities.Count;
		for (int i = 0; i < numEntities; i++)
		{
			retVal += entities [i].BaseObject.transform.position;
		}

		if (numEntities > 0)
		{
			retVal.x /= numEntities;
			retVal.y /= numEntities;
			retVal.z /= numEntities;
		}

		return retVal;
	}

	public void Update(float dt)
	{
		if (Input.GetKeyUp (KeyCode.W))
		{
			OnGizmoSwitched (translationHandle);
		}
		else if (Input.GetKeyUp (KeyCode.E))
		{
			OnGizmoSwitched (rotationHandle);
		}
	}

	public void OnGizmoSwitched(IHandleGizmo nextGizmo)
	{
		bool wasActive = currentGizmo.IsActive();
		currentGizmo.SetActive (false);
		currentGizmo = nextGizmo;
		currentGizmo.SetGizmoOnTarget (multiHolder);
		currentGizmo.SetActive (wasActive);
	}

	public void Unload()
	{
		Service.Events.RemoveListener (EventId.EntitiesSelected, OnEntitiesSelected);
		Service.FrameUpdate.UnregisterForUpdate (this);
	}
}
