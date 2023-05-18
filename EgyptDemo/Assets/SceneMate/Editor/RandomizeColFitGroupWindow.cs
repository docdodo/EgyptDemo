using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class RandomizeColFitGroupWindow : SceneMateGUI
{
	[MenuItem("Window/SceneMate/Randomize+ColFit",false,2)]
	static void Init()
	{
		//Create the window
		RandomizeColFitGroupWindow window = (RandomizeColFitGroupWindow)EditorWindow.GetWindow(typeof(RandomizeColFitGroupWindow));
		window.minSize = new Vector2(209, 98);
		window.maxSize = new Vector2(210, 98);
        window.titleContent = new GUIContent("RandomizeColFit");
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();
		
		RandomizeColFitGroup(141);
	}
}
