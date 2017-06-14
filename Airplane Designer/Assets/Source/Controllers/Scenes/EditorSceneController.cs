//EditorSceneController.cs
//Controller class for the main menu. Essentially a game state.

using System;
using Controllers.Interfaces;
using Game.Controllers.Interfaces;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Scenes
{
	public class EditorSceneController : IStateController, IUpdateObserver
	{
		private DebugCameraController debugCam;

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
		}
				
		private void RenderTestMesh()
		{
			
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

