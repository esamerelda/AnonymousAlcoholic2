using UnityEngine;
using System.Collections;

public class canMover : MonoBehaviour {

	
	private GameObject camera;
	ExternalAudio extAudio;

	// Use this for initialization
	void Awake ()
	{
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		extAudio = camera.GetComponent<ExternalAudio>();
	}

	void Start () 
	{
		Destroy(gameObject, 4);  //destroys can after x seconds if nothing else does first
	}

	void OnTriggerEnter2D(Collider2D gameObj)
	{
		if(gameObj.gameObject.tag == "ground")
		{
			extAudio.PlayCanHitsGround();
			Destroy(gameObject);
		}
		if(gameObj.gameObject.tag == "canDeath")
		{
			extAudio.PlayCanHitsEnemy();
			Destroy (gameObj.gameObject);
			Destroy(gameObject);	//destroy this can
		}
		if(gameObj.gameObject.tag == "enemy")
		{
			extAudio.PlayCanHitsEnemy();
			Destroy (gameObj.gameObject);
			Destroy(gameObject);	//destroy this can
		}

		//SUCCESSFUL SCIENCE OF DESTRUCTION
		/*if(gameObj.gameObject.tag == "Player")
		{
			//destroy enemy
			//Destroy(gameObj);				//this just destroys the player's rigidbody
			//Destroy(gameObj.gameObject);	//this destroys the entire player
		}*/
	}
}
