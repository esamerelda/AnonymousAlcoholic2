using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

	public AudioClip hitEnemy;
	public AudioClip hitGround;
	
	private bool active;
	public float projectileDamage;
	public float projectileSpeed;

	void Awake()
	{
		active = true;
	}
	// Use this for initialization
	void Start () {
		Destroy(gameObject, 5);			//destroys projectile after x seconds if nothing else does first.
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Enemy") && (active))
		{
			GameManager.Instance.Sound.PlayOneShot(hitEnemy);
			other.gameObject.SendMessage("TakeDamage", projectileDamage);
			//Debug.Log("SendMessageDamage");
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			active = false;
		}
		else if (other.gameObject.CompareTag("Ground"))
		{
			GameManager.Instance.Sound.PlayOneShot(hitGround);
			active = false;
		}
	}
}
