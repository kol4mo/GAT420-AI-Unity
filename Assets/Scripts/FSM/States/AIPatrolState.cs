using Assets.Scripts.FSM.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolState : AIState
{
	Vector3 destination;
	public AIPatrolState(AIStateAgent agent) : base(agent) {
	}

	public override void OnEnter() {
		var navNode = AINavNode.GetRandomAINavNode();
		destination = navNode.transform.position;
	}

	public override void OnExit() {
	}

	public override void OnUpdate() {
		agent.movement.MoveTowards(destination);
		if (Vector3.Distance(agent.transform.position, destination) < 1) {
			agent.stateMachine.SetState(nameof(AIIdleState));
		}

		var enemies = agent.enemyPerception?.GetGameObjects();
		if (enemies != null && enemies.Length > 0) {
			agent.stateMachine.SetState(nameof(AIChaseState));
			destination = enemies[0].transform.position;
		}
		var friends = agent.friendlyPerception?.GetGameObjects();
		if (friends != null && friends.Length > 0) {
			agent.stateMachine?.SetState(nameof(AIFriendlyState));
		}
		var scared = agent.ScaredPerception?.GetGameObjects();
		if (scared != null && scared.Length > 0) {
			agent.stateMachine?.SetState(nameof(AIFleeState));
		}
	}
}
