using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision collision){
		
	//	Debug.Log ("The bullet hit "+collision.contacts[0].otherCollider+" at "+collision.contacts[0].point);
		//Destroy (gameObject);
		//Destroy (collision.contacts [0].otherCollider.gameObject);
		//Debug.Log ("The gameObject has been destroyed");
	
	}


}
