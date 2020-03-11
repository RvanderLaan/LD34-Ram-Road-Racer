using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

	public GameObject world;
	public GameObject player;
	public float minBrightness = 0.4f;
	public float lightRange = 64;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			togglePause();
		}



		foreach (Transform t in world.transform) {
			Vector3 pos = t.transform.position;
			Vector3 scale = t.transform.localScale;


			float distance = pos.z - player.transform.position.z;


			float size =  24 / distance;


			pos.y = -size;
			t.transform.position = pos;

			scale = t.localScale;
			scale.x = size;
			scale.y = size;

			t.localScale = scale;

			// Brightness
			foreach (Transform child in t.GetComponentsInChildren<Transform>()) {
				SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
				if (sr != null && !t.gameObject.name.Contains("Cloud") && !sr.gameObject.name.Contains("Arrow")) {
					float brightness = Mathf.Clamp((1 - distance / lightRange), minBrightness, 1);
					Color c = sr.color;
					c.r = brightness;
					c.g = brightness;
					c.b = brightness;
					sr.color = c;
				}
			}
		}
	}

	public void togglePause() {
		if (player.GetComponent<Player>().gameOver)
			return;

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		} else
			Time.timeScale = 0;
	}
}
