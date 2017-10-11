using UnityEngine;
using System.Collections;
using GlobalSet;

public class DisplayManager : MonoBehaviour {

    public GameObject scoreLabel;
    public GameObject comboLabel;

    UILabel score;
    UILabel combo;

    // Use this for initialization
    void Start () {
        score = scoreLabel.GetComponent<UILabel>() as UILabel;
        combo = comboLabel.GetComponent<UILabel>() as UILabel;
	}
	
	// Update is called once per frame
	void Update () {
        score.text = Global.score + "";
        combo.text = Global.combo + "";
    }
}
