﻿//DynamicMesh.cs
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
		private const float WIDTH = 20f;
		private const float LENGTH = 30f;
		private const float HEIGHT = 0.25f;

		public void Render(Vector3[] vertices, int[] tri, Vector3[] normals)
		{
			MeshFilter mf = GetComponent<MeshFilter> ();
			var mesh = new Mesh ();

//			Vector3[] vertices = new Vector3[4];

			// Bottom side
//			vertices[0] = new Vector3(0, 0, 0);
//			vertices[1] = new Vector3(WIDTH, 0, 0);
//			vertices[2] = new Vector3(WIDTH, 0, LENGTH);
//			vertices[3] = new Vector3(0, 0, LENGTH);

			mesh.vertices = vertices;

//			int[] tri = new int[6];

//			tri[0] = 1;
//			tri[1] = 0;
//			tri[2] = 2;
//
//			tri[3] = 0;
//			tri[4] = 3;
//			tri[5] = 2;

			mesh.triangles = tri;

//			Vector3[] normals = new Vector3[4];

//			normals[0] = Vector3.up;
//			normals[1] = Vector3.up;
//			normals[2] = Vector3.up;
//			normals[3] = Vector3.up;

			mesh.normals = normals;
			mf.mesh = mesh;
		}
	}
}
