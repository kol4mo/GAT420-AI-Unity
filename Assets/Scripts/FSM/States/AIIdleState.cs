using Assets.Scripts.FSM.States;
using UnityEngine;

public class AIIdleState : AIState {
	public AIIdleState(AIStateAgent agent) : base(agent) {
	}
	float timer;
	public override void OnEnter() {
		timer =Time.time + Random.Range(1,2);
	}

	public override void OnExit() {
	}

	public override void OnUpdate() {
		if (Time.time > timer) {
			
			agent.stateMachine.SetState(nameof(AIPatrolState));
		}
		var enemies = agent.enemyPerception?.GetGameObjects();
		if (enemies != null && enemies.Length > 0) {
			agent.stateMachine.SetState(nameof(AIChaseState));
		} 
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
