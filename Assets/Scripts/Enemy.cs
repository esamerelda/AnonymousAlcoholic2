using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//public AudioClip clip_death;
	//public AudioClip clip_takeDamage;

	private int currentHealth;
	public int damage;
	public int maxHealth;

	public string enemyName;

	void Awake()
	{
		currentHealth = maxHealth;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

	private void Die()
	{
		Destroy(gameObject);
	}


	private void TakeDamage(int damage)
	{
		//Debug.Log(enemyName + " has taken " + damage + " damage!");
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			Die();
		}

	}
}
