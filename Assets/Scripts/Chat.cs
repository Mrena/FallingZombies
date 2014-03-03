using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Chat : MonoBehaviour {

	private string currentTypedMessage = "";
	private List<ChatMessage> chatMessages = new List<ChatMessage>(){new ChatMessage{Sender = "Mrena",Message = "This is the first message"},new ChatMessage{ Sender = "Khule",Message = "It's alive!"}};
	private float chatDisplayOffset = 1f;
	public GUISkin guiSkin;
	private Rect profileWindowRect = new Rect(0,Screen.height/11,(float)(Screen.width/5), (float)(Screen.height/1.4));
	private bool drawWindow = false;
	private string profileName = "";
	private int windowId = 0;
	private List<ProfileInView> profiles;
	private float defaultWindowWidth = (float)(Screen.width/5);
	private float defaultWindowHeight = (float)(Screen.height/1.4);
	private float previousY;
	private float DefaultDockY = (float)(Screen.height/1.3);
	private string PrivateChatText = "";
	private bool CharactersOverLimit = false;
	private float CharatersOverLimitTime;

	private Vector2 scrollViewVector = Vector2.zero;
    private List<ProfileInfo> profilesInfo;
	private List<int> populatedProfileInfo;


	// Use this for initialization
	void Start () {

		profiles = new List<ProfileInView> ();
		InvokeRepeating ("ChatMessageSlider",0.02f,0.02f);
		InvokeRepeating ("ShowMouseOnOverProfile",0.02f,0.02f);

	}
	
	// Update is called once per frame
	void Update () {
	  
	}

	void FixedUpdate(){

	}

	void profileWindow(int windowID){

		var objProfile = profiles.Find(profile => profile.Id == windowID);
		GUI.Label (new Rect(5,Screen.height/25,Screen.width/4,Screen.height/4),"Profile");
		if (GUI.Button (new Rect ((float)(Screen.width/5.8), Screen.height / 29, Screen.width / 40, Screen.height / 30), "x")) {

			profiles.Remove(profiles.Find(profile => profile.Id == windowID));
		
		}
		if (GUI.Button (new Rect ((float)(Screen.width/6.8), Screen.height / 29, Screen.width / 40, Screen.height / 30), "-")) {

			if(objProfile.Width == defaultWindowWidth && objProfile.Height == defaultWindowHeight){

				objProfile.Width = (float)(Screen.width/5);
				objProfile.Height = (float)(Screen.height/10);
				previousY = objProfile.Y;
				objProfile.Y = DefaultDockY;

			}else{
				   objProfile.Width = defaultWindowWidth;
				   objProfile.Height = defaultWindowHeight;
				   objProfile.Y = previousY;
			}
			
		}


	    var currentProfile = profilesInfo.FirstOrDefault(x => x.ProfileID == windowID);

	    if (currentProfile != null)
	    {
	        GUI.Box(new Rect(5, Screen.height/14, Screen.width/12, Screen.height/6), "Picture");
	        GUI.Label(new Rect(5, (float) (Screen.height/4), Screen.width/12, Screen.height/6),
	            "Name : " + currentProfile.FirstName + " " + currentProfile.LastName);
	        GUI.Label(new Rect(5, (float) (Screen.height/3.6), Screen.width/12, Screen.height/6),
	            "DOB : " + currentProfile.Birthday);
	        GUI.Label(new Rect(5, (float) (Screen.height/3.3), Screen.width/12, Screen.height/6),
	            "City : " + currentProfile.City);
	        GUI.Label(new Rect(5, (float) (Screen.height/3.1), Screen.width/12, Screen.height/6),
	            "Country : " + currentProfile.Country);

	    }
	    else
	    {

			GUI.Box(new Rect(5, Screen.height/14, Screen.width/12, Screen.height/6), "Picture");
			GUI.Label(new Rect(5, (float) (Screen.height/4), Screen.width/12, Screen.height/6),"Name : ");
			GUI.Label(new Rect(5, (float) (Screen.height/3.6), Screen.width/12, Screen.height/6),"DOB : ");
			GUI.Label(new Rect(5, (float) (Screen.height/3.3), Screen.width/12, Screen.height/6),"City : ");
			GUI.Label(new Rect(5, (float) (Screen.height/3.1), Screen.width/12, Screen.height/6),"Country : ");

	    }
		
	    GUI.SetNextControlName ("private_chat_text_screen");
		if (objProfile.PrivateChatText.Count == 0) {

						scrollViewVector =	GUI.BeginScrollView(new Rect(5, (float)(Screen.height / 2.7), (float)(Screen.width / 5.1), Screen.height / 7),scrollViewVector,new Rect(5, (float)(Screen.height / 2.7), (float)(Screen.width / 3.1), Screen.height));
						GUI.TextArea (new Rect (5, (float)(Screen.height / 2.7), (float)(Screen.width / 5.1), Screen.height * 250), "");
						GUI.EndScrollView();

				}else {
				    string chat = "";
					objProfile.PrivateChatText.ForEach(chatText => {
							chat += chatText;
						});

				    scrollViewVector =	GUI.BeginScrollView(new Rect(5, (float)(Screen.height / 2.7), (float)(Screen.width / 5.1), Screen.height / 7),scrollViewVector,new Rect(5, (float)(Screen.height / 2.7), (float)(Screen.width / 3.1), Screen.height));
					GUI.TextArea (new Rect (5, (float)(Screen.height / 2.7), (float)(Screen.width / 5.1), Screen.height * 250), chat);
					GUI.EndScrollView();
			        

				}

		GUI.SetNextControlName ("private_chat_text_area");
		objProfile.PrivateTypedText = GUI.TextArea (new Rect(6,(float)(Screen.height/1.9),(float)(Screen.width/6.1),Screen.height/12),objProfile.PrivateTypedText);

		GUI.SetNextControlName ("private_chat_send");
		if (GUI.Button (new Rect (6, (float)(Screen.height / 1.6), (float)(Screen.width / 9), Screen.height / 19), "Send")) {
			if(objProfile.PrivateTypedText.Length < 141 && objProfile.PrivateTypedText.Length > 0){
				objProfile.PrivateChatText.Add(objProfile.PlayerName+" : "+objProfile.PrivateTypedText+"\n");
				objProfile.PrivateTypedText = "";
			}else{
				CharactersOverLimit = true;
				CharatersOverLimitTime = 0;
			}
		}

		if (GUI.GetNameOfFocusedControl () == "private_chat_text_screen") {
			GUI.FocusControl("private_chat_text_area");
		}

		GUI.DragWindow(new Rect(0, 0, 10000, 10000));
	}

	void OnGUI(){

		if (GameState.gameOn == 1) {
						if (GameState.multiplayerGame) {

								if(drawWindow){

										UpdateProfileInViewViewport ();
										profiles.ForEach(profile => {
													profile.ProfileWindowRect = GUI.Window (profile.Id, profile.ProfileWindowRect, profileWindow, profile.PlayerName);
													if(populatedProfileInfo.FirstOrDefault(x => x == profile.Id) == null){
														getUserProfileInfo(profile.Id,profile.PlayerName);
														populatedProfileInfo.Add(profile.Id);
														Debug.Log("Kay");
													}
											});

										}

								if (GUI.Button (new Rect (0, (float)(Screen.height / 1.15), Screen.width, (float)(Screen.height / 13.1)), "Chat Messages", guiSkin.box)) {
					                    drawWindow = true;
					                    GUI.Box (new Rect(Screen.width/2,0,Screen.width/2,Screen.height/2),"Profile"); 
										if(!ProfileInView(chatMessages[0].Sender))
											profiles.Add(new ProfileInView(){Id = profiles.Count,PlayerName = chatMessages[0].Sender});
								}
	
								if (chatMessages.Count > 0) {
										if (GUI.Button (new Rect (Screen.width - chatDisplayOffset, (float)(Screen.height / 1.11), Screen.width, Screen.height), chatMessages [0].Message + " - " + chatMessages [0].Sender, guiSkin.label)) {
												GUI.Box (new Rect(Screen.width/2,0,Screen.width/2,Screen.height/2),"Profile"); 
												drawWindow = true;	
						                      if(!ProfileInView(chatMessages[0].Sender))
						                           profiles.Add(new ProfileInView(){Id = profiles.Count,PlayerName = chatMessages[0].Sender});
					}
				}
				
				currentTypedMessage = GUI.TextArea (new Rect ((float)(Screen.width / 1.22), (float)(Screen.height / 5.2), (float)(Screen.width / 5.69), Screen.height / 5), currentTypedMessage);
			
						if(CharactersOverLimit && CharatersOverLimitTime < 555){
									
									GUI.Label (new Rect ((float)(Screen.width / 2.5), (float)(Screen.height / 2.5), Screen.width/2, Screen.height/2), "Chat text needs to be less than or equal to 140.");
									++CharatersOverLimitTime;
				}else{
					CharactersOverLimit = false;
					CharatersOverLimitTime = 0;
				}


								if (GUI.Button (new Rect ((float)(Screen.width / 1.22), (float)(Screen.height / 2.5), Screen.width / 11, 50), "Send Chat")) {
										if (currentTypedMessage.Length < 140 && currentTypedMessage.Length > 0) {
												chatMessages.Add (new ChatMessage (){Sender = GameState.playerName,Message=currentTypedMessage.Replace("\n"," ").Replace("\t"," ")});
												currentTypedMessage = "";
												GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, Screen.width, Screen.height), "Message sent.");
										} else {
												CharactersOverLimit = true;
												CharatersOverLimitTime = 0;
												
										}
								}
			
						}
				}
	}

    void Awake()
    {

       profilesInfo = new List<ProfileInfo>();
	   populatedProfileInfo = new List<int> ();

    }

    void ChatMessageSlider(){
		if(chatMessages.Count > 0){
		  if(Screen.width - chatDisplayOffset > 0){
				++chatDisplayOffset;
			}else{
				chatDisplayOffset = 1f;
				chatMessages.RemoveAt(0);
				chatMessages.Add(new ChatMessage(){ Sender = "Me",Message ="FTW!"});
			}
		}
	}

	bool ProfileInView(string playerName){
		bool isInView = false;

		if (profiles.FirstOrDefault (profile => profile.PlayerName == playerName) != null)
						isInView = true;

		return isInView;
	}

	void ShowMouseOnOverProfile(){

	
	}

	void UpdateProfileInViewViewport(){

		profiles.ForEach (profile => {
			profile.Width = (float)(Screen.width/5);
			profile.Height = (float)(Screen.height/1.4);
		});

	}

	IEnumerator getUserProfileInfo(int profileId, string username){

		WWW w = new WWW (URL.userProfileURL+username);
		Debug.Log (w.url);

		 yield return w;

		if(!string.IsNullOrEmpty(w.error) || w.text == "false"){
			Debug.Log("There was an error fetching profile info.");
		}else{
			Debug.Log(w.text);

		}

	}
}
