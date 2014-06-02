using UnityEngine;
using System.Collections;
using G = GameManager;

public class WinningScreen : MonoBehaviour {

	// Use this for initialization
	
	// Update is called once per frame
	void OnGUI () {

		GUILayout.BeginArea (new Rect(Screen.width / 2 - 55, Screen.height/2 - 150, 150, 100));
		GUILayout.Label (G.getInstance ().first == 1 ? "Player 1" : "Player 2" + " Is The Winner!");
		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect(Screen.width/2 - 50, Screen.height/2 - 50, 150, 100));
		GUILayout.BeginVertical ();
		GUILayout.BeginHorizontal ();
		GUILayout.Label (G.getInstance().first == 1 ? "Player 1" : "Player 2");
		GUILayout.Label ("Time: " + G.getInstance ().firstTime);
		GUILayout.EndHorizontal ();
		GUILayout.Space (25);
		GUILayout.BeginHorizontal ();
		GUILayout.Label (G.getInstance().first == 1 ? "Player 1" : "Player 2");
		GUILayout.Label ("Time: " + G.getInstance ().secondTime);
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();
		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect(Screen.width - 150, Screen.height - 75, 100, 100));
		if(GUILayout.Button ("Main Menu"))
			Application.LoadLevel ("Main Menu");
		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect(Screen.width - 270, Screen.height - 75, 100, 100));
		if(GUILayout.Button ("High Scores"))
			//Display High Scores
			return;

		GUILayout.EndArea ();
	}
}
