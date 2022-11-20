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
		{//근접공격
			if (movePriority == eGolemMovePriority.None)
			{//공격 범위타입 다른애들 다 지우기

				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.RangeAtk);
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.MiddleAtk);

				//List<cGolemState> findRangeState = canState.FindAll(x => x.atkType == eGolemStateAtkType.RangeAtk);
				//for (int i = 0; i < findRangeState.Count; ++i)
				//{
				//	canState.Remove(findRangeState[i]);
				//}
				//findRangeState = canState.FindAll(x => x.atkType == eGolemStateAtkType.MiddleAtk);
				//for (int i = 0; i < findRangeState.Count; ++i)
				//{
				//	canState.Remove(findRangeState[i]);
				//findRangeState = canState.FindAll(x => x.stateCost == 0);
				//for (int i = 0; i < findRangeState.Count; ++i)
				//{
				//}
			}
			else 
			{ //포워드 까지 포함
			  //List<cGolemState> findRangeState = canState.FindAll(x => x.atkType == eGolemStateAtkType.RangeAtk);
			  //for (int i = 0; i < findRangeState.Count; ++i)
			  //{
			  //	canState.Remove(findRangeState[i]);
			  //}
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.RangeAtk);
			}

			//if (costPriority == eGolemCostPriority.High)
			//{
			//	canState = canState.OrderByDescending(x => x.stateCost).ToList();
			//}
			//else if (costPriority == eGolemCostPriority.Rand)
			//{
			//	Funcs.ListShuffle(ref canState);
			//}
			//else
			//{
			//	canState = canState.OrderByDescending(x => x.stateCost).ToList();
			//	canState.Reverse();
			//}

			//if (costWait == eGolemCostWait.None)
			//{
			//	table.EraseCondition(ref canState, (x => x.stateCost <= golem.status.curStamina));
			//}
			//else
			//{ 
				
			
			//}

			
			
		}
		else if (golem.distToTarget <= golem.rangeAtkRange)
		{
			//기본 포워드
			if (movePriority == eGolemMovePriority.None)
			{
				
				table.EraseCondition(ref canState, x => x.atkType == eGolemStateAtkType.RangeAtk);
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

			}
			else
			{

			}

		}


		table.SortStateByCostPriority(ref canState, costPriority);
		table.SortStateByCostWait(ref canState, costWait);

		golem.SetState(canState[0]);
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
