using UnityEngine;
using System.Collections;

public class EntityVertex : EntityBase
{
	public EntityShape ParentShape { get; private set; }

	public EntityVertex(Vector3 spawnPosition) : base()
	{
		BaseObject = GameObject.Instantiate((GameObject)Resources.Load ("Models/Vertex"));
		BaseObject.transform.position = spawnPosition;

		EntityRef entRef = BaseObject.AddComponent<EntityRef> ();
		entRef.Initialize (EntityId);
	}
}
