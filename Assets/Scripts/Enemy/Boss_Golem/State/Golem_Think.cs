using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


public class Golem_Think : cGolemState
{
	eGolemMovePriority movePriority;
	eGolemCostPriority costPriority;
	eGolemCostWait costWait;

	List<cGolemState> canState = new List<cGolemState>();

	public Golem_Think(int cost) : base(cost)
	{
		atkType = eGolemStateAtkType.None;
	}

	public void ResetCanState()
	{
		canState.Clear();

		foreach (cState state in golem.fsm)
		{
			canState.Add(state as cGolemState);
		}
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		//movePriority = (eGolemMovePriority)Random.Range((int)eGolemMovePriority.Move, (int)eGolemMovePriority.End);
		movePriority = eGolemMovePriority.None;
		//costPriority = (eGolemCostPriority)Random.Range((int)eGolemCostPriority.High, (int)eGolemCostPriority.End);
		costPriority = eGolemCostPriority.High;
		//costWait = (eGolemCostWait)Random.Range((int)eGolemCostWait.Wait, (int)eGolemCostWait.End);
		costWait = eGolemCostWait.None;

		foreach (cState state in golem.fsm)
		{
			canState.Add(state as cGolemState);
		}
	}

	public override void UpdateState()
	{
		table.EraseCondition(ref canState, x => x.stateCost == 0);

		if (golem.distToTarget <= golem.status.atkRange)
		{//��������
			if (movePriority == eGolemMovePriority.None)
			{//���� ����Ÿ�� �ٸ��ֵ� �� �����

				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.RangeAtk);
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.MiddleAtk);
			}
			else 
			{ //������ ���� ����
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.RangeAtk);
			}
		}
		else if (golem.distToTarget <= golem.rangeAtkRange)
		{
			//�⺻ ������
			if (movePriority == eGolemMovePriority.None)
			{
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.CloseAtk);
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.RangeAtk);
			}
			else
			{ 
				
			}
		}
		else
		{
			if (movePriority == eGolemMovePriority.None)
			{
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.MiddleAtk);
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.CloseAtk);
			}
			else
			{
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.CloseAtk);
			}
		}

		table.SortStateByCostPriority(ref canState, costPriority);
		table.SortStateByCostWait(ref canState, costWait);

		if (canState.Count != 0)
		{
			golem.SetState(canState[0]);
		}
		else
		{
			ResetCanState();
		}
	}

	public override void LateUpdateState()
	{
		base.LateUpdateState();

	}
	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
	}

	public override void ExitState()
	{
		canState.Clear();
	}
}
