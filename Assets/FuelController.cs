using UnityEngine;
using System.Collections;

public class FuelController : MonoBehaviour {
	GameObject fuel;
	GameObject repair;
	Player player;


	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();


		fuel = (GameObject) GameObject.Instantiate(Resources.Load("Fuel"), new Vector3(0, 0, 50), Quaternion.identity);
		fuel.transform.parent = GameObject.Find("World").transform;
		reset (fuel);

		repair = (GameObject) GameObject.Instantiate(Resources.Load("Repair"), new Vector3(0, 0, 50), Quaternion.identity);
		repair.transform.parent = GameObject.Find("World").transform;
		reset (repair);
	}
	
	// Update is called once per frame
	void Update () {
		// Update fuel
		Vector3 spritePos = fuel.transform.FindChild("sprite").gameObject.transform.position;
		float dX = Mathf.Abs(player.car.transform.position.x - spritePos.x);
		
		if (player.car.transform.position.z > fuel.transform.position.z && dX < 3.5) {
			// Pickup
			player.addFuel(Random.Range(3, 9));
			fuel.GetComponent<AudioSource>().Play();

			reset (fuel);
		} else if (player.transform.position.z > fuel.transform.position.z - 2) {
			reset (fuel);
		}

		spritePos = repair.transform.FindChild("sprite").gameObject.transform.position;
		dX = Mathf.Abs(player.car.transform.position.x - spritePos.x);
		if (player.car.transform.position.z > repair.transform.position.z && dX < 3.5) {
			// Pickup
			player.damage(-Random.Range(3, 9));
			repair.GetComponent<AudioSource>().Play();
			
			reset (repair);
		} else if (player.transform.position.z > repair.transform.position.z - 2) {
			reset (repair);
		}
	}

	public void reset(GameObject go) {
		// Reset 100 units further
		go.transform.Translate(0, 0, Random.Range(150, 250));
		go.GetComponentInChildren<SpriteRenderer>().transform.localPosition = new Vector3(Random.Range(-1, 2), 0, 0);
	}
}
