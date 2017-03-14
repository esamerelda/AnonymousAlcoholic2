using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationsManager : MonoBehaviour {

	//Internal ref to all listeners for notifications
	private Dictionary<string, List<Component>> Listeners = new Dictionary<string, List<Component>>();


	//Add listener for a notification to listeners list
	public void AddListener(Component Sender, string NotificationName)
	{
		//Add listener to dictionary
		if (!Listeners.ContainsKey(NotificationName))
		{
			Listeners.Add(NotificationName, new List<Component>());
		}

		//add object to listener list for this notification
		Listeners[NotificationName].Add(Sender);
	}


	//remove a listener for a notification
	public void RemoveListener(Component Sender, string NotificationName)
	{
		//If no key in dictionary exists, exit
		if (!Listeners.ContainsKey(NotificationName))
		{
			return;
		}

		//cycle through listeners and identify component, and remove
		for (int i = Listeners[NotificationName].Count - 1; i >= 0; i--)
		{
			//Check instance ID
			if (Listeners[NotificationName][i].GetInstanceID() == Sender.GetInstanceID())
			{
				Listeners[NotificationName].RemoveAt(i);  //matched -- remove from list
			}
		}
	}


	//Post a notification to a listener
	public void PostNotification(Component Sender, string NotificationName)
	{
		//if no key in dictionary exists, exit
		if (!Listeners.ContainsKey(NotificationName))
		{
			Debug.Log("The Key " + NotificationName + " is not in the dictionary");
			return;
		}

		//else post notification to all matching listeners
		foreach(Component Listener in Listeners[NotificationName])
		{
			//Debug.Log("Sending message");
			Listener.SendMessage(NotificationName, Sender, SendMessageOptions.DontRequireReceiver);
		}
	}


	//clear all listeners
	public void ClearListeners()
	{
		Listeners.Clear();
	}


	//remove repeat listeners - deleted and removed
	public void RemoveRedundancies()
	{
		//Create new dictionary
		Dictionary<string, List<Component>> TmpListeners = new Dictionary<string, List<Component>>();

		//Cycle through all dictionary entries
		foreach(KeyValuePair<string, List<Component>> Item in Listeners)
		{
			//cycle through all listener objects in list, remove null objects
			for (int i = Item.Value.Count-1; i>=0; i--)
			{
				//If null, remove item
				if(Item.Value[i] == null)
				{
					Item.Value.RemoveAt(i);
				}
			}

			//if items remain in list for this notification, add this to tmp dictionary
			if (Item.Value.Count > 0)
			{
				TmpListeners.Add(Item.Key, Item.Value);
			}
		}

		//Replace listeners object with new optimized dictionary
		Listeners = TmpListeners;
	}


	//Called when a new level is loaded.  Remove redundant entries from dictionary in case left over from previous scene
	//TODO - find un-deprecated version of this function!!!
	/*void OnLevelWasLoaded()
	{
		//clear redundancies
		RemoveRedundancies();
	}*/

	void SceneSetup(string scene)
	{
		RemoveRedundancies();
	}
}
