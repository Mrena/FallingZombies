using UnityEngine;
using System.Collections;

public class Profile : MonoBehaviour {

	private Vector2 scrollViewVector = Vector2.zero;
	public GUISkin guiSkin;
    private ProfileInfo profileInfo;

	// Use this for initialization
	void Start ()
	{
      //  Screen.orientation = ScreenOrientation.Landscape;
	 //   Screen.showCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        profileInfo = new ProfileInfo();
    }

	void OnGUI(){
		
		scrollViewVector = GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height),scrollViewVector,new Rect(0,0,Screen.width,Screen.height));
		
		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"FallingZombies - Profile");
		if (GUI.Button (new Rect (0, 0, Screen.width/9, Screen.height/7), "Back")) {
			Application.LoadLevel("Menu");		
		}


        GUI.Label(new Rect((float)(Screen.width/7),(float)(Screen.height/5.1),(float)(Screen.width),(float)(Screen.height)),"First Name : ");
        profileInfo.FirstName = GUI.TextField(new Rect((float)(Screen.width / 5), (float)(Screen.height / 5.5), (float)(Screen.width/5), (float)(Screen.height/15)), "Name");

        GUI.Label(new Rect((float)(Screen.width / 7), (float)(Screen.height / 3.6), (float)(Screen.width), (float)(Screen.height)), "Last Name : ");
        GUI.TextField(new Rect((float)(Screen.width / 5), (float)(Screen.height / 3.8), (float)(Screen.width / 5), (float)(Screen.height / 15)), "Last Name");

        GUI.Label(new Rect((float)(Screen.width / 7), (float)(Screen.height / 2.75), (float)(Screen.width), (float)(Screen.height)), "DOB : ");
        GUI.TextField(new Rect((float)(Screen.width / 5), (float)(Screen.height / 2.9), (float)(Screen.width / 17), (float)(Screen.height / 15)), "Date");
        GUI.TextField(new Rect((float)(Screen.width / 3.8), (float)(Screen.height / 2.9), (float)(Screen.width / 17), (float)(Screen.height / 15)), "Month");
        GUI.TextField(new Rect((float)(Screen.width / 3.05), (float)(Screen.height / 2.9), (float)(Screen.width / 17), (float)(Screen.height / 15)), "Year");

        GUI.Label(new Rect((float)(Screen.width / 7), (float)(Screen.height / 2.25), (float)(Screen.width), (float)(Screen.height)), "Sex : ");
        GUI.TextField(new Rect((float)(Screen.width / 5), (float)(Screen.height / 2.35), (float)(Screen.width / 5), (float)(Screen.height / 15)), "Male");

        GUI.Label(new Rect((float)(Screen.width / 7), (float)(Screen.height / 1.92), (float)(Screen.width), (float)(Screen.height)), "City : ");
        GUI.TextField(new Rect((float)(Screen.width / 5), (float)(Screen.height / 1.99), (float)(Screen.width / 5), (float)(Screen.height / 15)), "Ape Town");

        GUI.Label(new Rect((float)(Screen.width / 7), (float)(Screen.height / 1.68), (float)(Screen.width), (float)(Screen.height)), "Country : ");
        GUI.TextField(new Rect((float)(Screen.width / 5), (float)(Screen.height / 1.72), (float)(Screen.width / 5), (float)(Screen.height / 15)), "South Africa");

        GUI.Label(new Rect((float)(Screen.width / 7), (float)(Screen.height / 1.4), (float)(Screen.width), (float)(Screen.height)), "Biography : ");
        GUI.TextArea(new Rect((float)(Screen.width / 5), (float)(Screen.height / 1.5), (float)(Screen.width/5), (float)(Screen.height/10)), "Hello, this is DOG");
		
		
		
		
		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}
		
		GUI.EndScrollView ();
		
		
		
	}

}
