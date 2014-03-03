using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Linq;
using System.Threading;


public class ForgotPassword : MonoBehaviour {

	private Vector2 scrollViewVector = Vector2.zero;
	public GUISkin guiSkin;
	private string emailAddress = "";

	private Color guiDefaultColor;
	private string controlToFocus = "email_address";
	private List<string> controlsWithError = new List<string>();
	private List<string> errorMessages = new List<string>();
	private float chatDisplayOffset = 1f;
	private string errorMessageText = "";

	void Awake(){
		try{

			/*Client client = new Client("localhost:8080");
			client.RetryConnectionAttempts = 10;
			client.Opened += SocketOpened;
			client.Message += SocketMessage;
			client.SocketConnectionClosed += SocketClosed;
			client.Error += SocketError;

			client.Connect();
			Debug.Log("Socket initialization done "+client.IsConnected);*/
	 /*
			var connectionString = "mongodb://127.0.0.1:64016";
			var mongoClient = new MongoClient(connectionString);
			Debug.Log (mongoClient);
			var server = mongoClient.GetServer();
			Debug.Log (server);
			var database = server.GetDatabase("zombies");
			Debug.Log (database);

			var collection = database.GetCollection<Entity>("entities");
			Debug.Log(collection);

			var entity = new Entity{ Name = "Tom"};
			collection.Insert(entity);
			Debug.Log(entity);
			///var id = entity.Id;
			//Debug.Log ("Entity id "+id);

			//var query = Query<Entity>.EQ(e => e.Id,id);
			//var thatEntity = collection.FindOne(query);
			Debug.Log ("That entity "+thatEntity.Id+" "+thatEntity.Name);*/

		}catch(Exception e){

			Debug.Log(e);
		}
	
	}

    // Use this for initialization
	void Start () {

        //Screen.orientation = ScreenOrientation.Landscape;
        //Screen.showCursor = true;


	}


    // Update is called once per frame
	void Update () {
       
	}

	void OnGUI(){

		guiDefaultColor = GUI.color;
		
		GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height),scrollViewVector,new Rect(0,0,Screen.width,Screen.height));
		
		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"FallingZombies - Forgot Password");
		if (GUI.Button (new Rect (0, 0, Screen.width/9, Screen.height/7), "Back")) {
			Application.LoadLevel("Login");		
		}
		
		GUI.Box (new Rect(Screen.width/4,Screen.height/4,Screen.width/2,Screen.height/2),"Forgot Password");


		GUI.Label (new Rect((float)(Screen.width/3.5),(float)(Screen.height/2.3),Screen.width,Screen.height),"Email Address :");
		if(controlsWithError.Find(control => control == "email_address") != null)
			GUI.color = Color.red;
		GUI.SetNextControlName ("email_address");
		emailAddress = GUI.TextField (new Rect((float)(Screen.width/2.6),(float)(Screen.height/2.4),(float)(Screen.width/5),(float)(Screen.height/15)),emailAddress);
		GUI.color = guiDefaultColor;
		
		if (GUI.Button (new Rect((float)(Screen.width/4),(float)(Screen.height/1.46),(float)(Screen.width/2),(float)(Screen.height/15)),"Send")) {
			// Validate email address, and send it for password recovery
			errorMessages.Clear();
			controlsWithError.Clear();
			controlToFocus = "send_button";
			if(emailAddress.Length < 5 || emailAddress.Length > 16){
				controlToFocus = "email_address";
				controlsWithError.Add("email_address");
				errorMessages.Add("Invalid email address.");
				emailAddress = "";
			}

		}
		
		if(emailAddress.Length == 0 && controlToFocus == "email_address")
			GUI.FocusControl("email_address");

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

		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}
		
		GUI.EndScrollView ();
		
		
		
	}

}
