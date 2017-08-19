using UnityEngine;
using Utils;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace Controllers.UI
{
	public class EditorInterfaceController
	{
		private const string INTERFACE_RESOURCE = "GUI/GUI_EditorInterface";
		private const string BUILD_BUTTON = "BuildButton";
		private const string DRAFT_BUTTON = "DraftButton";

		private GameObject interfaceObj;

		public EditorInterfaceController(UnityAction onBuild, UnityAction OnDraft)
		{
			interfaceObj = (GameObject)GameObject.Instantiate(Resources.Load (INTERFACE_RESOURCE));
			Button buildBtn = EditorUtils.FindGameObject (interfaceObj, BUILD_BUTTON).GetComponent<Button>();
			buildBtn.onClick.AddListener (onBuild);
			Button draftBtn = EditorUtils.FindGameObject (interfaceObj, DRAFT_BUTTON).GetComponent<Button>();
			draftBtn.onClick.AddListener (OnDraft);
		}


	}
}
