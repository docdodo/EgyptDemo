using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class OffsetGroupWindow : SceneMateGUI
{
	[MenuItem("Window/SceneMate/Offset",false,3)]
	static void Init()
	{
		//Create the window
		OffsetGroupWindow window = (OffsetGroupWindow)EditorWindow.GetWindow(typeof(OffsetGroupWindow));
		window.minSize = new Vector2(85, 98);
		window.maxSize = new Vector2(86, 98);
        window.titleContent = new GUIContent("Offset");
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();
		
		OffsetGroup(353);
	}
}
