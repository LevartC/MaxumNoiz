using UnityEngine;
using GlobalSet;

public class ButtonAction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPress(bool isPressed)
    {
        if (isPressed)
        { Global.outGameBGM.PlayOneShot(Global.buttonSound); }
    }
}
