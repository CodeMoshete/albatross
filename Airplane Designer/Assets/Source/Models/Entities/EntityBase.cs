using UnityEngine;
using System.Collections;

public class EntityBase 
{
	public GameObject BaseObject { get; protected set; }
	public long EntityId { get; private set; }

	public virtual void Destroy()
	{
		if (BaseObject != null)
		{
			GameObject.Destroy (BaseObject);
			BaseObject = null;
			EntityId = -1;
		}
	}
}
