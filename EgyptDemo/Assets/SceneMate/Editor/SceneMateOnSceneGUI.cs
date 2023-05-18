using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SceneMateOnSceneGUI : SceneMate 
{
	void ObjectPainter(Event current)
	{
		if(objPainter && !paintGroup)
		{
			//Draw the painter brush outline aka disc
			Ray worldRayDisc = HandleUtility.GUIPointToWorldRay(current.mousePosition);
			RaycastHit hitInfoDisc;
			if (Physics.Raycast(worldRayDisc, out hitInfoDisc))
			{
				if(eraserMode)
				{
					Handles.color = Color.red;
				}
				else
					Handles.color = Color.green;
				if(alignToNormal)
				{
					if(hitInfoDisc.distance > 1)
					{
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/25);
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/50);
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/200);
					}
					else
					{
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.04f);
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.02f);
						Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.005f);
					}
				}
				else
				{
					if(hitInfoDisc.distance > 1)
					{
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),hitInfoDisc.distance/25);
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),hitInfoDisc.distance/50);
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),hitInfoDisc.distance/200);
					}
					else
					{
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),.04f);
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),.02f);
						Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),.005f);
					}
				}
				
				HandleUtility.Repaint();
			}
			//Make the last painted object the current selection.
			if(newObject != null)
			{
				if(Selection.activeTransform != newObject.transform)
				{
					if(selectLast)
					{
						Selection.activeTransform = newObject.transform;
						selectLast = false;
					}
				}
			}
			if(current.modifiers != EventModifiers.Alt)
			{
				if (current.type == EventType.MouseDown && current.button == 0)
				{
					if(current.type == EventType.MouseDown && current.button == 0)
						wasMouseDown = true;
					
					if(!painterKeyDown)
					{
						//HandleUtility.AddDefaultControl(controlID);
						painterKeyDown = true;
						Ray worldRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
						RaycastHit hitInfo;
						
						if (Physics.Raycast(worldRay, out hitInfo,Mathf.Infinity,painterMask))
						{
							if(eraserMode)
							{
								//Undo.RecordObject(hitInfo.transform.gameObject, "Eraser");
								DestroyImmediate(hitInfo.transform.gameObject);
							}
							else
							{
								if(objs.Count > 0)
								{
									if (objs[curObject] != null)
									{
										newObject = (GameObject)PrefabUtility.InstantiatePrefab (objs[curObject]);
										newObject.transform.position = hitInfo.point;
										Selection.activeTransform = newObject.transform;
										if(assignParent && parentObj != null)
											newObject.transform.parent = parentObj.transform;
										if(alignToNormal)
										{
											newObject.transform.localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
										}
										if(randRot)
										{
											if(randRotX)
											{
												float tempRot = Random.Range(0,360);
												if(randomRotSnap != 0)
													tempRot = (Mathf.Round(tempRot / randomRotSnap) * randomRotSnap);
												newObject.transform.Rotate(tempRot,0,0,Space.Self);
											}
											
											if(randRotY)
											{
												float tempRot = Random.Range(0,360);
												if(randomRotSnap != 0)
													tempRot = (Mathf.Round(tempRot / randomRotSnap) * randomRotSnap);
												newObject.transform.Rotate(0,tempRot,0,Space.Self);
											}
											
											if(randRotZ)
											{
												float tempRot = Random.Range(0,360);
												if(randomRotSnap != 0)
													tempRot = (Mathf.Round(tempRot / randomRotSnap) * randomRotSnap);
												newObject.transform.Rotate(0,0,tempRot,Space.Self);
											}
										}
										
										if(randScale)
										{
											if(uniformScale)
											{
												float scale = Random.Range(randScaleMin,randScaleMax);
												newObject.transform.localScale = new Vector3(scale,scale,scale);
											}
											else
											{
												newObject.transform.localScale = new Vector3(Random.Range(randScaleMin,randScaleMax),Random.Range(randScaleMin,randScaleMax),Random.Range(randScaleMin,randScaleMax));
											}
										}
									}
								}
							}
						}
						current.Use();
					}
				}
				if(current.type == EventType.KeyUp)
				{
					painterKeyDown = false;
				}
				
				else if(wasMouseDown)
				{
					painterKeyDown = false;
					wasMouseDown = false;
					selectLast = true;
				}
			}
		}
	}

	void AdvObjectPainter(Event current)
	{
		if(paintGroup)
		{
			//Draw the painter brush outline aka disc
			Ray worldRayDisc = HandleUtility.GUIPointToWorldRay(current.mousePosition);
			RaycastHit hitInfoDisc;
			if (Physics.Raycast(worldRayDisc, out hitInfoDisc))
			{
				if(eraserMode)
				{
					Handles.color = Color.red;
				}
				else
					Handles.color = Color.green;

				float falloffRadius = brushSize - (brushFalloff * brushSize);
				float densityRadius = brushDensity * brushSize;
				if(alignToNormal)
				{
					Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,brushSize);
					Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,falloffRadius);
					Handles.color = Color.blue;
					Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,densityRadius);
				}
				else
				{
					Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),brushSize);
					Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),falloffRadius);
					Handles.color = Color.blue;
					Handles.DrawWireDisc(hitInfoDisc.point,new Vector3(0,1,0),densityRadius);
				}
				
				HandleUtility.Repaint();
			}
			if(current.modifiers != EventModifiers.Alt)
			{
				if (current.type == EventType.MouseDown && current.button == 0)
				{
					if(current.type == EventType.MouseDown && current.button == 0)
						wasMouseDown = true;
					
					if(!painterKeyDown)
					{
						//HandleUtility.AddDefaultControl(controlID);
						painterKeyDown = true;
						Ray worldRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
						RaycastHit hitInfo;
						
						if (Physics.Raycast(worldRay, out hitInfo,Mathf.Infinity,painterMask))
						{
							//float objTotal = brushSize * brushSize * Mathf.PI * brushDensity;

							if(paintObjs.Count > 0)
							{
								/*foreach(GameObject curObj in paintObjs)
								{
									if (curObj != null)
									{
										newObject = (GameObject)PrefabUtility.InstantiatePrefab (objs[curObject]);
										Vector2 randPos = Random.insideUnitCircle * brushSize;
										Vector3 tempPos = hitInfo.point;
										//tempPos.x

										newObject.transform.position = hitInfo.point;
										Selection.activeTransform = newObject.transform;
										if(assignParent && parentObj != null)
											newObject.transform.parent = parentObj.transform;
										if(alignToNormal)
										{
											newObject.transform.localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
										}
										if(randRot)
										{
											if(randRotX)
											{
												float tempRot = Random.Range(0,360);
												if(randomRotSnap != 0)
													tempRot = (Mathf.Round(tempRot / randomRotSnap) * randomRotSnap);
												newObject.transform.Rotate(tempRot,0,0,Space.Self);
											}
											
											if(randRotY)
											{
												float tempRot = Random.Range(0,360);
												if(randomRotSnap != 0)
													tempRot = (Mathf.Round(tempRot / randomRotSnap) * randomRotSnap);
												newObject.transform.Rotate(0,tempRot,0,Space.Self);
											}
											
											if(randRotZ)
											{
												float tempRot = Random.Range(0,360);
												if(randomRotSnap != 0)
													tempRot = (Mathf.Round(tempRot / randomRotSnap) * randomRotSnap);
												newObject.transform.Rotate(0,0,tempRot,Space.Self);
											}
										}
										
										if(randScale)
										{
											if(uniformScale)
											{
												float scale = Random.Range(randScaleMin,randScaleMax);
												newObject.transform.localScale = new Vector3(scale,scale,scale);
											}
											else
											{
												newObject.transform.localScale = new Vector3(Random.Range(randScaleMin,randScaleMax),Random.Range(randScaleMin,randScaleMax),Random.Range(randScaleMin,randScaleMax));
											}
										}
									}
								}*/
							}
						}
						current.Use();
					}
				}
				if(current.type == EventType.KeyUp)
				{
					painterKeyDown = false;
				}
				
				else if(wasMouseDown)
				{
					painterKeyDown = false;
					wasMouseDown = false;
				}
			}
		}
	}

	void SnapDrag(Event current)
	{
		if (snapDrag)
		{
			if (Selection.transforms.Length > 0)
			{
				RaycastHit hitInfo = new RaycastHit();
				int currentLayer = Selection.transforms[0].gameObject.layer;
				Selection.transforms[0].gameObject.layer = 2;
				Vector3 snapRayStart = Selection.transforms[0].position;
				
				if(dragDirNegX)
				{
					if (Physics.Raycast(snapRayStart,Vector3.left, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float negX = hitInfo.point.x + Selection.transforms[0].GetComponent<Collider>().bounds.extents.x;
								Selection.transforms[0].position = new Vector3(negX,hitInfo.point.y,hitInfo.point.z);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x + snapOffset, Selection.transforms[0].position.y, Selection.transforms[0].position.z);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirPosX)
				{
					if (Physics.Raycast(snapRayStart,Vector3.right, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float posX = hitInfo.point.x - Selection.transforms[0].GetComponent<Collider>().bounds.extents.x;
								Selection.transforms[0].position = new Vector3(posX,hitInfo.point.y,hitInfo.point.z);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x + snapOffset, Selection.transforms[0].position.y, Selection.transforms[0].position.z);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirNegY)
				{
					if (Physics.Raycast(snapRayStart,Vector3.down, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float negY = hitInfo.point.y + Selection.transforms[0].GetComponent<Collider>().bounds.extents.y;
								Selection.transforms[0].position = new Vector3(hitInfo.point.x,negY,hitInfo.point.z);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x, Selection.transforms[0].position.y + snapOffset, Selection.transforms[0].position.z);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirPosY)
				{
					if (Physics.Raycast(snapRayStart,Vector3.up, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float posY = hitInfo.point.y - Selection.transforms[0].GetComponent<Collider>().bounds.extents.y;
								Selection.transforms[0].position = new Vector3(hitInfo.point.x,posY,hitInfo.point.z);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x, Selection.transforms[0].position.y + snapOffset, Selection.transforms[0].position.z);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirNegZ)
				{
					if (Physics.Raycast(snapRayStart,Vector3.back, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float negZ = hitInfo.point.z + Selection.transforms[0].GetComponent<Collider>().bounds.extents.z;
								Selection.transforms[0].position = new Vector3(hitInfo.point.x,hitInfo.point.y,negZ);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x, Selection.transforms[0].position.y, Selection.transforms[0].position.z + snapOffset);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				if(dragDirPosZ)
				{
					if (Physics.Raycast(snapRayStart,Vector3.forward, out hitInfo,snapDragThreshold,snapsMask))
					{
						if (hitInfo.transform != null)
						{
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
							}
							if(boundsSnapping)
							{
								float posZ = hitInfo.point.z + Selection.transforms[0].GetComponent<Collider>().bounds.extents.z;
								Selection.transforms[0].position = new Vector3(hitInfo.point.x,hitInfo.point.y,posZ);
							}
							else
							{
								Selection.transforms[0].position = hitInfo.point;
							}
							//snapOffset
							Selection.transforms[0].position = new Vector3(Selection.transforms[0].position.x, Selection.transforms[0].position.y, Selection.transforms[0].position.z + snapOffset);
						}
					}
					else
					{
						Selection.transforms[0].position = Selection.transforms[0].position;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;	
				}
				
				Handles.color = Color.blue;
				if(!alignToNormal)
				{
					Handles.DrawWireDisc(Selection.transforms[0].localPosition,new Vector3(0,1,0),.4f);
					Handles.DrawWireDisc(Selection.transforms[0].localPosition,new Vector3(0,1,0),.46f);
					HandleUtility.Repaint();
				}
				if(alignToNormal)
				{
					Handles.DrawWireDisc(Selection.transforms[0].localPosition,Selection.transforms[0].up,.4f);
					Handles.DrawWireDisc(Selection.transforms[0].localPosition,Selection.transforms[0].up,.46f);
					HandleUtility.Repaint();
				}
			}
		}
	}

	void SnapSurface(Event current)
	{
		if(snapSurface)
		{
			Ray worldRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
			RaycastHit hitInfo;
			RaycastHit hitInfoMesh;
			
			Handles.color = Color.blue;
			if(!alignToNormal)
			{
				Handles.DrawWireDisc(handlePos,new Vector3(0,1,0),.4f);
				Handles.DrawWireDisc(handlePos,new Vector3(0,1,0),.46f);
				HandleUtility.Repaint();
			}
			if(alignToNormal)
			{
				Handles.DrawWireDisc(handlePos,handleNormal,.4f);
				Handles.DrawWireDisc(handlePos,handleNormal,.46f);
				HandleUtility.Repaint();
			}
			
			if (Selection.transforms.Length > 0)
			{
				bool hasMeshCollider;
				bool meshColEnabled = true;
				if (current.keyCode == toolKeyCodes[11] && current.control == modControl[11] 
				    && current.shift == modShift[11] &&  current.alt == modAlt[11] 
				    && current.keyCode != KeyCode.None)
				{	
					hasMeshCollider = false;
#if !UNITY_3_5
					Undo.RecordObject(Selection.transforms[0], "Snap To Surface");
#else
					Undo.RegisterUndo(Selection.transforms[0], "Snap To Surface");
#endif
					int currentLayer = Selection.transforms[0].gameObject.layer;
					Selection.transforms[0].gameObject.layer = 2;
					
					if (Physics.Raycast(worldRay, out hitInfo,Mathf.Infinity,snapsMask))
					{
						MeshCollider meshCollider = hitInfo.transform.gameObject.GetComponent<MeshCollider>();
						if(meshCollider != null)
						{
							hasMeshCollider = true;
							if (meshCollider.enabled == false)
								meshColEnabled = false;
						}
						else
						{
							meshCollider = hitInfo.transform.gameObject.AddComponent<MeshCollider>();
						}
						hitInfo.collider.enabled = false;
						meshCollider.enabled = true;
						
						if (Physics.Raycast(worldRay, out hitInfoMesh))
						{
							Selection.transforms[0].position = hitInfoMesh.point;
							if(alignToNormal)
							{
								Selection.transforms[0].localRotation = Quaternion.FromToRotation(Vector3.up, hitInfoMesh.normal);
							}
							handlePos = hitInfoMesh.point;
							handleNormal = hitInfoMesh.normal;
						}
						if(!hasMeshCollider)
						{
							DestroyImmediate(meshCollider);
						}
						hitInfo.collider.enabled = true;
						
						if(!meshColEnabled)
							meshCollider.enabled = false;
					}
					Selection.transforms[0].gameObject.layer = currentLayer;
				}
			}
		}
	}

	void WaitForSelectionMatch(Event current)
	{
		if(waitForSelMatch)
		{
#if !UNITY_3_5
			Undo.RecordObjects(Selection.transforms, "Match");
#else
			Undo.RegisterUndo(Selection.transforms, "Match");
#endif
			if(!grabSource)
			{
				grabSource = true;
				sourceObjs = new GameObject[Selection.gameObjects.Length];
				for(int i = 0; i < sourceObjs.Length;i++)
				{
					sourceObjs[i] = Selection.gameObjects[i];
				}
			}
			else
			{
				Ray worldRayDisc = HandleUtility.GUIPointToWorldRay(current.mousePosition);
				RaycastHit hitInfoDisc;
				if (Physics.Raycast(worldRayDisc, out hitInfoDisc))
				{
					Handles.color = Color.yellow;
					if(matchPos)
					{
						if(hitInfoDisc.distance > 1)
						{
							Handles.DrawSolidDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/130);
						}
						else
						{
							Handles.DrawSolidDisc(hitInfoDisc.point,hitInfoDisc.normal,.0077f);
						}
					}
					if(matchRot)
					{
						if(hitInfoDisc.distance > 1)
						{
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/60);
						}
						else
						{
							
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.0167f);
						}
					}
					if(matchScale)
					{
						if(hitInfoDisc.distance > 1)
						{
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/30);
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,hitInfoDisc.distance/26);
						}
						else
						{
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.033f);
							Handles.DrawWireDisc(hitInfoDisc.point,hitInfoDisc.normal,.038f);
						}
					}
					HandleUtility.Repaint();
				}
				
				if(Selection.activeGameObject != null)
				{
					if(Selection.gameObjects[0] != sourceObjs[0])
					{
						for(int i = 0; i < sourceObjs.Length;i++)
						{
							if(matchPos)
							{
								Vector3 tempPos = sourceObjs[i].transform.position;
								if(posMatchX)
									tempPos.x = Selection.gameObjects[0].transform.position.x;
								if(posMatchY)
									tempPos.y = Selection.gameObjects[0].transform.position.y;
								if(posMatchZ)
									tempPos.z = Selection.gameObjects[0].transform.position.z;
								sourceObjs[i].transform.position = tempPos;
							}
							if(matchRot)
							{
								Vector3 tempRot = sourceObjs[i].transform.localEulerAngles;
								if(rotMatchX)
									tempRot.x = Selection.gameObjects[0].transform.localEulerAngles.x;
								if(rotMatchY)
									tempRot.y = Selection.gameObjects[0].transform.localEulerAngles.y;
								if(rotMatchZ)
									tempRot.z = Selection.gameObjects[0].transform.localEulerAngles.z;
								sourceObjs[i].transform.localEulerAngles = tempRot;
							}
							if(matchScale)
							{
								Vector3 tempScale = sourceObjs[i].transform.localScale;
								if(scaleMatchX)
									tempScale.x = Selection.gameObjects[0].transform.localScale.x;
								if(scaleMatchY)
									tempScale.y = Selection.gameObjects[0].transform.localScale.y;
								if(scaleMatchZ)
									tempScale.z = Selection.gameObjects[0].transform.localScale.z;
								sourceObjs[i].transform.localScale = tempScale;
							}
						}
						matchPos = false;
						matchRot = false;
						matchScale = false;
						grabSource = false;
						waitForSelMatch = false;
					}
				}
				else
				{
					if(matchPos)
						matchPos = false;
					if(matchRot)
						matchRot = false;
					if(matchScale)
						matchScale = false;
				}
				Selection.objects = sourceObjs;
			}	
		}
	}

	void AssignNextHotkey(Event current)
	{
		if (Event.current.type == EventType.KeyDown)
		{
			//Assign Hot Keys While viewport is active
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
					}
					assignNextKey = false;
					for(int o = 0; o < toolKeyCodes.Length;o++)
					{
						EditorPrefs.SetString(toolPrefStrings[o], toolKeyStrings[o]);
					}
				}
			}
			//Use Hot Keys-----------------------------
			
			for(int i = 0; i < toolKeyCodes.Length;i++)
			{
				if (current.keyCode == toolKeyCodes[i] && current.control == modControl[i] 
				    && current.shift == modShift[i] &&  current.alt == modAlt[i] 
				    && current.keyCode != KeyCode.None)
				{
					if(methodNames[i] != "")
					{
						System.Type myMate = this.GetType();
						MethodInfo theMethod = myMate.GetMethod(methodNames[i]);
						theMethod.Invoke(this,null);
					}
				}
			}
		}
	}

	public void OnSceneGUI (SceneView scnView)
	{		
		//Assign and use Hot Keys
		Event current = Event.current;
		
		AssignNextHotkey(current);
		
		//Drag Follows Collision
		SnapDrag(current);
		
		SnapSurface(current);
		
		//Object Painter
		ObjectPainter(current);

		AdvObjectPainter(current);
		
		//Match Tools
		WaitForSelectionMatch(current);
	}
}
