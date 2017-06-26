//EditorSceneController.cs
//Controller class for the main menu. Essentially a game state.

using System;
using Controllers.Interfaces;
using Game.Controllers.Interfaces;
using Services;
using Monobehaviors;
using UnityEngine;
using UnityEngine.UI;
using Models.Shapes;

namespace Controllers.Scenes
{
	public class EditorSceneController : IStateController, IUpdateObserver
	{
		private DebugCameraController debugCam;

		private GameObject dynamicMeshObj;
		private DynamicMesh dynamicMesh;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
			EditorStateParams loadParams = (EditorStateParams)passedParams;
			debugCam = Service.Cameras;
			onLoadedCallback();
		}

		public void Start()
		{
			Debug.Log ("Editor Scene Loaded");
			Service.FrameUpdate.RegisterForUpdate (this);
			RenderTestMesh ();
		}
				
		private void RenderTestMesh()
		{
			FoamSheetShape foamSheet = new FoamSheetShape (30f, 20f, 0.25f);

			dynamicMeshObj = new GameObject ();
			dynamicMeshObj.name = "DynamicMesh";
			dynamicMeshObj.AddComponent<MeshRenderer> ();
			dynamicMeshObj.AddComponent<MeshFilter> ();
			dynamicMesh = dynamicMeshObj.AddComponent<DynamicMesh> ();
			dynamicMesh.Render (foamSheet.GetVertices (), foamSheet.GetTris (), foamSheet.GetNormals ());

			Material newMat = Resources.Load("Materials/DynamicMeshMaterial", typeof(Material)) as Material;
			dynamicMeshObj.GetComponent<Renderer> ().material = newMat;
		}

		public void Update(float dt)
		{
			//Put update code in here.
		}

		public void Unload()
		{
			Service.FrameUpdate.UnregisterForUpdate(this);
		}
	}
}

