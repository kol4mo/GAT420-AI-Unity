using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FSM.States {
	internal class AIFriendlyState : AIState {

		float timer = 0;
		public AIFriendlyState(AIStateAgent agent) : base(agent) {
		}

		public override void OnEnter() {
			agent.animator?.SetTrigger("friendly");
			timer = Time.time + 2;
		}

		public override void OnExit() {
		}

		public override void OnUpdate() {
			if (Time.time > timer) {
				agent.stateMachine.SetState(nameof(AIIdleState));
			}
		}
	}
}
