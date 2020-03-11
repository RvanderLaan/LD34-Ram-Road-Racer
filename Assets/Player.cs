using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	public int score = 0;

	int HP;
	public int maxHP = 30;

	public bool gameOver = false;
	public GameObject gameOverPanel;
	public Text gameOverText;

	int fuel;
	public int maxFuel = 30;
	public int fuelDepletionTime = 1; // Afer how many secs fuel is lost
	float lastFuelTime; // Used for depleting fuel over time

	public int level = 1;
	public int baseMetalNeeded = 10;
	public float metalNeededModifier = 1.5f;
	public int scrapMetal = 0;
	

	public float speed = 24;
	public float maxSpeed = 64;
	public int speedIncreaseDistance = 100;
	public float speedIncrease = 0.2f;
	float lastIncrease;


	public float xSpeed = 2;
	public Car car;

	public RectTransform hpBar;
	public Text hpText;
	public RectTransform fuelBar;
	public Text fuelText;
	public RectTransform scrapBar;
	public Text scrapText;

	public Text scoreText;
	public Text levelText;

	public GameObject warningPanel;
	public Text warningText;

	float initCarDist;

	bool emit = false;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		HP = maxHP;
		fuel = maxFuel;
		lastIncrease = speedIncreaseDistance;

		initCarDist = car.transform.position.z - transform.position.z;
		lastFuelTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver) {
			speed -= Time.deltaTime * 20;
			if (speed < 0)
				setGameOver();

		}

		Vector3 pos = transform.position;
		pos.z += speed * Time.deltaTime;
		transform.position = pos;

		Vector3 carPos = car.transform.position;
		carPos.z = transform.position.z + initCarDist;

		// Left
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			carPos.x -= xSpeed * Time.deltaTime;
		}
		// Right
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			carPos.x += xSpeed * Time.deltaTime;
		}
		carPos.x = Mathf.Clamp(carPos.x, -4, 4);

		car.transform.position = carPos;

		if (emit) {
			car.GetComponentInChildren<ParticleSystem>().Emit((int) (100 * Time.deltaTime));
		}

		// Deplete fuel overr time
		if (Time.time > lastFuelTime + fuelDepletionTime) {
			addFuel (-1);
			lastFuelTime = Time.time;
		}

		if (car.transform.position.z > lastIncrease + speedIncreaseDistance) {
			lastIncrease = car.transform.position.z;
			if (speed < maxSpeed)
				speed += speedIncrease;
			else
				speed += speedIncrease / 2;
		}
		scoreText.text = score + "";
		levelText.text = level + "";

		if (fuel <= 0 || HP <= 0) {
			gameOver = true;

		} 

		if (HP < maxHP / 4) {
			warningPanel.SetActive(true);
			warningText.text = "WARNING: LOW HP";
			if (!warningPanel.GetComponent<AudioSource>().isPlaying)
				warningPanel.GetComponent<AudioSource>().Play();
		} else if (fuel < maxFuel / 4) {
			warningPanel.SetActive(true);
			warningText.text = "WARNING: LOW FUEL";
			if (!warningPanel.GetComponent<AudioSource>().isPlaying)
				warningPanel.GetComponent<AudioSource>().Play();
		} else
			warningPanel.SetActive(false);

	}

	public void damage(int damage) {
		HP -= damage;

		if (HP > maxHP)
			HP = maxHP;

		Vector3 scale = hpBar.transform.localScale;
		scale.x = HP / (float) maxHP;
		hpBar.transform.localScale = scale;
		hpText.text = "HP: " + HP;


		GameObject ft = GameObject.Instantiate(Resources.Load("FloatingText")) as GameObject;
		if (damage < 0)
			ft.GetComponent<TextMesh>().text = ("HP +" + -damage);
		else
			ft.GetComponent<TextMesh>().text = ("HP " + -damage);
	}

	public void addFuel(int amount) {
		fuel += amount;
		if (fuel > maxFuel)
			fuel = maxFuel;
		else if (fuel == 0) {
			Debug.Log ("YOU LOSE");
		}
		Vector3 scale = fuelBar.transform.localScale;
		scale.x = fuel / (float) maxFuel;
		fuelBar.transform.localScale = scale;
		fuelText.text = "FUEL: " + fuel;

		if (amount == -1)
			return;
		GameObject ft = GameObject.Instantiate(Resources.Load("FloatingText")) as GameObject;
		if (amount < 0)
			ft.GetComponent<TextMesh>().text = ("FUEL " + amount);
		else
			ft.GetComponent<TextMesh>().text = ("FUEL +" + amount);

	}

	public void startEmit() {
		emit = true;
		Invoke ("stopEmit", 0.2f);
	}

	public void stopEmit() {
		emit = false;
	}

	public void addScrap(int amount) {
		scrapMetal += amount;
		score += amount;

		Vector3 scale = scrapBar.transform.localScale;
		scale.x = Mathf.Clamp(scrapMetal / (float) metalForNextLevel(), 0, 1f);
		scrapBar.transform.localScale = scale;
		scrapText.text = "SCRAP: [" + scrapMetal + "/" + metalForNextLevel() + "]";

		GameObject ft = GameObject.Instantiate(Resources.Load("FloatingText")) as GameObject;
		if (amount > 0)
			ft.GetComponent<TextMesh>().text = ("SCRAP +" + amount);

		if (scrapMetal > metalForNextLevel()) {
			// Debug.Log("UPGRADE");
			scrapMetal -= metalForNextLevel();
			level++;
		}


	}

	public int metalForNextLevel() {
		return Mathf.RoundToInt(baseMetalNeeded * level * 1.5f);
	}

	public void setGameOver() {
		gameOverText.text = "SCORE: " + score + "\nLEVEL: " + level + "\nDISTANCE: " + car.transform.position.z;
		gameOverPanel.SetActive(true);
		Time.timeScale = 0;
	}
}
