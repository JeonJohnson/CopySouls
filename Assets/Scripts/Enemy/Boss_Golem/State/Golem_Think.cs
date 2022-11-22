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
		{//근접공격
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
		{//전진 공격
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
		{//사거리 공격
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
		//최종 결정 조건
		//1. 남아있는 조건이 없다? 이건 다시 Think돌리기
		//2. wait인디 코스트가 모자르다? Next State에 넣어두고 Move 돌리기
		//3. move인디 사거리 조건에 부합하지 않는다? Next State에 넣어두고 Move 돌리기
		//=> move / Turn / Idle 에서 일정 시간 이상 지나가면 다시 Think돌리기
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
		{ //wait인디 코스트가 모자를 경우
			table.nextState = nextState;
			golem.SetState((int)eGolemState.Idle);
			table.isWaitForCost = true;
		}

		if (movePriority == eGolemMovePriority.Move)
		{
			//move인디 사거리 조건에 부합하지 않는 경우
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
			{ //얜 걍 던짐됌 ㅋㅋ
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
