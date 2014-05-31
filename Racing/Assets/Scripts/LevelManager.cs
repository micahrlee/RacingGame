using UnityEngine;
using System.Collections;
using G = GameManager;

public class LevelManager : MonoBehaviour {
	public float lastCount = 0f;
	public int CountDown = 3;
	public bool showCountDown = true;
	// Use this for initialization
	void Start () {
		G.getInstance ().PauseMovement ();
	}
	
	// Update is called once per frame
	void Update () {
		if(showCountDown)
			InCountDown ();
	}

	#region CountDown
	void InCountDown(){
		if (CountDown > 0) {
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
			G.getInstance ().UnpauseMovement ();
			showCountDown = false;
		}
	}

	void UpdateCountDown(){
		GameObject echo = GameObject.Find ("CountDown");
		GUIText g = echo.GetComponent<GUIText> ();
		if(CountDown != 0)
			g.text = "" + CountDown;
		else
			g.text = "GO";
	}
	#endregion
}
