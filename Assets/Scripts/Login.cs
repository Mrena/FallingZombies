using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Login : MonoBehaviour {

	private Vector2 scrollViewVector = Vector2.zero;
	public GUISkin guiSkin;
	private string username = "";
	private string password = "";

	private Color guiDefaultColor;
	private string controlToFocus = "username";
	private List<string> controlsWithError = new List<string>();
	private List<string> errorMessages = new List<string>();
	private float chatDisplayOffset = 1f;
	private string errorMessageText = "";

    private string loginURL = URL.loginURL;
    private int loginSuccess = 0;
    private int startTime = 0;
    private int endTime = 300;
    private bool autoLogin = false;
    private bool rememberLogin = false;

	void Awake()
	{

	    if (PlayerPrefs.GetInt("autoLogin") == 1)
	    {
	        autoLogin = true;
	        username = PlayerPrefs.GetString("username");
	        password = PlayerPrefs.GetString("password");
            StopCoroutine("LoginUser");
            StartCoroutine("LoginUser");
	    }
	    else
	    {
	        autoLogin = false;
	    }

	    if (PlayerPrefs.GetInt("rememberLogin") == 1)
	    {
	        rememberLogin = true;
            username = PlayerPrefs.GetString("username");
            password = PlayerPrefs.GetString("password");
	    }
	    else
	    {
	        rememberLogin = false;
	    }

	}

	// Use this for initialization
	void Start () {

       // Screen.orientation = ScreenOrientation.Landscape;
       // Screen.showCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		guiDefaultColor = GUI.color;

	    scrollViewVector = GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height),scrollViewVector,new Rect(0,0,Screen.width,Screen.height));
		
		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"FallingZombies - Login");
		if (GUI.Button (new Rect (0, 0, Screen.width/9, Screen.height/7), "Back")) {
			Application.LoadLevel("SubMenu");				
		}

		GUI.Box (new Rect(Screen.width/4,Screen.height/4,Screen.width/2,Screen.height/2),"Login");

		GUI.Label (new Rect((float)(Screen.width/3.5),(float)(Screen.height/2.9),Screen.width,Screen.height),"Username :");

		if(controlsWithError.Find(control => control == "username") != null)
		    GUI.color = Color.red;
		GUI.SetNextControlName ("username");
		username = GUI.TextField (new Rect((float)(Screen.width/2.6),Screen.height/3,(float)(Screen.width/5),(float)(Screen.height/15)),username);
		GUI.color = guiDefaultColor;


		GUI.SetNextControlName ("password");
		GUI.Label (new Rect((float)(Screen.width/3.5),(float)(Screen.height/2.3),Screen.width,Screen.height),"Password :");
		if(controlsWithError.Find(control => control == "password") != null)
			GUI.color = Color.red;

		password = GUI.TextField (new Rect((float)(Screen.width/2.6),(float)(Screen.height/2.4),(float)(Screen.width/5),(float)(Screen.height/15)),password);
		GUI.color = guiDefaultColor;

        if (loginSuccess == -1)
        {
            ++startTime;
            if (startTime < endTime)
            {
                GUI.Label(new Rect(Screen.width / 4, Screen.height / 4, Screen.width, Screen.height), "Username or password is incorrect.");
            }
            else
            {
                startTime = 0;
                loginSuccess = 0;
            }
        }

		GUI.SetNextControlName ("login_button");
		if (GUI.Button (new Rect((float)(Screen.width/2.6),(float)(Screen.height/1.84),(float)(Screen.width/10),(float)(Screen.height/11)),"Login")) {
					// Get the entered values, and login the player
			bool isError = false;
			errorMessages.Clear();
			controlsWithError.Clear();
			controlToFocus = "login_button";
			if(password.Length < 5 || password.Length > 16){
				controlToFocus = "password";
				controlsWithError.Add("password");
				errorMessages.Add("Password needs to be greater than 4, and less than 16 characters.");
				password = "";
				isError = true;
			}

			if(username.Length < 5 || username.Length > 11){
				controlToFocus = "username";
				controlsWithError.Add ("username");
				errorMessages.Add("Username needs to be greater than 4 and less than 11 characters.");
				username = "";
				isError = true;
			}

			if(!isError){

			    StopCoroutine("LoginUser");
				StartCoroutine("LoginUser");
			}

		}

		if (username.Length == 0 && controlToFocus == "username")
			GUI.FocusControl ("username");
		else if(password.Length == 0 && controlToFocus == "password")
			GUI.FocusControl (controlToFocus);

		if (errorMessages.Count > 0) {

			errorMessageText = "";
			errorMessages.ForEach(errorMessage => {
				errorMessageText += errorMessage +" \t ";
			});

			GUI.Box (new Rect (0, (float)(Screen.height/1.15), Screen.width, (float)(Screen.height/1.01)), "Error Messages");
			GUI.Label(new Rect(Screen.width-chatDisplayOffset,(float)(Screen.height/1.11), Screen.width, Screen.height),errorMessageText);


			if(Screen.width - chatDisplayOffset > 0){
				++chatDisplayOffset;
			}else{
				chatDisplayOffset = 1f;
			}

		}

        autoLogin = GUI.Toggle(new Rect((float)(Screen.width / 2.59), (float)(Screen.height / 2.05), (float)(Screen.width / 10), (float)(Screen.height / 11)),autoLogin,"AutoLogin");

        rememberLogin = GUI.Toggle(new Rect((float)(Screen.width / 2.06), (float)(Screen.height / 2.05), (float)(Screen.width / 10), (float)(Screen.height / 11)), rememberLogin, "Remember Login");

		if (GUI.Button (new Rect((float)(Screen.width/2.06),(float)(Screen.height/1.85),(float)(Screen.width/10),(float)(Screen.height/11)),"Forgot password?")) {
			Application.LoadLevel("ForgotPassword");
		}

		if (GUI.Button (new Rect((float)(Screen.width/4),(float)(Screen.height/1.46),(float)(Screen.width/2),(float)(Screen.height/15)),"Register")) {
			Application.LoadLevel("Register");
		}
		
		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}

		GUI.EndScrollView ();

	}

	IEnumerator LoginUser(){

		var login = new WWW(loginURL+"/"+username+"/"+password);

		yield return login;
		
		if(!string.IsNullOrEmpty(login.error)){
			Debug.Log("Error sending login data "+login.error);
		}else{
			Debug.Log(login.text);
		    if (login.text == "true")
		    {
		        // Login the user to the multi player environment

                Debug.Log("Login success!");
		        GameState.playerName = username;
		        GameState.multiplayerGame = true;
                Application.LoadLevel("summer");

                /*
		        while (Application.isLoadingLevel)
		        {
		            Debug.Log("The level is still loading...");
		        }
               
				if(true){
					Debug.Log("Just testing something...");
				}
                */

		        if (autoLogin)
		        {
		            PlayerPrefs.SetString("username", username);
		            PlayerPrefs.SetString("password", password);
		            PlayerPrefs.SetInt("autoLogin", 1);
		        }
		        else
		        {
                    PlayerPrefs.SetInt("autoLogin",0);
		        }

		        if (rememberLogin)
		        {
		            PlayerPrefs.SetString("username", username);
		            PlayerPrefs.SetString("password", password);
		            PlayerPrefs.SetInt("rememberLogin", 1);
		        }
		        else
		        {
                    PlayerPrefs.SetInt("rememberLogin",0);
		        }

		    }
		    else
		    {
		        loginSuccess = -1;
		    }
		}

	}
}
