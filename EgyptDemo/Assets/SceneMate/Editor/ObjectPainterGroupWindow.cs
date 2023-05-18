using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ObjectPainterGroupWindow : SceneMateGUI 
{
	[MenuItem("Window/SceneMate/Object Painter",false,7)]
	static void Init()
	{
		//Create the window
		ObjectPainterGroupWindow window = (ObjectPainterGroupWindow)EditorWindow.GetWindow(typeof(ObjectPainterGroupWindow));
		window.minSize = new Vector2(288, 98);
		window.maxSize = new Vector2(289, 98);
        window.titleContent = new GUIContent("ObjectPainter");
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();
		
		objPainter = GUI.Toggle(new Rect(0,2,32,47),objPainter,guiContent[16],customSkin.toggle);
		ObjectPainter(767);
	}
}
