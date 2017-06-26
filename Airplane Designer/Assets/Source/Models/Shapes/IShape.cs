//IShape.cs
//Interface class defining the common characteristics of a Shape.

using UnityEngine;

namespace Models.Shapes
{
	public interface IShape
	{
		Vector3[] GetVertices();
		int[] GetTris();
		Vector3[] GetNormals();
	}
}
