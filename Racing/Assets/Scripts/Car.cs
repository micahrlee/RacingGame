using UnityEngine;
using System.Collections;
using G = GameManager;

public class Car : MonoBehaviour {
	public const float TOP_SPEED = 1.5f;
	public const float ACCEL = .003f;
//	public const float NITRO_TIME = 2f;
//	public const float NITRO_MULT = 2f;
//	public const float NITRO_DELAY = 20f;
	public float Speed = 0f;
	public float RotateAngle = 175f;
//	public float NitroSpeed = 1f;
//	public float currentNitro = 0f;
//	private bool nitro = false;
	//	private float lastNitro = -30f;
	//public string curCar = "";
	//Time
	public float currentTime;
	public bool time = false;
	public bool finished = false;
	public int Place = 0;
	public int Player = 2;

	public GameObject opp = null;

	void Start(){
		//receive which player this car is
		if(opp == null)
			opp = Resources.Load ("Prefabs/Opponent") as GameObject;
		if(Player == 1){
			transform.position = new Vector3 (-0.1269732f, -0.1496694f, 0);
			opp = (GameObject)Instantiate (opp, new Vector3 (0.1002542f, -0.1496694f, 0), Quaternion.identity);
		}
		else if(Player == 2){
			transform.position = new Vector3 (0.1002542f, -0.1496694f, 0);
			opp = (GameObject)Instantiate (opp, new Vector3 (-0.1269732f, -0.1496694f, 0), Quaternion.identity);
		}
	}

	void Update(){
		showTime ();
		if(!G.getInstance().paused && !finished){
			//UpdateNitro ();
			UpdateMovement ();
			//printTimeLimit ();
		}

		if(time){
			StartTimer();
		}

		showTime ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Border"){
			currentTime += 1f;
		}
	}

//	void UpdateNitro(){
//		#region If Boost Pressed
//		if ((Time.realtimeSinceStartup - lastNitro) > NITRO_DELAY) {
//			if (Input.GetKeyDown (KeyCode.Space)) {
//				nitro = true;
//				SpriteRenderer r = GetComponent<SpriteRenderer> ();
//				if (r != null) {
//					Sprite s = Resources.Load ("Textures/" + curCar +  "Boost", typeof(Sprite)) as Sprite;
//					r.sprite = s;
//				}
//				currentNitro = Time.realtimeSinceStartup;
//				lastNitro = Time.realtimeSinceStartup + NITRO_TIME;
//				NitroSpeed = 1.5f;
//			} 
//		}
//		#endregion
//		#region If Boost not active
//		else if(nitro && (Time.realtimeSinceStartup - currentNitro) > NITRO_TIME){
//			SpriteRenderer r = GetComponent<SpriteRenderer> ();
//			if (r != null) {
//				Sprite s = Resources.Load ("Textures/" + curCar, typeof(Sprite)) as Sprite;
//				r.sprite = s;
//			}
//			NitroSpeed = 1f;
//		}
//		#endregion
//	}

	void UpdateMovement(){
		if (Input.GetKey (KeyCode.Space)) {
			Speed -= .003f;
			if(Speed < 0)
				Speed = 0f;
		}
		else if(Input.GetAxis ("Vertical") != 0){
			if(Speed > TOP_SPEED)
				Speed = TOP_SPEED;
			else	
				Speed += ACCEL;
		}
		else
				Speed = 0f;
		transform.position += (Input.GetAxis("Vertical") * transform.up *(Speed * Time.smoothDeltaTime));
		float angle = Input.GetAxis ("Horizontal") * (RotateAngle * Time.smoothDeltaTime);
		transform.Rotate (transform.forward, -angle);
	}

//	private void printTimeLimit(){
//		float time = NITRO_DELAY - (Time.realtimeSinceStartup - lastNitro);
//		if (time < 0f)
//			time = 0f;
//		GameObject echo = GameObject.Find ("Boost");
//		GUIText g = echo.GetComponent<GUIText> ();
//		if (time == 0f) 
//			g.text = "Ready!";
//		else if(time > NITRO_DELAY)
//			g.text = "BOOST";
//		else
//			g.text = "" + (int)time;
//	}

	public void showTime(){
		GameObject echo = GameObject.Find ("Time Elapsed");
		GUIText g = echo.GetComponent<GUIText> ();
		g.text = "Elapsed Time: " + currentTime;
	}

	public void StartTimer(){
		currentTime += Time.deltaTime;
	}
}