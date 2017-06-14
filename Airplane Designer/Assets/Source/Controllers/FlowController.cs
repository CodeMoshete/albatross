//FlowController
//Controller class that strings any and all game states together.

using System;
using System.Collections.Generic;
using Controllers;
using Game.Factories;
using Controllers.Scenes;

namespace Game.Controllers
{
	public class FlowController
	{
		private StateFactory sceneFactory;

		public FlowController()
		{
			sceneFactory = new StateFactory();
		}

		public void StartGame()
		{
			LoadEditor();
		}

		public void LoadEditor()
		{
			EditorStateParams passedParams = new EditorStateParams();
			sceneFactory.LoadScene<EditorSceneController>(OnSceneLoaded, passedParams);
		}

		public void OnSceneLoaded()
		{
			// Intentionally empty for now...
		}
	}
}

