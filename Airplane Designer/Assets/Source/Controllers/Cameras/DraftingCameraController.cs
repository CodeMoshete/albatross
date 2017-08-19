using UnityEngine;
using System.Collections;
using Services;
using Controllers.Interfaces;

namespace Controllers
{
	public class DraftingCameraController : ICameraController, IUpdateObserver 
	{
		private const string CAMERA_NAME = "DraftingCamera";
		private readonly Vector3 INITIAL_ANGLES = new Vector3 (90f, 0f, 0f);
		private readonly Vector3 INITIAL_POSITION = new Vector3 (0f, 100f, 0f);
		private Camera camera;

		public DraftingCameraController()
		{
			GameObject cameraObj = new GameObject (CAMERA_NAME);
			camera = cameraObj.AddComponent<Camera> ();
			camera.depth = 1;
			camera.transform.eulerAngles = INITIAL_ANGLES;
			camera.transform.position = INITIAL_POSITION;
			camera.orthographic = true;
			Deactivate ();
		}

		public void Activate()
		{
			camera.gameObject.SetActive (true);
			Service.FrameUpdate.RegisterForUpdate (this);
		}

		public void Deactivate()
		{
			camera.gameObject.SetActive (false);
			Service.FrameUpdate.UnregisterForUpdate (this);
		}

		public Camera GetCamera()
		{
			return camera;
		}

		public void Update(float dt)
		{

		}
	}
}