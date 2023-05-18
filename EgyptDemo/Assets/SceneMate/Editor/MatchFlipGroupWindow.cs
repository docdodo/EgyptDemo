using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MatchFlipGroupWindow : SceneMateGUI
{
	[MenuItem("Window/SceneMate/Match+Flip",false,1)]
	static void Init()
	{
		//Create the window
		MatchFlipGroupWindow window = (MatchFlipGroupWindow)EditorWindow.GetWindow(typeof(MatchFlipGroupWindow));
		window.minSize = new Vector2(142, 98);
		window.maxSize = new Vector2(143, 98);
        window.titleContent = new GUIContent("MatchFlip");
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();
		
		MatchFlipGroup();
	}
}
