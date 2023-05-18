using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SceneMateFullBar : SceneMateGUI 
{
	[MenuItem("Window/SceneMate/SceneMate",false,0)]
	static void Init()
	{
		//Create the window
		SceneMateFullBar window = (SceneMateFullBar)EditorWindow.GetWindow(typeof(SceneMateFullBar));
		window.minSize = new Vector2(842, 98);
		window.maxSize = new Vector2(843, 98);
        window.titleContent = new GUIContent("SceneMate");
	}
	
	public override void ResizeWindow(int minWidth, int minHeight, int minWidth2, int minHeight2, bool compareBool)
	{
		SceneMateFullBar window = (SceneMateFullBar)EditorWindow.GetWindow(typeof(SceneMateFullBar));
		window.minSize = new Vector2(minWidth, minHeight);
		window.maxSize = new Vector2(minWidth + 1, minHeight);
		if(compareBool)
		{
			window.minSize = new Vector2(minWidth2, minHeight2);
			window.maxSize = new Vector2(minWidth2 + 1, minHeight2);
		}
	}
}
