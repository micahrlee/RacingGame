using UnityEngine;
using System.Collections;

public class BorderBehavior : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Car") {
			other.GetComponent<Car>().currentTime += 1f;
		}
	}
}
