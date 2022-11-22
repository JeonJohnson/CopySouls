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

	public IEnumerator DecisionCoroutine()
	{
		//table.Move();
		yield return new WaitForSeconds(golem.decisionTime);
		
		Decision();
		table.decisionCoroutine = null;
	}

	public void Decision()
	{
		foreach (cState state in golem.fsm)
		{
			cGolemState golemState = state as cGolemState;
			if(golemState != null)
			canState.Add(golemState);
		}

		table.EraseCondition(ref canState, x => x.stateCost == 0);

		if (golem.distToTarget <= golem.status.atkRange)
		{//��������
			if (movePriority == eGolemMovePriority.None)
			{
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.MiddleAtk);
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.RangeAtk);
			}
			else
			{
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.RangeAtk);
			}
		}
		else if (golem.distToTarget <= golem.rangeAtkRange)
		{//���� ����
			if (movePriority == eGolemMovePriority.None)
			{
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.CloseAtk);
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.RangeAtk);
			}
			else
			{
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.CloseAtk);
			}
		}
		else
		{//��Ÿ� ����
			if (movePriority == eGolemMovePriority.None)
			{
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.CloseAtk);
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.MiddleAtk);
			}
			else
			{
				//table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.MiddleAtk);
				//table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.CloseAtk);
			}
		}

		table.SortStateByCostPriority(ref canState, costPriority);
		
		table.SortStateByCostWait(ref canState, costWait);

		cGolemState nextState = null;
		//���� ���� ����
		//1. �����ִ� ������ ����? �̰� �ٽ� Think������
		//2. wait�ε� �ڽ�Ʈ�� ���ڸ���? Next State�� �־�ΰ� Move ������
		//3. move�ε� ��Ÿ� ���ǿ� �������� �ʴ´�? Next State�� �־�ΰ� Move ������
		//=> move / Turn / Idle ���� ���� �ð� �̻� �������� �ٽ� Think������
		if (canState.Count == 0)
		{
			golem.decisionTime = Random.Range(table.thinkMinTime, table.thinkMaxTime);
			table.decisionCoroutine = table.StartCoroutine(DecisionCoroutine());
		}
		else
		{
			 nextState = canState[0];
		}
		
		if(costWait == eGolemCostWait.Wait && nextState.stateCost > golem.status.curStamina)
		{ //wait�ε� �ڽ�Ʈ�� ���ڸ� ���
			table.nextState = nextState;
			golem.SetState((int)eGolemState.Idle);
			table.isWaitForCost = true;
		}

		if (movePriority == eGolemMovePriority.Move)
		{
			//move�ε� ��Ÿ� ���ǿ� �������� �ʴ� ���
			if (nextState.atkType == eGolemStateAtkType.CloseAtk)
			{
				if (golem.distToTarget > golem.status.atkRange)
				{
					table.nextState = nextState;
					golem.SetState((int)eGolemState.Move);
					table.isWaitForDist = true;
				}
				else 
				{
					golem.SetState(nextState);
				}
			}
			else if (nextState.atkType == eGolemStateAtkType.MiddleAtk)
			{
				if (golem.distToTarget < golem.rangeAtkRange)
				{
					golem.SetState(nextState);
				}
				else
				{
					table.nextState = nextState;
					golem.SetState((int)eGolemState.Move);
					table.isWaitForDist = true;
				}
			}
			else if (nextState.atkType == eGolemStateAtkType.RangeAtk)
			{ //�� �� ������ ����
				golem.SetState(nextState);
			}
		}

		//if (canState.Count != 0)
		//{
		//	golem.SetState(canState[0]);
		//}
		//else
		//{
		//	ResetCanState();
		//}
	}

	public override void EnterState(Enemy script)
	{
		base.EnterState(script);

		//movePriority = eGolemMovePriority.None;
		movePriority = (eGolemMovePriority)Random.Range((int)eGolemMovePriority.Move, (int)eGolemMovePriority.End);
		
		//costPriority = eGolemCostPriority.High;
		costPriority = (eGolemCostPriority)Random.Range((int)eGolemCostPriority.High, (int)eGolemCostPriority.End);
		
		costWait = eGolemCostWait.None;
		//costWait = (eGolemCostWait)Random.Range((int)eGolemCostWait.Wait, (int)eGolemCostWait.End);

		foreach (cState state in golem.fsm)
		{
			canState.Add(state as cGolemState);
		}

		if (table.decisionCoroutine == null)
		{
			golem.decisionTime = Random.Range(table.thinkMinTime, table.thinkMaxTime);
			table.decisionCoroutine = table.StartCoroutine(DecisionCoroutine());
		}

		golem.SetState((int)eGolemState.Idle);
	}

	public override void UpdateState()
	{

		
		//curTime += Time.deltaTime;


		//if (curTime <= golem.decisionTime)
		//{
		//	//table.Move();
		//}
		//else
		//{
		//	Decision();
		//	if (canState.Count != 0)
		//	{
		//		golem.SetState(canState[0]);
		//	}
		//	else
		//	{
		//		ResetCanState();
		//	}
		//}

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
