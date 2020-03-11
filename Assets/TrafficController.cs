using UnityEngine;
using System.Collections;

public class TrafficController : MonoBehaviour {

	public Sprite[] sprites;
	public Car prefab;
	public int amount = 32;
	public GameObject world;
	public float area;

	Player player;

	GameObject arrow;

	public AudioClip passingCar, explosion;
	Car[] cars;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		float start = 50;
		float end = start + area;



		cars = new Car[amount];
		for (int i = 0; i < amount; i++) {
			Car instance = (Car) GameObject.Instantiate(prefab);

			cars[i] = instance;
			instance.transform.parent = world.transform;

			initCar(i);

			Vector3 zPos = new Vector3(0, 0, start + Random.Range(end * (i / (float) amount), end * ((i+1) / (float) amount)));
			instance.transform.position = zPos;
		}

		arrow = (GameObject) GameObject.Instantiate(Resources.Load("Arrow"));
		arrow.transform.parent = cars[0].gameObject.transform;
		arrow.transform.localPosition = cars[0].GetComponentInChildren<SpriteRenderer>().gameObject.transform.localPosition;
		arrow.transform.Translate(new Vector3(0, 0.5f, 0)); 
	}

	void initCar(int i) {
		Car instance = cars[i];
		Vector3 xPos = new Vector3(Random.Range(-1, 2), 0, 0);
		Vector3 zPos = new Vector3(0, 0, area + Random.Range(-8f, 8f));
		instance.transform.position = zPos;
		instance.sprite.transform.localPosition = xPos;

		Vector3 shadowPos = instance.shadow.transform.localPosition;
		shadowPos.x = xPos.x;
		instance.shadow.transform.localPosition = shadowPos;

		int randomIdx = Random.Range(0, sprites.Length);
		instance.sprite.GetComponent<SpriteRenderer>().sprite = sprites[randomIdx];

		instance.moveSpeed = Random.Range(0.5f, 1.5f);
		instance.wobbleSpeed = Random.Range(0.8f, 1.2f);

		//instance.GetComponent<AudioSource>().clip = null;

		instance.transform.FindChild("sprite").gameObject.SetActive(true);
		instance.transform.FindChild("shadow").gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (arrow != null && arrow.transform.position.z < player.car.transform.position.z - 2)
			Destroy(arrow);

		for (int i = 0; i < amount; i++) {
			Car c = cars[i];

			Vector3 pos = c.transform.position;
			Vector3 spritePos = c.transform.FindChild("sprite").gameObject.transform.position;

			float dX = Mathf.Abs(player.car.transform.position.x - spritePos.x);

			// Check for player collision
			if (player.car.transform.position.z > pos.z && dX < 3.5) {
				if (c.GetComponent<AudioSource>().clip != explosion) {
					c.GetComponent<AudioSource>().clip = explosion;
					c.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
					c.GetComponent<AudioSource>().Play ();

					player.startEmit();

					c.transform.FindChild("sprite").gameObject.SetActive(false);
					c.transform.FindChild("shadow").gameObject.SetActive(false);
					player.addScrap(4 - (int) dX);

					GameObject x = (GameObject) GameObject.Instantiate(Resources.Load("Explosion"));
					x.GetComponent<Explosion>().init(c.sprite);
				}
			}

			// Play sound when passing player
			if (!c.playedSinceReset && player.transform.position.z > pos.z - 8) {
				c.GetComponent<AudioSource>().clip = passingCar;
				c.GetComponent<AudioSource>().pitch = Random.Range(0.6f, 1.2f);
				c.GetComponent<AudioSource>().Play();
				c.playedSinceReset = true;
			}


			// Also move cars

			if (player.transform.position.z > pos.z - 2) {
				// Move from behind camera to far back
				pos.z += 128;
				c.playedSinceReset = false;
				initCar(i); // Reset car 

			} else {
				// Drive car
				pos.z += Time.deltaTime * c.moveSpeed;
			}
			c.transform.position = pos;
		}
	}
}
