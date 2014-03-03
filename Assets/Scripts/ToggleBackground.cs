using UnityEngine;
using System.Collections;

public class ToggleBackground : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump")) {

			if(audio.isPlaying){
				audio.Pause();
			    GameState.backgroundMusic = false;
			}else{

				audio.Play();
			    GameState.backgroundMusic = true;
			}
	    }
	}
}
