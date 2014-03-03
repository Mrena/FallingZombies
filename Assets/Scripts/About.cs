using UnityEngine;
using System.Collections;

public class About : MonoBehaviour {

	private Vector2 scrollViewVector = Vector2.zero;
	public GUIStyle aboutTextStyle;
	public GUISkin guiSkin;

	void OnGUI(){

		aboutTextStyle.fontSize = (int)(Screen.height / 20);
		GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height),scrollViewVector,new Rect(0,0,Screen.width,Screen.height));
 
		GUI.Box (new Rect(0,0,Screen.width,Screen.height),"FallingZombies - About");

		if (GUI.Button (new Rect (0, 0, Screen.width/9, Screen.height/7), "Back")) {
			Application.LoadLevel("Menu");		
		}

		GUI.Label (new Rect((float)(Screen.width/2.769),Screen.height/2,Screen.width,Screen.height),"Developed by Maxxor",aboutTextStyle);

		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - MAXXOR",guiSkin.box)) { 
			Application.OpenURL("http://www.maxxor.com");
		}

		GUI.EndScrollView ();
		
	}

	void Start(){

       // Screen.orientation = ScreenOrientation.Landscape;
        //Screen.showCursor = true;
	}
}
