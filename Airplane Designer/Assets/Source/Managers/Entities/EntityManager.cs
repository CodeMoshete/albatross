using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityManager 
{
	private IDictionary<long, EntityBase> entities;
	private long currentIdIndex;

	public EntityManager()
	{
		entities = new Dictionary<long, EntityBase> ();
	}

	public EntityBase GetEntity(long id)
	{
		EntityBase entity = null;
		if (entities.ContainsKey (id))
		{
			entity = entities [id];
		}
		return entity;
	}

	public void AddEntity(EntityBase entity)
	{
		currentIdIndex++;
		entities.Add (currentIdIndex, entity);
	}

	public void RemoveEntity(EntityBase entity)
	{
		if (entities.ContainsKey (entity.EntityId))
		{
			entities.Remove (entity.EntityId);
		}
		else
		{
			Debug.LogError ("[EntityManager.RemoveEntity] ID " + entity.EntityId + " does not exist in Dictionary!");
		}
	}
}
