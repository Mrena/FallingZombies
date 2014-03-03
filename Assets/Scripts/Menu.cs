using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Menu : MonoBehaviour {

	SettingsController settings;
	private int offset = 10;
	private Vector2 scrollViewVector = Vector2.zero;
	public GUISkin guiSkin;

	
	// Update is called once per frame
	void OnGUI () {
	    
		 scrollViewVector = GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height*2),scrollViewVector,new Rect(0,0,Screen.width,Screen.height+(Screen.height/5)));

		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"FallingZombies");
		if (GUI.Button (new Rect (offset, (float)(Screen.height/14.3), Screen.width - (offset * 2),(float)(Screen.height/7.19)), "New Game")) { 
						Application.LoadLevel (7); 
				}

		if (GUI.Button (new Rect (offset, (float)(Screen.height/4.9), Screen.width - (offset * 2),(float)(Screen.height/7.19)), "Instructions")) { 
						Application.LoadLevel (2);
				}

		if (GUI.Button (new Rect (offset,(float)(Screen.height/2.95), Screen.width - (offset * 2), (float)(Screen.height/7.19)), "Settings")) { 
						// Settings has been clicked
						Application.LoadLevel (3); 
				}

		if (GUI.Button (new Rect (offset,(float)(Screen.height/2.12), Screen.width - (offset * 2),(float)(Screen.height/7.19)), "Leaderboard")) {
						// Leaderboard has been clicked
						Application.LoadLevel (4);
				}

		if (GUI.Button (new Rect (offset, (float)(Screen.height/1.65), Screen.width - (offset * 2),(float)(Screen.height/7.19)), "About")) { 
						// About has been clicked
						Application.LoadLevel (5);
				}

		if (GUI.Button (new Rect (offset,(float)(Screen.height/1.35), Screen.width - (offset * 2), (float)(Screen.height/7.19)), "Exit")) { 
						// Exit has been clicked
						Application.LoadLevel (6);
				}
	

		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}

		GUI.EndScrollView ();

	} 

	// Use this for initialization
	void Start () {

		//Screen.orientation = ScreenOrientation.Landscape;
		settings = new SettingsController ();
		settings.UpdateGameSettings ();
	  //  Screen.showCursor = true;
	
	}

}