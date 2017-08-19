//DynamicMesh.cs
//Description: An instance of this represents a single dynamic mesh piece in the scene.

using UnityEngine;
using System.Collections;
using Controllers;
using Game.Controllers;
using Services;
using Events;

namespace Monobehaviors
{
	public class DynamicMesh : MonoBehaviour
	{
		public void Render(Vector3[] vertices, int[] tri, Vector3[] normals)
		{
			MeshFilter mf = GetComponent<MeshFilter> ();
			var mesh = new Mesh ();
			mesh.vertices = vertices;
			mesh.triangles = tri;
			mesh.normals = normals;
			mf.mesh = mesh;
		}
	}
}
