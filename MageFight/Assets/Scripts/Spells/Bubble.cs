using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {
	private float floatSpeed = 10f;
	private float duration = 1f;
	private bool active = false;
	
	public void trapInBubble(Vector2 position, float trapDuration){
		transform.position = position;
		duration = trapDuration;
		active = true;
	}
	void Update () {
		if(active){
		duration -= Time.deltaTime;
		if(duration <= 0){
			Destroy(gameObject);
			return;
		}
		Vector2 newPosition = transform.position;
		newPosition.y += floatSpeed * Time.deltaTime;
		transform.position = newPosition;
		}
	}

	public Transform getBubble(){
		return transform;
	}
}
