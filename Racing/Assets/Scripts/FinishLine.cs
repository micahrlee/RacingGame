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
		finish = true;
	}

	void PrintFinish(){
		GUILayout.BeginArea (new Rect(Screen.width/2 - 250, Screen.height * .35f, 500, 500));
		GUILayout.Label ("You're Winner!");
		GUILayout.EndArea ();
	}
}
