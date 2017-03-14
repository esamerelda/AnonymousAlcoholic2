using UnityEngine;
using System.Collections;

public class PingPong : MonoBehaviour {

	public Vector3 moveDirection = Vector3.zero;    //direction to move
	public float speed = 0.0f;
	public float travelDistance = 0.0f;         //distance to travel before inverting and turning back
	private Transform thisTransform = null;

	IEnumerator Start()
	{
		thisTransform = transform;
		while (true)
		{
			moveDirection = moveDirection * -1;
			yield return StartCoroutine(Travel());
		}
	}


	//travel full distance in direction from current position
	IEnumerator Travel()
	{
		float DistanceTravelled = 0;

		while (DistanceTravelled < travelDistance)
		{
			Vector3 DistanceToTravel = moveDirection * speed * Time.deltaTime;  //get new position based on speed & direction
			thisTransform.position += DistanceToTravel;                     //update position
			DistanceTravelled += DistanceToTravel.magnitude;
			yield return null;												//wait until next update
		}
	}
}
