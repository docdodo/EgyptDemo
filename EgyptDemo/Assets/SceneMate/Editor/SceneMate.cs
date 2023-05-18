using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SceneMate : EditorWindow
{
	// Hot Keys
	protected bool assignHotKeysDropDown;
	protected bool assignHKwindowResize;
	protected bool assignNextKey;
	protected bool[] toolAssignKey = new bool[32];
	protected KeyCode[] toolKeyCodes = 
	{KeyCode.F1,KeyCode.F2,KeyCode.G,KeyCode.None,KeyCode.C,KeyCode.None,
		KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,
		KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.Alpha1,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,
		KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None,KeyCode.None};
	protected string[] toolKeyStrings = new string[32];
	protected string[] toolPrefStrings = {"QtBtn1","QtBtn2","QtBtn3","QtBtn4","QtBtn5",
		"QtBtn6","QtBtn7","QtBtn8","QtBtn9","QtBtn10","QtBtn11","QtBtn12","QtBtn13","QtBtn14","QtBtn15","QtBtn16","QtBtn17","QtBtn18","QtBtn19","QtBtn20","QtBtn21","QtBtn22","QtBtn23"
		,"QtBtn24","QtBtn25","QtBtn26","QtBtn27","QtBtn28","QtBtn29","QtBtn30","QtBtn31","QtBtn31"};
	protected string[] hkBtnNames = {"Rotate Clockwise","Rotate Counter Clockwise","Snap to Ground","Snap to Wall",
		"Snap to Center","Reset Objects","Match Position Target ","Match Rotation Target",
		"Match Scale Target","Offset Selected","Selected Objects Amount","Snap to Surface","Flip X","Flip Y","Flip Z","Object Painter","Fit Collider",
		"Cycle Rotation Axis","Replace Selection","Color Swatches","Snap Drag", "Align to Normal","Snap Rotation 90", "Randomize Rotation", "Randomize Scale", "Randomize Position",
		"Distribute","Align High","Align Low","Set Spacing","Bounds Spacing","Bounds Snapping"};
	protected bool[] modControl = new bool[32];
	protected bool[] modShift = new bool[32];
	protected bool[] modAlt = new bool[32];
	protected int assignHKsel;
	
	//Popup Menus
	protected string[] rotDir =
	{"(X)","(Y)","(Z)"};
	protected string[] snapDir =
	{"-X","+X","-Y","+Y","-Z","+Z"};
	protected Vector3[] snapDirVectors = 
	{Vector3.left,Vector3.right,Vector3.down,Vector3.up,Vector3.back,Vector3.forward};
	protected int rotAxis = 1;
	protected int stgDir = 2;
	protected int stwDir = 4;
	protected int stcDir = 2;
	
	//Offset Selected
	protected float offsetXAmount;
	protected float offsetYAmount;
	protected float offsetZAmount;
	protected string[] offsetStyle =
	{"Add","Multiply","Divide"};
	protected int curOffsetStyle;
	
	//Rotation Stuff
	protected bool snapRotation90;
	protected float rotationAmount = 10f;
	protected int rotateCurAxis;
	
	//Option Menu
	public static bool materialImport = true;
	public static bool animationImport = true;
	protected bool importOptionsDropDown;
	protected bool optionsWindowResize;
	
	//Color Swatches
	protected bool colorSwatches;
	protected bool CSwindowResize;
	protected Color color1 = new Color(1,1,1,1);
	protected Color curColor;
	protected int removeColor;
	protected List<Color> colorList = new List<Color>();
	
	//Fit Collider
	protected GameObject colSelectedObj = null;
	protected Object[] colSelectedObjArr = new Object[1];
	
	public class PaintedGroup
	{
		public string groupName;
		public List<GameObject> paintedObjs;
	}
	public class PaintObj
	{
		public GameObject obj;
		public float percentage;
		public bool randRot;
		public bool randScale;
		public bool uniformScale = true;
		public float randScaleMin = .5f;
		public float randScaleMax = 2;
		public bool randRotX;
		public bool randRotY = true;
		public bool randRotZ;
		public float randomRotSnap = 0;
	}

	//Object Painter Variables
	protected bool objPainter;
	protected bool wasMouseDown;
	protected bool selectLast;
	protected bool painterKeyDown;
	protected bool assignParent;
	protected bool randRot;
	protected bool randScale;
	protected bool uniformScale = true;
	protected float randScaleMin = .5f;
	protected float randScaleMax = 2;
	protected bool randRotX;
	protected bool randRotY = true;
	protected bool randRotZ;
	protected float randomRotSnap = 0;
	protected bool OPwindowResize;
	protected bool eraserMode;
	protected int removeObj;
	protected int rowWidth = 3;
	protected int curObject;
	protected string[] objNames;
	protected GameObject newObject;
	protected GameObject parentObj = null;
	protected Object[] parentObjs = new Object[1];
	protected List<Object> objs = new List<Object>();
	protected bool paintGroup = false;
	protected float brushSize;
	protected float brushDensity;
	protected float brushFalloff;
	protected bool modifyPaintedGroup;
	protected List<PaintedGroup> recordedGroups;
	protected string[] recordedGroupNames;
	protected int curSelectedGroup;
	protected List<PaintObj> paintObjs;
	protected bool allowOverlap = true;
	protected bool recordPaintedObjs;
	protected float[] lastPercentage;
	protected bool usePercentages = false;
	protected bool usingPercentages = false;
	protected bool isRecording = false;
	protected string groupNameInput = "";
	protected int lastGroup;
	protected bool showOptions;
	protected bool isValidGroup;
	protected float modifyScale;
	protected int curModRotAxis;
	protected string[] modRotAxis =
	{"Global","Local"};
	protected float modifyRotX;
	protected float modifyRotY;
	protected float modifyRotZ;
	protected int curModPosAxis;
	protected string[] modPosAxis =
	{"Global","Local"};
	protected float modifyPosX;
	protected float modifyPosY;
	protected float modifyPosZ;
	protected float modifyScaleX;
	protected float modifyScaleY;
	protected float modifyScaleZ;
	protected bool willModifyScale;
	protected bool willModifyScaleX;
	protected bool willModifyScaleY;
	protected bool willModifyScaleZ;
	protected bool willModifyRot;
	protected bool willModifyRotX;
	protected bool willModifyRotY;
	protected bool willModifyRotZ;
	protected bool willModifyPos;
	protected bool willModifyPosX;
	protected bool willModifyPosY;
	protected bool willModifyPosZ;
	protected bool willModifyColor;
	protected Color modifyColor;

	protected int curOffsetPosAxis;
	protected string[] offsetPosAxis =
	{"Global","Local"};
	protected bool willOffsetPosX;
	protected bool willOffsetPosY;
	protected bool willOffsetPosZ;
	protected float offsetPosX;
	protected float offsetPosY;
	protected float offsetPosZ;

	protected GameObject AOPassignParent;
	
	//Match Variables
	protected bool waitForSelMatch = false;
	protected bool matchPos;
	protected bool matchRot;
	protected bool matchScale;
	protected bool grabSource;
	protected GameObject[] sourceObjs;
	
	protected bool posMatchX = true;
	protected bool posMatchY = true;
	protected bool posMatchZ = true;
	protected bool rotMatchX = true;
	protected bool rotMatchY = true;
	protected bool rotMatchZ = true;
	protected bool scaleMatchX = true;
	protected bool scaleMatchY = true;
	protected bool scaleMatchZ = true;
	
	//Snap Stuff
	protected Vector3 handlePos = new Vector3(0,0,0);
	protected Vector3 handleNormal = new Vector3(0,0,0);
	protected bool snapSurface;
	protected bool boundsSnapping;
	protected bool alignToNormal = true;
	protected bool snapDrag;
	protected bool dragDirNegX = true;
	protected bool dragDirPosX = true;
	protected bool dragDirNegY = true;
	protected bool dragDirPosY = true;
	protected bool dragDirNegZ = true;
	protected bool dragDirPosZ = true;
	protected Vector3 wallSnapRayStart;
	protected float snapDragThreshold = 1;
	protected Vector3 boundsObjPos = new Vector3(0,0,0);
	protected Transform lclBoundsObj;
	protected float snapOffset;
	
	//Distribute and Align
	protected bool disX = true;
	protected bool disY;
	protected bool disZ;
	protected bool disBySize;
	protected bool setSpacing;
	protected float disSpacing;
	protected bool reverseDis;
	protected bool alignX = true;
	protected bool alignY;
	protected bool alignZ;
	
	//Button Icons and Tooltips
	protected string[] toolTips =
	{"Rotate Clockwise","Rotate Counter Clockwise","Snap to Ground: \nGlobal Axis","Snap to Wall: \nLocal Axis",
		"Snap to Center: \nGlobal Axis ","Reset Objects: \nReset Position, Rotation, Scale","Replace Selection","Match Position Target","Match Rotation Target",
		"Match Scale Target","Offset Selected","Debug Log Selected Objects","Snap to Surface","Flip X","Flip Y",
		"Flip Z","Object Painter","Fit Collider to Mesh","Cycle Rotation Axis","Align To Normal","Snap Rotation 90",
		"Snap Drag","Color Swatches","Assign Hot Keys","Import Options","Randomize Rotation Selected","Randomize Scale Selected","Randomize Position Selected",
		"Distribute","Align High","Align Low","Set Spacing","Space Object By Size, Using Object Bounds","Snap Using Object Bounds"};
	protected static string[] buttonFileNames =
	{"Rotate Left_00","Rotate Right_01","Snap to Ground_02","Snap to Wall_03",
		"Snap to Center_04","Reset Objects_05","Replace Selection_06","Match Pos Tgt_07","Match Rot Tgt_08",
		"Match Scale Tgt_09","Offset Selected_10","Selected Objs Amt_11","Snap to Surface_12","Flip X_13",
		"Flip Y_14","Flip Z_15","Object Painter_16","Fit Collider_17","Cycle Rotation Axis_18","Align To Normal_19",
		"Snap Rotation 90_20","Snap Drag_21","Color Swatches_22","Assign Hot Keys_23","Import Options_24",
		"Randomize Rotation_25","Randomize Scale_26","Randomize Position_27","Distribute_28","Align High_29","Align Low_30",
		"Set Spacing_31","By Size_32","Bounds Snap_33"};
	protected GUIContent[] guiContent = new GUIContent[34];
	protected GUISkin customSkin;
	protected GUISkin customSkin2;
	protected GUISkin customSkin3;
	
	//GUI Groups
	protected int optPixelHeight;
	protected int pixelHeight;
	
	//Scroll Bars
	protected float vScrollPos;
	protected float AOPvScrollPos;
	
	//Replace Selection
	protected bool inheritPos = true;
	protected bool inheritRot = true;
	protected bool inheritScale = true;
	protected bool keepOrig;
	
	//Layer Masking
	public static List<string> layers;
	public static List<int> layerNumbers;
	public static string[] layerNames;
	public static long lastUpdateTick;
	protected LayerMask snapsMask = -1;
	protected LayerMask painterMask = -1;
	
	//Randomize Variables
	protected string[] axis =
	{"Global","Local"};
	protected int randRotCurAxis;
	protected int randPosCurAxis;
	protected bool scaleX = true;
	protected bool scaleY = true;
	protected bool scaleZ = true;
	protected bool rotX = true;
	protected bool rotY = true;
	protected bool rotZ = true;
	protected bool positionX = true;
	protected bool positionY = true;
	protected bool positionZ = true;
	protected bool randScaleUniform = true;
	protected float randomizeScaleMin = .5f;
	protected float randomizeScaleMax = 2;
	protected float randomizePosMin = .5f;
	protected float randomizePosMax = 2;
	protected float randomizeRotSnap = 0;
	
	//Replace Selection
	protected int offsetSelCurAxis;
	
	//Selected Amount Debug
	protected List<string> componentNames = new List<string>();
	protected List<int> componentNumber = new List<int>();
	protected List<string> tagNames = new List<string>();
	protected List<int> tagNumber = new List<int>();
	protected List<string> layerNamesList = new List<string>();
	protected List<int> layerNumberList = new List<int>();
	protected int vertCount;
	
	//Method Names
	protected string[] methodNames = {
		"RotateLeft","RotateRight","SnapToGround","SnapToWall","SnapToCenter","ResetObjs","MatchPosToggle","MatchRotToggle","MatchScaleToggle","OffsetSel","SelAmt",
		"","FlipX","FlipY","FlipZ","ObjPainterToggle","FitCollider","CycleRotationAxis","ReplaceSelection","ColorSwatchesToggle","SnapDragToggle","AlignToNormalToggle",
		"SnapRotation90Toggle","RandomizeRotation","RandomizeScale","RandomizePosition","Distribute","AlignHigh","AlignLow","SetSpacingToggle","BySizeToggle","BoundsSnapToggle"};
	
	public void MatchPosToggle()
	{
		if(Selection.transforms.Length > 0)
		{
			matchPos = true;
			waitForSelMatch = true;
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void MatchRotToggle()
	{
		if(Selection.transforms.Length > 0)
		{
			matchRot = true;
			waitForSelMatch = true;	
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void MatchScaleToggle()
	{
		if(Selection.transforms.Length > 0)
		{
			matchScale = true;
			waitForSelMatch = true;	
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void ObjPainterToggle()
	{
		if(!objPainter)
			objPainter = true;
		else
			objPainter = false;
		Repaint ();
	}
	
	public void ColorSwatchesToggle()
	{
		if(!colorSwatches)
			colorSwatches = true;
		else
			colorSwatches = false;
		Repaint ();
	}
	
	public void SnapDragToggle()
	{
		if(!snapDrag)
			snapDrag = true;
		else
			snapDrag = false;
		Repaint ();
	}
	
	public void AlignToNormalToggle()
	{
		if(!alignToNormal)
			alignToNormal = true;
		else
			alignToNormal = false;
		Repaint ();
	}
	
	public void SnapRotation90Toggle()
	{
		if(!snapRotation90)
			snapRotation90 = true;
		else
			snapRotation90 = false;
		Repaint ();
	}
	public void SetSpacingToggle()
	{
		if(!setSpacing)
			setSpacing = true;
		else
			setSpacing = false;
		Repaint ();
	}
	public void BySizeToggle()
	{
		if(!disBySize)
			disBySize = true;
		else
			disBySize = false;
		Repaint ();
	}
	public void BoundsSnapToggle()
	{
		if(!boundsSnapping)
			boundsSnapping = true;
		else
			boundsSnapping = false;
		Repaint ();
	}
	
	public void RotateLeft()
	{
		if (Selection.transforms.Length > 0)
		{
			if(rotateCurAxis == 1)
			{
				if(rotAxis == 0)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate (rotationAmount,0,0);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.x = Mathf.Round(tempRot.x);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 1)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate (0,rotationAmount,0);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.y = Mathf.Round(tempRot.y);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 2)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate (0,0,rotationAmount);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.z = Mathf.Round(tempRot.z);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
			}
			else
			{
				if(rotAxis == 0)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate(rotationAmount,0,0,Space.World);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.x = Mathf.Round(tempRot.x);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 1)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						//Selection.transforms[r].Rotate(new Vector3(0,1,0),rotationAmount/57.296f);
						Selection.transforms[r].Rotate(0,rotationAmount,0,Space.World);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.y = Mathf.Round(tempRot.y);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 2)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate(0,0,rotationAmount,Space.World);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.z = Mathf.Round(tempRot.z);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
			}			
		}
	}
	
	public void RotateRight()
	{
		if (Selection.transforms.Length > 0)
		{
			if(rotateCurAxis == 1)
			{
				if(rotAxis == 0)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate (-rotationAmount,0,0);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.x = Mathf.Round(tempRot.x);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 1)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate (0,-rotationAmount,0);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.y = Mathf.Round(tempRot.y);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 2)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate (0,0,-rotationAmount);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.z = Mathf.Round(tempRot.z);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
			}
			else
			{
				if(rotAxis == 0)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate(-rotationAmount,0,0,Space.World);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.x = Mathf.Round(tempRot.x);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 1)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						//Selection.transforms[r].Rotate(new Vector3(0,1,0),rotationAmount/57.296f);
						Selection.transforms[r].Rotate(0,-rotationAmount,0,Space.World);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.y = Mathf.Round(tempRot.y);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
				if(rotAxis == 2)
				{
					for(int r = 0;r < Selection.transforms.Length; r++)
					{
#if !UNITY_3_5
						Undo.RecordObject(Selection.transforms[r], "Rotate");
#else
						Undo.RegisterUndo(Selection.transforms[r], "Rotate");
#endif
						Selection.transforms[r].Rotate(0,0,-rotationAmount,Space.World);
						Vector3 tempRot = Selection.transforms[r].eulerAngles;
						tempRot.z = Mathf.Round(tempRot.z);
						Selection.transforms[r].eulerAngles = tempRot;
					}
				}
			}
		}
	}
	
	public void BoundsGblObjSnap(Vector3 snapDir, Vector3 hitPoint, Vector3 boundsExtents)
	{
		if (snapDir == snapDirVectors[0])
		{
			float negX = hitPoint.x + boundsExtents.x;
			boundsObjPos = new Vector3(negX,hitPoint.y,hitPoint.z);
		}
		if (snapDir == snapDirVectors[1])
		{
			float posX = hitPoint.x - boundsExtents.x;
			boundsObjPos = new Vector3(posX,hitPoint.y,hitPoint.z);
		}
		if (snapDir == snapDirVectors[2])
		{
			float negY = hitPoint.y + boundsExtents.y;
			boundsObjPos = new Vector3(hitPoint.x,negY,hitPoint.z);
		}
		if (snapDir == snapDirVectors[3])
		{
			float posY = hitPoint.y - boundsExtents.y;
			boundsObjPos = new Vector3(hitPoint.x,posY,hitPoint.z);
		}
		if (snapDir == snapDirVectors[4])
		{
			float negZ = hitPoint.z + boundsExtents.z;
			boundsObjPos = new Vector3(hitPoint.x,hitPoint.y,negZ);
		}
		if (snapDir == snapDirVectors[5])
		{
			float posZ = hitPoint.z - boundsExtents.z;
			boundsObjPos = new Vector3(hitPoint.x,hitPoint.y,posZ);
		}
	}
	
	public void SnapToGround()
	{
		if (Selection.transforms.Length > 0)
		{
			RaycastHit hitInfo;
			for (int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Snap To Ground");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Snap To Ground");
#endif
				int currentLayer = Selection.transforms[i].gameObject.layer;
				Selection.transforms[i].gameObject.layer = 2;
				Vector3 snapRayStart = Selection.transforms[i].position;
				//snapRayStart = snapRayStart + new Vector3(0,1,0);
	
				if (Physics.Raycast(snapRayStart,snapDirVectors[stgDir], out hitInfo, Mathf.Infinity,snapsMask))
				{
					if (hitInfo.point != snapRayStart)
					{
						if(alignToNormal)
						{
							Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
						}
						if(boundsSnapping)
						{
							BoundsGblObjSnap(snapDirVectors[stgDir],hitInfo.point,Selection.transforms[i].GetComponent<Collider>().bounds.extents);
							Selection.transforms[i].position = boundsObjPos;
						}
						else
						{
							Selection.transforms[i].position = hitInfo.point;
						}
						//snapOffset
						if(stgDir == 0 || stgDir == 1)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x + snapOffset, Selection.transforms[i].position.y, Selection.transforms[i].position.z);
						if(stgDir == 2 || stgDir == 3)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y + snapOffset, Selection.transforms[i].position.z);
						if(stgDir == 4 || stgDir == 5)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y, Selection.transforms[i].position.z + snapOffset);
					}
				}
				Selection.transforms[i].gameObject.layer = currentLayer;				
			}
		}
	}
	
	public void SnapToWall()
	{
		if (Selection.transforms.Length > 0)
		{
			RaycastHit hitInfo;
			for (int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Snap To Wall");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Snap To Wall");
#endif
				int currentLayer = Selection.transforms[i].gameObject.layer;
				Selection.transforms[i].gameObject.layer = 2;
				Vector3 snapRayStart = Selection.transforms[i].position;// + new Vector3(0,.05f,0);
				
				{
					if(stwDir == 0)
					{
						if (Physics.Raycast(snapRayStart,-Selection.transforms[i].right, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(tempMesh.bounds.extents.x,0,0);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x + snapOffset, Selection.transforms[i].position.y, Selection.transforms[i].position.z);
							}
						}
					}
					if(stwDir == 1)
					{
						if (Physics.Raycast(snapRayStart,Selection.transforms[i].right, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(-tempMesh.bounds.extents.x,0,0);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x + snapOffset, Selection.transforms[i].position.y, Selection.transforms[i].position.z);
							}
						}
					}
					if(stwDir == 2)
					{
						if (Physics.Raycast(snapRayStart,-Selection.transforms[i].up, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(0,tempMesh.bounds.extents.y,0);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y + snapOffset, Selection.transforms[i].position.z);
							}
						}
					}
					if(stwDir == 3)
					{
						if (Physics.Raycast(snapRayStart,Selection.transforms[i].up, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(0,-tempMesh.bounds.extents.y,0);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y + snapOffset, Selection.transforms[i].position.z);
							}
						}
					}
					if(stwDir == 4)
					{
						if (Physics.Raycast(snapRayStart,-Selection.transforms[i].forward, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(0,0,tempMesh.bounds.extents.z);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
							}
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y, Selection.transforms[i].position.z + snapOffset);
						}
					}
					if(stwDir == 5)
					{
						if (Physics.Raycast(snapRayStart,Selection.transforms[i].forward, out hitInfo, Mathf.Infinity,snapsMask))
						{
							if (hitInfo.point != snapRayStart)
							{
								if(boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
									Mesh tempMesh = Selection.transforms[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
									if(tempMesh != null)
									{
										Vector3 tempTranslate = new Vector3(0,0,-tempMesh.bounds.extents.z);
										Selection.transforms[i].Translate(tempTranslate);
									}
								}
								if(alignToNormal)
								{
									Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
								}
								if(!boundsSnapping)
								{
									Selection.transforms[i].position = hitInfo.point;
								}
								Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y, Selection.transforms[i].position.z + snapOffset);
							}
						}
					}
				}
				Selection.transforms[i].gameObject.layer = currentLayer;
			}
		}
	}
	
	public void SnapToCenter()
	{
		if (Selection.transforms.Length > 0)
		{
			RaycastHit hitInfo;
			for (int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Snap To Center");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Snap To Center");
#endif
				int currentLayer = Selection.transforms[i].gameObject.layer;
				Selection.transforms[i].gameObject.layer = 2;
				Vector3 snapRayStart = Selection.transforms[i].position;
				//snapRayStart = snapRayStart + new Vector3(0,1,0);
	
				if (Physics.Raycast(snapRayStart,snapDirVectors[stcDir], out hitInfo, Mathf.Infinity,snapsMask))
				{
					if (hitInfo.point != snapRayStart)
					{
						if(alignToNormal)
						{
							Selection.transforms[i].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
						}
						if(boundsSnapping)
						{
							BoundsGblObjSnap(snapDirVectors[stcDir],hitInfo.collider.bounds.center,Selection.transforms[i].GetComponent<Collider>().bounds.extents);
							Selection.transforms[i].position = boundsObjPos;
						}
						else
						{
							Selection.transforms[i].position = hitInfo.collider.bounds.center;
						}
						//snapOffset
						if(stcDir == 0 || stcDir == 1)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x + snapOffset, Selection.transforms[i].position.y, Selection.transforms[i].position.z);
						if(stcDir == 2 || stcDir == 3)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y + snapOffset, Selection.transforms[i].position.z);
						if(stcDir == 4 || stcDir == 5)
							Selection.transforms[i].position = new Vector3(Selection.transforms[i].position.x, Selection.transforms[i].position.y, Selection.transforms[i].position.z + snapOffset);
					}
				}
				Selection.transforms[i].gameObject.layer = currentLayer;		
			}
		}
	}
	
	public void ResetObjs()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Reset Object");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Reset Object");
#endif
				Selection.transforms[i].localScale = new Vector3(1,1,1);
				Selection.transforms[i].position = new Vector3(0,0,0);
				Selection.transforms[i].localEulerAngles = new Vector3(0,0,0);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	public void ResetObjsPos()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Reset Object");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Reset Object");
#endif
				Selection.transforms[i].position = new Vector3(0,0,0);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	public void ResetObjsRot()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Reset Object");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Reset Object");
#endif
				Selection.transforms[i].localEulerAngles = new Vector3(0,0,0);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	public void ResetObjsScale()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Reset Object");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Reset Object");
#endif
				Selection.transforms[i].localScale = new Vector3(1,1,1);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void OffsetSel()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Offset Selected");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Offset Selected");
#endif
				Vector3 tempPos = Selection.transforms[i].position;
				if(offsetSelCurAxis == 0)
				{
					if(curOffsetStyle == 0)
					{
						tempPos.x += offsetXAmount;
						tempPos.y += offsetYAmount;
						tempPos.z += offsetZAmount;
						Selection.transforms[i].position = tempPos;
					}
					if(curOffsetStyle == 1)
					{
						tempPos.x = tempPos.x * offsetXAmount;
						tempPos.y = tempPos.y * offsetYAmount;
						tempPos.z = tempPos.z * offsetZAmount;
						Selection.transforms[i].position = tempPos;
					}
					if(curOffsetStyle == 2)
					{
						if(tempPos.x != 0 && offsetXAmount != 0)
							tempPos.x = tempPos.x / offsetXAmount;
						if(tempPos.y != 0 && offsetYAmount != 0)
							tempPos.y = tempPos.y / offsetYAmount;
						if(tempPos.z != 0 && offsetZAmount != 0)
							tempPos.z = tempPos.z / offsetZAmount;
						Selection.transforms[i].position = tempPos;
					}
				}
				else
				{
					if(curOffsetStyle == 0)
					{
						Selection.transforms[i].Translate(offsetXAmount,offsetYAmount,offsetZAmount);
					}
					if(curOffsetStyle == 1)
					{
						Selection.transforms[i].Translate(tempPos.x * offsetXAmount,tempPos.y * offsetYAmount,tempPos.z * offsetZAmount);
					}
					if(curOffsetStyle == 2)
					{
						if(tempPos.x != 0 && offsetXAmount != 0)
							Selection.transforms[i].Translate(tempPos.x / offsetXAmount,0,0);
						if(tempPos.y != 0 && offsetYAmount != 0)
							Selection.transforms[i].Translate(0,tempPos.y / offsetYAmount,0);
						if(tempPos.z != 0 && offsetZAmount != 0)
							Selection.transforms[i].Translate(0,0,tempPos.z / offsetZAmount);
					}
				}
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void SelAmt()
	{
		if (Selection.transforms.Length > 0)
		{
			componentNames.Clear();
			componentNumber.Clear();
			tagNames.Clear();
			tagNumber.Clear();
			layerNamesList.Clear();
			layerNumberList.Clear();
			vertCount = 0;
			//Prints out a "Debug" to the console of all the currently selected objects
			Debug.Log ("Total Selected: " + Selection.objects.Length);
			Component[] monoArr = new Component[50];
			for(int i = 0; i < Selection.transforms.Length;i++)
			{
				monoArr = new Component[50];
				monoArr = Selection.transforms[i].gameObject.GetComponents(typeof(Component));
				
				for(int o = 0; o < monoArr.Length;o++)
				{
					string tempName = monoArr[o].ToString();
					tempName = tempName.Substring(tempName.LastIndexOf(".") + 1,tempName.LastIndexOf(")") - tempName.LastIndexOf(".") - 1);
					if(componentNames.Contains(tempName) == false)
					{
						componentNames.Add(tempName);
						componentNumber.Add(0);
						componentNumber[componentNames.IndexOf(tempName)]++;
					}
					else
					{
						componentNumber[componentNames.IndexOf(tempName)]++;
					}
				}
			}
			for(int i = 0; i < Selection.transforms.Length;i++)
			{
				string tempName = Selection.transforms[i].tag.ToString();
				if(tagNames.Contains(tempName) == false)
				{
					tagNames.Add(tempName);
					tagNumber.Add(0);
					tagNumber[tagNames.IndexOf(tempName)]++;
				}
				else
				{
					tagNumber[tagNames.IndexOf(tempName)]++;
				}
				string tempLayerName = LayerMask.LayerToName(Selection.transforms[i].gameObject.layer);
				if(layerNamesList.Contains(tempLayerName) == false)
				{
					layerNamesList.Add(tempLayerName);
					layerNumberList.Add(0);
					layerNumberList[layerNamesList.IndexOf(tempLayerName)]++;
				}
				else
				{
					layerNumberList[layerNamesList.IndexOf(tempLayerName)]++;
				}
			}
			
			//Vert Count
			for(int i = 0; i < Selection.transforms.Length;i++)
			{
				MeshFilter tempMesh = Selection.transforms[i].GetComponent(typeof(MeshFilter)) as MeshFilter;
				if(tempMesh != null)
				{
					vertCount += tempMesh.sharedMesh.vertexCount;
				}
			}
			
			for(int i = 0; i < componentNames.Count;i++)
			{
				Debug.Log(componentNames[i] + ": " + componentNumber[i]); 
			}
			for(int i = 0; i < tagNames.Count;i++)
			{
				Debug.Log("Tag (" + tagNames[i] + ": " + tagNumber[i] + ")"); 
			}
			for(int i = 0; i < layerNamesList.Count;i++)
			{
				Debug.Log("Layer (" + layerNamesList[i] + ": " + layerNumberList[i] + ")"); 
			}
			Debug.Log("Vertex Count: " + vertCount);
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void FlipX()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Flip X");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Flip X");
#endif
				Selection.transforms[i].localScale = new Vector3(Selection.transforms[i].localScale.x * -1,Selection.transforms[i].localScale.y, Selection.transforms[i].localScale.z);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void FlipY()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Flip Y");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Flip Y");
#endif
				Selection.transforms[i].localScale = new Vector3(Selection.transforms[i].localScale.x,Selection.transforms[i].localScale.y * -1, Selection.transforms[i].localScale.z);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void FlipZ()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Flip Z");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Flip Z");
#endif
				Selection.transforms[i].localScale = new Vector3(Selection.transforms[i].localScale.x,Selection.transforms[i].localScale.y, Selection.transforms[i].localScale.z * -1);
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void FitCollider()
	{
		if(colSelectedObj != null)
		{
			if(colSelectedObj.GetComponent(typeof(MeshFilter)) as MeshFilter == null && colSelectedObj.GetComponent(typeof(MeshRenderer)) as MeshRenderer == null)
			{
				colSelectedObjArr[0] = null;
				colSelectedObj = null;
				Debug.Log ("Object needs to have a Mesh Filter or Mesh Renderer Component");
			}
		}
#if !UNITY_3_5
		Undo.RecordObject(Selection.activeTransform, "Fit Collider");
#else
		Undo.RegisterUndo(Selection.activeTransform, "Fit Collider");
#endif
		if(Selection.activeTransform == null)
		{
			Debug.Log ("Please Select an object with a collider component!");
		}
		if(colSelectedObj == null)
		{
			Debug.Log ("Please put a Game Object in the Fit To Mesh object field!");
		}
		else if(colSelectedObj != null && Selection.activeTransform != null)
		{
			if(Selection.activeTransform.GetComponent(typeof(BoxCollider)) as BoxCollider != null)
			{
				BoxCollider newBoxCol = colSelectedObj.AddComponent<BoxCollider>();
				BoxCollider curBoxCol = Selection.activeTransform.GetComponent(typeof(BoxCollider)) as BoxCollider;
				
				curBoxCol.size = newBoxCol.size;
				curBoxCol.center = newBoxCol.center;
				DestroyImmediate(newBoxCol);
			}
			if(Selection.activeTransform.GetComponent(typeof(SphereCollider)) as SphereCollider != null)
			{
				SphereCollider newSphereCol = colSelectedObj.AddComponent<SphereCollider>();
				SphereCollider curSphereCol = Selection.activeTransform.GetComponent(typeof(SphereCollider)) as SphereCollider;
				
				curSphereCol.radius = newSphereCol.radius;
				curSphereCol.center = newSphereCol.center;
				DestroyImmediate(newSphereCol);
			}
			if(Selection.activeTransform.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider != null)
			{
				CapsuleCollider newCapCol = colSelectedObj.AddComponent<CapsuleCollider>();
				CapsuleCollider curCapCol = Selection.activeTransform.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
				
				curCapCol.radius = newCapCol.radius;
				curCapCol.center = newCapCol.center;
				curCapCol.height = newCapCol.height;
				curCapCol.direction = newCapCol.direction;
				DestroyImmediate(newCapCol);
			}
			else
			{
				Debug.Log("Selected Object does not contain a Box, Sphere or Capsule Collider!");
			}
		}
	}
	
	public void CycleRotationAxis()
	{
		rotAxis++;
		if(rotAxis == 3)
			rotAxis = 0;
		Repaint();
	}
	
	public void ReplaceSelection()
	{
#if !UNITY_3_5
		Undo.RecordObject(Selection.activeTransform, "Replace Selection");
#else
		Undo.RegisterUndo(Selection.activeTransform, "Replace Selection");
#endif
		if(colSelectedObj != null)
		{
			GameObject[] selectedObjs = new GameObject[Selection.transforms.Length];
			for(int i = 0; i < Selection.transforms.Length;i++)
			{
				Transform tempParent = Selection.transforms[i].parent;
				GameObject newGO = PrefabUtility.InstantiatePrefab(colSelectedObj) as GameObject;
				Selection.transforms[i].parent = null;
				if(inheritPos)
					newGO.transform.position = Selection.transforms[i].position;
				if(inheritRot)
					newGO.transform.localRotation = Selection.transforms[i].localRotation;
				if(inheritScale)
					newGO.transform.localScale = Selection.transforms[i].localScale;
				newGO.transform.parent = tempParent;
				selectedObjs[i] = Selection.transforms[i].gameObject;
			}
			if(!keepOrig)
			{
				for(int i = 0; i < selectedObjs.Length;i++)
				{
					DestroyImmediate(selectedObjs[i]);
				}
			}
		}
		else
			Debug.Log ("Please put a Game Object in the object field");
	}
	
	public void RandomizeRotation()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Vector3 tempRot;
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Randomize Rotation");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Randomize Rotation");
#endif

				tempRot = Selection.transforms[i].eulerAngles;
			
				if(rotX)
				{
					float randRotX = Random.Range(0,360);
					if(randomizeRotSnap != 0)
					{
						randRotX = (Mathf.Round(randRotX / randomizeRotSnap) * randomizeRotSnap);
					}
					if(randRotCurAxis == 0)
						tempRot.x = randRotX;
					else
						Selection.transforms[i].Rotate(randRotX,0,0);
				}
				if(rotY)
				{
					float randRotY = Random.Range(0,360);
					if(randomizeRotSnap != 0)
					{
						randRotY = (Mathf.Round(randRotY / randomizeRotSnap) * randomizeRotSnap);
					}
					if(randRotCurAxis == 0)
						tempRot.y = randRotY;
					else
						Selection.transforms[i].Rotate(0,randRotY,0);
				}
				if(rotZ)
				{
					float randRotZ = Random.Range(0,360);
					if(randomizeRotSnap != 0)
					{
						randRotZ = (Mathf.Round(randRotZ / randomizeRotSnap) * randomizeRotSnap);
					}
					if(randRotCurAxis == 0)
						tempRot.z = randRotZ;
					else
						Selection.transforms[i].Rotate(0,0,randRotZ);
				}
				if(randRotCurAxis == 0)
					Selection.transforms[i].localEulerAngles = tempRot;
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void RandomizeScale()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Vector3 tempScale;
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Randomize Scale");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Randomize Scale");
#endif
				
				tempScale = Selection.transforms[i].localScale;
				
				if(randScaleUniform)
				{
					float randScale = Random.Range(randomizeScaleMin,randomizeScaleMax);
					if(scaleX)
						tempScale.x = randScale;
					if(scaleY)
						tempScale.y = randScale;
					if(scaleZ)
						tempScale.z = randScale;
				}
				else
				{
					if(scaleX)
					{
						float randScaleX = Random.Range(randomizeScaleMin,randomizeScaleMax);
						tempScale.x = randScaleX;
					}
					if(scaleY)
					{
						float randScaleY = Random.Range(randomizeScaleMin,randomizeScaleMax);
						tempScale.y = randScaleY;
					}
					if(scaleZ)
					{
						float randScaleZ = Random.Range(randomizeScaleMin,randomizeScaleMax);
						tempScale.z = randScaleZ;
					}
				}
				Selection.transforms[i].localScale = tempScale;
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void RandomizePosition()
	{
		if (Selection.transforms.Length > 0)
		{
			for (int i = 0;i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos;
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Randomize Position");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Randomize Position");
#endif

				tempPos = Selection.transforms[i].position;
			
				if(positionX)
				{
					float randPosX = Random.Range(randomizePosMin,randomizePosMax);
					if(randPosCurAxis == 0)
						tempPos.x = randPosX;
					else
						Selection.transforms[i].Translate(randPosX,0,0);
				}
				if(positionY)
				{
					float randPosY = Random.Range(randomizePosMin,randomizePosMax);
					if(randPosCurAxis == 0)
						tempPos.y = randPosY;
					else
						Selection.transforms[i].Translate(0,randPosY,0);
				}
				if(positionZ)
				{
					float randPosZ = Random.Range(randomizePosMin,randomizePosMax);
					if(randPosCurAxis == 0)
						tempPos.z = randPosZ;
					else
						Selection.transforms[i].Translate(0,0,randPosZ);
				}
				if(randPosCurAxis == 0)
					Selection.transforms[i].position = tempPos;
			}
		}
		else
			Debug.Log ("Please Select Object(s)");
	}
	
	public void Distribute()
	{
		if(Selection.transforms.Length > 0)
		{
			List<GameObject> disObjList = new List<GameObject>();
			List<float> disPosList = new List<float>();
			GameObject[] disObjListArr = null;
			float[] disPosListArr;
			float range = 0;
			float fixedSpacing = 0;
			float totalSize = 0;
			List<GameObject> noCollider = new List<GameObject>();
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Distribute");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Distribute");
#endif
				if(Selection.transforms[i].gameObject.GetComponent<Collider>() == null)
				{
					Selection.transforms[i].gameObject.AddComponent<BoxCollider>();
					noCollider.Add(Selection.transforms[i].gameObject);
				}
			}
			
			if(disX)
			{
				//Find Highest and Lowest
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					disObjList.Add(Selection.transforms[i].gameObject);
					disPosList.Add(Selection.transforms[i].position.x);
				}
				disObjListArr = new GameObject[disObjList.Count];
				disObjList.CopyTo(disObjListArr);
				disPosListArr = new float[disPosList.Count];
				disPosList.CopyTo(disPosListArr);
				System.Array.Sort(disPosListArr,disObjListArr);
				
				if(setSpacing)
				{
					if(!disBySize)
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[0].transform.position;
									tempPos = new Vector3(tempPos.x + (i * disSpacing), disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[disObjListArr.Length - 1].transform.position;
									tempPos = new Vector3(tempPos.x - (((disObjListArr.Length - 1) - i) * disSpacing), disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
					else
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[i -1].transform.position;
									tempPos = new Vector3(tempPos.x + disSpacing + disObjListArr[i].GetComponent<Collider>().bounds.extents.x + disObjListArr[i - 1].GetComponent<Collider>().bounds.extents.x, disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[i+1].transform.position;
									tempPos = new Vector3(tempPos.x - disSpacing - disObjListArr[i].GetComponent<Collider>().bounds.extents.x - disObjListArr[i + 1].GetComponent<Collider>().bounds.extents.x, disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
				}
				else
				{
					if(!disBySize)
					{
						//Find Range
						range = disObjListArr[disObjListArr.Length - 1].transform.position.x - disObjListArr[0].transform.position.x;
						//Find Fixed Spacing
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[0].transform.position;
								tempPos = new Vector3(tempPos.x + (i * fixedSpacing), disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
					else
					{
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								totalSize += disObjListArr[i].GetComponent<Collider>().bounds.extents.x * 2;
							}
							else
								totalSize += disObjListArr[i].GetComponent<Collider>().bounds.extents.x;
						}
						range = (disObjListArr[disObjListArr.Length - 1].transform.position.x - disObjListArr[0].transform.position.x) - totalSize;
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[i - 1].transform.position;
								tempPos = new Vector3(tempPos.x + fixedSpacing + disObjListArr[i].GetComponent<Collider>().bounds.extents.x + disObjListArr[i - 1].GetComponent<Collider>().bounds.extents.x, disObjListArr[i].transform.position.y, disObjListArr[i].transform.position.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
				}
			}
			if(disY)
			{
				//Find Highest and Lowest
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					disObjList.Add(Selection.transforms[i].gameObject);
					disPosList.Add(Selection.transforms[i].position.y);
				}
				disObjListArr = new GameObject[disObjList.Count];
				disObjList.CopyTo(disObjListArr);
				disPosListArr = new float[disPosList.Count];
				disPosList.CopyTo(disPosListArr);
				System.Array.Sort(disPosListArr,disObjListArr);
				
				if(setSpacing)
				{
					if(!disBySize)
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[0].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y + (i * disSpacing), disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[disObjListArr.Length - 1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y -(((disObjListArr.Length - 1) - i) * disSpacing), disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
					else
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[i -1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y + disSpacing + disObjListArr[i].GetComponent<Collider>().bounds.extents.y + disObjListArr[i - 1].GetComponent<Collider>().bounds.extents.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[i+1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y - disSpacing - disObjListArr[i].GetComponent<Collider>().bounds.extents.y - disObjListArr[i+1].GetComponent<Collider>().bounds.extents.y, disObjListArr[i].transform.position.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
				}
				else
				{
					if(!disBySize)
					{
						//Find Range
						range = disObjListArr[disObjListArr.Length - 1].transform.position.y - disObjListArr[0].transform.position.y;
						//Find Fixed Spacing
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[0].transform.position;
								tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y + (i * fixedSpacing), disObjListArr[i].transform.position.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
					else
					{
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								totalSize += disObjListArr[i].GetComponent<Collider>().bounds.extents.y * 2;
							}
							else
								totalSize += disObjListArr[i].GetComponent<Collider>().bounds.extents.y;
						}
						range = (disObjListArr[disObjListArr.Length - 1].transform.position.y - disObjListArr[0].transform.position.y) - totalSize;
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[i - 1].transform.position;
								tempPos = new Vector3(disObjListArr[i].transform.position.x, tempPos.y + fixedSpacing + disObjListArr[i].GetComponent<Collider>().bounds.extents.y + disObjListArr[i - 1].GetComponent<Collider>().bounds.extents.y, disObjListArr[i].transform.position.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
				}
			}
			if(disZ)
			{
				//Find Highest and Lowest
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					disObjList.Add(Selection.transforms[i].gameObject);
					disPosList.Add(Selection.transforms[i].position.z);
				}
				disObjListArr = new GameObject[disObjList.Count];
				disObjList.CopyTo(disObjListArr);
				disPosListArr = new float[disPosList.Count];
				disPosList.CopyTo(disPosListArr);
				System.Array.Sort(disPosListArr,disObjListArr);
				
				if(setSpacing)
				{
					if(!disBySize)
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[0].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z + (i * disSpacing));
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[disObjListArr.Length - 1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z - (((disObjListArr.Length - 1) - i) * disSpacing));
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
					else
					{
						if(!reverseDis)
						{
							for(int i = 0; i < disObjListArr.Length; i++)
							{
								if(i != 0)
								{
									Vector3 tempPos = disObjListArr[i -1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z + disSpacing + disObjListArr[i].GetComponent<Collider>().bounds.extents.z + disObjListArr[i - 1].GetComponent<Collider>().bounds.extents.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
						else
						{
							for(int i = disObjListArr.Length - 1; i >= 0; i--)
							{
								if(i != disObjListArr.Length - 1)
								{
									Vector3 tempPos = disObjListArr[i+1].transform.position;
									tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z - disSpacing - disObjListArr[i].GetComponent<Collider>().bounds.extents.z - disObjListArr[i+1].GetComponent<Collider>().bounds.extents.z);
									disObjListArr[i].transform.position = tempPos;
								}
							}
						}
					}
				}
				else
				{
					if(!disBySize)
					{
						//Find Range
						range = disObjListArr[disObjListArr.Length - 1].transform.position.z - disObjListArr[0].transform.position.z;
						//Find Fixed Spacing
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[0].transform.position;
								tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z + (i * fixedSpacing));
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
					else
					{
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								totalSize += disObjListArr[i].GetComponent<Collider>().bounds.extents.z * 2;
							}
							else
								totalSize += disObjListArr[i].GetComponent<Collider>().bounds.extents.z;
						}
						range = (disObjListArr[disObjListArr.Length - 1].transform.position.z - disObjListArr[0].transform.position.z) - totalSize;
						fixedSpacing = range / (Selection.transforms.Length - 1);
						//Place Objects
						for(int i = 0; i < Selection.transforms.Length; i++)
						{
							if(i != 0 && i != Selection.transforms.Length - 1)
							{
								Vector3 tempPos = disObjListArr[i - 1].transform.position;
								tempPos = new Vector3(disObjListArr[i].transform.position.x, disObjListArr[i].transform.position.y, tempPos.z + fixedSpacing + disObjListArr[i].GetComponent<Collider>().bounds.extents.z + disObjListArr[i - 1].GetComponent<Collider>().bounds.extents.z);
								disObjListArr[i].transform.position = tempPos;
							}
						}
					}
				}
			}
			
			for(int i = 0; i < noCollider.Count; i++)
			{
					DestroyImmediate(noCollider[i].GetComponent<BoxCollider>());
			}
		}
	}
	
	public void AvgMatchPos()
	{		
		matchPos = false;
		waitForSelMatch = false;
		
		if(posMatchX)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Average Postion");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Average Postion");
#endif
				total += Selection.transforms[i].position.x;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].position;
				tempPos.x = avg;
				Selection.transforms[i].position = tempPos;
			}
		}
		if(posMatchY)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Average Postion");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Average Postion");
#endif
				total += Selection.transforms[i].position.y;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].position;
				tempPos.y = avg;
				Selection.transforms[i].position = tempPos;
			}
		}
		if(posMatchZ)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Average Postion");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Average Postion");
#endif
				total += Selection.transforms[i].position.z;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].position;
				tempPos.z = avg;
				Selection.transforms[i].position = tempPos;
			}
		}
	}
	
	public void AvgMatchRot()
	{
		matchRot = false;
		waitForSelMatch = false;
		
		if(rotMatchX)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Average Rotation");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Average Rotation");
#endif
				total += Selection.transforms[i].eulerAngles.x;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].eulerAngles;
				tempPos.x = avg;
				Selection.transforms[i].eulerAngles = tempPos;
			}
		}
		if(rotMatchY)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Average Rotation");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Average Rotation");
#endif
				total += Selection.transforms[i].eulerAngles.y;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].eulerAngles;
				tempPos.y = avg;
				Selection.transforms[i].eulerAngles = tempPos;
			}
		}
		if(rotMatchZ)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Average Rotation");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Average Rotation");
#endif
				total += Selection.transforms[i].eulerAngles.z;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].eulerAngles;
				tempPos.z = avg;
				Selection.transforms[i].eulerAngles = tempPos;
			}
		}
	}
	
	public void AvgMatchScale()
	{
		matchScale = false;
		waitForSelMatch = false;
		
		if(scaleMatchX)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Average Scale");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Average Scale");
#endif
				total += Selection.transforms[i].localScale.x;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].localScale;
				tempPos.x = avg;
				Selection.transforms[i].localScale = tempPos;
			}
		}
		if(scaleMatchY)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Average Scale");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Average Scale");
#endif
				total += Selection.transforms[i].localScale.y;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].localScale;
				tempPos.y = avg;
				Selection.transforms[i].localScale = tempPos;
			}
		}
		if(scaleMatchZ)
		{
			float total = 0;
			float avg = 0;
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Average Scale");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Average Scale");
#endif
				total += Selection.transforms[i].localScale.z;
			}
			avg = total / Selection.transforms.Length;
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
				Vector3 tempPos = Selection.transforms[i].localScale;
				tempPos.z = avg;
				Selection.transforms[i].localScale = tempPos;
			}
		}
	}
	
	public void AlignHigh()
	{
		if(Selection.transforms.Length > 0)
		{
			List<GameObject> alignObjList = new List<GameObject>();
			List<float> alignPosList = new List<float>();
			GameObject[] alignObjListArr = null;
			float[] alignPosListArr;
			float offsetAmount;
			List<GameObject> noCollider = new List<GameObject>();
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Align");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Align");
#endif
				if(Selection.transforms[i].gameObject.GetComponent<Collider>() == null)
				{
					Selection.transforms[i].gameObject.AddComponent<BoxCollider>();
					noCollider.Add(Selection.transforms[i].gameObject);
				}
			}
			
			if(alignX)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.x + Selection.transforms[i].GetComponent<Collider>().bounds.extents.x);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[alignObjListArr.Length-1].GetComponent<Collider>().bounds.extents.x - alignObjListArr[i].GetComponent<Collider>().bounds.extents.x;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[alignObjListArr.Length-1].transform.position.x + offsetAmount, alignObjListArr[i].transform.position.y, alignObjListArr[i].transform.position.z);
				}			
			}
			if(alignY)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.y + Selection.transforms[i].GetComponent<Collider>().bounds.extents.y);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[alignObjListArr.Length-1].GetComponent<Collider>().bounds.extents.y - alignObjListArr[i].GetComponent<Collider>().bounds.extents.y;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[i].transform.position.x ,alignObjListArr[alignObjListArr.Length-1].transform.position.y + offsetAmount, alignObjListArr[i].transform.position.z);
				}	
			}
			if(alignZ)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.z + Selection.transforms[i].GetComponent<Collider>().bounds.extents.z);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[alignObjListArr.Length-1].GetComponent<Collider>().bounds.extents.z - alignObjListArr[i].GetComponent<Collider>().bounds.extents.z;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[i].transform.position.x , alignObjListArr[i].transform.position.y, alignObjListArr[alignObjListArr.Length-1].transform.position.z + offsetAmount);
				}	
			}
			for(int i = 0; i < noCollider.Count; i++)
			{
					DestroyImmediate(noCollider[i].GetComponent<BoxCollider>());
			}
		}
	}
	
	public void AlignLow()
	{
		if(Selection.transforms.Length > 0)
		{
			List<GameObject> alignObjList = new List<GameObject>();
			List<float> alignPosList = new List<float>();
			GameObject[] alignObjListArr = null;
			float[] alignPosListArr;
			float offsetAmount;
			List<GameObject> noCollider = new List<GameObject>();
			
			for(int i = 0; i < Selection.transforms.Length; i++)
			{
#if !UNITY_3_5
				Undo.RecordObject(Selection.transforms[i], "Align");
#else
				Undo.RegisterUndo(Selection.transforms[i], "Align");
#endif
				if(Selection.transforms[i].gameObject.GetComponent<Collider>() == null)
				{
					Selection.transforms[i].gameObject.AddComponent<BoxCollider>();
					noCollider.Add(Selection.transforms[i].gameObject);
				}
			}
			
			if(alignX)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.x - Selection.transforms[i].GetComponent<Collider>().bounds.extents.x);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[0].GetComponent<Collider>().bounds.extents.x - alignObjListArr[i].GetComponent<Collider>().bounds.extents.x;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[0].transform.position.x - offsetAmount, alignObjListArr[i].transform.position.y, alignObjListArr[i].transform.position.z);
				}			
			}
			if(alignY)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.y - Selection.transforms[i].GetComponent<Collider>().bounds.extents.y);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[0].GetComponent<Collider>().bounds.extents.y - alignObjListArr[i].GetComponent<Collider>().bounds.extents.y;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[i].transform.position.x ,alignObjListArr[0].transform.position.y - offsetAmount, alignObjListArr[i].transform.position.z);
				}	
			}
			if(alignZ)
			{
				for(int i = 0; i < Selection.transforms.Length; i++)
				{
					alignObjList.Add(Selection.transforms[i].gameObject);
					alignPosList.Add(Selection.transforms[i].position.z - Selection.transforms[i].GetComponent<Collider>().bounds.extents.z);
				}
				alignObjListArr = new GameObject[alignObjList.Count];
				alignObjList.CopyTo(alignObjListArr);
				alignPosListArr = new float[alignPosList.Count];
				alignPosList.CopyTo(alignPosListArr);
				System.Array.Sort(alignPosListArr,alignObjListArr);
				
				for(int i = 0; i < alignObjListArr.Length; i++)
				{
					offsetAmount = alignObjListArr[0].GetComponent<Collider>().bounds.extents.z - alignObjListArr[i].GetComponent<Collider>().bounds.extents.z;
					
					alignObjListArr[i].transform.position = new Vector3(alignObjListArr[i].transform.position.x , alignObjListArr[i].transform.position.y, alignObjListArr[0].transform.position.z - offsetAmount);
				}	
			}
			for(int i = 0; i < noCollider.Count; i++)
			{
					DestroyImmediate(noCollider[i].GetComponent<BoxCollider>());
			}
		}
	}
}
