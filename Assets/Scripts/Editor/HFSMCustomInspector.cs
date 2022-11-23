using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEditor;

[CustomEditor (typeof(HFSMCtrl))]
public class HFSMCustomInspector : Editor
{
	HFSMCtrl controller = null;

	void OnEnable()
	{
		controller = (HFSMCtrl)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.LabelField("--BaseState ReadOnly--");

		if (controller.preBaseState == null)
		{
			EditorGUILayout.LabelField("Pre BaseState: ", "null");
		}
		else
		{
			EditorGUILayout.LabelField("Pre BaseState: ", controller.preBaseState.ToString());
		}

		if (controller.curBaseState == null)
		{
			EditorGUILayout.LabelField("Cur BaseState: ", "null");
		}
		else
		{
			EditorGUILayout.LabelField("Cur BaseState: ", controller.curBaseState.ToString());
		}

		if (controller.nextBaseState == null)
		{
			EditorGUILayout.LabelField("Next BaseState: ", "null");
		}
		else
		{
			EditorGUILayout.LabelField("Next BaseState: ", controller.nextBaseState.ToString());
		}


		EditorGUILayout.LabelField("--SubState ReadOnly--");
		if (controller.curBaseState == null)
		{
			EditorGUILayout.LabelField("Cur BaseState is Null");
		}
		else
		{

			if (controller.curBaseState.preSubState != null)
			{ EditorGUILayout.LabelField("Pre SubState: ", controller.curBaseState.preSubState.ToString()); }
			else { EditorGUILayout.LabelField("Pre SubState: ", "null"); }

			if(controller.curBaseState.curSubState != null)
			EditorGUILayout.LabelField("Cur SubState: ", controller.curBaseState.curSubState.ToString());
			else { EditorGUILayout.LabelField("Cur SubState: ", "null"); }

			if (controller.curBaseState.nextSubState != null)
			EditorGUILayout.LabelField("Next SubState: ", controller.curBaseState.nextSubState.ToString());
			else { EditorGUILayout.LabelField("Next SubState: ", "null"); }

		}


	}
}
