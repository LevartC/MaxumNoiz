using UnityEngine;
using System.Collections;

public class ProfileButton : MonoBehaviour {

	public GameObject target;

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown() {
		Debug.Log ("Profile Open");
		target.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
