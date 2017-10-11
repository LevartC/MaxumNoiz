using UnityEngine;
using System.Collections;

public class StatusCloseButton : MonoBehaviour {

	public Sprite buttonDownSprite;
	Sprite originalSprite;

	// Use this for initialization
	void Start () {
		originalSprite = GetComponent<SpriteRenderer> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			GetComponent<SpriteRenderer> ().sprite = buttonDownSprite;
		}
		if (Input.GetMouseButtonUp (0)) {
			GetComponent<SpriteRenderer> ().sprite = originalSprite;
		}
	}
}
