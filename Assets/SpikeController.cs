using UnityEngine;
using System.Collections;

public class SpikeController : MonoBehaviour {
	public GameObject prefab;
	GameObject[] spikes;
	public int amount = 8;
	public int start = 150;
	public int area = 128;
	Player player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();

		spikes = new GameObject[amount];
		for (int i = 0; i < amount; i++) {
			GameObject instance = (GameObject) GameObject.Instantiate(prefab);
			
			spikes[i] = instance;
			instance.transform.parent = GameObject.Find("World").transform;
			
			initSpike(i);

			// Set initial position
			Vector3 zPos = new Vector3(0, 0, start + Random.Range(128 * (i / (float) amount), 128 * ((i+1) / (float) amount)));
			instance.transform.position = zPos;
		}
	}

	void initSpike(int i) {
		GameObject go = spikes[i];

		go.transform.Translate(0, 0, Random.Range(100, 200));
		Vector3 xPos = new Vector3(Random.Range(-1, 2), 0, 0);
		go.GetComponentInChildren<SpriteRenderer>().gameObject.transform.localPosition = xPos;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < amount; i++) {
			Vector3 spritePos = spikes[i].GetComponentInChildren<SpriteRenderer>().gameObject.transform.position;
			float dX = Mathf.Abs(player.car.transform.position.x - spritePos.x);
			
			if (player.car.transform.position.z > spikes[i].transform.position.z && dX < 3) {
				// Pickup
				player.damage((int) (4 - dX + 1));
				spikes[i].GetComponent<AudioSource>().Play();
				
				initSpike (i);
			} else if (player.transform.position.z > spikes[i].transform.position.z - 2) {
				initSpike (i);
			}
		}
	}
}
