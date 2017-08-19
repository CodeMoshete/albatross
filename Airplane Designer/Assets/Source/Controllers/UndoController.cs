using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UndoController
{
	private IList<IEditorAction> loggedActions;
	private int currentActionIndex;

	public UndoController()
	{
		loggedActions = new List<IEditorAction> ();
		currentActionIndex = -1;
	}

	public void ExecuteAction(IEditorAction action)
	{
		int numActions = loggedActions.Count;
		if (numActions > 0 && currentActionIndex < (numActions - 1))
		{
			while (loggedActions.Count > (currentActionIndex + 1))
			{
				loggedActions.RemoveAt (loggedActions.Count - 1);
			}
		}

		loggedActions.Add (action);
		action.ApplyAction ();
		currentActionIndex++;
	}

	public void Undo()
	{
		if (currentActionIndex > 0)
		{
			loggedActions [currentActionIndex].ReverseAction ();
			currentActionIndex--;
		}
	}

	public void Redo()
	{
		if (currentActionIndex < (loggedActions.Count - 1))
		{
			currentActionIndex++;
			loggedActions [currentActionIndex].ApplyAction ();
		}
	}
}
