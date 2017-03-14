using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(GuiManager))]
[RequireComponent(typeof(HudManager))]
[RequireComponent(typeof(NotificationsManager))]
[RequireComponent(typeof(SoundManager))]
public class GameManager : MonoBehaviour {

	private AudioSource sound;
	public AudioSource Sound
	{
		get
		{
			if (sound == null) { sound = Instance.GetComponent<AudioSource>(); }
			return sound;
		}
	}


	private bool gamePaused;
	public bool GamePaused
	{
		get { return gamePaused; }
		set
		{
			gamePaused = value;
			//Debug.Log("Pause = " + gamePaused);
			if (gamePaused)
			{
				PauseTrue();
				Gui.EnablePopup(Gui.screen_Pause);
			}
			else
			{
				PauseFalse();
				Gui.DisablePopup(Gui.screen_Pause);
			}
		}
	}


	private bool inputAllowed;
	public bool InputAllowed
	{
		get
		{
			return inputAllowed;
		}
		set
		{
			inputAllowed = value;
		}
	}


	private bool inGameLevel;
	public bool InGameLevel
	{
		get { return inGameLevel; }
		set { inGameLevel = value; }
	}

	private static GameManager instance = null;
	public static GameManager Instance
	{
		get
		{
			if (instance == null) instance = new GameObject("GameManager").AddComponent<GameManager>();
			return instance;
		}
	}


	private static GuiManager gui = null;
	public static GuiManager Gui
	{
		get
		{
			if (gui == null) gui = Instance.GetComponent<GuiManager>();
			return gui;
		}

	}

	private int testNumber = 0;


	private static HudManager hud = null;
	public static HudManager Hud
	{
		get
		{
			if (hud == null) hud = Instance.GetComponent<HudManager>();
			return hud;
		}
		
	}


	private static NotificationsManager notifications = null;
	public static NotificationsManager Notifications
	{
		get
		{
			if (notifications == null) notifications = Instance.GetComponent<NotificationsManager>();
			return notifications;
		}
	}

	private string currentScene;

	

	void Awake()
	{
		//check if there is an existing instance of this object
		if((instance) && (instance.GetInstanceID() != GetInstanceID()))
		{
			DestroyImmediate(gameObject);			//delete duplicate
		}
		else
		{
			instance = this;						//make this gameobj. the only instance
			DontDestroyOnLoad(gameObject);
		}

		
		inputAllowed = true;
	}


	void Start()
	{
		CheckLevelType();
	}

	void Update()
	{
		//check if is in a playable level, only bring up pause menu if they are
		if (InGameLevel && Input.GetButtonDown("Cancel"))
		{
			GamePaused = !GamePaused;
		}
	}

	private void CheckLevelType()
	{
		//check name of current scene to determine settings for timescale, ui, input
		currentScene = SceneManager.GetActiveScene().name;
		if (currentScene == "TitleScene")
		{
			InGameLevel = false;
			InputAllowed = false;
		}
		else
		{
			InGameLevel = true;
			InputAllowed = true;
			PauseFalse();
		}
	}


	private void EndGame_Lose()
	{
		EndGame();
	}


	private void EndGame_Win()
	{
		EndGame();
	}


	private void EndGame()
	{
		InGameLevel = false;
		PauseTrue();
	}


	public void LoadScene(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}

	void OnDisable()
	{
		//tell function to stop listening for scene change - replaces OnLevelWasLoaded
		SceneManager.sceneLoaded -= OnSceneChange;
	}

	void OnEnable()
	{
		//tell function to listen for scene change - replaces OnLevelWasLoaded
		SceneManager.sceneLoaded += OnSceneChange;
	}

	void OnSceneChange(Scene scene, LoadSceneMode mode)
	{
		//Debug.Log(scene.name);
		//Debug.Log("mode = " + mode);
		CheckLevelType();
		gameObject.SendMessage("SceneSetup", scene.name);
	}
	

	private void PauseFalse()
	{
		Time.timeScale = 1;
		InputAllowed = true;
	}


	private void PauseTrue()
	{
		Time.timeScale = 0;
		InputAllowed = false;
	}


	public void RestartLevel()
	{
		Notifications.PostNotification(this, "RestartLevel");
		SceneManager.LoadScene("Level1");						//TODO - make this depend on the actual current level
		gameObject.SendMessage("PauseFalse");

	}

	
	
}
