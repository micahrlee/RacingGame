using UnityEngine;
using System.Collections;
using G = GameManager;

public class LevelManager : MonoBehaviour {
	int playersSpawned = 0;

	public float lastCount = 0f;
	public int CountDown = 3;
	public bool showCountDown = true;
	private bool startTimer = false;
	public int CarsPassed = 0;

	GameObject car = null;

	// Use this for initialization
	void Start () {
		G.getInstance ().PauseMovement ();
	}
	
	// Update is called once per frame
	void Update () {
		if(showCountDown)
			InCountDown ();

		else{
			if(startTimer){
				StartTimer ();
			}
		}

		if (CarsPassed == 2) {
			Application.LoadLevel ("RaceOver");
		}
	}

	void StartTimer(){
		GameObject[] list = GameObject.FindGameObjectsWithTag ("Car");
		foreach(GameObject car in list){
			car.GetComponent<Car>().time = true;
		}
		startTimer = false;
	}

	#region CountDown
	void InCountDown(){
		if (CountDown > -1f) {
			UpdateCountDown ();
			lastCount += Time.deltaTime;
			if(lastCount >= 1){
				CountDown--;
				lastCount = 0f;
			}
		}
		else{
			GameObject echo = GameObject.Find ("CountDown");
			echo.SetActive(false);
			startTimer = true;
			showCountDown = false;
		}
	}

	void UpdateCountDown(){
		GameObject echo = GameObject.Find ("CountDown");
		GUIText g = echo.GetComponent<GUIText> ();
		if(CountDown > 0)
			g.text = "" + CountDown;
		else{
			g.text = "GO!!!!!";
			G.getInstance ().UnpauseMovement ();
		}
	}
	#endregion
}
