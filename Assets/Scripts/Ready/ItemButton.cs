using UnityEngine;
using GlobalSet;
using System.Collections;

public class ItemButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void buttonSound()
    {
        if (Global.bgmVolume < 1f)
        {
            if (gameObject.GetComponent<UIToggle>().isChecked)
            { Global.outGameBGM.PlayOneShot(Global.buttonSound_ItemSelect); }
            else
            { Global.outGameBGM.PlayOneShot(Global.buttonSound_ItemDeselect); }
        }
    }
}
