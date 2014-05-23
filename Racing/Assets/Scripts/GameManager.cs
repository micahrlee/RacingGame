using UnityEngine;
using System.Collections;

public class GameManager : UnitySingleton<GameManager> {
	#region FPS
	public bool showFPS = false;
	private float accum = 0f;
	private int frames = 0;
	private float timeLeft;
	private const float updateInterval = 0.5f;
	GUIText FPS = null;
	#endregion
	#region Graphics Quality
	public int graphicsQuality = 4;
	#endregion

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		LoadFPS ();
		LoadOptions ();
	}
	
	// Update is called once per frame
	void Update () {
		ShowFPSText ();
	}

	#region FPS
	private void LoadFPS(){
		if(FPS == null){
			GameObject go = new GameObject();
			go.name = "FPS";
			
			FPS = (GUIText) go.AddComponent(typeof(GUIText));
			
			FPS.anchor = TextAnchor.UpperRight;
			FPS.transform.position = new Vector3 (1f, 1f, 0f);
			FPS.text = "";
		}
	}
	
	private void ShowFPSText(){
		LoadFPS ();
		if(showFPS){
			timeLeft -= Time.deltaTime;
			accum += Time.timeScale / Time.deltaTime;
			++frames;
			FPSControl ();
		}
		else
			FPS.text = "";
	}
	
	private void FPSControl(){
		if (showFPS) {
			if (timeLeft <= 0.0f) {
				float fps = accum/frames;
				string fpsFormat = System.String.Format ("{0:F2} FPS", fps);
				FPS.text = fpsFormat;
				if(fps < 30)
					FPS.material.color = Color.yellow;
				else if(fps < 10)
					FPS.material.color = Color.red;
				else
					FPS.material.color = Color.green;
				timeLeft = updateInterval;
				accum = 0.0f;
				frames = 0;
			}
		}
	}
	#endregion
	#region Save/Load Options
	public void SaveOptions(){
		PlayerPrefs.SetFloat ("Volume", AudioListener.volume);
		PlayerPrefs.SetInt ("Graphics", graphicsQuality);
		PlayerPrefs.SetInt ("FPS", showFPS == true ? 1 : 0);
	}
	private void LoadOptions(){
		graphicsQuality = PlayerPrefs.GetInt ("Graphics");
		if(!PlayerPrefs.HasKey ("Graphics"))
			graphicsQuality = 4;
		AudioListener.volume = PlayerPrefs.GetFloat ("Volume");
		if(!PlayerPrefs.HasKey ("Volume"))
			AudioListener.volume = 1f;
		showFPS = PlayerPrefs.GetInt ("FPS") == 1 ? true : false;
	}
	#endregion



}
