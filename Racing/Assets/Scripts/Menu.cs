using UnityEngine;
using System.Collections;
using G = GameManager;
public class Menu : MonoBehaviour {
	#region Network Stuff
	public int Port = 23466;
	private const string registeredName = "Racing Game for CSS 432";
	public string gameName = "";

	float refreshRequestLength = 3.5f;
	HostData[] hostData;
	Vector2 scrollPos;
	#endregion
	#region Pages
	private Page currentPage = Page.Main;
	public enum Page{
		Main, StartServer, GameName, JoinGame, Options
	}
	#endregion

	void Start(){

	}

	void OnGUI(){
		switch (currentPage) {
		case Page.Main: ShowMain(); break;
		case Page.StartServer: ServerCreate(); break;
		case Page.GameName: TypeName(); break;
		case Page.JoinGame: ListGames(); break;
		case Page.Options: ShowOptions(); break;
		}
	}

	#region Main Menu
	void ShowMain () {
		GUILayout.BeginArea (new Rect ((Screen.width/2) - 150, Screen.height / 2, 300f, 100));
		if (GUILayout.Button ("Join Game"))
			currentPage = Page.JoinGame;
		if (GUILayout.Button ("Start Server"))
			currentPage = Page.StartServer;
		if(GUILayout.Button ("Options"))
			currentPage = Page.Options;
		if(GUILayout.Button ("Quit Game")){
			Application.Quit ();
		}
		GUILayout.EndArea ();
	}
	#endregion

	#region Options
	void ShowOptions(){
		GUILayout.BeginArea (new Rect(Screen.width /2 - 250, Screen.height/2.5f, 500, 400));
		GUILayout.BeginVertical ();
		GUILayout.BeginHorizontal ();
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		GUILayout.Space (230);
		GUILayout.Label ("Volume");
		GUILayout.EndHorizontal ();
		VolumeControl ();
		GUILayout.Space (25);
		GUILayout.BeginHorizontal ();
		GUILayout.Space (200);
		GUILayout.Label ("Graphics Quality");
		GUILayout.EndHorizontal ();
		GraphicControl ();
		GUILayout.BeginHorizontal ();
		GUILayout.Space (25);
		G.getInstance().showFPS = GUILayout.Toggle (G.getInstance().showFPS, "Show FPS");
		GUILayout.EndHorizontal ();
		GUILayout.Space (20);
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
		GUILayout.BeginArea (new Rect(Screen.width - 150f, Screen.height - 75, 110, 50));
		if(GUILayout.Button ("Save")){
			G.getInstance().SaveOptions ();
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
		GUILayout.Space (235);
		GUILayout.Label ((int)(AudioListener.volume * 100) + "%");
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();
	}
	
	private void GraphicControl(){
		GUILayout.BeginHorizontal ();
		G.getInstance().graphicsQuality = (int)GUILayout.HorizontalSlider ((float)G.getInstance().graphicsQuality, 1.0f, 6.0f);
		GUILayout.EndHorizontal ();
		GUILayout.BeginVertical ();
		GUILayout.BeginHorizontal ();
		GUILayout.Space (220);
		switch (G.getInstance().graphicsQuality) {
		case 1:
			QualitySettings.SetQualityLevel (1);
			GUILayout.Label("Fastest");
			break;
		case 2:
			QualitySettings.SetQualityLevel (2);
			GUILayout.Label("Fast");
			break;
		case 3:
			QualitySettings.SetQualityLevel (3);
			GUILayout.Label("Simple");
			break;
		case 4:
			QualitySettings.SetQualityLevel (4);
			GUILayout.Label("Good");
			break;
		case 5:
			QualitySettings.SetQualityLevel (5);
			GUILayout.Label("Beautiful");
			break;
		case 6:
			QualitySettings.SetQualityLevel (6);
			GUILayout.Label("Fantastic");
			break;
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();
	}
	#endregion

	#region Server Page Stuff
	void ServerCreate(){
		GUILayout.BeginArea (new Rect ((Screen.width/2) - 150, Screen.height / 2, 300f, 100));
		if (GUILayout.Button ("Start Server")) {
			currentPage = Page.GameName;
		}
		if(GUILayout.Button ("Logout")){
			Network.Disconnect (250);
			currentPage = Page.Main;
		}
		GUILayout.EndArea ();
		HomePage ();
	}

	void TypeName(){
		GUILayout.BeginArea (new Rect ((Screen.width/2) - 150, Screen.height / 2, 300f, 100));
		GUILayout.Label ("Enter a name for your game");
		gameName = GUILayout.TextField (gameName.Replace ("\n", "").Replace ("\r", ""), 20);
		if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
			StartServer ();
		if(GUILayout.Button ("Enter"))
			StartServer ();
		GUILayout.EndArea ();
		HomePage ();
	}

	void OnServerInitialized(){
		Debug.Log ("Server Initialized");
	}

	void OnMasterServerEvent(MasterServerEvent e){
		if(e == MasterServerEvent.RegistrationSucceeded)
			Debug.Log ("Registration Successful");
	}

	void StartServer(){
		Network.InitializeServer (2, 25002, false);
		MasterServer.RegisterHost (registeredName, gameName, "CSS432 Racing Game");
	}
	#endregion

	#region Games List
	void ListGames(){
		GUI.Box (new Rect ((Screen.width/2) - 250, 75, 500, Screen.height - 100), "");
		GUILayout.BeginArea (new Rect ((Screen.width/2) - 250, 50, 500, Screen.height - 50));
		if(GUILayout.Button ("Refresh Games")){
			StartCoroutine ("RefreshHostList");
		}
		scrollPos = GUILayout.BeginScrollView (scrollPos, GUILayout.Width(500), GUILayout.Height(Screen.height - 50));
		if(hostData != null){
			foreach(HostData data in hostData){
				if(GUILayout.Button (data.gameName)){
					Network.Connect(data.gameName);
				}
			}
		}
		GUILayout.EndScrollView ();
		GUILayout.EndArea ();
		GUILayout.BeginArea (new Rect ((Screen.width) - 110, Screen.height - 25, 100f, 100));
		if(GUILayout.Button ("Home")){
			currentPage = Page.Main;
		}
		GUILayout.EndArea();
		HomePage ();
	}

	public IEnumerator RefreshHostList(){
		Debug.Log ("Refreshing...");
		MasterServer.RequestHostList (registeredName);
		float endTime = Time.time + refreshRequestLength;

		while(Time.time < endTime){
			//update host data
			hostData = MasterServer.PollHostList();
			yield return new WaitForEndOfFrame();
		}

		if(hostData == null || hostData.Length == 0)
			Debug.Log ("No Active Servers");
	}
	#endregion

	private void HomePage(){
		GUILayout.BeginArea (new Rect ((Screen.width) - 110, Screen.height - 25, 100f, 100));
		if(GUILayout.Button ("Home")){
			currentPage = Page.Main;
		}
		GUILayout.EndArea ();
	}
}
