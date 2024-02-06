using System;
using UnityEngine;

namespace Assets.Scripts.FSM.States {
	internal class AIFleeState : AIState {
		float initialSpeed;
		public AIFleeState(AIStateAgent agent) : base(agent) {
		}

		public override void OnEnter() {
			initialSpeed = agent.movement.maxSpeed;
			agent.movement.maxSpeed *= 1.5f;
		}

		public override void OnExit() {
			agent.movement.maxSpeed = initialSpeed;
		}

		public override void OnUpdate() {
			var enemies = agent.ScaredPerception.GetGameObjects();
			if (enemies.Length > 0) {
				var enemy = enemies[0];
				agent.movement.MoveTowards(agent.gameObject.transform.position + (agent.gameObject.transform.position - enemy.gameObject.transform.position));
			} else {
				agent.stateMachine.SetState(nameof(AIIdleState));
			}
		}
	}
}
