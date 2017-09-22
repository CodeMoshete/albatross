using UnityEngine;
using System.Collections;
using Services;

public class EntityRef : MonoBehaviour 
{
	private long entityId;

	public void Initialize(long entityId)
	{
		this.entityId = entityId;
		MeshCollider collider = gameObject.AddComponent<MeshCollider> ();
	}

	public EntityBase GetEntity()
	{
		return Service.EntityManager.GetEntity (entityId);
	}
}
