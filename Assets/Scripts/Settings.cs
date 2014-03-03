using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	private Vector3 scrollViewVector = Vector3.zero;

	private int backgroundMusicIndex = 1;
	private float backgroundMusicVolume = 100f;

	private int gameSoundIndex = 1;
	private float gameSoundVolume = 100f;

	private int forceFeedIndex = 1;

	public GUISkin selectionGridStyle;

	SettingsController settings;
	public GUISkin guiSkin;

    private void Awake()
    {
        if(PlayerPrefs.GetString("background_music") != "" || PlayerPrefs.GetString("background_music") != null)
         backgroundMusicIndex = PlayerPrefs.GetString("background_music") == "on" ? 1 : 0;
         backgroundMusicVolume = PlayerPrefs.GetFloat("background_volume");


        if (PlayerPrefs.GetString("game_sound") != "" || PlayerPrefs.GetString("game_sound") != null)
            gameSoundIndex = PlayerPrefs.GetString("game_sound") == "on" ? 1 : 0;
            gameSoundVolume = PlayerPrefs.GetFloat("game_sound_volume");


        if (PlayerPrefs.GetString("force_feedback") != "" || PlayerPrefs.GetString("force_feedback") != null)
            forceFeedIndex = PlayerPrefs.GetString("force_feedback") == "on" ? 1 : 0;

    }

    void OnGUI(){

		GUI.BeginScrollView (new Rect(0,0,Screen.width,Screen.height),scrollViewVector,new Rect(0,0,Screen.width,Screen.height));

		GUI.Box (new Rect(0,0,Screen.width,Screen.height),"Settings");
		if (GUI.Button (new Rect (0, 0, Screen.width/9, Screen.height/7), "Back")) {

			if(GameState.backgroundMusic)
				PlayerPrefs.SetString("background_music","on");
			  else
				PlayerPrefs.SetString("background_music","off");

			PlayerPrefs.SetFloat("background_volume",GameState.backgroundVolume);

			if(GameState.gameSound)
				PlayerPrefs.SetString("game_sound","on");
			else
				PlayerPrefs.SetString("game_sound","off");

			PlayerPrefs.SetFloat("game_sound_volume",GameState.soundVolume);

		    if (GameState.forceFeedback)
		    {
		        PlayerPrefs.SetString("force_feedback", "on");
		    }
		    else
		    {
                PlayerPrefs.SetString("force_feedback","off");
		    }

		    settings.UpdateGameSettings();

			Application.LoadLevel("Menu");		
		}

		GUI.Label (new Rect((float)(Screen.width/11),(float)(Screen.height/2.9),Screen.width/3,Screen.height/3),"Background Music : ");
		string[] backgroundMusicButtons = {"On","Off"};
		backgroundMusicIndex = GUI.SelectionGrid (new Rect((Screen.width/4),(float)(Screen.height/3.2),Screen.width/6,Screen.height/12),backgroundMusicIndex,backgroundMusicButtons,2);
		if (GUI.changed) {
						if (backgroundMusicIndex == 1) {
								GameState.backgroundMusic = true;
						} else {
								GameState.backgroundMusic = false;
						}
				}

		GUI.Label (new Rect((Screen.width/11),(float)(Screen.height/2.3),Screen.width/3,Screen.height/3),"Volume : ");
		backgroundMusicVolume = GUI.HorizontalSlider (new Rect((Screen.width/4),(float)(Screen.height/2.3),Screen.width/3,Screen.height/20),backgroundMusicVolume,0f,100f);
		if(GUI.changed)
		   GameState.backgroundVolume = backgroundMusicVolume;

		GUI.Label (new Rect((Screen.width/11),(float)(Screen.height/1.97),Screen.width/3,Screen.height/3),"Game Sound : ");
		string[] gameSoundButtons = {"On","Off"};
		gameSoundIndex = GUI.SelectionGrid (new Rect((Screen.width/4),(float)(Screen.height/2.1),Screen.width/6,Screen.height/12),gameSoundIndex,gameSoundButtons,2);
		if (GUI.changed) {
						if (gameSoundIndex == 1) {
								GameState.gameSound = true;
						} else {
								GameState.gameSound = false;
						}
				}

		GUI.Label (new Rect((Screen.width/11),(float)(Screen.height/1.68),Screen.width/3,Screen.height/3),"Volume : ");
		gameSoundVolume = GUI.HorizontalSlider (new Rect((Screen.width/4),(float)(Screen.height/1.65),Screen.width/3,Screen.height/20),gameSoundVolume,0f,100f);
		GameState.soundVolume = gameSoundVolume;

		GUI.Label (new Rect((Screen.width/11),(float)(Screen.height/1.45),Screen.width/3,Screen.height/3),"Force Feedback : ");
		string[] forceFeedButtons = {"On","Off"};
		forceFeedIndex = GUI.SelectionGrid (new Rect((Screen.width/4),(float)(Screen.height/1.51),Screen.width/6,Screen.height/12),forceFeedIndex,forceFeedButtons,2);
		if (GUI.changed) {
						if (forceFeedIndex == 1) {
								GameState.forceFeedback = true;
						} else {
								GameState.forceFeedback = false;
						}
				}

		if (GUI.Button (new Rect (0,(float)(Screen.height/1.05), Screen.width, (float)(Screen.height/7.19)), "2014 - Mrena Systems",guiSkin.box)) { 
			Application.OpenURL("http://www.mrena.co.za");
		}

		GUI.EndScrollView ();

	}

	void Start(){


		//Screen.orientation = ScreenOrientation.Landscape;

        settings = new SettingsController ();

		string music_state = PlayerPrefs.GetString("background_music");
		if (music_state == "on")
			 GameState.backgroundMusic = true;
				else
				  GameState.backgroundMusic = false;

		GameState.backgroundVolume = PlayerPrefs.GetFloat("background_volume");

		string sound_state = PlayerPrefs.GetString ("game_sound");
		if (sound_state == "on")
			  GameState.gameSound = true;
				else
				  GameState.gameSound = false;

		GameState.soundVolume =	PlayerPrefs.GetFloat("game_sound_volume");
		settings.UpdateGameSettings ();

	   // Screen.showCursor = true;

	}

  
}
