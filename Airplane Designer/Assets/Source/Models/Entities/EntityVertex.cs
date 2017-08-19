using UnityEngine;
using System.Collections;

public class EntityVertex : EntityBase
{
	public EntityVertex(Vector3 spawnPosition)
	{
		BaseObject = GameObject.Instantiate((GameObject)Resources.Load ("Models/Vertex"));
		BaseObject.transform.position = spawnPosition;
	}
}
