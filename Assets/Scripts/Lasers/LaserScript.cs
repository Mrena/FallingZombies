using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserScript : MonoBehaviour {

	public LineRenderer line;
	public List<ZombieController> zombies;
	public ZombieController zombie;
	private GameObject current_zombie;
	public GUIStyle gameOverStyle;

	// Use this for initialization
	void Start () {

		if (GameState.gameState == GameState.gameOn) {

					line = gameObject.GetComponent<LineRenderer> ();
					line.enabled = false;
			        //Screen.showCursor = false;
			}

	}
	
	// Update is called once per frame
	void Update () {
	
		/*
		if (GameState.gameState == GameState.gameOn) {
						if (GameState.gameState == GameState.gameOn) {
								if (Input.GetButtonDown ("Fire1")) {

										StopCoroutine ("FireLaser");
										StartCoroutine ("FireLaser");
								}
			}else{
			  //	GUI.Label(new Rect(Screen.width/2,Screen.height/2,Screen.width/3,Screen.height/3),"GAME OVER",gameOverStyle);
			}
	    }*/
	}

	IEnumerator FireLaser(){

        if(GameState.gameSound)
		        audio.Play ();


		line.enabled = true;
		while (Input.GetButton("Fire1")) {
		
			Ray ray = new Ray(transform.position,transform.forward);
			RaycastHit hit;

			line.SetPosition (0,ray.origin);

			if(Physics.Raycast(ray, out hit, 100)){
				line.SetPosition(1,hit.point);
				if(hit.rigidbody && hit.rigidbody.gameObject.name == "zombie_hires"){
					hit.rigidbody.AddForceAtPosition(transform.forward * 500,hit.point);
				    zombie = hit.rigidbody.gameObject.GetComponent<ZombieController>();
				
					if(zombie.hash_code == 0){
						zombie.hash_code = hit.rigidbody.GetHashCode();
						++zombie.number_of_hit;
					}else{
						++zombie.number_of_hit;
						if(zombie.number_of_hit == 10){
                            if(GameState.gameSound)
							   hit.rigidbody.gameObject.audio.Play ();
							current_zombie = hit.rigidbody.gameObject;
							Invoke("DestroyZombie",1.5f);
							GameState.playerLife+=5;
						}
					}

				}
			}else
			   line.SetPosition(1,ray.GetPoint(100));

			yield return null;
		}

		line.enabled = false;
	}

	void DestroyZombie(){

		Destroy (current_zombie);
		++GameState.numberOfKilledZombies;
		--GameState.numberOfZombies;
		if (GameState.numberOfZombies == 0) {
				// Level complete, load new level
		}

	}
}
