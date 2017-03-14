using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using System;

public class WalkinDudeController : MonoBehaviour {

	// Use this for initialization	

	//walking
	public float moveSpeed;  		//may need to turn private
	public float moveSpeedNormal = 40f;

	//jumping
	bool jump = false; 					// Condition for whether the player should jump
	float jumpForce;  					//Amount of force added when the player jumps
	float jumpForceNormal = 2250f;
	private bool isOnGround = false;  	// Whether or not player is grounded
	private Transform groundCheck;  	//position marking where to check if the player is grounded

	//THROWING
	public int canCount = 0;  				//start game with 0 cans
	[HideInInspector]
	public bool facingRight = true;			//determine which direction player faces to throw can in that direction
	[HideInInspector]
	public float canSpeed;
	float canSpeedNormal = 60f;
	public AudioClip gulpSound;
	public AudioClip canCrushSound;

	//public AudioClip belchSound;

	

	//DRINKING
	int beerAbv = 10;

	int tolerance;
	int toleranceMargin = 20;			//how far below or above 50 the BAC must be before alternate stats kick in
	int bac;

	//DYING
	[HideInInspector]
	public bool isDead = false;
	[HideInInspector]
	bool deadWithdrawal = false;		//If player's BAC meter reaches 0, dies of withdrawals
	bool deadBloodPoisoning = false;	//If player drinks too much
	bool deadFalling = false;			//if user falls off screen and dies
	bool deadEnemy = false;
	//death Box
	int guiBoxWidth;
	int guiBoxHeight;
	int guiBoxX;
	int guiBoxY;
	//death Button
	int guiButtonWidth;
	int guiButtonHeight;
	int guiButtonX;
	int guiButtonY; 

	//winning
	bool wonGame = false;

	//scripts
	GameObject player;  				//the player
	PlayerBAC playerBac;				//reference to player's bac script

	public GameObject canCountDisplay;

	private AudioSource source;


	void Awake()
	{
		// Setting up references...?
		transform.position = new Vector3(0,-16,0);   //puts character in starting position
		player = GameObject.FindGameObjectWithTag("Player");
		playerBac = player.GetComponent <PlayerBAC> ();

		groundCheck = transform.Find ("groundCheck");

		source = GetComponent<AudioSource>();  //reference to WalkinDude's audioSource component

		//guiDeathBoxVariables
		guiBoxWidth = 300;
		guiBoxHeight = 50;
		guiBoxX = (Screen.width / 2) - (guiBoxWidth / 2);  
		guiBoxY = (Screen.height / 2) - (guiBoxHeight / 2);
		guiButtonWidth = 40;
		guiButtonHeight = 20;
		guiButtonX = (Screen.width / 2) - (guiButtonWidth / 2);
		guiButtonY = guiBoxY + guiBoxHeight - guiButtonHeight;

		//set variables to starting
		isDead = false;
		moveSpeed = moveSpeedNormal;
		jumpForce = jumpForceNormal;
		canSpeed = canSpeedNormal;

		//update can UI text, starts at 0
		UpdateCanCount();
	}


	
	// Update is called once per frame
	void Update ()
	{

		if (isDead == false)  		//TODO - Stop Camera if player dies
		{
			UpdateCanCount();		//updates cancount on bottom left of screen

			CheckBacVariables();

			transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal")* moveSpeed);    //moves player < and >.  replace with "Vertical" for up and down 

			//JUMPING
			// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
			isOnGround = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
			if ((Input.GetButtonDown("Jump")) && (isOnGround))	//jump if player is on the ground and pressing ^
			{
				jump = true;
			}

			//THROWING
			//Check direction so can knows which way to go
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				facingRight = true;
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				facingRight = false;
			}
		}
	}

	void FixedUpdate()
	{
		if (jump) 
		{
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (0f, jumpForce));		//Add a vertical force to the player
				jump = false;		//Make sure player can't jump again until the jump conditions from UPdate are satisfied
		}
	}


	void OnTriggerEnter2D(Collider2D gameObj) //collision detection
	{
		if (gameObj.gameObject.tag == "Death") 
		{
			isDead = true;
			deadFalling = true;
		}
		else if (gameObj.gameObject.tag == "beer") {
			canCount += 1;					//add a crushed can to the inventory
			source.PlayOneShot(gulpSound);
			if (!source.isPlaying)
			{
				source.PlayOneShot(canCrushSound);  //starts canCrushSound after gulpSound
			}
			Destroy(gameObj.gameObject); 	//destroys beer on screen
			playerBac.Drink(beerAbv);		//function in PlayerBAC to adjust player's BAC
		}
		else if (gameObj.gameObject.tag == "enemy")
		{
			isDead = true;
			deadEnemy = true;
		}
		else if (gameObj.gameObject.tag == "exit")
		{
			isDead = true;
			wonGame = true;
		}
	}

	void OnGUI()
	{
		if (deadBloodPoisoning == true)
		{
			GUI.Box(new Rect(guiBoxX,guiBoxY,guiBoxWidth,guiBoxHeight), "You drank too much and died of blood poisoning.");
			if(GUI.Button(new Rect(guiButtonX,guiButtonY,guiButtonWidth,guiButtonHeight),"OK"))
			{
				Death();
			}
		}
		if (deadFalling == true)
		{
			GUI.Box(new Rect(guiBoxX,guiBoxY,guiBoxWidth,guiBoxHeight), "You have fallen to your death.");
			if(GUI.Button(new Rect(guiButtonX,guiButtonY,guiButtonWidth,guiButtonHeight),"OK"))
			{
				Death();
			}
		}
		if (deadWithdrawal == true)
		{
			GUI.Box(new Rect(guiBoxX,guiBoxY,guiBoxWidth,guiBoxHeight), "You have died from severe alcohol withdrawals.");
			if(GUI.Button(new Rect(guiButtonX,guiButtonY,guiButtonWidth,guiButtonHeight),"OK"))
			{
				Death();
			}
		}
		if (deadEnemy == true)
		{
			GUI.Box(new Rect(guiBoxX,guiBoxY,guiBoxWidth,guiBoxHeight), "You have been murdered to death.");
			if(GUI.Button(new Rect(guiButtonX,guiButtonY,guiButtonWidth,guiButtonHeight),"OK"))
			{
				Death();
			}
		}
		if (wonGame == true)
		{
			GUI.Box(new Rect(guiBoxX,guiBoxY,guiBoxWidth,guiBoxHeight), "You WIN!");
			if(GUI.Button(new Rect(guiButtonX,guiButtonY,guiButtonWidth,guiButtonHeight),"OK"))
			{
				Win();
			}
		}
	}

	void UpdateCanCount()
	{
		Text canDisplayText = canCountDisplay.GetComponent<Text>();
		canDisplayText.text = canCount.ToString();
	}

	void CheckBacVariables()
	{
		//check if bac is above max or below min
		if (PlayerBAC.deadWithdrawal == true)
		{
			deadWithdrawal = true;
			isDead = true;
		}
		else if (PlayerBAC.deadBloodPoisoning == true)
		{
			deadBloodPoisoning = true;
			isDead = true;
		}
		//if it is, set variables to true and negate movements

		tolerance = PlayerBAC.currentTolerance;
		bac = PlayerBAC.currentBac;
		if (bac > (tolerance + toleranceMargin)) //if above tolerance / is schwasted, wider range of random stats
		{
			moveSpeed = Random.Range(0f, moveSpeedNormal * 2f);
			jumpForce = Random.Range(0f, jumpForceNormal * 2f);
			canSpeed = Random.Range(0f, canSpeedNormal * 2f);
			//TODO - CHANGE BAC SLIDER COLOR
		}
		else if ((bac <= (tolerance + toleranceMargin)) && (bac >= (tolerance - toleranceMargin))) //if within tolerance, stats normal
		{
			moveSpeed = moveSpeedNormal;
			jumpForce = jumpForceNormal;
			canSpeed = canSpeedNormal;
		}
		else if (bac < (tolerance - toleranceMargin)) //if below tolerance range / having withdrawals, lower randomized stats.
		{
			moveSpeed = Random.Range(0f, moveSpeedNormal);
			jumpForce = Random.Range(jumpForceNormal / 2f, jumpForceNormal);
			canSpeed = Random.Range(canSpeedNormal / 2f, canSpeedNormal);
			//TODO - CHANGE BAC SLIDER COLOR
		}

	}

	//death function
	void Death()
	{
		//TODO make something cooler happen when you die.  Just reset for now.
		Application.LoadLevel(Application.loadedLevel);		//reloads this same level
	}
	void Win()
	{
		Application.LoadLevel(Application.loadedLevel);	
	}
}