using UnityEngine;
using System.Collections;

public class ExternalAudio : MonoBehaviour {

	public AudioClip throwSound;
	public AudioClip canHitEnemy;
	public AudioClip canHitsGround;
	public AudioClip stompSound;

	private AudioSource source;

	// Use this for initialization
	void Awake ()
	{
		source = GetComponent<AudioSource>();
	}
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void PlayCanHitsGround()
	{
		source.PlayOneShot(canHitsGround);
	}

	public void PlayCanHitsEnemy()
	{
		source.PlayOneShot(canHitEnemy);
	}
	public void PlayThrowSound()
	{
		source.PlayOneShot(throwSound);
	}
	public void PlayStompSound()
	{
		source.PlayOneShot(stompSound);
	}
}
