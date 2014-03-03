using UnityEngine;
using System.Collections;
using System;

public class LeaderboardRender : MonoBehaviour {

	private Vector2 scrollViewVector = Vector2.zero;
	private float drawing_y = 180f;
	private int current_increment = 0;
	public GUIStyle leaderboardHeaderStyle;
	public GUIStyle leaderboardTextStyle;
	public GUIStyle headerTextStyle;
	public GUISkin guiSkin;
	
	void OnGUI(){

		GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height),scrollViewVector,new Rect(0,0,Screen.width,Screen.height));

		GUI.Box (new Rect((float)(Screen.width/2.5),Screen.height/18,Screen.width,Screen.height),"FallingZombies - Leaderboard",headerTextStyle);
		if (GUI.Button (new Rect (0, 0, Screen.width/9, Screen.height/7), "Back")) {
			Application.LoadLevel("Menu");		
		}

		string leaders = PlayerPrefs.GetString ("leaderboard");
		string[] leads = leaders.Split(',');

		GUI.Label (new Rect((float)(Screen.width/9),Screen.height/5,Screen.width,Screen.height),"Number",leaderboardHeaderStyle);
		GUI.Label (new Rect((float)(Screen.width/2.5),Screen.height/5,Screen.width,Screen.height),"Name",leaderboardHeaderStyle);
		GUI.Label (new Rect((float)(Screen.width/1.0)-170,Screen.height/5,Screen.width,Screen.height),"Score",leaderboardHeaderStyle);

		GUI.Label (new Rect((float)(Screen.width/6.5),(float)(Screen.height/3.5),Screen.width,Screen.height),leads[0].Split(':')[0],leaderboardTextStyle);
		GUI.Label (new Rect((float)(Screen.width/3),(float)(Screen.height/3.5),Screen.width,Screen.height),leads[0].Split(':')[1],leaderboardTextStyle);
		GUI.Label (new Rect((Screen.width/1)-150,(float)(Screen.height/3.5),Screen.width,Screen.height),leads[0].Split(':')[2],leaderboardTextStyle);

		GUI.Label (new Rect((float)(Screen.width/6.5),(float)(Screen.height/2.8),Screen.width,Screen.height),leads[1].Split(':')[0],leaderboardTextStyle);
		GUI.Label (new Rect((float)(Screen.width/3),(float)(Screen.height/2.8),Screen.width,Screen.height),leads[1].Split(':')[1],leaderboardTextStyle);
		GUI.Label (new Rect((Screen.width/1)-150,(float)(Screen.height/2.8),Screen.width,Screen.height),leads[1].Split(':')[2],leaderboardTextStyle);

		GUI.Label (new Rect((float)(Screen.width/6.5),(float)(Screen.height/2.35),Screen.width,Screen.height),leads[2].Split(':')[0],leaderboardTextStyle);
		GUI.Label (new Rect((float)(Screen.width/3),(float)(Screen.height/2.35),Screen.width,Screen.height),leads[2].Split(':')[1],leaderboardTextStyle);
		GUI.Label (new Rect((Screen.width/1)-150,(float)(Screen.height/2.35),Screen.width,Screen.height),leads[2].Split(':')[2],leaderboardTextStyle);

		GUI.Label (new Rect((float)(Screen.width/6.5),(float)(Screen.height/2.03),Screen.width,Screen.height),leads[3].Split(':')[0],leaderboardTextStyle);
		GUI.Label (new Rect((float)(Screen.width/3),(float)(Screen.height/2.03),Screen.width,Screen.height),leads[3].Split(':')[1],leaderboardTextStyle);
		GUI.Label (new Rect((Screen.width/1)-150,(float)(Screen.height/2.03),Screen.width,Screen.height),leads[3].Split(':')[2],leaderboardTextStyle);

		GUI.Label (new Rect((float)(Screen.width/6.5),(float)(Screen.height/1.78),Screen.width,Screen.height),leads[4].Split(':')[0],leaderboardTextStyle);
		GUI.Label (new Rect((float)(Screen.width/3),(float)(Screen.height/1.78),Screen.width,Screen.height),leads[4].Split(':')[1],leaderboardTextStyle);
		GUI.Label (new Rect((Screen.width/1)-150,(float)(Screen.height/1.78),Screen.width,Screen.height),leads[4].Split(':')[2],leaderboardTextStyle);
	
		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - MAXXOR",guiSkin.box)) { 
			Application.OpenURL("http://www.maxxor.com");
		}

		GUI.EndScrollView ();

	}

	void Start(){

		//Screen.orientation = ScreenOrientation.Landscape;

		if(PlayerPrefs.GetInt("high_score") != -1)
			GameState.currentHighScore = PlayerPrefs.GetInt("high_score");

		if (PlayerPrefs.GetString ("leaderboard") == "") {
			PlayerPrefs.SetString("leaderboard","1:Teddy Roxpin:50,2:Ritz Raynolds:40,3:Hannibal King:30,4:Iman Omari:20,5:Sir Michael Rocks:10");
		}
	

		GameState.currentHighScore = Leaderboard.getCurrentHighScore ();
		GameState.leaderboardNames = Leaderboard.getLeaderBoardNames ();
		GameState.leaderboardScores = Leaderboard.getLeaderBoardScores ();
      //  Screen.showCursor = true;


	}

	void Update(){

		if (Input.GetButtonDown ("Fire2")) {
		//	Screen.lockCursor = !Screen.lockCursor;	
			Debug.Log ("Locker unlocked");
		}
	}


}
