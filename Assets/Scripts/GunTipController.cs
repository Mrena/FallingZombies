using UnityEngine;
using System.Collections;

public class GunTipController : MonoBehaviour {

	public Rigidbody bullet;
	float speed = 100f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (GameState.gameOn == 1) {
			if (Input.GetButtonDown ("Fire1")) {
				//Rigidbody gunBullet = Instantiate(bullet,transform.position,transform.rotation) as Rigidbody;
					//gunBullet.AddForce(-transform.forward * Time.deltaTime * speed,ForceMode.Impulse);
					if(GameObject.Find("ak47") != null && GameState.gameSound)
						GameObject.Find("ak47").audio.Play();
					else
						Debug.Log("ak47 could not be found");

					//Debug.Log ("Firing the gun from the gun tip");

				}
			}
	
	}
}
