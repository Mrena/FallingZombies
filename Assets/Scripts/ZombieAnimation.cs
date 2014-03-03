using UnityEngine;
using System.Collections;

public class ZombieAnimation : MonoBehaviour {

	void Awake(){
		/*try{
			animation ["walk01"].layer = 1; animation ["walk01"].wrapMode = WrapMode.Loop;
		}catch(UnityException e){
			Debug.Log(e);
		}*/
		if (animation) {
						animation.wrapMode = WrapMode.Loop;
						animation.CrossFade ("walk");
				} else {
			Debug.Log("Animation is not defined");
				}
		
	}

}
