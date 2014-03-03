using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;


public class Register : MonoBehaviour {

	private Vector2 scrollViewVector = Vector2.zero;
	public GUISkin guiSkin;
	private string username = "";
	private string emailAddress = "";
	private string password = "";
	private string passwordTwo = "";

	private Color guiDefaultColor;
	private string controlToFocus = "username";
	private List<string> controlsWithError = new List<string>();
	private List<string> errorMessages = new List<string>();
	private float chatDisplayOffset = 1f;
	private string errorMessageText = "";

    private string registrationURL = URL.registrationURL;
	private int registrationSuccess = 0;
	private int startTime = 0;
	private int endTime = 100;

// Use this for initialization
	void Start ()
	{

       // Screen.orientation = ScreenOrientation.Landscape;
	   // Screen.showCursor = true;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		guiDefaultColor = GUI.color;
		
		GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height),scrollViewVector,new Rect(0,0,Screen.width,Screen.height));
		
		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"FallingZombies - Register");
		if (GUI.Button (new Rect (0, 0, Screen.width/9, Screen.height/7), "Back")) {
			Application.LoadLevel("Login");		
		}

		GUI.Box (new Rect(Screen.width/4,Screen.height/4,Screen.width/2,Screen.height/2),"Register");
		
		GUI.Label (new Rect((float)(Screen.width/3.5),(float)(Screen.height/2.9),Screen.width,Screen.height),"Username :");
		if(controlsWithError.Find(control => control == "username") != null)
			GUI.color = Color.red;
		GUI.SetNextControlName("username");
		username = GUI.TextField (new Rect((float)(Screen.width/2.3),Screen.height/3,(float)(Screen.width/4),(float)(Screen.height/15)),username);
		GUI.color = guiDefaultColor;

		GUI.Label (new Rect((float)(Screen.width/3.5),(float)(Screen.height/2.3),Screen.width,Screen.height),"Password :");
		if(controlsWithError.Find(control => control == "password") != null || controlsWithError.Find(control => control == "re_password") != null)
			GUI.color = Color.red;
		GUI.SetNextControlName ("password");
		password = GUI.TextField (new Rect((float)(Screen.width/2.3),(float)(Screen.height/2.4),(float)(Screen.width/4),(float)(Screen.height/15)),password);
		GUI.color = guiDefaultColor;

		GUI.Label (new Rect((float)(Screen.width/3.6),(float)(Screen.height/1.9),Screen.width,Screen.height),"Re-type Password :");
		if(controlsWithError.Find(control => control == "re_password") != null || controlsWithError.Find(control => control == "password") != null)
			GUI.color = Color.red;
		GUI.SetNextControlName ("re-password");
		passwordTwo = GUI.TextField (new Rect((float)(Screen.width/2.3),(float)(Screen.height/1.98),(float)(Screen.width/4),(float)(Screen.height/15)),passwordTwo);
		GUI.color = guiDefaultColor;

		GUI.Label (new Rect((float)(Screen.width/3.6),(float)(Screen.height/1.66),Screen.width,Screen.height),"Email Address :");
		if(controlsWithError.Find(control => control == "email_address") != null)
			GUI.color = Color.red;
		GUI.SetNextControlName ("email_address");
		emailAddress = GUI.TextField (new Rect((float)(Screen.width/2.3),(float)(Screen.height/1.71),(float)(Screen.width/4),(float)(Screen.height/15)),emailAddress);
		GUI.color = guiDefaultColor;

		if (registrationSuccess == 1) {
			Debug.Log ("Registration success");
						++startTime;
            Debug.Log(startTime+" "+endTime);
						if (startTime < endTime) {
								GUI.Label (new Rect (Screen.width / 4, Screen.height / 4, Screen.width, Screen.height), "You have been successfully registered.");
						} else {
								Application.LoadLevel ("Login");
								startTime = 0;
						}
		
		} else if (registrationSuccess == -1) {
			Debug.Log("Registration error");
			++startTime;
			if (startTime < endTime) {
				GUI.Label (new Rect (Screen.width / 4, Screen.height / 4, Screen.width, Screen.height), "There was an error registering you.");
			} else {
				startTime = 0;
				registrationSuccess = 0; 
			}
		}

		GUI.SetNextControlName ("register_button");
		if (GUI.Button (new Rect((float)(Screen.width/4),(float)(Screen.height/1.46),(float)(Screen.width/2),(float)(Screen.height/15)),"Register")) {
		   // Get the entered values, and register the player
		    registrationSuccess = 0;
			errorMessages.Clear();
			controlsWithError.Clear();
			controlToFocus = "register_button";
			if(emailAddress.Length < 5 || emailAddress.Length > 50){
				controlToFocus = "email_address";
				controlsWithError.Add("email_address");
				errorMessages.Add("Invalid email address.");
				emailAddress = "";
			}

			if(password.Length < 5 || password.Length > 16){
				controlToFocus = "password";
				controlsWithError.Add("password");
				errorMessages.Add("Password needs to be greater than 4, and less than 16 characters.");
				password = "";
			}

			if(password != passwordTwo){
				controlsWithError.Add("re_password");
				errorMessages.Add("Passwords need to be the same.");
				password = "";
			}

			if(username.Length < 5 || username.Length > 11){
				controlToFocus = "username";
				controlsWithError.Add ("username");
				errorMessages.Add("Username needs to be greater than 4 and less than 11 characters.");
				username = "";
			}

           

			if(errorMessages.Count == 0)
			{
                StopCoroutine("RegisterUser");
			    StartCoroutine("RegisterUser");

			}

		}
		  
		if (username.Length == 0 && controlToFocus == "username")
			GUI.FocusControl ("username");
		else if(password.Length == 0 && controlToFocus == "password")
			GUI.FocusControl (controlToFocus);
		else if(passwordTwo.Length == 0 && controlToFocus == "re_password")
			GUI.FocusControl("re-password");
		else if(emailAddress.Length == 0 && controlToFocus == "email_address")
				GUI.FocusControl("email_address");

		if (errorMessages.Count > 0) {
			
						errorMessageText = "";
						errorMessages.ForEach (errorMessage => {
								errorMessageText += errorMessage + " \t ";
						});
			
						GUI.Box (new Rect (0, (float)(Screen.height / 1.15), Screen.width, (float)(Screen.height / 1.01)), "Error Messages");
						GUI.Label (new Rect (Screen.width - chatDisplayOffset, (float)(Screen.height / 1.11), Screen.width, Screen.height), errorMessageText);

						if (Screen.width - chatDisplayOffset > 0) {
								++chatDisplayOffset;
						} else {
								chatDisplayOffset = 1f;
						}

		}

		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}
		
		GUI.EndScrollView ();

	}

	IEnumerator RegisterUser(){
		
		var w = new WWW(registrationURL+"/"+username+"/"+password+"/"+emailAddress);
		yield return w;
		
		if(!string.IsNullOrEmpty(w.error)){
			Debug.Log("An error occured "+w.error);
		}else{

			if(w.text == "true"){
				registrationSuccess = 1;
			    Debug.Log("Registration success");
			}else if (w.text == "false")
			{
			    registrationSuccess = -1;
			    Debug.Log("Registration error!");
			}
			else
			{
                Debug.Log("No data returned");
			}

		}

	}

}
