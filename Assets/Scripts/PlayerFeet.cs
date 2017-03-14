using UnityEngine;
using System.Collections;

public class PlayerFeet : MonoBehaviour
{
	public AudioClip stompSound;
	private PlayerControllerPlatformer2D playerController;

	void Awake()
	{
		playerController = GameObject.Find("Player").GetComponent<PlayerControllerPlatformer2D>();
	}

	void Start()
	{

	}

	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			GameManager.Instance.Sound.PlayOneShot(stompSound);
			other.gameObject.SendMessage("Die");
		}
	}
}
