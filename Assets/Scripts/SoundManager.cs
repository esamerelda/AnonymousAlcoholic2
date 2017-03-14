using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioSource))]
public class SoundManager : MonoBehaviour {

	public AudioClip canCrush;

	// Use this for initialization
	void Start () {
		//GameManager.Notifications.AddListener(this, "Powerup_Alcohol_Beer_Collected");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void Powerup_Alcohol_Beer_Collected()
	{
		GameManager.Instance.Sound.PlayOneShot(canCrush);
	}
}
