using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

	public float speed = 1;
	public float lifeTime = 2;
	float startTime;
	Car car;
	float startX;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		transform.parent = GameObject.Find("TextSpawner").transform;
		car = GameObject.Find("Player").GetComponent<Player>().car;

		startX = car.transform.position.x;


	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > startTime + lifeTime) {
			Destroy(gameObject);
			return;
		}
		Vector3 pos = transform.position;
		pos.y += Time.deltaTime * speed;
		pos.x = startX;
		pos.z = GameObject.Find("TextSpawner").transform.position.z;
		transform.position = pos;
	}

	public void setText(string text) {
		GetComponent<TextMesh>().text = text;
	}
}
