using UnityEngine;
using System.Collections;
using Utils;
using UnityEngine.UI;
using Services;
using Events;

public class DraftPanel
{
	private GameObject wrappedObject;
	private Button createButton;

	public DraftPanel(GameObject wrappedObject)
	{
		this.wrappedObject = wrappedObject;
		createButton = EditorUtils.FindGameObject (wrappedObject, "CreateNewObjectButton").GetComponent<Button> ();
		createButton.onClick.AddListener (CreateButtonPressed);
		this.wrappedObject.SetActive (true);
	}

	private void CreateButtonPressed()
	{
		Service.Events.SendEvent (EventId.CreateButtonPressed, null);
	}
}
