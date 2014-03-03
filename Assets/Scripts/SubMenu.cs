using UnityEngine;
using System.Collections;

public class SubMenu : MonoBehaviour {

	private int offset = 20;
	private Vector2 scrollViewVector = Vector2.zero;
	public GUISkin guiSkin;

	void OnGUI(){

		GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height+(Screen.height/3)),scrollViewVector,new Rect(0,0,Screen.width,Screen.height+(Screen.height/3)));

		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"FallingZombies");

		if (GUI.Button (new Rect (0, 0, Screen.width/9, Screen.height/7), "Back")) {
		
			Application.LoadLevel("Menu");		
		}
		
		if (GUI.Button (new Rect (Screen.width/68 ,(float)(Screen.height/4.25), Screen.width-(offset*2), (float)(Screen.height/5.9)),"Multi Player Game")) {
			if(PlayerPrefs.GetString("username") == ""){

	
				Application.LoadLevel("Login");

			}else{

				GameState.resetGameState();
				GameState.multiplayerGame = true;
				Application.LoadLevel(8);

			}
			 
		}

		if (GUI.Button (new Rect (Screen.width/68, (float)(Screen.height/2.49), Screen.width-(offset*2), (float)(Screen.height/5.9)),"Single Player Game")) { 
			GameState.resetGameState();
			GameState.multiplayerGame = false;
			Application.LoadLevel(8);
		}

		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}

		GUI.EndScrollView ();

	}

	void Start(){

		//Screen.orientation = ScreenOrientation.Landscape;
     //   Screen.showCursor = true;
	}
}
