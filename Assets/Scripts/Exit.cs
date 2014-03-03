using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	private Vector2 scrollViewVector = Vector2.zero;
	private int selectedButton = 1;
	public GUISkin guiSkin;

	void OnGUI(){

		GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height),scrollViewVector,new Rect(0,0,Screen.width,Screen.height));

		GUI.Box (new Rect(0,0,Screen.width,Screen.height),"FallingZombies - Exit");

		GUI.Label (new Rect((float)(Screen.width/2.5),(Screen.height/3)-30,Screen.width,Screen.height/10),"Are you sure you want to exit?");
		string[] exitButtons = {"Yes","No"};
		//selectedButton = GUI.SelectionGrid (new Rect((float)(Screen.width/2.8),Screen.height/3,Screen.width/5,Screen.height/10),selectedButton,exitButtons,2);
		if (GUI.Button (new Rect ((float)(Screen.width / 2.8), Screen.height / 3, Screen.width / 7, Screen.height / 10), "Yes")) {
            PlayerPrefs.Save();
			Application.Quit();
		}

		if (GUI.Button (new Rect ((float)(Screen.width / 1.8), Screen.height / 3, Screen.width / 7, Screen.height / 10), "No")) {
			Application.LoadLevel("Menu");		
			
		}

		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}


		GUI.EndScrollView ();
	}

	void Start(){

		//Screen.orientation = ScreenOrientation.Landscape;
	  //  Screen.showCursor = true;

	}
}
