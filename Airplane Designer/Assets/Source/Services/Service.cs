using UnityEngine;
using System.Collections;
using Game.Controllers;
using Controllers;

namespace Services
{
	public static class Service
	{
		private static EventService eventService;
		public static EventService Events
		{
			get
			{
				if (eventService == null)
				{
					eventService = new EventService ();
				}
				return eventService;
			}
			private set{ }
		}

		private static FrameTimeUpdateService frameService;
		public static FrameTimeUpdateService FrameUpdate
		{
			get
			{
				if (frameService == null)
				{
					frameService = new FrameTimeUpdateService ();
				}

				return frameService;
			}
		}

		private static CameraManager cameras;
		public static CameraManager Cameras 
		{
			get
			{
				if (cameras == null)
				{
					cameras = new CameraManager ();
				}

				return cameras;
			}
		}

		private static EntityManager entityManager;
		public static EntityManager EntityManager 
		{
			get
			{
				if (entityManager == null)
				{
					entityManager = new EntityManager ();
				}

				return entityManager;
			}
		}

		private static UndoController actionManager;
		public static UndoController ActionManager 
		{
			get
			{
				if (actionManager == null)
				{
					actionManager = new UndoController ();
				}

				return actionManager;
			}
		}

		private static SelectionController selectionController;
		public static SelectionController SelectionController 
		{
			get
			{
				if (selectionController == null)
				{
					selectionController = new SelectionController ();
				}

				return selectionController;
			}
		}

		private static EditorModeManager editorModeManager;
		public static EditorModeManager EditorModeManager
		{
			get
			{
				if (editorModeManager == null)
				{
					editorModeManager = new EditorModeManager ();
				}

				return editorModeManager;
			}
		}
	}
}