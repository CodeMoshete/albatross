//Engine.cs
//Description: This serves as the entry point for the program

using UnityEngine;
using System.Collections;
using Controllers;
using Game.Controllers;
using Services;
using Events;

public class Engine : MonoBehaviour
{
	private FlowController gameFlow;

	public void Start ()
	{
		gameFlow = new FlowController();
		gameFlow.StartGame();
	}

	public void Update()
	{
		Service.FrameUpdate.Update(this);
	}

	public void OnApplicationQuit()
	{
		Service.Events.SendEvent (EventId.ApplicationExit, null);
	}
}
