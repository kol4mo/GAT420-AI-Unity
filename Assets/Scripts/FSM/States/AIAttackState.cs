using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState {

	public AIAttackState(AIStateAgent agent) : base(agent) {
		AIStateTransition transition = new AIStateTransition(nameof(AIPatrolState));
		transition.AddCondition(new FloatCondition(agent.timer, Condition.Predicate.LESS, 0));
		transitions.Add(transition);
	}

	public override void OnEnter() {
		agent.animator?.SetTrigger("Attack");
		agent.timer.value = 1f;
	}

	public override void OnExit() {

	}

	public override void OnUpdate() {
	}

	private void Attack() {
		// check for collision with surroundings
		var colliders = Physics.OverlapSphere(agent.transform.position, 1);
		foreach (var collider in colliders) {
			// don't hit self or objects with the same tag
			if (collider.gameObject == agent.gameObject || collider.gameObject.CompareTag(agent.gameObject.tag)) continue;

			// check if collider object is a state agent, reduce health
			if (collider.gameObject.TryGetComponent<AIStateAgent>(out var stateAgent)) {
				stateAgent.ApplyDamage(Random.Range(20, 50));
			}
		}
	}
}
