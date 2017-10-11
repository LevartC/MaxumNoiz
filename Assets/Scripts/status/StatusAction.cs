using UnityEngine;
using System.Collections;

public class StatusAction : MonoBehaviour {

	public GameObject profileTarget;

	public GameObject denyTarget;

	public void profileClicked()
	{
		Debug.Log ("Profile Opened");
		profileTarget.SetActive (true);
	}

	public void profileCloseClicked()
	{
		Debug.Log ("Profile Closed");
		profileTarget.SetActive (false);
	}

	public void denyMessage()
	{
		Debug.Log ("Request Denied");
		denyTarget.SetActive (true);
	}

	public void denyMessageDown()
	{
		Debug.Log ("Request deny message down");
		denyTarget.SetActive (false);
	}

}
