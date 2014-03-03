using UnityEngine;
using System.Collections;

public class BirdAnimator : MonoBehaviour {

	private float speed = 0.2f;
	private int offset = 5;
	private int current = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (current++ < 5) {
						transform.position = Vector3.forward * Time.deltaTime * speed;
			     
				} else if (current++ > 5)
						transform.position = Vector3.right * Time.deltaTime * speed;
				else if (current++ > 10) {
						transform.position = Vector3.right * Time.deltaTime * speed;
			current = 0;
				}

		Debug.Log ("Bird position updated");
	}
}
