using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	public GameObject howTo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play() {
		Application.LoadLevel("Scene");
		Time.timeScale = 1;
	}

	public void MainMenu() {
		Application.LoadLevel("Menu");
		Time.timeScale = 1;
	}

	public void Exit() {
		Application.Quit();
	}

	public void HowTo() {
		howTo.SetActive(true);
	}
	public void HideHowTo() {
		howTo.SetActive(false);
	}

	public void Hover() {
		GetComponent<AudioSource>().Play();
	}
}
