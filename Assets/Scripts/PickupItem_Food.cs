using UnityEngine;
using System.Collections;

public class PickupItem_Food : PickupItem {

	public enum FoodType { hamburger };
	public FoodType foodType;

	public int healthInc = 10;               //how much health this item restores
	public int bacInc = -5;					//how much bac changes, negative for food

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected override void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Player"))
		{
			if (foodType == FoodType.hamburger)
			{
				GameManager.Notifications.PostNotification(this, "Powerup_Food_Hamburger_Collected");
				GameManager.Instance.Sound.PlayOneShot(pickupSound);
				collider.gameObject.SendMessage("ChangeBac", bacInc);
				collider.gameObject.SendMessage("ChangeHealth", healthInc);
				Destroy(gameObject);
			}
		}
	}
}
