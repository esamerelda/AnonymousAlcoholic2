using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public Transform[] backgrounds;
	private float[] parallaxScales;			//proportion of cam's movement to move bg by
	public float smoothing = 1f;                //make sure to set this above 0
	private Transform cam;
	private Vector3 previousCamPos;				//position of camera in previous frame

	void Awake()
	{
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		previousCamPos = cam.position;

		//assign corresponding parallax scales
		parallaxScales = new float[backgrounds.Length];
		for(int i = 0; i < backgrounds.Length; i++)
		{
			parallaxScales[i] = backgrounds[i].position.z * -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < backgrounds.Length; i++)
		{
			//the parallax is the opposite of the camera movement bc previous frame * by the scale
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

			//set target x position which is current pos plus parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			//create target pos for background current pos with its target x...???
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			//fade between current pos and target pos
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		//set previousCamPos to cam's pos at end of frame
		previousCamPos = cam.position;
	}
}
