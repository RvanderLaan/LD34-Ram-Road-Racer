using UnityEngine;
using System.Collections;

public class RoadController : MonoBehaviour {

	public GameObject prefab;
	public Sprite[] sprites;
	public int amount = 128;
	public GameObject world;
	GameObject[] roads;
	GameObject player;
	public int gap = 1;
	public Sprite[] scenery;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");


		roads = new GameObject[amount];
		for (int i = 0; i < amount; i++) {
			GameObject instance = (GameObject) GameObject.Instantiate(prefab, new Vector3(0, 0, (gap + 1) * i+1), Quaternion.identity); // X: Mathf.Cos (i/100f * 2 * Mathf.PI)
			instance.GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
			roads[i] = instance;
			instance.transform.parent = world.transform;
			resetScenery(i);
		}

	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < roads.Length; i++) {
			GameObject go = roads[i];
			Vector3 pos = go.transform.position;
			if (player.transform.position.z > go.transform.position.z - 2) {
				pos.z += (gap+1) * amount;
				go.transform.position = pos;
				resetScenery(i);
			} 
		}
	}

	void resetScenery(int idx) {
		Transform current = roads[idx].transform.FindChild("scenery");
		if (current != null)
			Destroy (current.gameObject);

		int randomIdx = Random.Range(0, 2);
		if (Random.Range(0, 1f) > 0.5f) 
			randomIdx = 2;

		GameObject go = new GameObject("scenery");
		go.transform.parent = roads[idx].transform;
		float xPos = (Random.Range(0, 2) * 2 - 1) * Random.Range(3, 6f);
		go.transform.localPosition = new Vector3(xPos, 0, 0);
		float scale = Random.Range(0.9f, 1.5f);
		go.transform.localScale = new Vector3(scale, scale, 1);


		go.AddComponent<SpriteRenderer>();
		go.GetComponent<SpriteRenderer>().sprite = scenery[randomIdx];

		// Flip image
		if (Random.Range(0, 1f) > 0.5f) 
			go.transform.localRotation = Quaternion.Euler(0, 180, 0);


	}
}
