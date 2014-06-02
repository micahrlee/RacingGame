using UnityEngine;
using System.Collections;
using G = GameManager;

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
			if(car.Place == 1){	
				G.getInstance ().first = car.Player;
				G.getInstance ().firstTime = car.currentTime;
			}
			if(car.Place == 2){
				G.getInstance ().second = car.Player;
				G.getInstance ().secondTime = car.currentTime;
			}
			finish = true;
		}
	}

	void PrintFinish(){

	}
}
