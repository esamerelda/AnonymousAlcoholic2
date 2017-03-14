using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

//this script controls the in-level Heads Up Display to show player relevant stats and variables.

public class HudManager : MonoBehaviour {

	public Canvas hudCanvas;

	public Image currentWeaponImage;
	public Sprite CurrentWeaponSprite
	{
		set
		{
			currentWeaponImage.sprite = value;
		}
	}

	private int ammo;
	public int Ammo
	{
		get { return ammo; }
		set
		{
			ammo = value;
			ammoCount.text = ammo.ToString();
		}
	}

	private int bac;
	public int Bac
	{
		get { return bac; }
		set
		{
			bac = value;
			
			sliderB.value = bac;		//update slider
		}
	}

	private int health;
	public int Health
	{
		get { return health; }
		set
		{
			health = value;
			sliderH.value = health;		//update slider
		}
	}

	private int tolerance;
	public int Tolerance
	{
		get { return tolerance; }
		set
		{
			tolerance = value;
			sliderT.value = tolerance;
		}
	}

	public Slider sliderB;			//BAC slider
	public Slider sliderH;			//health slider
	public Slider sliderT;          //tolerance slider

	private string currentScene;
	private string currentState;
	public string CurrentState
	{
		set
		{
			currentState = value;
			StateDisplay.text = currentState;
		}
	}

	public Text ammoCount;
	public Text StateDisplay;


	private void PauseFalse()
	{
		hudCanvas.enabled = true;
	}


	private void PauseTrue()
	{
		hudCanvas.enabled = false;
	}


	private void SceneSetup(string scene)
	{
		if (scene == "TitleScene")
		{
			//Debug.Log("Disabled");
			hudCanvas.enabled = false;
		}
		else
		{
			hudCanvas.enabled = true;
		}
	}
}
