using UnityEngine;
using System.Collections;

public class PickupItem_Alcohol : PickupItem {

	public enum AlcoholType { beer, whiskey };
	public AlcoholType alcoholType;

	//public AudioClip pickupSound;

	public int abv;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected override void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			if(alcoholType == AlcoholType.beer)
			{
				GameManager.Notifications.PostNotification(this, "Powerup_Alcohol_Beer_Collected");
			}
			
			GameManager.Notifications.PostNotification(this, "Powerup_Alcohol_Collected");
			GameManager.Instance.Sound.PlayOneShot(pickupSound);
			//GameManager.Instance.GetComponent<HudManager>().CurrentBAC += abv;	//change alcohol on hud
			collider.gameObject.SendMessage("ChangeBac", abv);
			//GameManager.Instance.gameObject.SendMessage("OnAlcoholPickup");
			Destroy(gameObject);
		}
	}
}
