using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Models.Shapes
{
	public class DynamicFlatShape : BaseShape, IShape
	{
		private float height;

		public DynamicFlatShape(List<Vector3> ccwShapePoints, float height)
		{
			this.height = height;
			SetVerts (ccwShapePoints);
		}

		public void SetVerts(List<Vector3> ccwShapePoints)
		{
			int initialNumVerts = ccwShapePoints.Count;
			int finalNumVerts = ccwShapePoints.Count * 2;

			// Set up vertices & normals
			vertices = new Vector3[finalNumVerts];
			normals = new Vector3[finalNumVerts];

			for (int i = 0; i < finalNumVerts; i++)
			{
				bool topHalf = i >= initialNumVerts;
				int vertIndex = (topHalf ? i - initialNumVerts : i);
				vertices [i] = 
					new Vector3 (ccwShapePoints [vertIndex].x, topHalf ? height : 0f, ccwShapePoints [vertIndex].z);

				normals [i] = topHalf ? Vector3.up : Vector3.down;
			}

			GenerateTrisForShape ();
		}

		public Vector3[] GetVertices()
		{
			return vertices;
		}

		public int[] GetTris()
		{
			return tris;
		}

		public Vector3[] GetNormals()
		{
			return normals;
		}
	}
}
