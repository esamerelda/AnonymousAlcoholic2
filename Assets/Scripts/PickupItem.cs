using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {

	public AudioClip pickupSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected virtual void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			GameManager.Instance.Sound.PlayOneShot(pickupSound);
			Destroy(this.gameObject);
		}
	}
}
