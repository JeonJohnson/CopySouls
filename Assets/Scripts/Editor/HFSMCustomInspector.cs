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

		if (controller.GetCurBaseState == null)
		{
			EditorGUILayout.LabelField("Cur BaseState: ", "null");
		}
		else
		{
			EditorGUILayout.LabelField("Cur BaseState: ", controller.GetCurBaseState.ToString());
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
		if (controller.GetCurBaseState == null)
		{
			EditorGUILayout.LabelField("Cur BaseState is Null");
		}
		else
		{
			if (controller.GetCurBaseState.preSubState != null)
			{ EditorGUILayout.LabelField("Pre SubState: ", controller.GetCurBaseState.preSubState.ToString()); }
			else { EditorGUILayout.LabelField("Pre SubState: ", "null"); }

			if(controller.GetCurBaseState.curSubState != null)
			EditorGUILayout.LabelField("Cur SubState: ", controller.GetCurBaseState.curSubState.ToString());
			else { EditorGUILayout.LabelField("Cur SubState: ", "null"); }

			if (controller.GetCurBaseState.nextSubState != null)
			EditorGUILayout.LabelField("Next SubState: ", controller.GetCurBaseState.nextSubState.ToString());
			else { EditorGUILayout.LabelField("Next SubState: ", "null"); }

		}


		EditorGUILayout.LabelField("--Next Attack State ReadOnly--");

		if (controller.baseStates != null)
		{
			if (controller.baseStates[(int)eGolemBaseState.Attack] != null)
			{
				Golem_SubState temp = controller.baseStates[(int)eGolemBaseState.Attack].nextSubState;
				if (temp != null)
				{ EditorGUILayout.LabelField("Next Attack SubState: ", temp.stateName); }
			}
		}

	}
}
