using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ImportOptionsGroupWindow : SceneMateGUI 
{
	[MenuItem("Window/SceneMate/Import Options",false,10)]
	static void Init()
	{
		//Create the window
		ImportOptionsGroupWindow window = (ImportOptionsGroupWindow)EditorWindow.GetWindow(typeof(ImportOptionsGroupWindow));
		window.minSize = new Vector2(193, 98);
		window.maxSize = new Vector2(194, 98);
        window.titleContent = new GUIContent("Import Options");
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();
		
		ImportOptions(46);
	}
}
