using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PartController : MonoBehaviour {


	public GameObject prefab;
	public Sprite[] sprites;


	GameObject spikePrefab;
	GameObject[] spikes;

	public AudioClip pickup, hit;


	GameObject[] parts;
	public int amount;
	public int area = 128;
	public int start = 32;

	Player player;
	GameObject world;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		world = GameObject.Find("World");

		parts = new GameObject[amount];

		for (int i = 0; i < amount; i++) {
			int randomIdx = Random.Range(0, sprites.Length);
			Vector3 xPos = new Vector3(Random.Range(-1, 1), 0, 0);
			Vector3 zPos = new Vector3(0, 0, start + Random.Range(area * (i / (float) amount), area * ((i+1) / (float) amount)));
			
			GameObject instance = (GameObject) GameObject.Instantiate(prefab, zPos, Quaternion.identity);

			GameObject sprite = instance.GetComponentInChildren<SpriteRenderer>().gameObject;
			sprite.transform.localPosition = xPos;
			sprite.GetComponent<SpriteRenderer>().sprite = sprites[randomIdx];

			parts[i] = instance;
			instance.transform.parent = world.transform;
		}
	}

	void initPart() {

	}
	
	// Update is called once per frame
	void Update () {
		// Update metal parts
		foreach (GameObject c in parts) {

			Vector3 pos = c.transform.position;
			Vector3 spritePos = c.transform.FindChild("sprite").gameObject.transform.position;

			// Check for player collision
			if (player.car.transform.position.z > pos.z && 
			   		player.car.transform.position.x -2 < spritePos.x && player.car.transform.position.x + 2 > spritePos.x) {
				if (!c.GetComponent<AudioSource>().isPlaying) {
					c.GetComponent<AudioSource>().clip = pickup;
					c.GetComponent<AudioSource>().pitch = Random.Range(0.6f, 1.2f);
					c.GetComponent<AudioSource>().Play ();
					// GameObject.Destroy(c, pickup.length);
					c.transform.FindChild("sprite").gameObject.SetActive(false);

					addPickup();
				}
			}

			// Reset if behind camera
			else if (player.transform.position.z > pos.z - 2 && !c.GetComponent<AudioSource>().isPlaying) {
				pos.z += 128;

			} 
		}
	}

	void addPickup() {
		player.damage(-Random.Range(1, 4));
	}
}
