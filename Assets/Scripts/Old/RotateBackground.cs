using UnityEngine;
using System.Collections;

public class RotateBackground : MonoBehaviour {

	float rotateSpeed = 20f;  //how many degrees bg image will rotate per second

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
    	transform.Rotate (0, 0, rotateSpeed * Time.deltaTime); //rotates x degrees per second around z axis
    }
}
