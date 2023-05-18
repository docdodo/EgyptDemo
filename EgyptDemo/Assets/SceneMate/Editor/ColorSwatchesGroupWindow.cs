using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ColorSwatchesGroupWindow : SceneMateGUI 
{
	[MenuItem("Window/SceneMate/Color Swatches",false,8)]
	static void Init()
	{
		//Create the window
		ColorSwatchesGroupWindow window = (ColorSwatchesGroupWindow)EditorWindow.GetWindow(typeof(ColorSwatchesGroupWindow));
		window.minSize = new Vector2(85, 98);
		window.maxSize = new Vector2(86, 98);
        window.titleContent = new GUIContent("ColorSwatches");
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();
		
		ColorSwatches();
	}
}
