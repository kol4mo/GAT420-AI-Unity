using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine{
	private Dictionary<string, AIState> states = new Dictionary<string, AIState>();
	public AIState currentState {get; private set; } = null;

	public void Update() {
		currentState?.OnUpdate();
	}

	public void AddState(string name, AIState state) {
		Debug.Assert(!states.ContainsKey(name),"State machine already contains state " + name);
		states[name] = state;
	}

	public void SetState(string name) {
		Debug.Assert(states.ContainsKey(name), "State machine doesnt contain state " + name);
		AIState state = states[name];

		if (state == currentState) return;

		//exit current state
		currentState?.OnExit();
		//set next state
		currentState = state;
		//enter new state
		currentState?.OnEnter();
	}
}
