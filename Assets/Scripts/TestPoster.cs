using UnityEngine;
using System.Collections;

public class TestPoster : MonoBehaviour {

	//ref to global notifications manager
	public NotificationsManager Notifications = null;

	void Update()
	{
		//check for keyboard input
		if(Input.anyKeyDown && Notifications != null)
		{
			Notifications.PostNotification(this, "OnKeyboardInput");
		}
	}
}
