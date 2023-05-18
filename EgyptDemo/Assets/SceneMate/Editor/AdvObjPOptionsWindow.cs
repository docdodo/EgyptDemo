using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class AdvObjPOptionsWindow : SceneMateGUI 
{
	/*
	[MenuItem("Window/SceneMate/AdvObjectPainter")]
	public static void Init()
	{
		//Create the window
		AdvObjPOptionsWindow window = (AdvObjPOptionsWindow)EditorWindow.GetWindow(typeof(AdvObjPOptionsWindow));
		window.minSize = new Vector2(315, 545);
		window.maxSize = new Vector2(316, 546);
		window.title = ("Advanced Options");
	}

	public override void OnEnable()
	{
		base.OnEnable();
		paintObjs = new List<PaintObj>();
		lastPercentage = new float[0];
		recordedGroups = new List<PaintedGroup>();
		recordedGroups.Add(new PaintedGroup());
		recordedGroups[0].groupName = "___New Group___";
	}

	public override void AdvObjectPainterOptions()
	{
		//protected bool paintGroup = false;
		//protected float brushSize;
		//protected float brushDensity;
		//protected bool modifyPaintedGroup;
		//protected List<PaintedGroup> OPgroups;
		//protected List<PaintObj> currentPaintGroup;
		//protected bool allowOverlap;
		//Paint groups, single object, multiple objects, multiple objects with defined percentages of occurence
		//paint brush size/(density/hardness)
		//Modify painted objects scale/rotation/color
		//position offset after painting

		paintGroup = GUI.Toggle(new Rect(8,4,80,38),paintGroup,"Paint Group",customSkin3.toggle);
		if(paintGroup)
			modifyPaintedGroup = false;

		GUI.Label(new Rect(120,8,60,16),"Brush:");
		GUI.Label(new Rect(190,8,60,16),"radius");
		brushSize = EditorGUI.FloatField(new Rect(230,8,50,16),"", brushSize);
		if(brushSize < 0)
			brushSize = 0;
		if(brushSize > 100)
			brushSize = 100;
		GUI.Label(new Rect(116,26,60,16),"density");
		brushDensity = EditorGUI.Slider(new Rect(164,26,116,16),"", brushDensity,0,1);
		GUI.Label(new Rect(122,44,60,16),"falloff");
		brushFalloff = EditorGUI.Slider(new Rect(164,44,116,16),"", brushFalloff,0,1);

		allowOverlap = GUI.Toggle(new Rect(6,44,120,16),allowOverlap, "Allow Overlap");

		GUI.BeginGroup(new Rect(6,64,304,241),customSkin.textArea);
		if(GUI.Button(new Rect(0,0,17,16),"+",customSkin.button))
		{
			paintObjs.Add(new PaintObj());
		}
		if(paintObjs.Count == 0)
		{
			paintObjs.Add(new PaintObj());
		}
		int spacing = 38;
		if(!showOptions)
			spacing = 16;
		AOPvScrollPos = GUI.VerticalScrollbar(new Rect(288,0,16,240),AOPvScrollPos,1,0,paintObjs.Count*spacing - 240);
		
		for(int i = 0; i < paintObjs.Count; i++)
		{
			paintObjs[i].obj = EditorGUI.ObjectField(new Rect(27,i*spacing-AOPvScrollPos,138,16),"",paintObjs[i].obj, typeof(GameObject), true) as GameObject;
			if(usePercentages)
			{
				paintObjs[i].percentage = EditorGUI.Slider(new Rect(170,i*spacing-AOPvScrollPos,115,16),"",paintObjs[i].percentage,0,100);
			}
			if(GUI.Button(new Rect(17,i*spacing-AOPvScrollPos,10,16),"X",customSkin.button))
			{
				paintObjs.RemoveAt(i);
			}
			if(paintObjs.Count > 0 && showOptions)
			{
				paintObjs[i].randRot = GUI.Toggle(new Rect(13,i*spacing+16-AOPvScrollPos,52,14),paintObjs[i].randRot,"rand rot",customSkin3.toggle);
				paintObjs[i].randRotX = GUI.Toggle(new Rect(68,i*spacing+16-AOPvScrollPos,12,14),paintObjs[i].randRotX,"x",customSkin3.toggle);
				paintObjs[i].randRotY = GUI.Toggle(new Rect(78,i*spacing+16-AOPvScrollPos,12,14),paintObjs[i].randRotY,"y",customSkin3.toggle);
				paintObjs[i].randRotZ = GUI.Toggle(new Rect(88,i*spacing+16-AOPvScrollPos,12,14),paintObjs[i].randRotZ,"z",customSkin3.toggle);

				paintObjs[i].randomRotSnap = EditorGUI.FloatField(new Rect(100,i*spacing+16-AOPvScrollPos,30,16),"", paintObjs[i].randomRotSnap);

				paintObjs[i].randScale = GUI.Toggle(new Rect(142,i*spacing+16-AOPvScrollPos,62,14),paintObjs[i].randScale,"rand scale",customSkin3.toggle);
				paintObjs[i].uniformScale = GUI.Toggle(new Rect(206,i*spacing+16-AOPvScrollPos,16,16),paintObjs[i].uniformScale, "");
				paintObjs[i].randScaleMin = EditorGUI.FloatField(new Rect(222,i*spacing+16-AOPvScrollPos,30,16),"", paintObjs[i].randScaleMin);
				paintObjs[i].randScaleMax = EditorGUI.FloatField(new Rect(255,i*spacing+16-AOPvScrollPos,30,16),"", paintObjs[i].randScaleMax);
			}
		}
		GUI.EndGroup();

		usingPercentages = usePercentages;
		usePercentages = GUI.Toggle(new Rect(6,304,120,16),usePercentages, "Use Percentages");

		showOptions = GUI.Toggle(new Rect(6,320,120,16),showOptions, "Show Options");

		GUI.Label(new Rect(162,304,60,16),"Offset:");

		curOffsetPosAxis = EditorGUI.Popup(new Rect(206,304,58,16),"",curOffsetPosAxis,offsetPosAxis);

		willOffsetPosX = GUI.Toggle(new Rect(130,320,16,16),willOffsetPosX,"x",customSkin3.toggle);
		willOffsetPosY = GUI.Toggle(new Rect(188,320,16,16),willOffsetPosY,"y",customSkin3.toggle);
		willOffsetPosZ = GUI.Toggle(new Rect(246,320,16,16),willOffsetPosZ,"z",customSkin3.toggle);
		
		offsetPosX = EditorGUI.FloatField(new Rect(146,320,40,16),"", offsetPosX);
		offsetPosY = EditorGUI.FloatField(new Rect(204,320,40,16),"", offsetPosY);
		offsetPosZ = EditorGUI.FloatField(new Rect(262,320,40,16),"", offsetPosZ);

		GUI.Label(new Rect(6,340,100,16),"Assign Parent");
		AOPassignParent = EditorGUI.ObjectField(new Rect(92,340,180,16),"",AOPassignParent, typeof(GameObject), true) as GameObject;

		if(!usingPercentages && usePercentages)
		{
			usingPercentages = true;
			AveragePercentages();
		}
		if(usePercentages)
			AdjustPercentages();
		isRecording = recordPaintedObjs;
		recordPaintedObjs = GUI.Toggle(new Rect(6,372,110,36),recordPaintedObjs,"Record Painting",customSkin3.toggle);
		if(!paintGroup)
		{
			if(recordPaintedObjs)
			{
				recordPaintedObjs = false;
				Debug.Log("Paint Group must be enabled to enable Record Painting");
			}
		}
		if(!isRecording && recordPaintedObjs)
		{
			isRecording = recordPaintedObjs;
			if(recordedGroups[curSelectedGroup].groupName == "___New Group___")
			{
				bool isInList = false;
				for(int i = 0; i < recordedGroupNames.Length; i++)
				{
					if(recordedGroupNames[i] == groupNameInput)
					{
						isInList = true;
						Debug.Log("There is already a group with that name");
						recordPaintedObjs = false;
					}

				}
				if(!string.IsNullOrEmpty(groupNameInput) && !isInList)
				{
					recordedGroups[curSelectedGroup].groupName = groupNameInput;
					recordedGroups.Add(new PaintedGroup());
					recordedGroups[curSelectedGroup+1].groupName = "___New Group___";
					groupNameInput = "";
				}
				else if(!isInList)
				{
					Debug.Log("Please enter a group name, or select another group to record to, ___New Group___ is not a valid group");
					recordPaintedObjs = false;
				}
			}
		}

		GUI.Label(new Rect(120,373,110,16),"Group Name:");
		groupNameInput = GUI.TextField(new Rect(200,372,110,16),groupNameInput);

		recordedGroupNames = new string[recordedGroups.Count];
		for(int i = 0; i < recordedGroups.Count; i++)
		{
			recordedGroupNames[i] = recordedGroups[i].groupName;
		}

		lastGroup = curSelectedGroup;
		curSelectedGroup = EditorGUI.Popup(new Rect(120,392,190,16),"",curSelectedGroup,recordedGroupNames);
		if(lastGroup != curSelectedGroup)
		{
			lastGroup = curSelectedGroup;
			recordPaintedObjs = false;
		}
		if(GUI.Button(new Rect(6,414,100,16),"Save Groups"))
		{
			
		}
		if(GUI.Button(new Rect(6,431,100,16),"Load Groups"))
		{
			
		}
		if(GUI.Button(new Rect(120,414,190,16),"Add Selection to Group"))
		{

		}
		if(GUI.Button(new Rect(160,431,150,16),"Select Group Objs"))
		{
			
		}

		GUI.BeginGroup(new Rect(6,450,304,93),customSkin.textArea);
		//Color
		willModifyColor = GUI.Toggle(new Rect(16,10,58,16),willModifyColor,"Color",customSkin3.toggle);

		modifyColor = EditorGUI.ColorField(new Rect(18,28,74,16),"",modifyColor);

		//Rotation
		willModifyRot = GUI.Toggle(new Rect(109,10,58,16),willModifyRot,"Rotation",customSkin3.toggle);

		curModRotAxis = EditorGUI.Popup(new Rect(109,28,58,16),"",curModRotAxis,modRotAxis);

		willModifyRotX = GUI.Toggle(new Rect(110,44,16,16),willModifyRotX,"x",customSkin3.toggle);
		willModifyRotY = GUI.Toggle(new Rect(110,60,16,16),willModifyRotY,"y",customSkin3.toggle);
		willModifyRotZ = GUI.Toggle(new Rect(110,76,16,16),willModifyRotZ,"z",customSkin3.toggle);

		modifyRotX = EditorGUI.FloatField(new Rect(126,44,40,16),"", modifyRotX);
		modifyRotY = EditorGUI.FloatField(new Rect(126,60,40,16),"", modifyRotY);
		modifyRotZ = EditorGUI.FloatField(new Rect(126,76,40,16),"", modifyRotZ);

		//Position
		willModifyPos = GUI.Toggle(new Rect(175,10,58,16),willModifyPos,"Position",customSkin3.toggle);

		curModPosAxis = EditorGUI.Popup(new Rect(175,28,58,16),"",curModPosAxis,modPosAxis);

		willModifyPosX = GUI.Toggle(new Rect(176,44,16,16),willModifyPosX,"x",customSkin3.toggle);
		willModifyPosY = GUI.Toggle(new Rect(176,60,16,16),willModifyPosY,"y",customSkin3.toggle);
		willModifyPosZ = GUI.Toggle(new Rect(176,76,16,16),willModifyPosZ,"z",customSkin3.toggle);
		
		modifyPosX = EditorGUI.FloatField(new Rect(192,44,40,16),"", modifyPosX);
		modifyPosY = EditorGUI.FloatField(new Rect(192,60,40,16),"", modifyPosY);
		modifyPosZ = EditorGUI.FloatField(new Rect(192,76,40,16),"", modifyPosZ);

		//Scale
		willModifyScale = GUI.Toggle(new Rect(241,28,58,16),willModifyScale,"Scale",customSkin3.toggle);
		willModifyScaleX = GUI.Toggle(new Rect(242,44,16,16),willModifyScaleX,"x",customSkin3.toggle);
		willModifyScaleY = GUI.Toggle(new Rect(242,60,16,16),willModifyScaleY,"y",customSkin3.toggle);
		willModifyScaleZ = GUI.Toggle(new Rect(242,76,16,16),willModifyScaleZ,"z",customSkin3.toggle);
		
		modifyScaleX = EditorGUI.FloatField(new Rect(258,44,40,16),"", modifyScaleX);
		modifyScaleY = EditorGUI.FloatField(new Rect(258,60,40,16),"", modifyScaleY);
		modifyScaleZ = EditorGUI.FloatField(new Rect(258,76,40,16),"", modifyScaleZ);

		modifyPaintedGroup = GUI.Toggle(new Rect(8,52,80,38),modifyPaintedGroup,"Modify Group",customSkin3.toggle);
		if(modifyPaintedGroup)
		{
			if(CheckForValidGroup())
			{
				recordPaintedObjs = false;
				paintGroup = false;
			}
			else
			{
				modifyPaintedGroup = false;
			}
		}

		GUI.EndGroup();
	}

	bool CheckForValidGroup()
	{
		if(recordedGroups[curSelectedGroup] != null && recordedGroups[curSelectedGroup].paintedObjs != null)
		{
			if(recordedGroups[curSelectedGroup].groupName != "___New Group___")
			{
				return true;
			}
		}
		Debug.Log("Please select a valid group to modify. A valid group needs to have atleast 1 painted object and not named ___New Group___");
		return false;
	}

	void AveragePercentages()
	{
		float amountForEach = 100/paintObjs.Count;
		for(int i = 0; i < paintObjs.Count; i++)
		{
			paintObjs[i].percentage = amountForEach;
		}
	}

	void AdjustPercentages()
	{
		float total = 0;
		int changedIndex = -1;
		for(int i = 0; i < paintObjs.Count; i++)
		{
			if(paintObjs[i].percentage > 100)
				paintObjs[i].percentage = 100;
			total += paintObjs[i].percentage;

			if(lastPercentage.Length == paintObjs.Count)
			{
				if(paintObjs[i].percentage != lastPercentage[i])
					changedIndex = i;
			}
		}
		lastPercentage = new float[paintObjs.Count];
		for(int i = 0; i < paintObjs.Count; i++)
		{
			lastPercentage[i] = paintObjs[i].percentage;
		}
		if(total > 100)
		{
			MinusFromOtherPercentages(total, changedIndex);
		}
		if(total < 100)
		{
			if(paintObjs.Count > 1)
			{
				AddToOtherPercentages(total, changedIndex);
			}
		}
	}

	void MinusFromOtherPercentages(float total, int index)
	{
		float amountToMinus = total - 100;
		int moreThanZero = 0;
		for(int i = 0; i < paintObjs.Count; i++)
		{
			if(paintObjs[i].percentage > 0 && i != index)
			{
				moreThanZero++;
			}
		}
		if(moreThanZero != 0)
		{
			float amountForEach = amountToMinus/moreThanZero;

			for(int i = 0; i < paintObjs.Count; i++)
			{
				if(paintObjs[i].percentage > 0 && i != index && amountToMinus > 0)
				{
					if(paintObjs[i].percentage >= amountForEach)
					{
						paintObjs[i].percentage -= amountForEach;
						amountToMinus -= amountForEach;
					}
					else if(paintObjs[i].percentage < amountForEach)
					{
						amountToMinus -= paintObjs[i].percentage; 
						paintObjs[i].percentage = 0;
					}
				}
			}
			if(amountToMinus > 0)
				MinusFromOtherPercentages(100 + amountToMinus, index);
		}
	}
	void AddToOtherPercentages(float total, int index)
	{
		float amountToAdd = 100 - total;
		int lessThan100 = 0;
		for(int i = 0; i < paintObjs.Count; i++)
		{
			if(paintObjs[i].percentage < 100 && i != index)
			{
				lessThan100++;
			}
		}
		if(lessThan100 != 0)
		{
			float amountForEach = amountToAdd/lessThan100;
			
			for(int i = 0; i < paintObjs.Count; i++)
			{
				if(i != index && amountToAdd > 0)
				{
					if((100-paintObjs[i].percentage) < amountForEach)
					{
						amountToAdd -= (100-paintObjs[i].percentage);
						paintObjs[i].percentage = 100;
					}
					else if(100-paintObjs[i].percentage >= amountForEach)
					{
						paintObjs[i].percentage += amountForEach;
						amountToAdd -= amountForEach; 
					}
				}
			}
		}
		if(amountToAdd > 0)
			AddToOtherPercentages(100-amountToAdd, index);
	}
	
	public override void OnGUI()
	{
		CustomUIStyles();
		OnSceneGUIDelegate();

		AdvObjectPainterOptions();
	}
*/
}
