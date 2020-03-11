using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {
	public Sprite[] sprites;
	public GameObject prefab;

	public int amount = 8;
	public int area = 128;
	GameObject[] clouds;
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		clouds = new GameObject[amount];

		for (int i = 0; i < amount; i++) {
			GameObject instance = (GameObject) GameObject.Instantiate(prefab);
			instance.transform.position = new Vector3(0, 0, Random.Range(area * (i / (float) amount), area * ((i+1) / (float) amount)));
			instance.transform.parent = GameObject.Find("World").transform;
			clouds[i] = instance;
			initCloud (i);
		}
	}

	void initCloud(int id) {
		Vector3 xPos = new Vector3(Random.Range(-64f, 64f), Random.Range(3f, 8), 0);
		clouds[id].GetComponentInChildren<SpriteRenderer>().gameObject.transform.localPosition = xPos;

		Vector3 cloudPos = new Vector3(xPos.x, 0, 0);
		clouds[id].transform.FindChild("shadow").transform.localPosition = cloudPos;

		int randomIdx = Random.Range(0, sprites.Length);
		clouds[id].GetComponentInChildren<SpriteRenderer>().sprite = sprites[randomIdx];
	}
	
	// Update is called once per frame
	void Update () {


		for (int i = 0; i < amount; i++) {
			Vector3 pos = clouds[i].transform.position;
			if (player.transform.position.z > pos.z - 2) {
				// Move from behind camera to far back
				pos.z += area;
				clouds[i].transform.position = pos;
				initCloud(i);
			}
		}
	}
}
