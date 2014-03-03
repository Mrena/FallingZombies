using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour {

	private Vector2 scrollViewVector = Vector2.zero;
	public GUIStyle instructionsTextStyle;
	public GUIStyle instructionsHeaderStyle;
	public GUISkin guiSkin;
	
	void OnGUI(){

		instructionsTextStyle.fontSize = (int)(Screen.height/40);

		GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height),scrollViewVector,new Rect(0,0,Screen.width,Screen.height));

		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"FallingZombies - Instructions");
		if (GUI.Button (new Rect (0, 0, Screen.width/9, Screen.height/7), "Back")) {
			Application.LoadLevel("Menu");		
		}
		GUI.Label (new Rect((float)(Screen.width/30),(float)(Screen.height/4.5),Screen.width,Screen.height),"In FallingZombies your objective is to shot down all of the zombies in a level in the fastest amount of time.",instructionsTextStyle);
		GUI.Label (new Rect((float)(Screen.width/30),(float)(Screen.height/3.95),Screen.width,Screen.height),"As you take longer to shoot down the zombies, your life point decreases by one.",instructionsTextStyle);
		GUI.Label (new Rect((float)(Screen.width/30),(float)(Screen.height/3.47),Screen.width,Screen.height),"When you shoot down a zombie you gain five life points.",instructionsTextStyle);
		GUI.Label (new Rect((float)(Screen.width/30),(float)(Screen.height/3.11),Screen.width,Screen.height),"When a zombie attacks you your life points decreases depending on the level of impact.",instructionsTextStyle);

		GUI.Label (new Rect((float)(Screen.width/2.2),(float)(Screen.height/2.2),Screen.width,Screen.height),"Buttons",instructionsHeaderStyle);

		GUI.Label (new Rect((float)(Screen.width/30),(float)(Screen.height/1.87),Screen.width,Screen.height),"W - Forward",instructionsTextStyle);
		GUI.Label (new Rect((float)(Screen.width/30),(float)(Screen.height/1.77),Screen.width,Screen.height),"A - Left",instructionsTextStyle);
		GUI.Label (new Rect((float)(Screen.width/30),(float)(Screen.height/1.67),Screen.width,Screen.height),"S - Backward",instructionsTextStyle);
		GUI.Label (new Rect((float)(Screen.width/30),(float)(Screen.height/1.57),Screen.width,Screen.height),"D - Right",instructionsTextStyle);

		GUI.Label (new Rect((float)(Screen.width/30),(float)(Screen.height/1.47),Screen.width,Screen.height),"Use your mouse to look around.",instructionsTextStyle);

		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}


		GUI.EndScrollView ();

	}

	void Start(){

		//Screen.orientation = ScreenOrientation.Landscape;
       // Screen.showCursor = true;

	}
}
