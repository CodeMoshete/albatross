using UnityEngine;
using System.Collections;
using TriangleNet.Geometry;
using System.Collections.Generic;

namespace Models.Shapes
{
	public class BaseShape
	{
		protected Vector3[] vertices;
		protected Vector3[] normals;
		protected int[] tris;

		protected void GenerateTrisForShape()
		{
			if (vertices.Length >= 6 && vertices.Length % 2 == 0)
			{
				InputGeometry bottomGeometry = new InputGeometry();
				List<Point> shape = new List<Point> ();
				int halfVertLen = vertices.Length / 2;
				for(int i = 0; i < halfVertLen; i++)
				{
					shape.Add (new Point (vertices[i].x, vertices[i].z));
				}
				bottomGeometry.AddRing (shape);

				TriangleNet.Mesh bottomMesh = new TriangleNet.Mesh ();
				bottomMesh.Triangulate (bottomGeometry);

				List<Vector3> verts = new List<Vector3>(bottomMesh.triangles.Count * 3);
				List<int> triangleIndices = new List<int>(bottomMesh.triangles.Count * 3);
				foreach (KeyValuePair<int, TriangleNet.Data.Triangle> pair in bottomMesh.triangles)
				{
					TriangleNet.Data.Triangle triangle = pair.Value;

					TriangleNet.Data.Vertex vertex0 = triangle.GetVertex(0);
					TriangleNet.Data.Vertex vertex1 = triangle.GetVertex(1);
					TriangleNet.Data.Vertex vertex2 = triangle.GetVertex(2);

					Vector3 p0 = new Vector3( vertex0.x, 0, vertex0.y);
					Vector3 p1 = new Vector3( vertex1.x, 0, vertex1.y);
					Vector3 p2 = new Vector3( vertex2.x, 0, vertex2.y);

					verts.Add(p0);
					verts.Add(p1);
					verts.Add(p2);

					triangleIndices.Add (GetIndexForVertex (p0));
					triangleIndices.Add (GetIndexForVertex (p1));
					triangleIndices.Add (GetIndexForVertex (p2));
				}

				List<int> triangleIndecesReversed = triangleIndices;

				tris = triangleIndices.ToArray();
			}
			else
			{
				Debug.LogError ("[BaseShape.GenerateTrisForShape] Invalid vertices set for shape!");
			}
		}

		private int GetIndexForVertex(Vector3 vertex)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				if (vertex.x == vertices [i].x && vertex.z == vertices [i].z)
				{
					return i;
				}
			}

			return -1;
		}
	}
}
