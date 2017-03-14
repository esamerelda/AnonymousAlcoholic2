using UnityEngine;
using System.Collections;

public class AI_PingPong : MonoBehaviour {

	private Vector3 moveDirection = new Vector3(1, 0, 0);    //direction to move
	private Vector3 destination;
	public float speed = 1.0f;
	private float travelDistance = 1.0f;         //distance to travel before inverting and turning back
	private Transform thisTransform = null;
	private float smoothing = 1.0f;

	public IEnumerator StartWalking(Vector3 leftPoint, Vector3 rightPoint)
	{
		//Debug.Log("START WALKING");
		thisTransform = transform;
		while (true)
		{
			//moveDirection = moveDirection * -1;
			Debug.Log("Switch");
			if(moveDirection.x < 0)
			{
				destination = leftPoint;
				travelDistance = thisTransform.position.x - destination.x;
				yield return StartCoroutine(WalkLeft());
			
			}
			else
			{
				destination = rightPoint;
				travelDistance = destination.x - thisTransform.position.x;
				yield return StartCoroutine(WalkRight());
			}

			//yield return StartCoroutine(Travel());
		}
	}
	
	IEnumerator WalkLeft()
	{
		Debug.Log("Left");
		float DistanceTravelled = 0;
		//while (thisTransform.position.x > destination.x)
		while (DistanceTravelled < travelDistance)
        {
			//Vector3 DistanceToTravel = moveDirection * speed * Time.deltaTime;	//get new position based on speed & direction
			Vector3 DistanceToTravel = moveDirection * speed * Time.deltaTime;
			thisTransform.position += DistanceToTravel;								//update position
			DistanceTravelled += DistanceToTravel.magnitude;
		}
		yield return null;
	}

	IEnumerator WalkRight()
	{
		Debug.Log("Right");
		float DistanceTravelled = 0;
		//while (thisTransform.position.x < destination.x)
		while(DistanceTravelled < travelDistance)
		{
			Vector3 DistanceToTravel = moveDirection * speed * Time.deltaTime;  //get new position based on speed & direction
			thisTransform.position += DistanceToTravel;                         //update position
			DistanceTravelled += DistanceToTravel.magnitude;
		}
		yield return null;
	}


	public void StopWalking()
	{
		Debug.Log("STOP WALKING");
		StopAllCoroutines();
	}


	//travel full distance in direction from current position
	IEnumerator Travel()
	{
		Debug.Log("Travel");
		float DistanceTravelled = 0;

		//does nothing.
		//thisTransform.position = Vector3.Lerp(transform.position, destination, smoothing * Time.deltaTime);
		while (DistanceTravelled < travelDistance)
		{
			Vector3 DistanceToTravel = moveDirection * speed * Time.deltaTime;  //get new position based on speed & direction
			thisTransform.position += DistanceToTravel;							//update position
			DistanceTravelled += DistanceToTravel.magnitude;

			//can't control speed.  too fast.  stupid.
			//thisTransform.position = Vector3.Lerp(transform.position, destination, smoothing * Time.deltaTime);

			yield return null;													//wait until next update
		}
	}
}
