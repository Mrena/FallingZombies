using UnityEngine;
using System.Collections;

public class SettingsController {


	public void UpdateGameSettings(){


	/*
		GameObject musicPlayer = GameObject.FindWithTag ("MusicPlayer");
		if (!GameState.backgroundMusic) {
						musicPlayer.audio.Pause ();
				}
		//musicPlayer.audio.volume = GameState.backgroundVolume;
	/*	
		GameObject[] soundEmitters = GameObject.FindGameObjectsWithTag("SoundEmitter");
		if (soundEmitters != null) {
			for(int i = 0; i<soundEmitters.Length;i++){
				if(!GameState.gameSound)
					soundEmitters[i].audio.Pause();
				//soundEmitters[i].audio.volume = GameState.soundVolume;
			}	
		}*/
	}
}
