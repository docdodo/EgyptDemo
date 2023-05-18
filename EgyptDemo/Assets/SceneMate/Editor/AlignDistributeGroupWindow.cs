using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class AlignDistributeGroupWindow : SceneMateGUI
{
	[MenuItem("Window/SceneMate/Align+Distribute",false,6)]
	static void Init()
	{
		//Create the window
		AlignDistributeGroupWindow window = (AlignDistributeGroupWindow)EditorWindow.GetWindow(typeof(AlignDistributeGroupWindow));
		window.minSize = new Vector2(84, 98);
		window.maxSize = new Vector2(85, 98);
		window.titleContent = new GUIContent("AlignDistribute");
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();
		
		AlignDistributeGroup(681);
	}
}
