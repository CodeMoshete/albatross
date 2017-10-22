//EditorSceneController.cs
//Controller class for the main menu. Essentially a game state.

using System;
using System.Collections.Generic;
using Controllers.Interfaces;
using Game.Controllers.Interfaces;
using Services;
using Monobehaviors;
using UnityEngine;
using UnityEngine.UI;
using Models.Shapes;
using Controllers.UI;

namespace Controllers.Scenes
{
	// Main scene class for the program. Encapsulates the Build and Draft editor modes.
	public class EditorSceneController : IStateController, IUpdateObserver
	{

		private GameObject dynamicMeshObj;
		private DynamicMesh dynamicMesh;

		private EditorInterfaceController interfaceController;
		private EditorModeManager modeManager;

		private SelectionController selectionController;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
			EditorStateParams loadParams = (EditorStateParams)passedParams;
			modeManager = Service.EditorModeManager;
			modeManager.SwitchToMode (EditorMode.Build);
			onLoadedCallback();
		}

		public void Start()
		{
			Debug.Log ("Editor Scene Loaded");
			Service.FrameUpdate.RegisterForUpdate (this);
			interfaceController = new EditorInterfaceController (OnBuildPressed, OnDraftPressed);
			RenderTestMesh ();
			selectionController = Service.SelectionController;
		}
				
		private void RenderTestMesh()
		{
//			FoamSheetShape foamSheet = new FoamSheetShape (30f, 20f, 0.25f);

			List<Vector3> sheetPoints = new List<Vector3> ();
			sheetPoints.Add(new Vector3(-5f, 0f, -5f));
			sheetPoints.Add(new Vector3(-3f, 0f, -7f));
			sheetPoints.Add(new Vector3(-0f, 0f, -2f));
			sheetPoints.Add(new Vector3(3f, 0f, -7f));
			sheetPoints.Add(new Vector3(5f, 0f, -5f));
			sheetPoints.Add(new Vector3(5f, 0f, 5f));
			sheetPoints.Add(new Vector3(3f, 0f, 7f));
			sheetPoints.Add(new Vector3(0f, 0f, 2f));
			sheetPoints.Add(new Vector3(-3f, 0f, 7f));
			sheetPoints.Add(new Vector3(-5f, 0f, 5f));

			DynamicFlatShape foamSheet = new DynamicFlatShape(sheetPoints, 0.5f);

			dynamicMeshObj = new GameObject ();
			dynamicMeshObj.name = "DynamicMesh";
			dynamicMeshObj.AddComponent<MeshRenderer> ();
			dynamicMeshObj.AddComponent<MeshFilter> ();
			dynamicMesh = dynamicMeshObj.AddComponent<DynamicMesh> ();
			dynamicMesh.Render (foamSheet.GetVertices (), foamSheet.GetTris (), foamSheet.GetNormals ());

			Material newMat = Resources.Load("Materials/DynamicMeshMaterial", typeof(Material)) as Material;
			dynamicMeshObj.GetComponent<Renderer> ().material = newMat;
		}

		private void OnBuildPressed()
		{
			ChangeEditorModeAction changeModeAction = new ChangeEditorModeAction ();
			changeModeAction.SetArguments (EditorMode.Build, Service.EditorModeManager.CurrentMode);
			Service.ActionManager.ExecuteAction (changeModeAction);
		}

		private void OnDraftPressed()
		{
			ChangeEditorModeAction changeModeAction = new ChangeEditorModeAction ();
			changeModeAction.SetArguments (EditorMode.Draft, Service.EditorModeManager.CurrentMode);
			Service.ActionManager.ExecuteAction (changeModeAction);
		}

		public void Update(float dt)
		{
			//Put update code in here.
		}

		public void Unload()
		{
			Service.FrameUpdate.UnregisterForUpdate(this);
			selectionController = null;
		}
	}
}

