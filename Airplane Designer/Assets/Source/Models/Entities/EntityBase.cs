using UnityEngine;
using System.Collections;
using Services;

public class EntityBase 
{
	public GameObject BaseObject { get; protected set; }
	public long EntityId { get; private set; }

	public EntityBase()
	{
		EntityId = Service.EntityManager.AddEntity(this);
	}

	public virtual void Destroy()
	{
		if (BaseObject != null)
		{
			Service.EntityManager.RemoveEntity (this);
			GameObject.Destroy (BaseObject);
			BaseObject = null;
			EntityId = -1;
		}
	}
}
