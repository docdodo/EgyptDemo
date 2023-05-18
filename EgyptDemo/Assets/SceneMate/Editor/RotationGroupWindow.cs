using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class RotationGroupWindow : SceneMateGUI
{
	[MenuItem("Window/SceneMate/Rotation",false,5)]
	static void Init()
	{
		//Create the window
		RotationGroupWindow window = (RotationGroupWindow)EditorWindow.GetWindow(typeof(RotationGroupWindow));
		window.minSize = new Vector2(76, 98);
		window.maxSize = new Vector2(77, 98);
        window.titleContent = new GUIContent("Rotation");
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();
		
		RotationGroup(604);
	}
}
