using UnityEngine;
using System.Collections;

public class Enemy_Jock : Enemy {

	//private AI_PingPong walkScript;
	private bool checkGrounded;
	//private bool flipSpriteX;
	[SerializeField]
	private bool grounded = false;
	private bool Grounded
	{
		get { return grounded; }
		set
		{
			grounded = value;
		}
	}
	private bool walking;

	private float groundLeftX = 0f;
	private float groundRightX = 0f;
	public float travelDistance = 0.0f;
	public float walkSpeed = 5.0f;

	private float buffer = 0.5f;                        //distance from edge where enemy turns and goes the other way
	private SpriteRenderer spRenderer;
	private Transform groundCheck;
	public Vector3 moveDirection = Vector3.zero;
	private Vector3 leftPoint = new Vector3(0, 0, 0);
	private Vector3 rightPoint = new Vector3(0, 0, 0);

	void Awake()
	{
		//flipSpriteX = true;
		grounded = false;
		Grounded = false;
		groundCheck = transform.FindChild("groundCheck");
		spRenderer = GetComponent<SpriteRenderer>();
		//flipSpriteX = spRenderer.flipX;
		walking = false;
	}
	

	// Update is called once per frame
	void Update () {

		checkGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		if (checkGrounded != Grounded)
		{
			Grounded = checkGrounded;
			//Debug.Log("checkGrounded != Grounded, checkGrounded = " + checkGrounded);
			if (!checkGrounded)
			{
				walking = false;
				//walkScript.StopWalking();
				//Destroy(gameObject, 5);
			}
		}

		
		
	}

	void FixedUpdate()
	{
		if (walking)
		{
			transform.Translate(Vector3.right * Time.deltaTime * walkSpeed);
			if ((transform.position.x < leftPoint.x) || (transform.position.x > rightPoint.x))
			{
				spRenderer.flipX = !spRenderer.flipX;
				walkSpeed = walkSpeed * -1;
				//Debug.Log("FLIP");
				//Debug.Log("spRenderer = " + spRenderer.flipX);
				//Debug.Log("walkSpeed = " + walkSpeed);
			}
		}
	}


	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			//Debug.Log("Ground Position: " + other.transform.position.x);
			//Debug.Log("Ground Width : " + other.collider.bounds.size.x / 2);
			//Debug.Log("Ground Left X: " + (other.transform.position.x - other.collider.bounds.size.x / 2));
			leftPoint = new Vector3((other.transform.position.x - other.collider.bounds.size.x / 2) + buffer, 0, 0);
			//Debug.Log("Ground Right X: " + (other.transform.position.x + other.collider.bounds.size.x / 2));
			rightPoint = new Vector3((other.transform.position.x + other.collider.bounds.size.x / 2) - buffer, 0, 0);
			if (Grounded)
			{
				//Debug.Log("We should be walking right now...");
				//StartCoroutine(walkScript.StartWalking(leftPoint, rightPoint));
				walking = true;
			}
		}
		else if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.SendMessage("ChangeHealth", damage);
			//Debug.Log("HIT!");
		}
		
	}
}
