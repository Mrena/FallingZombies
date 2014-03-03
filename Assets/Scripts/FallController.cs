using UnityEngine;
using System.Collections;

public class FallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(){

		GameState.gameOn = 0;


		Debug.Log ("The player went through the plane");


	}

	void OnGUI(){

		if (GameState.gameOn == 0) {
				
			GUI.Label(new Rect(Screen.width/2,Screen.height/2,Screen.width/3,Screen.height/3),"GAME OVER");
		
		}

	}

}
