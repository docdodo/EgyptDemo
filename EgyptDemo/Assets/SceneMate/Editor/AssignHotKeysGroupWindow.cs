using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class AssignHotKeysGroupWindow : SceneMateGUI 
{
	[MenuItem("Window/SceneMate/Assign HotKeys",false,9)]
	static void Init()
	{
		//Create the window
		AssignHotKeysGroupWindow window = (AssignHotKeysGroupWindow)EditorWindow.GetWindow(typeof(AssignHotKeysGroupWindow));
		window.minSize = new Vector2(163, 98);
		window.maxSize = new Vector2(164, 98);
        window.titleContent = new GUIContent("Assign HotKeys");
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();
		
		AssignHotKeys(46);
		HotKeyAssignment(48);
	}
}
