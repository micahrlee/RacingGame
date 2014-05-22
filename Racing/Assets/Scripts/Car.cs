using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	public const float TOP_SPEED = 3f;
	public const float ACCEL = .01f;
	public float Speed = 0f;
	public float RotateAngle = 250f;
	public float NitroSpeed = 1f;
	public float NitroTime = 5f;
	private float startTime = -1f;

	void Start(){
	}

	void Update(){
		UpdateNitro ();
		UpdateMovement ();
	}

	void UpdateNitro(){
		if(Input.GetKey(KeyCode.Space) && NitroTime > 0f){
			startTime = Time.time;
			NitroSpeed = 2f;
			NitroTime -= (Time.time - startTime);
		}
		else
			NitroSpeed = 1f;
	}

	void UpdateMovement(){
		if(Input.GetAxis ("Vertical") != 0){
			if(Speed > TOP_SPEED)
				Speed = TOP_SPEED;
			else	
				Speed += ACCEL;
		}
		else{
			if(Speed > 0f)
				Speed -= ACCEL * 2;
			else
				Speed = 0f;
		}
		transform.position += (Input.GetAxis("Vertical") * transform.up *(Speed * Time.smoothDeltaTime)) * NitroSpeed;
		float angle = Input.GetAxis ("Horizontal") * (RotateAngle * Time.smoothDeltaTime);
		transform.Rotate (transform.forward, -angle);
	}
}