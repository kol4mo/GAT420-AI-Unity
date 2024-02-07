using Assets.Scripts.FSM.States;
using UnityEngine;

public class AIIdleState : AIState {

	public AIIdleState(AIStateAgent agent) : base(agent) {
		AIStateTransition transition = new AIStateTransition(nameof(AIPatrolState));
		transition.AddCondition(new FloatCondition(agent.timer, Condition.Predicate.LESS, 0));
		transitions.Add(transition);

		transition = new AIStateTransition(nameof(AIChaseState));
		transition.AddCondition(new BoolCondition(agent.enemySeen));
		transitions.Add(transition);
	}
	public override void OnEnter() {
		agent.timer.value = Random.Range(1,2);
	}

	public override void OnExit() {
	}

	public override void OnUpdate() {

		//if (transition.ToTransition()) agent.stateMachine.SetState(transition.nextState);

		var friends = agent.friendlyPerception?.GetGameObjects();
		if (friends != null && friends.Length > 0) {
			agent.stateMachine?.SetState(nameof(AIFriendlyState));
		}
		var scared = agent.ScaredPerception?.GetGameObjects();
		if (scared != null && scared.Length > 0) {
			agent.stateMachine?.SetState(nameof(AIFleeState));
		}
		if (Random.Range(0, 500) == 0) {
			agent.stateMachine?.SetState(nameof(AIDanceState));
		}
	}
}
