using UnityEngine;
using System.Collections;

public class Player_Stats : MonoBehaviour
{

	public enum Status { Withdrawing, Sober, Buzzed, Drunk, Blackout, Dead };
	private Status currentStatus;
	public Status CurrentStatus
	{
		get { return currentStatus; }
		set { currentStatus = value; }
	}


	//standard sober variables - static to allow basing other variables off of it
	private static float baseJumpForce = 1300f;
	private static float baseThrowSpeed = 1000f;
	private static float baseWalkSpeed = 15f;


	//adjustment variables to reconfigure stats based on inebriation level
	private float buzzedJump = (baseJumpForce / 2);
	private float drunkJump = baseJumpForce * .75f;
	private float blackoutJump = baseJumpForce * 1.5f;

	private float buzzedThrow = baseThrowSpeed * 1.25f;
	private float drunkThrow = baseThrowSpeed * 1.5f;
	private float blackoutThrow = baseThrowSpeed * 2f;

	private float buzzedWalk = (baseWalkSpeed / 2); //how much above or below baseWalkSpeed that walk speed might be set to
	private float drunkWalk = baseWalkSpeed; //how much above or below baseWalkSpeed...  Keeping var in case of tweaks needed
	private float blackoutWalk = (baseWalkSpeed * 2);	//same as above, may cause reverse walking?


	//timer variables
	private float timer_Bac;
	private float timer_Bac_Max = .5f;	
	private float timer_Health;
	private float timer_Health_Max = 1f;
	private float timer_Tol;
	private float timer_Tol_Max = 2f;

	public GameObject Thrower;

	private int currentBac;
	public int CurrentBac
	{
		get { return currentBac; }
		set
		{
			currentBac = value;
			//integers to make calculating stat effect ranges easier.
			//int withdrawalStart = 1;
			int soberStart = CurrentTol / 2; //withdrawal max is half of current tolerance
			int buzzedStart = CurrentTol;
			int drunkStart = CurrentTol + (StatusMargin / 2);
			int blackoutStart = CurrentTol + StatusMargin;

			//WITHDRAWING
			if(currentBac <= soberStart - 1)
			{
				if (currentBac < 0) currentBac = 0;						//set bac to 0 if it falls below
				CurrentStatus = Status.Withdrawing;						//update status
				pc.JumpForce = Random.Range(baseJumpForce / 4, baseJumpForce);
				pc.WalkSpeed = Random.Range(baseWalkSpeed/4, baseWalkSpeed);
				ps.ProjectileSpeed = Random.Range(baseThrowSpeed / 2, baseThrowSpeed);
			}

			//SOBER
			else if((currentBac >= soberStart) && (currentBac <= buzzedStart - 1))
			{
				CurrentStatus = Status.Sober;
				pc.JumpForce = baseJumpForce;
				pc.WalkSpeed = baseWalkSpeed;
				ps.ProjectileSpeed = baseThrowSpeed;
			}

			//BUZZED
			else if ((currentBac >= buzzedStart) && (currentBac <= drunkStart - 1))
			{
				CurrentStatus = Status.Buzzed;
				pc.JumpForce = Random.Range(baseJumpForce - buzzedJump, baseJumpForce + buzzedJump);
				pc.WalkSpeed = Random.Range(baseWalkSpeed - buzzedWalk, baseWalkSpeed + buzzedWalk);
				ps.ProjectileSpeed = Random.Range(baseThrowSpeed - buzzedThrow, baseThrowSpeed + buzzedThrow);
			}

			//DRUNK
			else if ((currentBac >= drunkStart) && (currentBac <= blackoutStart - 1))
			{
				CurrentStatus = Status.Drunk;
				pc.JumpForce = Random.Range(baseJumpForce - drunkJump, baseJumpForce + drunkJump);
				pc.WalkSpeed = Random.Range(baseWalkSpeed - drunkWalk, baseWalkSpeed + drunkWalk);
				ps.ProjectileSpeed = Random.Range(baseThrowSpeed - drunkThrow, baseThrowSpeed + drunkThrow);
			}

			//BLACKOUT DRUNK
			else if ((currentBac >= blackoutStart) && (currentBac <= 99))
			{
				CurrentStatus = Status.Blackout;
				pc.JumpForce = Random.Range(baseJumpForce - blackoutJump, baseJumpForce + blackoutJump);
				pc.WalkSpeed = Random.Range(baseWalkSpeed - blackoutWalk, baseWalkSpeed + blackoutWalk);
				ps.ProjectileSpeed = Random.Range(baseThrowSpeed - blackoutThrow, baseThrowSpeed + blackoutThrow);
			}

			//DEAD if bac = 100
			else if (currentBac >= 100)
			{
				currentBac = 100;
				CurrentStatus = Status.Dead;
				Debug.Log("DEATH - Alcohol Poisoning");
			}
			
			//ERROR if something else
			else
			{
				Debug.Log("ERROR - CURRENT BAC UNACCOUNTED FOR");
				Debug.Log("Bac = " + CurrentBac);
				Debug.Log("sober " + soberStart);
				Debug.Log("buzz " + buzzedStart);
				Debug.Log("drunk " + drunkStart);
				Debug.Log("blackout " + blackoutStart);
			}

			//Debug.Log(CurrentStatus + ": walk = " + pc.WalkSpeed);
			//Debug.Log(CurrentStatus + ": jump = " + pc.JumpForce);
			//Debug.Log(CurrentStatus + ": throw = " + ps.ProjectileSpeed);
			GameManager.Hud.Bac = currentBac;                       //update HUD B.A.C.
			GameManager.Hud.CurrentState = CurrentStatus.ToString();
		}
	}

	private Player_ProjectileSpawner ps;


	private int currentHealth;
	public int CurrentHealth
	{
		get { return currentHealth; }
		set
		{
			currentHealth = value;
			//make sure health is within 0 and 100
			if (currentHealth <= 0)
			{
				currentHealth = 0;
				GameManager.Instance.SendMessage("EndGame_Lose");
			}
			if (currentHealth > 100) { currentHealth = 100; }
			//Debug.Log(currentHealth);
			GameManager.Hud.Health = currentHealth;                 //update HUD Health
		}
	}


	private int currentTol;
	private int CurrentTol
	{
		get { return currentTol; }
		set
		{
			currentTol = value;
			GameManager.Hud.Tolerance = currentTol;                 //update HUD Tolerance
		}
	}

	private int incBac = 1;
	private int incHealth = 3;              //rate at which health falls when BAC = 0;
	private int incTol = 1;

	private int startBac = 50;              // Blood Alcohol content at beginning of level
	private int startHealth = 100;          // health at beginning of level
	private int startTol = 50;              // tolerance at beginning of level

	private int statusMargin = 25;			//used to determine how far above or below tolerance that BAC can be before status effects (drunk, withdrawals) kick in.
	public int StatusMargin
	{
		get { return statusMargin; }
		set { statusMargin = value; }		//maybe we want this to be affected by something?
	}


	private PlayerControllerPlatformer2D pc;


	//-------------------------------------------SETUP-------------------------------------------------

	void Awake()
	{
	}


	void Start()
	{
		pc = GetComponent<PlayerControllerPlatformer2D>();
		ps = Thrower.GetComponent<Player_ProjectileSpawner>();
		Set_Start_Stats();
		Set_Timers();
	}


	void Update()
	{
		Update_Timers();
		//Debug.Log(currentHealth);
	}


	private void Set_Start_Stats()
	{
		CurrentBac = startBac;
		CurrentHealth = startHealth;
		CurrentTol = startTol;
	}

	private void Set_Timers()
	{
		timer_Bac = timer_Bac_Max;
		timer_Health = timer_Health_Max;
		timer_Tol = timer_Tol_Max;
	}

	private void Update_Timers()
	{
		//BAC falls over time
		if (CurrentBac > 0)
		{
			timer_Bac -= Time.deltaTime;
			if (timer_Bac <= 0)
			{
				CurrentBac -= incBac;
				timer_Bac = timer_Bac_Max;
			}
		}

		//HEALTH - falls only if BAC = 0 or WITHDRAWING
		//if (CurrentBac <= 0)
		if (CurrentStatus == Status.Withdrawing)
		{
			timer_Health -= Time.deltaTime;
			if (timer_Health <= 0)
			{
				CurrentHealth -= incHealth;
				timer_Health = timer_Health_Max;
				//TODO - reset timer if it stops halfway between starting timer and executing, if player drinks a beer or something
			}
		}

		//TOLERANCE moves toward BAC over time
		timer_Tol -= Time.deltaTime;
		if (timer_Tol <= 0)
		{
			if (CurrentBac > CurrentTol)            //if BAC is higher than tolerance
			{
				CurrentTol += incTol;           //increase tolerance
				timer_Tol = timer_Tol_Max;
			}
			else if (CurrentBac < CurrentTol)   //if BAC is less than tolerance
			{
				CurrentTol -= incTol;           //decrease tolerance
				timer_Tol = timer_Tol_Max;
			}
			else
			{
				timer_Tol = timer_Tol_Max;
			}
		}
	}

	//---------------------------------------------GAMEPLAY-------------------------------------------------

	private void ChangeBac(int bac_Change)
	{
		CurrentBac += bac_Change;
	}

	private void ChangeHealth(int health_Change)
	{
		CurrentHealth += health_Change;
	}
}