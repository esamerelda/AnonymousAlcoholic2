using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//this script controls all UI elements which the player can interact with.

public class GuiManager : MonoBehaviour {

	//public Canvas guiCanvas;
	public Canvas screen_Controls;
	public Canvas screen_Credits;
	public Canvas screen_EndGame;
	public Canvas screen_Pause;
	public Canvas screen_Title;

	private List<Canvas> ScreenList = new List<Canvas>();

	private string currentScene;

	public Text endGameMessage;
	public Text endGameReason;

	void Awake()
	{
		ScreenList.Add(screen_Controls);
		ScreenList.Add(screen_Credits);
		ScreenList.Add(screen_EndGame);
		ScreenList.Add(screen_Pause);
		ScreenList.Add(screen_Title);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisableAllScreens()
	{
		foreach(Canvas screen in ScreenList)
		{
			screen.enabled = false;
		}
	}

	public void DisableAllScreensExcept(Canvas exception)
	{
		foreach (Canvas screen in ScreenList)
		{
			if (screen != exception)
			{
				screen.enabled = false;
			}
			else
			{
				screen.enabled = true;
			}
		}
	}


	public void DisablePopup(Canvas screen)
	{
		screen.enabled = false;
	}


	public void EnablePopup(Canvas screen)
	{
		screen.enabled = true;
	}


	public void EndGame_Lose()
	{
		screen_EndGame.enabled = true;
		endGameMessage.text = "You have died.";
		endGameReason.text = "Eat food and avoid enemies to keep your health up.  If your B.A.C. (green meter) gets too low, your health will start to drain.";
	}

	
	public void EndGame_Win()
	{
		screen_EndGame.enabled = true;
		endGameMessage.text = "Level Complete!";
	}


	private void SceneSetup(string scene)
	{

		screen_Pause.enabled = false;

		if (scene == "TitleScene")
		{
			//Debug.Log("Disabled");
			//screen_Pause.enabled = false;
			//screen_Title.enabled = true;
			DisableAllScreensExcept(screen_Title);
		}
		else
		{
			//screen_Pause.enabled = true;
			//screen_Title.enabled = false;
			DisableAllScreens();
		}
	}
	

	private void PauseFalse()
	{
		screen_Pause.enabled = false;
	}


	private void PauseTrue()
	{
		screen_Pause.enabled = true;
	}


	
}
