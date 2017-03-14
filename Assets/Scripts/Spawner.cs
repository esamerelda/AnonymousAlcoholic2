using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	private GameObject spawnedObject;
	public GameObject prefab;

	//spawn as Rigidbody
	//private Rigidbody2D spawnedObject;
	//public Rigidbody2D prefab;

	private float spawnTimer;
	public float spawnTimerMax = 1f;

	void Awake()
	{
		spawnTimer = spawnTimerMax;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (spawnedObject == null)
		{
			spawnTimer -= Time.deltaTime;
			if (spawnTimer <= 0)
			{
				//to spawn as RigidBody
				//spawnedObject = Instantiate(prefab, transform.position, Quaternion.identity) as Rigidbody2D;
				//spawnTimer = spawnTimerMax;


				//to spawn as GameObject
				spawnedObject = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
				spawnedObject.transform.parent = transform;
				spawnTimer = spawnTimerMax;
			}
		}
	}
}
