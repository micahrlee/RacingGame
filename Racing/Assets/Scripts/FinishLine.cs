using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {
	public bool finish = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		if(finish)
			PrintFinish ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Car"){
			Car car = other.GetComponent<Car>();
			car.time = false;
			car.finished = true;
			car.Place = GameObject.Find ("LevelManager").GetComponent<LevelManager>().CarsPassed++ + 1;
			finish = true;
		}
	}

	void PrintFinish(){

	}
}
