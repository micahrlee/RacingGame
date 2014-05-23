using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	public const float TOP_SPEED = 2f;
	public const float ACCEL = .005f;
	public const float NITRO_TIME = 2f;
	public const float NITRO_MULT = 2f;
	public const float NITRO_DELAY = 20f;
	public float Speed = 0f;
	public float RotateAngle = 150f;
	public float NitroSpeed = 1f;
	public float currentNitro = 0f;
	public string curCar = "";
	
	private bool nitro = false;
	private float lastNitro = -30f;

	void Start(){
	}

	void Update(){
		UpdateNitro ();
		UpdateMovement ();
		printTimeLimit ();
	}

	void UpdateNitro(){
		#region If Boost Pressed
		if ((Time.realtimeSinceStartup - lastNitro) > NITRO_DELAY) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				nitro = true;
				SpriteRenderer r = GetComponent<SpriteRenderer> ();
				if (r != null) {
					Sprite s = Resources.Load ("Textures/" + curCar +  "Boost", typeof(Sprite)) as Sprite;
					r.sprite = s;
				}
				currentNitro = Time.realtimeSinceStartup;
				lastNitro = Time.realtimeSinceStartup + NITRO_TIME;
				NitroSpeed = 1.5f;
			} 
		}
		#endregion
		#region If Boost not active
		else if(nitro && (Time.realtimeSinceStartup - currentNitro) > NITRO_TIME){
			SpriteRenderer r = GetComponent<SpriteRenderer> ();
			if (r != null) {
				Sprite s = Resources.Load ("Textures/" + curCar, typeof(Sprite)) as Sprite;
				r.sprite = s;
			}
			NitroSpeed = 1f;
		}
		#endregion
	}

	void UpdateMovement(){
		if(Input.GetAxis ("Vertical") != 0){
			if(Speed > TOP_SPEED)
				Speed = TOP_SPEED;
			else	
				Speed += ACCEL;
		}
		else
				Speed = 0f;
		transform.position += (Input.GetAxis("Vertical") * transform.up *(Speed * Time.smoothDeltaTime)) * NitroSpeed;
		float angle = Input.GetAxis ("Horizontal") * (RotateAngle * Time.smoothDeltaTime);
		transform.Rotate (transform.forward, -angle);
	}

	private void printTimeLimit(){
		float time = NITRO_DELAY - (Time.realtimeSinceStartup - lastNitro);
		if (time < 0f)
			time = 0f;
		GameObject echo = GameObject.Find ("Boost");
		GUIText g = echo.GetComponent<GUIText> ();
		if (time == 0f) 
			g.text = "Ready!";
		else if(time > NITRO_DELAY)
			g.text = "BOOST";
		else
			g.text = "" + (int)time;
	}
}