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

		private static DebugCameraController cameras;
		public static DebugCameraController Cameras 
		{
			get
			{
				if (cameras == null)
				{
					cameras = new DebugCameraController ();
				}

				return cameras;
			}
		}
	}
}