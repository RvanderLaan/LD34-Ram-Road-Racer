using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public Sprite[] sprites;
	public float timePerSprite = 0.2f;
	float changeTime;
	SpriteRenderer sprite;
	GameObject playerCar;
	float xPos;


	int spriteId = 0;

	// Use this for initialization
	void Start () {
		sprite = GetComponentInChildren<SpriteRenderer>();
		transform.parent = GameObject.Find("World").transform;
		playerCar = GameObject.Find("Player").GetComponent<Player>().car.gameObject;

		changeTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > changeTime + timePerSprite) {
			spriteId++;
			changeTime = Time.time;
			if (spriteId < sprites.Length) {
				sprite.sprite = sprites[spriteId];
			}
		}	    
		if (spriteId >= sprites.Length) {
			Destroy(gameObject);
			sprite.enabled = false;
			return;
		}
		Vector3 pos = transform.localPosition;
		pos.z = playerCar.transform.position.z;
		transform.localPosition = pos;

	}

	public void init(GameObject par) {
		if (sprite == null)
			sprite = GetComponentInChildren<SpriteRenderer>();
		Vector3 pos = sprite.transform.localPosition;
		pos.x = par.transform.localPosition.x;
		sprite.transform.localPosition = pos;
	}
}
