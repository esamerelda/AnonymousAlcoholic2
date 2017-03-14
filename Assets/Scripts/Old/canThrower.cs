using UnityEngine;
using System.Collections;

public class canThrower : MonoBehaviour {

	// Use this for initialization

	public Rigidbody2D canShot;
	float canSpeed;		//TODO - make this depend on inebriation level

	private WalkinDudeController playerCtrl;		//ref to WalkinDudeController script
	private GameObject camera;
	ExternalAudio extAudio;

	void Awake()
	{
		camera = GameObject.FindGameObjectWithTag("MainCamera");		//ref to camera
		extAudio = camera.GetComponent<ExternalAudio>();				//ref to camera's ExternalAudio Script
		playerCtrl = transform.root.GetComponent<WalkinDudeController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if fire button is pressed...
			if((Input.GetKeyDown(KeyCode.Mouse0)) && (playerCtrl.canCount > 0) && (playerCtrl.isDead == false))
			{
				canSpeed = playerCtrl.canSpeed;  //get current canSpeed from WalkinDudeController script
				// If the player is facing right...
				if(playerCtrl.facingRight)
				{
					// ... instantiate the rocket facing right and set it's velocity to the right. 
					Rigidbody2D bulletInstance = Instantiate(canShot, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					//Rigidbody2D bulletInstance = Instantiate(Resources.Load(canShot), transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(canSpeed, 0);
					extAudio.PlayThrowSound();
					playerCtrl.canCount = playerCtrl.canCount - 1;
				}
				else
				{
					// Otherwise instantiate the rocket facing left and set it's velocity to the left.
					Rigidbody2D bulletInstance = Instantiate(canShot, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(-canSpeed, 0);
					extAudio.PlayThrowSound();
					playerCtrl.canCount = playerCtrl.canCount - 1;
				}
			}
	
	}
}
