using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameStats : MonoBehaviour {

	private bool calculation_done = false;
	private int screenWidth = Screen.width;

	private int down_time_hours = 0;
	private int down_time_min = 60;
	private int down_time_sec = 0;
	private string currentDownTime;

	private int up_time_hours = 0;
	private int up_time_min = 0;
	private int up_time_sec = 0;
	private string currentUpTime;
	public GUISkin guiSkin;
	public GUISkin lifeSkin;
    public string friendsListURL = URL.friendsListURL;

	private bool gotFriends = false;
	//public GUIStyle playerLifeStyle;
    ///public Texture2D playerLowLife;

    //public GUIStyle gameOverStyle; 

    private bool friendsCollapsed = true;
	

	            void OnGUI(){

						GUI.Box (new Rect ((float)(Screen.width/1.23), 0, Screen.width/5, (float)(Screen.height/1.8)), "Game Stats");
		                GUI.Label (new Rect ((float)(Screen.width/1.21), Screen.height/18, Screen.width/5, 20), "Time : " + GameState.count);
		                GUI.Label (new Rect ((float)(Screen.width/1.21), Screen.height/12, Screen.width/5, 20), "Score : " + GameState.currentScore);
		                GUI.Label (new Rect ((float)(Screen.width/1.21), Screen.height/9, Screen.width/5, 20), "Zombies : " + GameState.numberOfZombies);
		                GUI.Label (new Rect ((float)(Screen.width/1.21), (float)(Screen.height/7.3), Screen.width/5, 20), "Zombies Killed : " + GameState.numberOfKilledZombies);

						GUI.Box (new Rect (0, 0,Screen.width/5, Screen.height/12), "");
						GUI.Label (new Rect (Screen.width/205, Screen.height/34, Screen.width, Screen.height), "Life :");



	                if (friendsCollapsed)
	                {

                        // Display a non-collapsed friends menu
	                    GUI.Label(new Rect(0, (float) (Screen.height/10.9), Screen.width/5, Screen.height/12), "Friends :",guiSkin.box);
                        if (GUI.Button(new Rect(0, (float)(Screen.height / 10.9), Screen.width / 23, Screen.height / 18), "+"))
                        {
                            friendsCollapsed = false;
                        }

					


	                }
	                else
	                {
                        // Display a collapsed friends menu
	                    GUI.Label(new Rect(0, (float) (Screen.height/10.9), Screen.width/5, Screen.height), "Friends :",
	                        guiSkin.box);
                        if (GUI.Button(new Rect(0, (float)(Screen.height / 10.9), Screen.width / 23, Screen.height / 18), "-"))
                        {
                            friendsCollapsed = true;
                        }

                        // Populate the user's friends
						gotFriends = true;
						if(gotFriends){
								GUI.Label(new Rect(Screen.width/20, (float) (Screen.height/5), Screen.width/5, Screen.height/12), "Friends List Empty ");
							}			

	                }




	                if (GameState.playerLife == 50) {
							//	GameState.playerLife = GUI.HorizontalScrollbar (new Rect (95, 5, GameState.playerLife, 40), GameState.playerLife, 1.0f, 1.0f, 100.0f, playerLifeStyle);
			                     GUI.Label(new Rect (15, 15, 80, 100),"|||||||||||||||||||||||||||||||||||||||||||||||||||||||");
								
							//Debug.Log ("Player life : " + GameState.playerLife);		
						} else if (GameState.playerLife == 0) {
								
								GUI.Box (new Rect (Screen.width, Screen.height, Screen.width, Screen.height), "");
						//		GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, Screen.width / 3, Screen.height / 3), "GAME OVER", gameOverStyle);
			                    GUI.Label (new Rect (Screen.width, Screen.height / 2,Screen.width/2, Screen.height / 3), "GAME OVER");
								GameState.setGameState (0);
								//Debug.Log ("The player got killed.");
								Invoke ("GameController.LoadStartScreen", 4);

						} else {
								//GameState.playerLife = GUI.HorizontalScrollbar (new Rect (95, 5, GameState.playerLife, 40), GameState.playerLife, 1.0f, 1.0f, 100.0f, playerLifeStyle);
							//	Debug.Log ("Player life : " + GameState.playerLife);	
							GUI.Box(new Rect(Screen.width/20, Screen.height/34,(float)(Screen.width/7.5), Screen.height/25),"",lifeSkin.box);
						}
					
				 /*else {	

						GUI.Box (new Rect (Screen.width, Screen.height, Screen.width, Screen.height), "");
						GUI.Label(new Rect(Screen.width/2,Screen.height/2,Screen.width/3,Screen.width/3),"The level is complete!");
			            Application.LoadLevel(8);

				}*/

		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}

	}

	void Awake(){

	    if (GameState.backgroundMusic)
	    {
           AudioSource musicPlayer =   GameObject.FindGameObjectWithTag(Tags.musicPlayer).GetComponent<AudioSource>();
	                   musicPlayer.volume = GameState.backgroundVolume / 100;
                       musicPlayer.Play();
	    }

	    if (GameState.gameSound)
	    {
	        GameObject[] soundEmitters = GameObject.FindGameObjectsWithTag(Tags.soundEmitter);
	        for (var i = 0; i < soundEmitters.Length; i++)
	        {
	            AudioSource soundPlayer = soundEmitters[i].GetComponent<AudioSource>();
	                        soundPlayer.volume = GameState.soundVolume / 100;
                            soundPlayer.Play();
	        }
	    }

	    getUsersFriends();
		getUsersFriendsInvite ();


	}

	void OnApplicationQuit(){

		PlayerPrefs.Save ();
	
	}

	void Update(){

		GameState.count = getUpTime ();
	
	}

	string getUpTime(){

		if (GameState.gameOn == 1) {
						++up_time_sec;
						if (up_time_sec == 60) {
								up_time_sec = 0;
								++up_time_min;
								--GameState.playerLife;
								--GameState.currentScore;

								if (up_time_min == 60) {
										up_time_min = 0;
										++up_time_hours;
								}
						}

						currentUpTime = up_time_hours < 10 ? "0" + up_time_hours.ToString () : up_time_hours.ToString ();
						currentUpTime += ":";
						currentUpTime += up_time_min < 10 ? "0" + up_time_min.ToString () : up_time_min.ToString ();
						currentUpTime += ":";
						currentUpTime += up_time_sec < 10 ? "0" + up_time_sec.ToString () : up_time_sec.ToString ();
				}
		return currentUpTime;

	  }

	string getDownTime(){

		if(GameState.gameOn == 1){

				--down_time_sec;
				if (down_time_sec == 0 && down_time_min > 0) {
						down_time_sec = 60;
						--down_time_min;
						--GameState.playerLife;
						--GameState.currentScore;
						if (down_time_min == 0 && down_time_hours > 0) {
								down_time_min = 0;
								--down_time_hours;
						} else {
								// Time up!
								//GUI.Box (new Rect(Screen.width,Screen.height,Screen.width,Screen.height),"");
								//GUI.Label(new Rect(Screen.width/2,Screen.height/2,Screen.width/3,Screen.height/3),"GAME OVER",gameOverStyle);
								GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, Screen.width / 3, Screen.height / 3), "GAME OVER");
								GameState.setGameState (0);
								Debug.Log ("The game timed out");
								Invoke ("GameController.LoadStartScreen", 4);

						}
				} else {
						// Time up!
						//GUI.Box (new Rect(Screen.width,Screen.height,Screen.width,Screen.height),"");
						//GUI.Label(new Rect(Screen.width/2,Screen.height/2,Screen.width/3,Screen.height/3),"GAME OVER",gameOverStyle);
						GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, Screen.width / 3, Screen.height / 3), "GAME OVER");
						GameState.setGameState (0);
						Debug.Log ("The game timed out");
						Invoke ("GameController.LoadStartScreen", 4);

				}
		
				currentDownTime = down_time_hours < 10 ? "0" + down_time_hours.ToString () : down_time_hours.ToString ();
				currentDownTime += ":";
				currentDownTime += down_time_min < 10 ? "0" + down_time_min.ToString () : down_time_min.ToString ();
				currentDownTime += ":";
				currentDownTime += down_time_sec < 10 ? "0" + down_time_sec.ToString () : down_time_sec.ToString ();

		}
		
		return currentDownTime;

	}

	void Start(){

		//Screen.showCursor = false;
		//Screen.orientation = ScreenOrientation.Landscape;

	//	Application.LoadLevel (GameState.levels[0]);

	}

	void LoadNextLevel(){
		if (GameState.levels.Length - 1 != GameState.currentLevelIndex) {
						Application.LoadLevel (GameState.levels [++GameState.currentLevelIndex]);
				}else {
					
				//	GUI.Box (new Rect (Screen.width, Screen.height, Screen.width, Screen.height), "");
					GUI.Label(new Rect(Screen.width/2,Screen.height/2,Screen.width/3,Screen.width/3),"Congratulations. You have completed all the levels!");
					CheckForHighScore();

		        }
	}

	void CheckForHighScore(){

		if (GameState.currentScore > GameState.currentHighScore) {

						GUI.Box (new Rect (Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2), "");
						GUI.Label (new Rect ((Screen.width / 2) + 20, (Screen.height / 2) + 20, (Screen.width / 2), (Screen.height / 2) - 200), "Enter a high score name");
						string highScoreName = GUI.TextField (new Rect ((Screen.width / 2) + 30, (Screen.height / 2) + 20, (Screen.width / 2), (Screen.height / 2) - 200), "Enter a high score name");
						if (highScoreName != "") {
								PlayerPrefs.SetString ("high_score_name", highScoreName);
								PlayerPrefs.SetInt ("high_score", GameState.currentScore);
								Invoke ("GameController.LoadStartScreen", 4);
						}

				
	} else {
			int isFound =  Leaderboard.checkScoreAgainstLeaderboard(GameState.currentScore);        
			if(isFound != -1){

				GUI.Box (new Rect (Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2), "");
				GUI.Label (new Rect ((Screen.width / 2) + 20, (Screen.height / 2) + 20, (Screen.width / 2), (Screen.height / 2) - 200), "Congratulations! You made it to the leaderboard. You are number "+isFound);
				GUI.Label (new Rect ((Screen.width / 2) + 40, (Screen.height / 2) + 20, (Screen.width / 2), (Screen.height / 2) - 200), "Enter a high score name");
				string highScoreName = GUI.TextField (new Rect ((Screen.width / 2) + 60, (Screen.height / 2) + 20, (Screen.width / 2), (Screen.height / 2) - 200), "Enter a high score name");
				if (highScoreName != "") {
					PlayerPrefs.SetString ("high_score_name", highScoreName);
					PlayerPrefs.SetInt ("high_score", GameState.currentScore);

					Invoke ("GameController.LoadStartScreen", 4);
				}

			}     
	}
 }

    IEnumerator getUsersFriends()
    {

        var friendsList = new WWW(URL.friendsListURL + GameState.playerName);

        yield return friendsList;

        if (!string.IsNullOrEmpty(friendsList.error))
        {
            Debug.Log("Error sending login data " + friendsList.error);
        }
        else
        {
            Debug.Log(friendsList.text);
            if (friendsList.text != "")
            {
             // Populate friends list
                Debug.Log(friendsList.text);
                /*if (GUI.Button(new Rect(0, (float)(Screen.height / 10.9), Screen.width / 23, Screen.height / 18), "-"))
                {
                    friendsCollapsed = false;
                }*/
				gotFriends = true;
            }
            else
            {
				gotFriends = false;
                Debug.Log("Frends list is empty");
            }
        }

    }

    IEnumerator getUsersFriendsInvite()
    {

		var friendsInviteList = new WWW(URL.friendsInviteListURL + GameState.playerName);

        yield return friendsInviteList;

        if (!string.IsNullOrEmpty(friendsInviteList.error))
        {
            Debug.Log("Error sending login data " + friendsInviteList.error);
        }
        else
        {
            Debug.Log(friendsInviteList.text);
            if (friendsInviteList.text != "")
            {
                // Populate friends list
                Debug.Log(""+friendsInviteList.text);
                /*if (GUI.Button(new Rect(0, (float)(Screen.height / 10.9), Screen.width / 23, Screen.height / 18), "-"))
                {
                    friendsCollapsed = false;
                }*/
				gotFriends = true;

            }
            else
            {
				gotFriends = false;
                Debug.Log("Frends list is empty");
            }
        }

    }

}
