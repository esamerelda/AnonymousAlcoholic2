using UnityEngine;
using System.Collections;

public class jockController : MonoBehaviour {

	float walkSpeed= 17.0f; 
	//float moveSpeed = 17.0f;
	bool walkingRight = false;
	private bool isOnGround = false;  	// Whether or not player is grounded
	private bool hasBeenGrounded = false;
	private Transform groundCheck;  	//position marking where to check if the player is grounded

	private GameObject player;
	WalkinDudeController playerCtrl;
	bool playerDead;
	public bool isDead = false;

	private GameObject camera;
	ExternalAudio extAudio;
	

	// Use this for initialization
	void Awake () 
	{
		camera = GameObject.FindGameObjectWithTag("MainCamera");		//ref to camera
		extAudio = camera.GetComponent<ExternalAudio>();				//ref to camera's ExternalAudio Script

		player = GameObject.FindGameObjectWithTag("Player");
		playerCtrl = player.GetComponent<WalkinDudeController>();
		playerDead = playerCtrl.isDead;

		groundCheck = transform.Find ("groundCheck");
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		playerDead = playerCtrl.isDead;
		if (!playerDead)
		{
			isOnGround = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

			if (isOnGround && !walkingRight)	
			{
				hasBeenGrounded = true;
				transform.Translate(Vector3.left * walkSpeed * Time.deltaTime);
			}
			else if (isOnGround && walkingRight)
			{
				hasBeenGrounded = true;
				transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);
			}
			else
			{
				walkingRight = !walkingRight;
				changeDirection();	
			}
		}
	}

	void changeDirection()
	{
		if (walkingRight && hasBeenGrounded)
		{
			transform.localScale = new Vector3(-5f, 5f, 1f);
			transform.Translate(Vector3.right * (walkSpeed * 10) * Time.deltaTime);
		}
		else if (!walkingRight && hasBeenGrounded)
		{
			transform.localScale = new Vector3(5f, 5f, 1f);
			transform.Translate(Vector3.left * (walkSpeed * 10) * Time.deltaTime);
		}

	}

	void OnTriggerEnter2D(Collider2D gameObj) //collision detection
	{
		if (gameObj.gameObject.tag == "Death") 
		{
			Destroy (gameObject);
		}
		if (gameObj.gameObject.tag == "playerFeet" && !playerDead) 
		{
			isDead = true;
			extAudio.PlayStompSound();
			Destroy (gameObject);
		}
		if (gameObj.gameObject.tag == "Bullet") 
		{
			isDead = true;
			Destroy (gameObject);
		}
	}
}
