using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {
	/*
	public GameObject bullet;
	private float forcePower = 2.0f;
	void ShotGun(){

		Rigidbody b = Instantiate (bullet) as Rigidbody;
		          b.gameObject.renderer.enabled = true;
		          b.AddForce (transform.forward * Time.deltaTime * forcePower);
	
	}*/

	void Update(){


	}

	void OnCollisionEnter(Collision collision){

		Debug.Log ("The bullet hit something at "+collision.contacts[0].point);
		Destroy (gameObject);
	}

}
