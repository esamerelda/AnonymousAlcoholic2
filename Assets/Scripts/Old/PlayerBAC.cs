using UnityEngine;
using UnityEngine.UI; //text, images, sliders
using System.Collections;

public class PlayerBAC : MonoBehaviour {

	// Use this for initialization

	//BAC
	public int startBac = 5;
	public static int currentBac;
	public int maxBac = 100; 									//TODO slider maxValue set to same, should synchronize through code.
	public Slider bacSlider;								//slider bar on right, shows bac and tolerance
	float soberTimer;										//timer variable to count down to bac drop
	float soberTimerMax = .5f;								//how much time for bac to drop
	int sobriety = 1;										//how much bac drops when soberTimer reaches 0

	//Tolerance
	int startTolerance = 50;
	public static int currentTolerance;
	//int tolMax = 100;
	int tolChange = 1;
	float tolTimer;
	float tolTimerMax = 3.5f;
	public Slider toleranceSlider;

	//Blacking Out
	public Image blackoutImage;  							//black out randomly when severely drunk
	float blackoutTimer;									//will set to random number and count down while player is really drunk
	float blackoutFrequencyMin = .01f;
	float blackoutFrequencyMax = 4f;								// TODO - give random value to randomize blackouts
	//float blackoutFlashSpeed = 1.8f;  						//the speed blackoutImage will fade at
	float blackoutFlashSpeed;
	float blackoutFlashSpeedMin = .01f;
	float blackoutFlashSpeedMax = 2.5f;
								
	int blackoutLimit = 75;									// minimum bac necessary to induce blackouts
	bool blackedOut;										//if player is currently blacking out
	public Color blackoutFlashColor = new Color(0, 0, 0, 255);

	//belching
	//public AudioClip belchSound;
	//private AudioSource source;

	//Dying
	bool isDead;  										//whether player is dead
	public static bool deadWithdrawal;						//tells WalkinDudeController if player died of withdrawal
	public static bool deadBloodPoisoning;					//tells WalkinDudeController if player drank too much


	//an error pops up saying this is never used, but don't listen to it.
	WalkinDudeController playerMovement;  					//reference to player's movement script.

	

	void Awake()
	{
		//set up references

		playerMovement = GetComponent <WalkinDudeController> ();
		//source = GetComponent<AudioSource>();  				//reference to walkinDude's audioSource Component

		//set initial stats
		bool isDead;
		deadWithdrawal = false;
		deadBloodPoisoning = false;
		
		currentBac = startBac;
		currentTolerance = startTolerance;

		//set timers
		soberTimer = soberTimerMax;
		tolTimer = tolTimerMax;
		blackoutTimer = blackoutFrequencyMax / 2;

	}


	
	// Update is called once per frame
	void Update () 
	{
		isDead = playerMovement.isDead;
		if (isDead == false)
		{
			//BAC falls slowly over time
			soberTimer -= Time.deltaTime;
			if (soberTimer <= 0)
			{
				soberTimerUpdate();
			}
			//tolerance matches bac over time
			tolTimer -= Time.deltaTime;
			if (tolTimer <= 0)
			{
				tolTimerUpdate();
			}

			//Debug.Log(this + " bac: " + currentBac);

			Blackout();  //checks to see if screen should be flashing black / if player is hammered.
		}
		else
		{
			Destroy (this);
		}

	}


	public void Drink (int abv)  //TakeDamage in tutorial
	{
		//Increase currentBac by amount of alcohol in drink
		currentBac += abv;
		//source = GetComponent<AudioSource>();
		reactToBac();
	}


	//When soberTimer reaches 0
	private void soberTimerUpdate()
	{
		//reduce currentBac
		currentBac -= sobriety;
		reactToBac();

		//reset timer
		soberTimer = soberTimerMax;
	}

	private void tolTimerUpdate()  //update tolerance timer
	{
		if (currentBac > currentTolerance)
		{
			currentTolerance += tolChange;
		}
		else if (currentBac < currentTolerance)
		{
			currentTolerance -= tolChange;
		}

		//update tolerance slider
		toleranceSlider.value = currentTolerance;
		//reset timer
		tolTimer = tolTimerMax;
	}


	private void reactToBac() {
		//adjust slider accordingly
		bacSlider.value = currentBac;

		//if BAC reaches 0, player dies of withdrawal
		if((currentBac <= 0) && !isDead)
		{
			deadWithdrawal = true;
			isDead = true;
			//Death();
		}
		else if (currentBac >= maxBac)
		{
			deadBloodPoisoning = true;
			isDead = true;
		}

		//if player is really drunk, blackouts happen 
		if (currentBac >= blackoutLimit)
		{
			Blackout();
		}
	}


	//TODO - change color of bac meter to indicate danger
	void bacMeterColor()
	{
		if ((currentBac >= 75) ||(currentBac <=25))
		{
			//turn meter red
			//bacSlider.Background.color = redColor;
		}
		else
		{
			//bacSlider.Background.color = whiteColor;
		}
	}


	//When really drunk, blackouts can occur
	void Blackout()
	{
		
		if (currentBac >= blackoutLimit)
		{
			blackoutTimer -= Time.deltaTime;
			if (blackoutTimer <= 0)
			{
					//TODO fade to black
				//set color of blackoutImage to flash color 
				blackoutImage.color = blackoutFlashColor;
				//reset blackout timer  
				//blackoutTimer = blackoutFrequencyMax;
				blackoutFlashSpeed = Random.Range(blackoutFlashSpeedMin, blackoutFlashSpeedMax);
				blackoutTimer = Random.Range(blackoutFrequencyMin, blackoutFrequencyMax);
			}
			else
			{
				blackoutImage.color = Color.Lerp (blackoutImage.color, Color.clear, blackoutFlashSpeed * Time.deltaTime);
			}
		}
		else
		{
			//transition color back to clear
			blackoutImage.color = Color.Lerp (blackoutImage.color, Color.clear, blackoutFlashSpeed * Time.deltaTime);
		}
	}
}
