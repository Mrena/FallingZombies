using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieController : MonoBehaviour {

	public Transform target;
	public Rigidbody body;
	public int number_of_hit;
	public AudioSource zombie_die;
	public int hash_code;
	public int number_of_zombies = 5;
	private Vector3 body_position;
	private Rigidbody zombie;
	private float x;
	private float z;
	
	// Use this for initialization
	void Start () {

		/*
		if (GameState.gameState == GameState.gameOn) {
						body_position = new Vector3 (5 * Time.deltaTime, 0, 5 * Time.deltaTime);
						hash_code = 0;
						number_of_hit = 0;

						if (animation) {
								animation.wrapMode = WrapMode.Loop;
				              
								animation.CrossFade ("alpa@zombie_running",1f,PlayMode.StopSameLayer);
				                Debug.Log(animation.GetType());
						} else {
								Debug.Log ("Animation is not defined");
						}


						//if (number_of_zombies > 0) {
						//	Invoke ("CloneZombie", 2);
						//	CloneZombie ();
						//	--number_of_zombies;
						//} else {
						//Debug.Log("All five zombies have been cloned.");
						//	}

				}*/
	Rigidbody zombie =	Instantiate (this) as Rigidbody;
		zombie.AddForce (new Vector3 (4,4,6));
		Debug.Log ("zombie clone");
	}
	
	// Update is called once per frame
	void Update () {

		if (GameState.gameState == GameState.gameOn) {
				transform.LookAt (target);

				if (animation) {
						animation.wrapMode = WrapMode.Loop;
				animation.CrossFade ("alpha@zombie_running");
				} else {
					Debug.Log ("Animation is not defined");
			}
		}
	
	}

	void CloneZombie(){

	
		x = Random.Range (-2.0f, -2.0f);
		z = Random.Range (-2.0f,-2.0f);

		if (number_of_zombies > 0) {
				zombie = Instantiate (body,new Vector3(x,2,z),Quaternion.identity) as Rigidbody;
			//	zombie.AddForce (body_position, ForceMode.Impulse);
			--number_of_zombies;
		} else {

			CancelInvoke("CloneZombie");

				}

	}
	
}
