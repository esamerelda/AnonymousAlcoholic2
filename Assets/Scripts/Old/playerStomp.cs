using UnityEngine;
using System.Collections;

public class playerStomp : MonoBehaviour {

	// Use this for initialization
	private GameObject camera;
	ExternalAudio extAudio;

	void Awake () {

		camera = GameObject.FindGameObjectWithTag("MainCamera");		//ref to camera
		extAudio = camera.GetComponent<ExternalAudio>();				//ref to camera's ExternalAudio Script
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D gameObj) //collision detection
	{
		if (gameObj.gameObject.tag == "enemy") 
		{
			extAudio.PlayStompSound();
			Destroy (gameObj);
		}
	}
}
