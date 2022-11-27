using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGolemMovePriority
{
	None,
	Move,
	End
}

public enum eGolemCostPriority
{
	High,
	Rand,
	Low,
	End
}

public enum eGolemCostWait
{
	None,
	Wait,
	End
}

public class Sub_Think : Golem_SubState
{
	eGolemMovePriority movePriority;
	//eGolemCostPriority costPriority;
	//eGolemCostWait costWait;

	List<Golem_SubState> canStateList;
	public Sub_Think(Golem_BaseState _baseState, string name) : base(_baseState, name)
	{
		stateCost = 0;
		canStateList = new List<Golem_SubState>();
	}

	public void Decision()
	{
		movePriority = (eGolemMovePriority)Random.Range((int)eGolemMovePriority.Move, (int)eGolemMovePriority.End);
		//costPriority = (eGolemCostPriority)Random.Range((int)eGolemCostPriority.High, (int)eGolemCostPriority.End);
		//costWait = eGolemCostWait.None;

		foreach (Golem_SubState state in baseState.subStates)
		{
			if (state != null)
			{ canStateList.Add(state); }
		}

		table.EraseCondition(ref canStateList, x => x.stateCost == 0);

		if (golem.distToTarget <= golem.status.atkRange)
		{//��������
			if (movePriority == eGolemMovePriority.None)
			{
				table.EraseCondition(ref canStateList, x => x.atkRangeType == eGolemAtkRangeType.MiddleAtk);
				table.EraseCondition(ref canStateList, x => x.atkRangeType == eGolemAtkRangeType.RangeAtk);
			}
			else
			{
				table.EraseCondition(ref canStateList, x => x.atkRangeType == eGolemAtkRangeType.RangeAtk);
			}
		}
		else if (golem.distToTarget <= golem.rangeAtkRange)
		{//���� ����
			if (movePriority == eGolemMovePriority.None)
			{
				table.EraseCondition(ref canStateList, x => x.atkRangeType == eGolemAtkRangeType.CloseAtk);
				table.EraseCondition(ref canStateList, x => x.atkRangeType == eGolemAtkRangeType.RangeAtk);
			}
			else
			{
				table.EraseCondition(ref canStateList, x => x.atkRangeType == eGolemAtkRangeType.RangeAtk);
			}
		}
		else
		{//��Ÿ� ����
			if (movePriority == eGolemMovePriority.None)
			{
				table.EraseCondition(ref canStateList, x => x.atkRangeType == eGolemAtkRangeType.CloseAtk);
				table.EraseCondition(ref canStateList, x => x.atkRangeType == eGolemAtkRangeType.MiddleAtk);
			}
			else
			{
				//table.EraseCondition(ref canStateList, x => x.atkType == eGolemStateAtkType.MiddleAtk);
				//table.EraseCondition(ref canStateList, x => x.atkType == eGolemStateAtkType.CloseAtk);
			}
		}

		Funcs.ListShuffle<Golem_SubState>(ref canStateList);
		//table.SortStateByCostPriority(ref canStateList, costPriority);
		//table.SortStateByCostWait(ref canStateList, costWait);

		Golem_SubState nextState = null;
		//���� ���� ����
		//1. �����ִ� ������ ����? �̰� �ٽ� Think������
		//3. move�ε� ��Ÿ� ���ǿ� �������� �ʴ´�? Next State�� �־�ΰ� Move ������
		//=> move / Turn / Idle ���� ���� �ð� �̻� �������� �ٽ� Think������
		if (canStateList.Count == 0)
		{
			hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Move));
			
		}
		else
		{
			nextState = canStateList[0];

			baseState.SetSubState(nextState);

			switch (nextState.atkRangeType)
			{
				case eGolemAtkRangeType.CloseAtk:
					{
						if (golem.distToTarget > golem.status.atkRange)
						{
							hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Move));
						}
					}
					break;
				case eGolemAtkRangeType.MiddleAtk:
					{
						if (golem.distToTarget >= golem.rangeAtkRange)
						{
							hfsmCtrl.SetNextBaseState(hfsmCtrl.GetBaseState((int)eGolemBaseState.Move));
						}
					}
					break;
			}
		}

	

		#region oldThings
		//if (canStateList.Count == 0)
		//{
		//	golem.decisionTime = Random.Range(table.thinkMinTime, table.thinkMaxTime);
		//}
		//else
		//{
		//	nextState = canStateList[0];
		//}

		//if (costWait == eGolemCostWait.Wait && nextState.stateCost > golem.status.curStamina)
		//{ //wait�ε� �ڽ�Ʈ�� ���ڸ� ���
		//	table.nextState = nextState;
		//	golem.SetState((int)eGolemState.Idle);
		//	table.isWaitForCost = true;
		//}

		//if (movePriority == eGolemMovePriority.Move)
		//{
		//	//move�ε� ��Ÿ� ���ǿ� �������� �ʴ� ���
		//	if (nextState.atkType == eGolemStateAtkType.CloseAtk)
		//	{
		//		if (golem.distToTarget > golem.status.atkRange)
		//		{
		//			table.nextState = nextState;
		//			golem.SetState((int)eGolemState.Move);
		//			table.isWaitForDist = true;
		//		}
		//		else
		//		{
		//			golem.SetState(nextState);
		//		}
		//	}
		//	else if (nextState.atkType == eGolemStateAtkType.MiddleAtk)
		//	{
		//		if (golem.distToTarget < golem.rangeAtkRange)
		//		{
		//			golem.SetState(nextState);
		//		}
		//		else
		//		{
		//			table.nextState = nextState;
		//			golem.SetState((int)eGolemState.Move);
		//			table.isWaitForDist = true;
		//		}
		//	}
		//	else if (nextState.atkType == eGolemStateAtkType.RangeAtk)
		//	{ //�� �� ������ ����
		//		golem.SetState(nextState);
		//	}
		//}
		#endregion
	}


	public override void EnterState()
	{
		base.EnterState();

		

		//baseState.SetSubState(baseState.GetSubState((int)eGolemAttackState.RockThrow));
	}
	public override void UpdateState()
	{
		base.UpdateState();

		Decision(); 

	}

	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();
	}

	public override void ExitState()
	{
		base.ExitState();
		canStateList.Clear();
	}
}
