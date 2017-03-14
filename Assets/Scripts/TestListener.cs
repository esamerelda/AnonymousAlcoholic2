using UnityEngine;
using System.Collections;


//works even when it is not the only script component on GameManager (have to add gameobject to listener and poster gameObjects)
//??? Will this be a pain in the ass when we have a ton of objects???
public class TestListener : MonoBehaviour {

	//ref to global Notifications Manager
	public NotificationsManager Notifications = null;
	private int keyboardEvents = 0;

	
	void Start()
	{
		//Register this object as a listener for keyboard notifications
		if(Notifications != null)
		{
			Notifications.AddListener(this, "OnKeyboardInput");
		}
	}


	public void OnKeyboardInput(Component Sender)
	{
		//print to console
		keyboardEvents++;
		//Debug.Log("Keyboard Event #" + keyboardEvents + " Occurred");
	}
}
