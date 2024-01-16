using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class AIAutonomousAgent : AIAgent {
	[SerializeField] AIPerception seekPerception = null;
    [SerializeField] AIPerception fleePerception = null;
    [SerializeField] AIPerception flockPerception = null;
    [SerializeField] AIPerception ObstaclePerception = null;

	private void Update() {
		//seek
		if (seekPerception != null) {
			var gameObjects = seekPerception.GetGameObjects();
			if (gameObjects.Length > 0) {
				movement.ApplyForce(Seek(gameObjects[0]));
			}
		}

		//flee
		if (fleePerception != null) {
			var gameObjects = fleePerception.GetGameObjects();
			if (gameObjects.Length > 0) {
				movement.ApplyForce(Flee(gameObjects[0]));
			}
		}

		//flock
		if (flockPerception != null) {
			var gameObjects = flockPerception.GetGameObjects();
			if (gameObjects.Length > 0) {
				movement.ApplyForce(Cohesion(gameObjects));
				movement.ApplyForce(Seperation(gameObjects, 3));
				movement.ApplyForce(Alignment(gameObjects));
			}
		}

		//obstacle avoidance
		if (ObstaclePerception != null) {
			if (((AIRaycastPerception)ObstaclePerception).CheckDirection(Vector3.forward)) {
				Vector3 open = Vector3.zero;
				if (((AIRaycastPerception)ObstaclePerception).GetOpenDirection(ref open)) {
					movement.ApplyForce(GetSteeringForce(open) * 5);
				}

			}
		}

		Vector3 acceleration = movement.Acceleration;
		acceleration.y = 0;
		movement.Acceleration = acceleration;

		//transform.position = Utilities.Wrap(transform.position, new Vector3(55, -10, 20), new Vector3(80, 10, 70));
		//transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		//transform.rotation.SetEulerAngles(0, Mathf.Deg2Rad * transform.rotation.eulerAngles.y, 0);
	}

	private Vector3 Seek(GameObject target) {
		Vector3 direction = target.transform.position - transform.position;
		return GetSteeringForce(direction);
	}

	private Vector3 Flee(GameObject target) {
		Vector3 direction = transform.position - target.transform.position;
		return GetSteeringForce(direction);
	}

	private Vector3 Cohesion(GameObject[] neighbors) {
		Vector3 positions = Vector3.zero;
		foreach (var neighbor in neighbors) {
			positions += neighbor.transform.position;
		}

		Vector3 center = positions / neighbors.Length;
		Vector3 direction = center - transform.position;
		return GetSteeringForce(direction);
	}

	private Vector3 Seperation(GameObject[] neighbors, float radius) {
		Vector3 seperation = Vector3.zero;
		foreach (var neighbor in neighbors) {
			Vector3 direction = (transform.position - neighbor.transform.position);
			if (direction.magnitude < radius) {
				seperation += direction / direction.sqrMagnitude;
			}
		}

		return GetSteeringForce(seperation);
	}

	private Vector3 Alignment(GameObject[] neighbors) {
		Vector3 velocities = Vector3.zero;
		foreach (var neighbor in neighbors) {
			velocities += neighbor.GetComponent<AIAgent>().movement.Velocity;
		}

		Vector3 averageVelocity = velocities/ neighbors.Length;
		return GetSteeringForce(averageVelocity);
	}

	public Vector3 GetSteeringForce(Vector3 direction) {
		Vector3 desired = direction.normalized * movement.maxSpeed;
		Vector3 steer = desired - movement.Velocity;
		return Vector3.ClampMagnitude(steer, movement.maxForce);
	}
}
