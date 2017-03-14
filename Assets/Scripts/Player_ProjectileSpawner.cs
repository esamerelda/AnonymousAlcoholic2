using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_ProjectileSpawner : MonoBehaviour {

	public AudioClip throwSound;

	private int currentAmmo = 0;
	public int CurrentAmmo
	{
		set
		{
			currentAmmo += value;
			//currentWeaponCount.text = inventory_cans.ToString();
			GameManager.Hud.Ammo = currentAmmo;
		}
		get
		{
			return currentAmmo;
		}
	}


	private float projectileSpeed = 100f;
	public float ProjectileSpeed
	{
		get { return projectileSpeed; }
		set { projectileSpeed = value; }
	}

	public GameObject beerCan;

	public Sprite testSprite;

	public Text currentWeaponCount;

	void Awake()
	{
		CurrentAmmo = 0;
	}

	// Use this for initialization
	void Start () {
		GameManager.Notifications.AddListener(this, "Powerup_Alcohol_Beer_Collected");

	}


	void Update () {
		if (GameManager.Instance.InputAllowed)
		{
			//keep projectile launcher pointed toward the cursor
			Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
			Vector3 dir = Input.mousePosition - pos;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		

		//this works better here than in FixedUpdate for some reason.
		if ((Input.GetButtonDown("Fire1")) && (GameManager.Instance.InputAllowed))
		{
            if (CurrentAmmo > 0)
			{
				GameManager.Instance.Sound.PlayOneShot(throwSound);
				GameObject projectile = Instantiate(beerCan, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;                         //spawn projectile
				projectile.transform.parent = transform.parent;
				//projectile.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * projectile.GetComponent<ProjectileController>().projectileSpeed);
				projectile.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * ProjectileSpeed);
				//Debug.Log("Throw Speed: " + ProjectileSpeed);
				projectile.transform.parent = null;

				CurrentAmmo = -1;
			}
			else if (CurrentAmmo <= 0)
			{
				Debug.Log("Out of Ammo");
				//play a sound!
			}
			
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			GameManager.Hud.CurrentWeaponSprite = testSprite;
		}
	
	}


	private void Powerup_Alcohol_Beer_Collected()
	{
		CurrentAmmo = 1;
		//Debug.Log("Total Cans: " + inventory_cans);
	}
}
