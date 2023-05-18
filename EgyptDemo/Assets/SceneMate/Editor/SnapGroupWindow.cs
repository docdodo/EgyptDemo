using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SnapGroupWindow : SceneMateGUI
{
	[MenuItem("Window/SceneMate/Snaps",false,4)]
	static void Init()
	{
		//Create the window
		SnapGroupWindow window = (SnapGroupWindow)EditorWindow.GetWindow(typeof(SnapGroupWindow));
		window.minSize = new Vector2(159, 98);
		window.maxSize = new Vector2(160, 98);
        window.titleContent = new GUIContent("Snaps");
	}
	
	public override void SnapGroup(int moveLeft)
	{
		base.SnapGroup(moveLeft);
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();

		SnapGroup(439);
	}
}
