using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : UnitySingleton<GameManager>
{
	#region Pause Menu
	public bool paused = false;
	public bool ShowPauseMenu = false;
	#region FPS
	public bool showFPS = false;
	private float accum = 0f;
	private int frames = 0;
	private float timeLeft;
	private const float updateInterval = 0.5f;
	GUIText FPS = null;
	#endregion

	#region Game Stuff
	public int first = 1;
	public int second = 2;
	public float firstTime;
	public float secondTime;
	#endregion
	
	#region Page
	public enum Page{
		Main, Options
	}
	private Page currentPage = Page.Main;
	#endregion
	public int graphicsQuality = 4;
	#endregion

	void Start ()
    {
		Application.targetFrameRate = 60;
		LoadFPS ();
		LoadOptions ();
	}
	
	void Update()
    {
		UpdateKeyboard ();
		ShowFPSText ();
	}
	void OnGUI() {
		Pause ();
	}
	
	#region Update Methods
	private void UpdateKeyboard(){
		#region Escape
		if (Input.GetKeyDown (KeyCode.Escape)){
			if(Application.loadedLevelName != "MainMenu" && Application.loadedLevelName != "Loading" && !paused)
				PauseGame ();
			else if(paused && currentPage == Page.Main)
				UnpauseGame ();
			else if (Input.GetKeyDown (KeyCode.Escape) && currentPage != Page.Main){
				currentPage = Page.Main;
			}
		}
		#endregion
	}
	#endregion
	
	#region Pause Menu Methods
	void Pause(){
		if(paused && ShowPauseMenu)
		switch (currentPage) {
			case Page.Main: showPauseMenu(); break;
			case Page.Options: ShowOptions(); break;
		}
	}
	
	void showPauseMenu(){
		GUILayout.BeginArea (new Rect(Screen.width/2f - 175, Screen.height/2f - 200, 400, 400));
			#region Pause Menu Options
			GUILayout.BeginVertical ();
				if (GUILayout.Button ("Continue"))
						UnpauseGame ();
				GUILayout.Space (10);
				if (GUILayout.Button ("Main Menu")) {
						Application.LoadLevel ("MainMenu");
						UnpauseGame ();
				}
				GUILayout.Space (10);
				if (GUILayout.Button ("Options"))
						currentPage = Page.Options;
				GUILayout.Space (10);
				if (GUILayout.Button ("Quit Game")) {
						Application.Quit ();
				}
			GUILayout.EndVertical ();
			#endregion
		GUILayout.EndArea ();
	}
	
	void ShowBackButton(){
		GUILayout.BeginArea (new Rect(Screen.width - 150, Screen.height - 75 , 110, 50));
		if(GUILayout.Button ("Back"))
			currentPage = Page.Main;
		GUILayout.EndArea ();
	}
	
	private void EndPage() {
		GUILayout.EndArea();
		if (currentPage != Page.Main) {
			ShowBackButton();
		}
	}
	#region Pause Methods
	public void UnpauseGame ()
	{
			paused = false;
			Time.timeScale = 1f;
	}

	public void PauseGame ()
	{
			paused = true;
			Time.timeScale = 0f;
			ShowPauseMenu = true;
	}

	public void PauseMovement ()
	{
			paused = true;
			ShowPauseMenu = false;
	}

	public void UnpauseMovement ()
	{
			paused = false;
	}

	public void PauseMovementTS ()
	{
			paused = true;
			ShowPauseMenu = false;
			Time.timeScale = 0f;
	}

	public void UnpauseMovementTS ()
	{
			paused = false;
			Time.timeScale = 1f;
	}
	#endregion

	#region Options
	void ShowOptions(){
		GUILayout.BeginArea (new Rect(Screen.width/2f - 150, Screen.height/2 - 200, 300, 400));
			#region Options Menu
			GUILayout.BeginVertical ();
				GUILayout.BeginHorizontal ();
					GUILayout.Space (145);
					GUILayout.Label ("Volume");
				GUILayout.EndHorizontal ();

				VolumeControl ();
				GUILayout.Space (25);

				GUILayout.BeginHorizontal ();
					GUILayout.Space (120);
					GUILayout.Label ("Graphics Quality");
				GUILayout.EndHorizontal ();

				GraphicControl ();

				GUILayout.BeginHorizontal ();
					showFPS = GUILayout.Toggle (showFPS, "Show FPS");
				GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
			#endregion 
		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect(Screen.width - 150f, Screen.height - 75f, 110, 50));
		if(GUILayout.Button ("Save")){
			SaveOptions ();
			currentPage = Page.Main;
		}
		GUILayout.EndArea ();
	}
	
	private void VolumeControl(){
		GUILayout.BeginHorizontal ();
			AudioListener.volume = GUILayout.HorizontalSlider (AudioListener.volume, 0f, 1f);
		GUILayout.EndHorizontal ();

		GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
				GUILayout.Space (150);
				GUILayout.Label ((int)(AudioListener.volume * 100) + "%");
			GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();
	}
	
	private void GraphicControl(){	
		GUILayout.BeginHorizontal ();
			graphicsQuality = (int)GUILayout.HorizontalSlider ((float)graphicsQuality, 1.0f, 6.0f);
		GUILayout.EndHorizontal ();

		GUILayout.BeginVertical ();
			#region Quality Labels
			GUILayout.BeginHorizontal ();
				GUILayout.Space (145);
				switch (graphicsQuality) {
				case 1:
						QualitySettings.SetQualityLevel (1);
						GUILayout.Label ("Fastest");
						break;
				case 2:
						QualitySettings.SetQualityLevel (2);
						GUILayout.Label ("Fast");
						break;
				case 3:
						QualitySettings.SetQualityLevel (3);
						GUILayout.Label ("Simple");
						break;
				case 4:
						QualitySettings.SetQualityLevel (4);
						GUILayout.Label ("Good");
						break;
				case 5:
						QualitySettings.SetQualityLevel (5);
						GUILayout.Label ("Beautiful");
						break;
				case 6:
						QualitySettings.SetQualityLevel (6);
						GUILayout.Label ("Fantastic");
						break;
				}
			GUILayout.EndHorizontal ();
			#endregion
		GUILayout.EndVertical ();
	}
	#region FPS
	private void LoadFPS(){
		if(FPS == null){
			GameObject go = new GameObject();
			go.name = "FPS";

			FPS = (GUIText) go.AddComponent(typeof(GUIText));
			//FPS.transform.position = new Vector3 (.92f, .98f, 0);

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
	#endregion
	#endregion
}
