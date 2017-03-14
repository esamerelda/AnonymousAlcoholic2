using UnityEngine;
using System.Collections;

public class PlayerControllerPlatformer2D : MonoBehaviour {

	//private bool facingRight = true;		// this feels clunky and stupid
	private bool grounded;
	public bool Grounded
	{
		get
		{
			return grounded;
		}
		set
		{
			grounded = value;
			//Debug.Log("Player Grounded = " + grounded);
		}
	}

	private bool isTouchingEnemy = false;
	private bool jump;

	private float jumpForce;
	public float JumpForce
	{
		get
		{
			return jumpForce;
		}
		set
		{
			jumpForce = value;
		}
	}
	

	private float walkSpeed;
	public float WalkSpeed
	{
		get
		{
			return walkSpeed;
		}
		set
		{
			walkSpeed = value;
		}
	}


	private NotificationsManager Notifications;

	private Transform groundCheck;			//requires child gameobject with collider set to isTrigger on or near feet of player

	

	void Awake()
	{
		grounded = false;
		groundCheck = transform.FindChild("playerFeet").transform;
		isTouchingEnemy = false;
		jump = false;
		jumpForce = 1125;
		Notifications = GameManager.Notifications;
		walkSpeed = 15f;
	}

	// Use this for initialization
	void Start () {
	}

	
	// Update is called once per frame
	void Update () {
		if (!GameManager.Instance.InputAllowed) return;         //skip this whole function if gameManager doesn't allow input.

		//WALKING
		transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * WalkSpeed);    //moves player < and >

		//JUMPING
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		//grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		if (((Input.GetButtonDown("Jump")) && (grounded)) || ((isTouchingEnemy) && Input.GetButtonDown("Jump")))  //jump if player is on the ground and pressing ^
		{
			jump = true;
		}
	}


	void FixedUpdate()
	{
		if (!GameManager.Instance.InputAllowed) return;
		if (jump && grounded)
		{
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));       //Add a vertical force to the player
			jump = false;       //Make sure player can't jump again until the jump conditions from UPdate are satisfied
		}
		//grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		if(Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")) || Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Obstacle")))
		{
			grounded = true;
		}
		else
		{
			grounded = false;
		}
		//Debug.Log(grounded);
	}


	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			isTouchingEnemy = true;
			//Debug.Log("Touching Enemy!");
		}

		if (other.gameObject.CompareTag("LevelEnd"))
		{
			GameManager.Instance.SendMessage("EndGame_Win");
			Debug.Log("WIN!");
		}
	}


	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			isTouchingEnemy = false;
		}
	}
}
