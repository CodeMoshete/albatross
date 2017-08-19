using UnityEngine;
using System.Collections;

public interface ICameraController 
{
	void Activate ();
	void Deactivate ();
	Camera GetCamera();
}
