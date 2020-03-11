using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

	public float wobbleHeight = 1;
	public float wobbleSpeed = 1;
	public float moveSpeed = 2;
	public GameObject sprite, shadow;
	public bool playedSinceReset = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 carPos = sprite.transform.localPosition;

		float wobble = transform.localScale.y * wobbleHeight * (1 + Mathf.Sin(Time.time * Mathf.PI * 2 * wobbleSpeed)) / 128;
		carPos.y = wobble;
		sprite.transform.localPosition = carPos;

	}
}
