//IShape.cs
//Interface class defining the common characteristics of a Shape.

using UnityEngine;
using System.Collections.Generic;

namespace Models.Shapes
{
	public interface IShape
	{
		Vector3[] GetVertices();
		int[] GetTris();
		Vector3[] GetNormals();
		void SetVerts (List<Vector3> ccwShapePoints);
	}
}
