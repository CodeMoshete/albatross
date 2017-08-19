using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Models.Shapes
{
	public class FoamSheetShape : BaseShape, IShape
	{
		public FoamSheetShape(float length, float width, float height)
		{
			// Set up vertices & normals
			vertices = new Vector3[8];
			normals = new Vector3[8];

			// Two height layers
			for (int i = 0; i < 2; i++)
			{
				// Four corners in counter-clockwise formation
				for (int j = 0; j < 4; j++)
				{
					int index = 4 * i + j;
					vertices [index] = new Vector3 (
						(j > 0 && j < 3) ? width : 0f, 
						i * height, 
						j > 1 ? length : 0f);

					normals [index] = i < 1 ? Vector3.down : Vector3.up;
				}
			}

			GenerateTrisForShape ();
		}

		public void SetVerts(List<Vector3> ccwShapePoints)
		{
			// Do nothing for this shape
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
