//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// Editor for VRInput
//
//=============================================================================

using UnityEngine;
using UnityEditor;
using System.Collections;
//using Valve.VR;

namespace VRInteraction
{
	[CustomEditor(typeof(VRInputOculus))]
	public class VRInputOculusEditor : Editor {

		// target component
		public VRInputOculus input = null;
		SerializedObject serializedInput;

		static bool editActionsFoldout;
		string newActionName = "";

		public virtual void OnEnable()
		{
			input = (VRInputOculus)target;
			serializedInput = new SerializedObject(input);
		}

		public override void OnInspectorGUI()
		{
			serializedInput.Update();

			SerializedProperty controllerHand = serializedInput.FindProperty("controllerHand");
			controllerHand.intValue = (int)(OVRInput.Controller)EditorGUILayout.EnumPopup("ControllerHand", (OVRInput.Controller)controllerHand.intValue);

			if (controllerHand.intValue == 0)
			{
				EditorGUILayout.HelpBox("Must set to L and R Touch depending on left or right hand or no input will be processed.", MessageType.Warning);
			}

			editActionsFoldout = EditorGUILayout.Foldout(editActionsFoldout, "Edit Actions");
			if (editActionsFoldout)
			{
				if (input.VRActions != null)
				{
					for(int i=0; i<input.VRActions.Length; i++)
					{
						EditorGUILayout.BeginHorizontal();
						input.VRActions[i] = EditorGUILayout.TextField(input.VRActions[i]);
						if (GUILayout.Button("X"))
						{
							string[] newActions = new string[input.VRActions.Length-1];
							int offset = 0;
							for(int j=0; j<newActions.Length; j++)
							{
								if (i == j) offset = 1;
								newActions[j] = input.VRActions[j+offset];
							}
							input.VRActions = newActions;

							if (input.triggerKey > i)
								input.triggerKey -= 1;
							else if (input.triggerKey == i)
								input.triggerKey = 0;
							if (input.stickTop > i)
								input.stickTop -= 1;
							else if (input.stickTop == i)
								input.stickTop = 0;
							if (input.stickLeft > i)
								input.stickLeft -= 1;
							else if (input.stickLeft == i)
								input.stickLeft = 0;
							if (input.stickRight > i)
								input.stickRight -= 1;
							else if (input.stickRight == i)
								input.stickRight = 0;
							if (input.stickBottom > i)
								input.stickBottom -= 1;
							else if (input.stickBottom == i)
								input.stickBottom = 0;
							if (input.stickCentre > i)
								input.stickCentre -= 1;
							else if (input.stickCentre == i)								
								input.stickTouch = 0;
							if (input.stickTouch > i)
								input.stickTouch -= 1;
							else if (input.stickTouch == i)
								input.stickTouch = 0;
							if (input.gripKey > i)
								input.gripKey -= 1;
							else if (input.gripKey == i)
								input.gripKey = 0;
							if (input.BYKey > i)
								input.BYKey -= 1;
							else if (input.BYKey == i)
								input.BYKey = 0;
							if (input.AXKey > i)
								input.AXKey -= 1;
							else if (input.AXKey == i)
								input.AXKey = 0;
							EditorUtility.SetDirty(input);
							break;
						}
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.BeginHorizontal();
				newActionName = EditorGUILayout.TextField(newActionName);
				GUI.enabled = (newActionName != "");
				if (GUILayout.Button("Add Action"))
				{
					string[] newActions = new string[1];
					if (input.VRActions != null) newActions = new string[input.VRActions.Length+1];
					else input.VRActions = new string[0];
					for(int i=0; i<newActions.Length; i++)
					{
						if (i == input.VRActions.Length)
						{
							newActions[i] = newActionName;
							break;
						}
						newActions[i] = input.VRActions[i];
					}
					input.VRActions = newActions;
					newActionName = "";
					EditorUtility.SetDirty(input);
				}
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();
			}

			if (input.VRActions == null)
			{
				serializedInput.ApplyModifiedProperties();
				return;
			}

			SerializedProperty triggerKey = serializedInput.FindProperty("triggerKey");
			SerializedProperty padTop = serializedInput.FindProperty("stickTop");
			SerializedProperty padLeft = serializedInput.FindProperty("stickLeft");
			SerializedProperty padRight = serializedInput.FindProperty("stickRight");
			SerializedProperty padBottom = serializedInput.FindProperty("stickBottom");
			SerializedProperty padCentre = serializedInput.FindProperty("stickCentre");
			SerializedProperty padTouch = serializedInput.FindProperty("stickTouch");
			SerializedProperty gripKey = serializedInput.FindProperty("gripKey");
			SerializedProperty menuKey = serializedInput.FindProperty("BYKey");
			SerializedProperty aButtonKey = serializedInput.FindProperty("AXKey");

			triggerKey.intValue = EditorGUILayout.Popup("Trigger Key", triggerKey.intValue, input.VRActions);
			padTop.intValue = EditorGUILayout.Popup("Thumbstick Up", padTop.intValue, input.VRActions);
			padLeft.intValue = EditorGUILayout.Popup("Thumbstick Left", padLeft.intValue, input.VRActions);
			padRight.intValue = EditorGUILayout.Popup("Thumbstick Right", padRight.intValue, input.VRActions);
			padBottom.intValue = EditorGUILayout.Popup("Thumbstick Down", padBottom.intValue, input.VRActions);
			padCentre.intValue = EditorGUILayout.Popup("Thumbstick Button", padCentre.intValue, input.VRActions);
			padTouch.intValue = EditorGUILayout.Popup("Thumbstick Touch", padTouch.intValue, input.VRActions);
			gripKey.intValue = EditorGUILayout.Popup("Grip Key", gripKey.intValue, input.VRActions);
			menuKey.intValue = EditorGUILayout.Popup("B/Y", menuKey.intValue, input.VRActions);
			aButtonKey.intValue = EditorGUILayout.Popup("A/X", aButtonKey.intValue, input.VRActions);

			SerializedProperty debugMode = serializedInput.FindProperty("debugMode");
			debugMode.boolValue = EditorGUILayout.Toggle("Debug Mode", debugMode.boolValue);

			serializedInput.ApplyModifiedProperties();
		}
	}

}
