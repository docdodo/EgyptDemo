using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SceneMateGUI : SceneMateOnSceneGUI 
{
	public virtual void OnEnable()
	{	
		customSkin = ScriptableObject.CreateInstance<GUISkin>();
		customSkin2 = ScriptableObject.CreateInstance<GUISkin>();
		customSkin3 = ScriptableObject.CreateInstance<GUISkin>();
		waitForSelMatch = false;
		grabSource = false;
		//Check for and get saved values
		if (EditorPrefs.HasKey("materialImport"))
			materialImport = EditorPrefs.GetBool("materialImport");	
		if (EditorPrefs.HasKey("animationImport"))	
			animationImport = EditorPrefs.GetBool("animationImport");
		if (EditorPrefs.HasKey("objPainter"))
			objPainter = EditorPrefs.GetBool("objPainter");
		if (EditorPrefs.HasKey("assignHotKeysDropDown"))
			assignHotKeysDropDown = EditorPrefs.GetBool("assignHotKeysDropDown");
		if (EditorPrefs.HasKey("importOptionsDropDown"))
			importOptionsDropDown = EditorPrefs.GetBool("importOptionsDropDown");
		if (EditorPrefs.HasKey("rotAxis"))	
			rotAxis = EditorPrefs.GetInt("rotAxis");;
		if (EditorPrefs.HasKey("stgDir"))	
			stgDir = EditorPrefs.GetInt("stgDir");
		if (EditorPrefs.HasKey("stwDir"))	
			stwDir = EditorPrefs.GetInt("stwDir");
		if (EditorPrefs.HasKey("stcDir"))	
			stcDir = EditorPrefs.GetInt("stcDir");
		if (EditorPrefs.HasKey("rowWidth"))	
			rowWidth = EditorPrefs.GetInt("rowWidth");
		if (EditorPrefs.HasKey("snapsMask"))	
			snapsMask = EditorPrefs.GetInt("snapsMask");
		if (EditorPrefs.HasKey("painterMask"))	
			painterMask = EditorPrefs.GetInt("painterMask");
		
		for(int i = 0; i < toolKeyCodes.Length;i++)
		{
			toolKeyStrings[i] = toolKeyCodes[i].ToString();	
		}
		
		for(int i = 0; i < toolKeyCodes.Length;i++)
		{
			if (EditorPrefs.HasKey(toolPrefStrings[i]))
			{
				toolKeyStrings[i] = EditorPrefs.GetString(toolPrefStrings[i]);
				toolKeyCodes[i] = (KeyCode)System.Enum.Parse( typeof(KeyCode), toolKeyStrings[i]);
			}
		}
		
		//Create the editor prefs for modfiers if they don't already exist, "SMmodControl0" is just one of the many that should already exist
		if(!EditorPrefs.HasKey("SMmodControl0"))
		{
			for(int i = 0;i < hkBtnNames.Length;i++)
			{
				//Set Control Modifer Editor Prefs
				EditorPrefs.SetBool("SMmodControl" + i.ToString(),modControl[i]);
				//Set Shift Modifer Editor Prefs
				EditorPrefs.SetBool("SMmodShift" + i.ToString(),modShift[i]);
				//Set Alt Modifer Editor Prefs
				EditorPrefs.SetBool("SMmodAlt" + i.ToString(),modAlt[i]);
			}
		}
		//Get all the editor prefs for the modifiers
		for(int i = 0;i < hkBtnNames.Length;i++)
		{
			modControl[i] = EditorPrefs.GetBool("SMmodControl" + i.ToString());
			modShift[i] = EditorPrefs.GetBool("SMmodShift" + i.ToString());
			modAlt[i] = EditorPrefs.GetBool("SMmodAlt" + i.ToString());
		}
		string scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
		string assetPath = scriptPath.Remove(scriptPath.LastIndexOf("/"));
		assetPath = assetPath.Remove(assetPath.LastIndexOf("/") + 1);
		//Load Icon Images
		for(int i = 0; i < buttonFileNames.Length; i++)
		{
			//load all the icons in the GUI content
			guiContent[i] = new GUIContent("",AssetDatabase.LoadAssetAtPath(assetPath + "Icons/" + buttonFileNames[i].Substring(0,buttonFileNames[i].Length - 3) + ".psd",typeof(Texture)) as Texture,toolTips[i]);			
		}
	}
	
	public virtual void ResizeWindow(int minWidth, int minHeight, int minWidth2, int minHeight2, bool compareBool)
	{
	}
	
	public virtual void MatchFlipGroup()
	{
		//Divder made out of empty group
		GUI.BeginGroup(new Rect(0,54,141,2),customSkin.textArea);
		GUI.EndGroup();
		
		if (GUI.Button(new Rect(2,60,34,34),guiContent[13],customSkin.button))
			FlipX();
		
		if (GUI.Button(new Rect(2,2,34,34),guiContent[7],customSkin.button))
		{
			if(!waitForSelMatch && !matchPos)
			{
				if(Selection.transforms.Length > 0)
				{
					matchPos = true;
					waitForSelMatch = true;
				}
				else
					Debug.Log ("Please Select Object(s)");
			}
			else
			{
				AvgMatchPos();
			}
		}
		posMatchX = GUI.Toggle(new Rect(3,36,12,14),posMatchX,"x",customSkin3.toggle);
		posMatchY = GUI.Toggle(new Rect(13,36,12,14),posMatchY,"y",customSkin3.toggle);
		posMatchZ = GUI.Toggle(new Rect(23,36,12,14),posMatchZ,"z",customSkin3.toggle);
		
		if (GUI.Button(new Rect(36,60,34,34),guiContent[14],customSkin.button))
			FlipY();
	
		if (GUI.Button(new Rect(36,2,34,34),guiContent[8],customSkin.button))
		{
			if(!waitForSelMatch && !matchRot)
			{
				if(Selection.transforms.Length > 0)
				{
					matchRot = true;
					waitForSelMatch = true;
				}
				else
					Debug.Log ("Please Select Object(s)");
			}
			else
			{
				AvgMatchRot();
			}
		}
		rotMatchX = GUI.Toggle(new Rect(37,36,12,14),rotMatchX,"x",customSkin3.toggle);
		rotMatchY = GUI.Toggle(new Rect(47,36,12,14),rotMatchY,"y",customSkin3.toggle);
		rotMatchZ = GUI.Toggle(new Rect(57,36,12,14),rotMatchZ,"z",customSkin3.toggle);
		
		if (GUI.Button(new Rect(70,60,34,34),guiContent[15],customSkin.button))
			FlipZ();
		
		if (GUI.Button(new Rect(70,2,34,34),guiContent[9],customSkin.button))
		{
			if(!waitForSelMatch && !matchScale)
			{
				if(Selection.transforms.Length > 0)
				{
					matchScale = true;
					waitForSelMatch = true;
				}
				else
					Debug.Log ("Please Select Object(s)");
			}
			else
			{
				AvgMatchScale();
			}
		}
		scaleMatchX = GUI.Toggle(new Rect(71,36,12,14),scaleMatchX,"x",customSkin3.toggle);
		scaleMatchY = GUI.Toggle(new Rect(81,36,12,14),scaleMatchY,"y",customSkin3.toggle);
		scaleMatchZ = GUI.Toggle(new Rect(91,36,12,14),scaleMatchZ,"z",customSkin3.toggle);
		
		if (GUI.Button(new Rect(106,2,34,30),guiContent[5],customSkin.button))
			ResetObjs();
		if (GUI.Button(new Rect(104,32,14,20),"P",customSkin.button))
			ResetObjsPos();	
		if (GUI.Button(new Rect(116,32,14,20),"R",customSkin.button))
			ResetObjsRot();
		if (GUI.Button(new Rect(128,32,14,20),"S",customSkin.button))
			ResetObjsScale();
		
		
		if (GUI.Button(new Rect(106,60,34,34),guiContent[11],customSkin.button))
			SelAmt();
	}
	
	public virtual void RandomizeColFitGroup(int moveLeft)
	{
		//Divder made out of empty group
		GUI.BeginGroup(new Rect(141-moveLeft,0,106,96),customSkin.textArea);
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(141-moveLeft,58,209,38),customSkin.textArea);
		GUI.EndGroup();
		//rand rotation buttons
		if (GUI.Button(new Rect(144-moveLeft,60,34,34),guiContent[25],customSkin.button))
			RandomizeRotation();
		
		rotX = GUI.Toggle(new Rect(178-moveLeft,58,14,12),rotX,"x",customSkin3.toggle);
		rotY = GUI.Toggle(new Rect(178-moveLeft,70,14,12),rotY,"y",customSkin3.toggle);
		rotZ = GUI.Toggle(new Rect(178-moveLeft,82,14,12),rotZ,"z",customSkin3.toggle);
		
		GUI.Label(new Rect(219-moveLeft,64,84,80),"snap",customSkin.label);
		randomizeRotSnap = EditorGUI.FloatField(new Rect(192-moveLeft,62,30,16),"", randomizeRotSnap);
		randRotCurAxis = EditorGUI.Popup(new Rect(196-moveLeft,80,50,16),"",randRotCurAxis,axis);		
		
		//Rand Scale Button
		if (GUI.Button(new Rect(250-moveLeft,60,34,34),guiContent[26],customSkin.button))
			RandomizeScale();
		
		scaleX = GUI.Toggle(new Rect(284-moveLeft,58,14,12),scaleX,"x",customSkin3.toggle);
		scaleY = GUI.Toggle(new Rect(284-moveLeft,70,14,12),scaleY,"y",customSkin3.toggle);
		scaleZ = GUI.Toggle(new Rect(284-moveLeft,82,14,12),scaleZ,"z",customSkin3.toggle);
		
		randomizeScaleMin = EditorGUI.FloatField(new Rect(298-moveLeft,60,30,16),"", randomizeScaleMin);
		GUI.Label(new Rect(325-moveLeft,57,84,80),"min",customSkin.label);
		GUI.Label(new Rect(325-moveLeft,80,84,80),"max",customSkin.label);
		randomizeScaleMax = EditorGUI.FloatField(new Rect(298-moveLeft,76,30,16),"", randomizeScaleMax);
		
		randScaleUniform = GUI.Toggle(new Rect(328-moveLeft,67,16,16),randScaleUniform, "");
		
		//Rand Position Button
		if (GUI.Button(new Rect(144-moveLeft,2,34,34),guiContent[27],customSkin.button))
			RandomizePosition();
		
		positionX = GUI.Toggle(new Rect(178-moveLeft,0,14,12),positionX,"x",customSkin3.toggle);
		positionY = GUI.Toggle(new Rect(178-moveLeft,12,14,12),positionY,"y",customSkin3.toggle);
		positionZ = GUI.Toggle(new Rect(178-moveLeft,24,14,12),positionZ,"z",customSkin3.toggle);
		
		randomizePosMin = EditorGUI.FloatField(new Rect(192-moveLeft,2,30,16),"", randomizePosMin);
		GUI.Label(new Rect(220-moveLeft,4,84,80),"min",customSkin.label);
		GUI.Label(new Rect(220-moveLeft,20,84,80),"max",customSkin.label);
		randomizePosMax = EditorGUI.FloatField(new Rect(192-moveLeft,18,30,16),"", randomizePosMax);
		
		randPosCurAxis = EditorGUI.Popup(new Rect(196-moveLeft,38,50,16),"",randPosCurAxis,axis);
		
		//Divder
		GUI.BeginGroup(new Rect(250-moveLeft,0,100,55),customSkin.textArea);
		
		if (GUI.Button(new Rect(6,2,34,34),guiContent[17],customSkin.button))
			FitCollider();
		
		keepOrig = GUI.Toggle(new Rect(76,25,12,12),keepOrig,"O",customSkin3.toggle);
		inheritPos = GUI.Toggle(new Rect(46,25,12,12),inheritPos,"P",customSkin3.toggle);
		inheritRot = GUI.Toggle(new Rect(56,25,12,12),inheritRot,"R",customSkin3.toggle);
		inheritScale = GUI.Toggle(new Rect(66,25,12,12),inheritScale,"S",customSkin3.toggle);
		if (GUI.Button(new Rect(50,2,34,24),guiContent[6],customSkin.button))
			ReplaceSelection();
		
		colSelectedObjArr[0] = EditorGUI.ObjectField(new Rect(3,39,95,15),"", colSelectedObjArr[0], typeof(GameObject), true) as GameObject;
		colSelectedObj = colSelectedObjArr[0] as GameObject;
		
		GUI.EndGroup();
	}
	
	public virtual void OffsetGroup(int moveLeft)
	{
		GUI.BeginGroup(new Rect(353-moveLeft,0,83,96),customSkin.textArea);
		
		if (GUI.Button(new Rect(4,2,34,34),guiContent[10],customSkin.button))
			OffsetSel();
		
		offsetSelCurAxis = EditorGUI.Popup(new Rect(16,56,64,16),"",offsetSelCurAxis,axis);
		
		curOffsetStyle = EditorGUI.Popup(new Rect(16,72,64,16),"",curOffsetStyle,offsetStyle);
		
		GUI.Label(new Rect(38,2,30,16),"X");
		offsetXAmount = EditorGUI.FloatField(new Rect(49,2,30,16),"", offsetXAmount);
		GUI.Label(new Rect(38,18,30,16),"Y");
		offsetYAmount = EditorGUI.FloatField(new Rect(49,18,30,16),"", offsetYAmount);
		GUI.Label(new Rect(38,34,30,16),"Z");
		offsetZAmount = EditorGUI.FloatField(new Rect(49,34,30,16),"", offsetZAmount);
		
		GUI.EndGroup();
	}
	
	public virtual void SnapGroup(int moveLeft)
	{
		GUI.BeginGroup(new Rect(439-moveLeft,0,162,96),customSkin.textArea);
		
		snapDrag = GUI.Toggle(new Rect(16,2,34,34),snapDrag,guiContent[21],customSkin.toggle);
		
		GUI.Label(new Rect(4,35,30,16),"-",customSkin2.label);
		GUI.Label(new Rect(3,48,30,16),"+",customSkin2.label);
		dragDirNegX = GUI.Toggle(new Rect(14,36,14,14),dragDirNegX,"x",customSkin3.toggle);
		dragDirPosX = GUI.Toggle(new Rect(14,48,14,14),dragDirPosX,"x",customSkin3.toggle);
		dragDirNegY = GUI.Toggle(new Rect(26,36,14,14),dragDirNegY,"y",customSkin3.toggle);
		dragDirPosY = GUI.Toggle(new Rect(26,48,14,14),dragDirPosY,"y",customSkin3.toggle);
		dragDirNegZ = GUI.Toggle(new Rect(38,36,14,14),dragDirNegZ,"z",customSkin3.toggle);
		dragDirPosZ = GUI.Toggle(new Rect(38,48,14,14),dragDirPosZ,"z",customSkin3.toggle);
		
		GUI.Label(new Rect(19,78,60,16),"Thld",customSkin.label);
		snapDragThreshold = EditorGUI.FloatField(new Rect(17,64,33,15),"", snapDragThreshold);
		if(snapDragThreshold < .1f)
			snapDragThreshold = .1f;
		if(snapDragThreshold > 9999)
			snapDragThreshold = 9999;
		
		if (GUI.Button(new Rect(54,2,34,34),guiContent[3],customSkin.button))
			SnapToWall();
		stwDir = EditorGUI.Popup(new Rect(54,36,34,16),"",stwDir,snapDir);
		
		GUI.Label(new Rect(54,78,60,16),"Offst",customSkin.label);
		snapOffset = EditorGUI.FloatField(new Rect(54,64,33,15),"", snapOffset);
		if(snapOffset < -9999)
			snapOffset = -9999;
		if(snapOffset > 9999)
			snapOffset = 9999;
		
		if (GUI.Button(new Rect(88,2,34,34),guiContent[4],customSkin.button))
			SnapToCenter();
		stcDir = EditorGUI.Popup(new Rect(88,36,34,16),"",stcDir,snapDir);
		
		if (GUI.Button(new Rect(122,2,34,34),guiContent[2],customSkin.button))
			SnapToGround();
		stgDir = EditorGUI.Popup(new Rect(122,36,34,16),"",stgDir,snapDir);
		
		alignToNormal = GUI.Toggle(new Rect(88,54,34,17),alignToNormal,guiContent[19],customSkin.toggle);
		boundsSnapping = GUI.Toggle(new Rect(88,71,34,17),boundsSnapping,guiContent[33],customSkin.toggle);
		
		snapSurface = GUI.Toggle(new Rect(122,54,34,34),snapSurface,guiContent[12],customSkin.toggle);
		
		GUI.EndGroup();
	}
	
	public virtual void RotationGroup(int moveLeft)
	{
		GUI.BeginGroup(new Rect(604-moveLeft,0,74,96),customSkin.textArea);
		
		if (GUI.Button(new Rect(3,2,34,34),guiContent[0],customSkin.button))
			RotateLeft();

		if (GUI.Button(new Rect(3,42,34,34),guiContent[1],customSkin.button))
			RotateRight();
		
		rotateCurAxis = EditorGUI.Popup(new Rect(3,77,50,16),"",rotateCurAxis,axis);
		
		rotAxis = EditorGUI.Popup(new Rect(37,2,34,16),"",rotAxis,rotDir);
		snapRotation90 = GUI.Toggle(new Rect(37,22,24,24),snapRotation90, guiContent[20],customSkin.toggle);
		rotationAmount = EditorGUI.FloatField(new Rect(37,50,33,15),"", rotationAmount);
		
		if (snapRotation90)
			rotationAmount = 90f;
		
		GUI.EndGroup();
	}
	
	public virtual void AlignDistributeGroup(int moveLeft)
	{
		GUI.BeginGroup(new Rect(681-moveLeft,0,84,96),customSkin.textArea);
		
		if (GUI.Button(new Rect(7,2,34,34),guiContent[28],customSkin.button))
			Distribute();
		disX = GUI.Toggle(new Rect(8,36,12,14),disX,"x",customSkin3.toggle);
		if(disX)
		{
			disY = false;
			disZ = false;
		}
		if(!disX && !disY && !disZ)
			disX = true;
		disY = GUI.Toggle(new Rect(18,36,12,14),disY,"y",customSkin3.toggle);
		if(disY)
		{
			disX = false;
			disZ = false;
		}
		if(!disX && !disY && !disZ)
			disY = true;
		disZ = GUI.Toggle(new Rect(28,36,12,14),disZ,"z",customSkin3.toggle);
		if(disZ)
		{
			disX = false;
			disY = false;
		}
		if(!disX && !disY && !disZ)
			disZ = true;
		
		disBySize = GUI.Toggle(new Rect(6,50,17,17),disBySize,guiContent[32],customSkin.toggle);
		setSpacing = GUI.Toggle(new Rect(23,50,17,17),setSpacing,guiContent[31],customSkin.toggle);
		if(setSpacing)
		{
			disSpacing = EditorGUI.FloatField(new Rect(6,68,34,14),"", disSpacing);
			reverseDis = GUI.Toggle(new Rect(6,80,12,12),reverseDis, "");
			GUI.Label(new Rect(17,82,84,80),"Last",customSkin.label);
			
			if(disSpacing < 0)
				disSpacing = 0;
		}
		
		//Align Buttons
		if (GUI.Button(new Rect(44,2,34,34),guiContent[29],customSkin.button))
			AlignHigh();
		if (GUI.Button(new Rect(44,36,34,34),guiContent[30],customSkin.button))
			AlignLow();
		
		alignX = GUI.Toggle(new Rect(45,70,12,14),alignX,"x",customSkin3.toggle);
		if(alignX)
		{
			alignY = false;
			alignZ = false;
		}
		if(!alignX && !alignX && !alignZ)
			alignX = true;
		alignY = GUI.Toggle(new Rect(55,70,12,14),alignY,"y",customSkin3.toggle);
		if(alignY)
		{
			alignX = false;
			alignZ = false;
		}
		if(!alignX && !alignY && !alignZ)
			alignY = true;
		alignZ = GUI.Toggle(new Rect(65,70,12,14),alignZ,"z",customSkin3.toggle);
		if(alignZ)
		{
			alignX = false;
			alignY = false;
		}
		if(!alignX && !alignY && !alignZ)
			alignZ = true;
		
		GUI.EndGroup();
	}
	
	public virtual void ObjectPainter(int moveLeft)
	{	
		//Second Group where all the other buttons reside. This allows the next group to start at 0
		GUI.BeginGroup(new Rect(802-moveLeft,2,250,94));
		//When mouse is over the window update the menus, 
		//this updates the names
		if(EditorWindow.mouseOverWindow)
		{
			for(int i = 0; i < objs.Count; i++)
			{
				if(objs[i] == null)
					objNames[i] = "None";
				else
				{
					objNames[i] = objs[i].name;
				}
			}
		}
		//Object painter group
		GUI.BeginGroup(new Rect(0,0,250,94),customSkin.textArea);
		if(objs.Count > 0)
		{
			curObject = EditorGUI.Popup(new Rect(10,2,136,16),"",curObject,objNames);

			if(GUI.Button(new Rect(0,2,10,16),"X",customSkin.button))
			{
				objs.RemoveAt(curObject);
				RebuildNames();
			}
			if(objNames.Length >= curObject + 1)
			{					
				randRot = GUI.Toggle(new Rect(0,29,16,16),randRot, "");
				GUI.Label(new Rect(10,31,84,80),"Rand Rotation",customSkin.label);
				if(randRot)
				{
					randRotX = GUI.Toggle(new Rect(10,41,24,16),randRotX,"X");
					randRotY = GUI.Toggle(new Rect(34,41,24,16),randRotY,"Y");
					randRotZ = GUI.Toggle(new Rect(58,41,24,16),randRotZ,"Z");
					
					GUI.Label(new Rect(28,58,84,80),"snap",customSkin.label);
					randomRotSnap = EditorGUI.FloatField(new Rect(28,72,30,16),"", randomRotSnap);
				}
				
				randScale = GUI.Toggle(new Rect(90,29,16,16),randScale, "");
				GUI.Label(new Rect(100,31,84,80),"Rand Scale",customSkin.label);
				if(randScale)
				{
					randScaleMin = EditorGUI.FloatField(new Rect(102,74,30,16),"", randScaleMin);
					GUI.Label(new Rect(80,69,84,80),"min",customSkin.label);
					GUI.Label(new Rect(80,77,84,80),"max",customSkin.label);
					randScaleMax = EditorGUI.FloatField(new Rect(134,74,30,16),"", randScaleMax);
					uniformScale = GUI.Toggle(new Rect(116,42,16,16),uniformScale, "");
					GUI.Label(new Rect(84,56,84,16),"Uniform Scale",customSkin.label);
					
					if(randScaleMax < randScaleMin)
						randScaleMax = randScaleMin;
					
					if(randScaleMin > randScaleMax)
						randScaleMin = randScaleMax;
				}					
			}
		}
		else
			GUI.Label(new Rect(0,1,116,80),"Select one or multiple \nGameObjects in \nthe Project Tab, \nthen click \n\"Add Objects\"",customSkin.label);
		
		GUI.EndGroup();
		//}
		
		GUI.BeginGroup(new Rect(160,2,84,78));
		assignParent = GUI.Toggle(new Rect(0,16,16,16),assignParent, "");
		if(assignParent)
		{
			parentObjs[0] = EditorGUI.ObjectField(new Rect(0,32,84,16),"", parentObjs[0], typeof(GameObject), true) as GameObject;
			parentObj = parentObjs[0] as GameObject;
		}
		GUI.Label(new Rect(8,18,84,80),"Assign Parent",customSkin.label);
		
		//Area for drag and dropping of objects
		if(GUI.Button(new Rect(0,0,86,16),"Add Objects"))
		{
			if(Selection.objects.Length > 0)
			{
				for(int i = 0; i < Selection.objects.Length; i++)
				{
					if(PrefabUtility.GetPrefabType(Selection.objects[i]) == PrefabType.Prefab)
					{
						bool isInList = false;
						foreach(GameObject obj in objs)
						{
							if(obj.name == Selection.objects[i].name)
								isInList = true;
						}
						if(!isInList)
						{
							objs.Add(Selection.objects[i]);
						}
						else
							Debug.Log ("Object with same name is already in the Object Painter List");
					}
				}
				RebuildNames();
			}
		}
		//if(GUI.Button(new Rect(6,60,80,16),"Advanced"))
		//{
		//	AdvObjPOptionsWindow.Init();
		//}
		eraserMode = false;
		eraserMode = GUI.Toggle(new Rect(0,100,16,16),eraserMode, "");
		GUI.Label(new Rect(10,102,84,16),"Eraser Mode",customSkin.label);
		
		GUI.EndGroup();
		GUI.EndGroup();
	}
	
	public virtual void ColorSwatches()
	{
		GUI.BeginGroup(new Rect(pixelHeight,2,84,94),customSkin.textArea);
		if(GUI.Button(new Rect(0,0,17,16),"+",customSkin.button))
		{
			colorList.Add(color1);
		}
		if(colorList.Count == 0)
		{
			colorList.Add(color1);
		}
		
		vScrollPos = GUI.VerticalScrollbar(new Rect(68,0,16,93),vScrollPos,1,0,colorList.Count*16 - 93);
		
		for(int i = 0; i < colorList.Count; i++)
		{
			colorList[i] = EditorGUI.ColorField(new Rect(27,i*16-vScrollPos,42,16),"",colorList[i]);
			if(GUI.Button(new Rect(17,i*16-vScrollPos,10,16),"X",customSkin.button))
			{
				colorList.RemoveAt(i);
			}
		}
		GUI.EndGroup();
	}
	
	public virtual void AssignHotKeys(int moveLeft)
	{
		//Check if any modifiers have changed and save them
		for(int i = 0;i < hkBtnNames.Length;i++)
		{
			if(modControl[i] != EditorPrefs.GetBool("SMmodControl" + i.ToString()))
				EditorPrefs.SetBool("SMmodControl" + i.ToString(),modControl[i]);
			
			if(modShift[i] != EditorPrefs.GetBool("SMmodShift" + i.ToString()))
				EditorPrefs.SetBool("SMmodShift" + i.ToString(),modShift[i]);
			
			if(modAlt[i] != EditorPrefs.GetBool("SMmodAlt" + i.ToString()))
				EditorPrefs.SetBool("SMmodAlt" + i.ToString(),modAlt[i]);
		}
		
		GUI.BeginGroup(new Rect(48-moveLeft,2,250,94));
		assignHKsel = EditorGUI.Popup(new Rect(0,2,150,16),assignHKsel,hkBtnNames);
		
		GUI.Label(new Rect(6,20,84,80),toolKeyStrings[assignHKsel],customSkin3.label);
		
		GUI.Label(new Rect(100,20,84,80),"Modifiers:",customSkin.label);
		
		modControl[assignHKsel] = GUI.Toggle(new Rect(100,30,84,16),modControl[assignHKsel], "Control");
		modShift[assignHKsel] = GUI.Toggle(new Rect(100,44,84,16),modShift[assignHKsel], "Shift");
		modAlt[assignHKsel] = GUI.Toggle(new Rect(100,58,84,16),modAlt[assignHKsel], "Alt");
		
		if(GUI.Button(new Rect(2,42,78,16),"Assign Key",customSkin.button))
		{
			toolAssignKey[assignHKsel] = true;
			assignNextKey = true;
			Debug.Log("Please press the key you would like to assign to " + hkBtnNames[assignHKsel] + " ||  If you would like to remove the hot key assignment Press Escape");
			Repaint();
		}
		if(GUI.Button(new Rect(2,74,78,16),"Print All",customSkin.button))
		{
			string allFormatted = "Highlight to see all Key Assignments...";
			for(int i = 0;i < toolKeyCodes.Length; i++)
			{
				string tempControl;
				string tempShift;
				string tempAlt;
				if(modControl[i])
					tempControl = "Control";
				else
					tempControl = "";
				if(modShift[i])
					tempShift = "Shift";
				else
					tempShift = "";
				if(modAlt[i])
					tempAlt = "Alt";
				else
					tempAlt = "";
				
				allFormatted += "\n" + hkBtnNames[i] + " =  " + "((" + toolKeyStrings[i] + "))" + "  Modifiers(" + tempControl + ", " + tempShift + ", " + tempAlt + ")";
			}
			Debug.Log(allFormatted);
		}
		GUI.EndGroup();
	}
	
	public virtual void ImportOptions(int moveLeft)
	{
		GUI.BeginGroup(new Rect(48-moveLeft,2,400,90));
			
		GUI.Label(new Rect(0,0,84,16),"Import Settings",customSkin3.label);
		materialImport = GUI.Toggle(new Rect(0,20,84,16),materialImport, "Materials",customSkin2.toggle);
		animationImport = GUI.Toggle(new Rect(0,36,84,16),animationImport, "Animations",customSkin2.toggle);
		
		GUI.Label(new Rect(100,0,84,16),"Layer Masks",customSkin3.label);
		
		//find all layers and use them for the layer mask popup
		if (layers == null || (System.DateTime.Now.Ticks - lastUpdateTick > 10000000L && Event.current.type == EventType.Layout)) 
		{
        	lastUpdateTick = System.DateTime.Now.Ticks;
       		if (layers == null) 
			{
	            layers = new List<string>();
	            layerNumbers = new List<int>();
	            layerNames = new string[4];
        	}
			else 
			{
	            layers.Clear ();
	            layerNumbers.Clear ();
        	}

	        int emptyLayers = 0;
	        for (int i=0;i<32;i++) 
			{
           		string layerName = LayerMask.LayerToName (i);

	            if (layerName != "") 
				{
	                for (;emptyLayers>0;emptyLayers--) layers.Add ("Layer "+(i-emptyLayers));
	                layerNumbers.Add (i);
	                layers.Add (layerName);
	            } 
				else 
				{
                	emptyLayers++;
            	}
        	}

       		if (layerNames.Length != layers.Count) 
			{
            	layerNames = new string[layers.Count];
       		}
        for (int i=0;i<layerNames.Length;i++) layerNames[i] = layers[i];
		}
		int tempSnapsMask = snapsMask.value;
		//Snaps Layer Masking
		GUI.Label(new Rect(100,14,84,16),"Snaps",customSkin.label);
		snapsMask.value = EditorGUI.MaskField(new Rect(100,26,84,16),"",snapsMask.value,layerNames);
		
		//Object Painter Layer Masking
		GUI.Label(new Rect(100,44,84,16),"Painter",customSkin.label);
		painterMask.value = EditorGUI.MaskField(new Rect(100,56,84,16),"",painterMask.value,layerNames);
		
		//Don't allow the user to check Ignore Raycast, since that is used for snaps
		if(snapsMask.value == tempSnapsMask + 4)
			snapsMask.value = snapsMask.value - 4;
		if(snapsMask.value == tempSnapsMask -4)
			snapsMask.value = snapsMask.value + 4;
		if(snapsMask.value == -1)
			snapsMask.value = snapsMask.value - 4;
		
		GUI.EndGroup();
	}
	
	public virtual void HotKeyAssignment(int moveLeft)
	{
		if(assignNextKey)
		{
			GUI.Label(new Rect(50-moveLeft,60,78,16),"Press Any Key",customSkin.label);
		}
				
		//Assign Hot Keys while GUI is active
		Event current = Event.current;
		
		if (Event.current.type == EventType.KeyDown)
		{
			if(assignNextKey)
			{
				for(int i = 0;i < toolKeyCodes.Length; i++)
				{
					if(toolAssignKey[i] == true)
					{
						if(current.keyCode == KeyCode.Escape || current.keyCode == KeyCode.LeftControl 
							|| current.keyCode == KeyCode.RightControl || current.keyCode == KeyCode.LeftShift 
							|| current.keyCode == KeyCode.RightShift || current.keyCode == KeyCode.LeftAlt 
							|| current.keyCode == KeyCode.RightAlt)
						{
							toolKeyCodes[i] = KeyCode.None;
							toolKeyStrings[i] = KeyCode.None.ToString();
							toolAssignKey[i] = false;
							Debug.Log("Hot key assignment removed.");
						}
						else
						{
							toolKeyCodes[i] = current.keyCode;
							toolKeyStrings[i] = toolKeyCodes[i].ToString();
							toolAssignKey[i] = false;
						}
						assignNextKey = false;
						for(int o = 0; o < toolKeyCodes.Length;o++)
						{
							EditorPrefs.SetString(toolPrefStrings[o], toolKeyStrings[o]);
						}
						Repaint();
					}
				}
			}
		}
	}
	
	public virtual void AdvObjectPainterOptions()
	{
	}
	
	public virtual void CustomUIStyles()
	{
		//Custom Styles
		customSkin.button =  new GUIStyle(GUI.skin.button);
		customSkin.button.padding = new RectOffset(0,0,0,0);
		
		customSkin2.button =  new GUIStyle(GUI.skin.button);
		customSkin2.button.padding = new RectOffset(3,3,1,1);
		customSkin2.button.fontSize = 8;
		customSkin2.button.fontStyle = FontStyle.Bold;
		
		customSkin2.label = new GUIStyle(GUI.skin.label);
		customSkin2.label.fontSize = 12;
		customSkin2.label.fontStyle = FontStyle.Bold;
		
		customSkin2.toggle = new GUIStyle(GUI.skin.toggle);
		customSkin2.toggle.fontSize = 9;
		
		customSkin.toggle = new GUIStyle(GUI.skin.button);
		customSkin.toggle.padding = new RectOffset(0,0,0,0);
		
		customSkin.textArea = new GUIStyle(GUI.skin.textArea);
		
		customSkin.label = new GUIStyle(GUI.skin.label);
		customSkin.label.fontSize = 9;
		
		customSkin3.label = new GUIStyle(GUI.skin.label);
		customSkin3.label.fontSize = 9;
		customSkin3.label.fontStyle = FontStyle.Bold;
		
		customSkin3.toggle = new GUIStyle(GUI.skin.button);
		customSkin3.toggle.padding = new RectOffset(0,0,0,0);
		customSkin3.toggle.fontSize = 9;
		customSkin3.toggle.fontStyle = FontStyle.Bold;
	}
	
	public virtual void OnSceneGUIDelegate()
	{
		//onSceneGUIDelegate allows use of controls while focused in the viewport and not focused on the GUI.
		if(SceneView.onSceneGUIDelegate != this.OnSceneGUI)
	    {
	       SceneView.onSceneGUIDelegate = this.OnSceneGUI;
	    }
	}
	
	public virtual void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();		
		//First Group of Buttons goes to the color swatches and object painter from the top.
		GUI.BeginGroup(new Rect(1,0,1000,530));
		
		MatchFlipGroup();
		RandomizeColFitGroup(0);		
		OffsetGroup(0);
		SnapGroup(0);
		RotationGroup(0);
		AlignDistributeGroup(0);
		
		objPainter = GUI.Toggle(new Rect(767,2,32,47),objPainter,guiContent[16],customSkin.toggle);
		if(!objPainter)
		{
			if(OPwindowResize)
			{
				if(!importOptionsDropDown && !assignHotKeysDropDown)
				{
					ResizeWindow(842, 98, 928, 98, CSwindowResize);
				}
				OPwindowResize = false;
			}
		}
		
		GUI.EndGroup();
		
		if(objPainter)
		{
			if(!OPwindowResize)
			{
				ResizeWindow(1094, 98, 1178, 98, CSwindowResize);
				OPwindowResize = true;
			}
			//close other menus to make room for this one
			if(importOptionsDropDown)
				importOptionsDropDown = false;
			if(assignHotKeysDropDown)
				assignHotKeysDropDown = false;
			ObjectPainter(0);
		}
		
		if(!objPainter)
			optPixelHeight = pixelHeight = 802;
		if(objPainter)
			optPixelHeight = pixelHeight = 1052;
		if(colorSwatches)
			optPixelHeight = 886;
		if(objPainter && colorSwatches)
			optPixelHeight = 1136;
		
		colorSwatches = GUI.Toggle(new Rect(768,54,34,34),colorSwatches,guiContent[22],customSkin.toggle);
		if(!colorSwatches)
		{
			if(CSwindowResize)
			{
				if(!importOptionsDropDown && !assignHotKeysDropDown)
				{
					ResizeWindow(842, 98, 1094, 98, OPwindowResize);
				}
				CSwindowResize = false;
			}
		}
		if(colorSwatches)
		{
			if(!CSwindowResize)
			{
				ResizeWindow(928, 98, 1178, 98, OPwindowResize);
				CSwindowResize = true;
			}
			//close other menus to make room for this one
			if(importOptionsDropDown)
				importOptionsDropDown = false;
			if(assignHotKeysDropDown)
				assignHotKeysDropDown = false;
			
			ColorSwatches();
		}
			
		GUI.BeginGroup(new Rect(optPixelHeight,2,400,94));
		//Drop down for assigning hot keys. contains a button for each hot key that is assignable
		assignHotKeysDropDown = GUI.Toggle(new Rect(2,0,34,26),assignHotKeysDropDown,guiContent[23],customSkin.toggle);		
		if(!assignHotKeysDropDown)
		{
			if(assignHKwindowResize)
			{
				if(!objPainter && !importOptionsDropDown && !colorSwatches)
				{
					ResizeWindow(842, 98, -1, -1, false);
				}
				assignHKwindowResize = false;
			}
		}
		if(assignHotKeysDropDown)
		{
			if(!assignHKwindowResize)
			{
				ResizeWindow(1050, 98, -1, -1, false);
				assignHKwindowResize = true;
			}
			//close other menus to make room for this one
			if(importOptionsDropDown)
				importOptionsDropDown = false;
			if(colorSwatches)
				colorSwatches = false;
			if(objPainter)
				objPainter = false;
			
			AssignHotKeys(0);
		}
		//Dropdown for extra options
		importOptionsDropDown = GUI.Toggle(new Rect(2,31,34,26),importOptionsDropDown,guiContent[24],customSkin.toggle);
		if(!importOptionsDropDown)
		{
			if(optionsWindowResize)
			{
				if(!objPainter && !assignHotKeysDropDown && !colorSwatches)
				{
					ResizeWindow(842, 98, -1, -1, false);
				}
				optionsWindowResize = false;
			}
		}
		
		if(importOptionsDropDown)
		{			
			if(!optionsWindowResize)
			{
				ResizeWindow(1050, 98, -1, -1, false);
				optionsWindowResize = true;
			}
			//close other menus to make room for this one
			if(assignHotKeysDropDown)
				assignHotKeysDropDown = false;
			if(colorSwatches)
				colorSwatches = false;
			if(objPainter)
				objPainter = false;
			
			ImportOptions(0);
		}
		HotKeyAssignment(0);
		
		GUI.EndGroup();
	}
	//Rebuilds the names of the buttons in the selection grid for the object painter
	public void RebuildNames()
	{
		objNames = new string[objs.Count];
		for(int i = 0; i < objs.Count; i++)
		{
			if(objs[i] == null)
				objNames[i] = "None";
			else
				objNames[i] = objs[i].name;
		}
	}
		
	public void OnDestroy()
	{
		EditorPrefs.SetBool("materialImport", materialImport);
		EditorPrefs.SetBool("animationImport", animationImport);
		EditorPrefs.SetBool("importOptionsDropDown",importOptionsDropDown);
		EditorPrefs.SetBool("objPainter",objPainter);
		EditorPrefs.SetBool("assignHotKeysDropDown",assignHotKeysDropDown);
		
		for(int i = 0; i < toolKeyCodes.Length;i++)
		{
			EditorPrefs.SetString(toolPrefStrings[i], toolKeyStrings[i]);
		}
			
		snapDrag = false;
		snapRotation90 = false;
		
		EditorPrefs.SetInt("stgDir",stgDir);
		EditorPrefs.SetInt("stwDir",stwDir);
		EditorPrefs.SetInt("stcDir",stcDir);
		EditorPrefs.SetInt("rotAxis",rotAxis);
		EditorPrefs.SetInt("rowWidth",rowWidth);
		EditorPrefs.SetInt("snapsMask",snapsMask);
		EditorPrefs.SetInt("painterMask",painterMask);

	}	
	public class SceneMateImport : AssetPostprocessor 
	{   
		public void OnPreprocessTexture()
		{
			for(int i = 0; i < buttonFileNames.Length; i++)
			{
				//load all the icons in the GUI content
				string fileName = "SceneMate/Icons/" + buttonFileNames[i].Substring(0,buttonFileNames[i].Length - 3) + ".psd";
				if(assetPath.EndsWith(fileName))
				{
					TextureImporter curIcon = AssetImporter.GetAtPath(assetPath) as TextureImporter;
					curIcon.wrapMode = TextureWrapMode.Clamp;
					curIcon.filterMode = FilterMode.Point;
					curIcon.textureType = TextureImporterType.Default;
					//curIcon.textureFormat = TextureImporterFormat.RGBA32;
					curIcon.npotScale = TextureImporterNPOTScale.None;
					curIcon.mipmapEnabled = false;
					curIcon.maxTextureSize = 64;
				}
			}
		}
			
		public void OnPreprocessModel() 
		{			
			if (!materialImport)
			{
				ModelImporter modelImporter = (ModelImporter) assetImporter;
				//modelImporter.importMaterials = false;
			}
			if (!animationImport)
			{
				ModelImporter modelImporter = (ModelImporter) assetImporter;
				modelImporter.generateAnimations = ModelImporterGenerateAnimations.None;
			}
	    }
	}
}
