using UnityEngine;
using System.Collections;
using GlobalSet;

public class NoteManager : MonoBehaviour {

    public NoteInfo noteInfo;
    public double absoluteTime;
    public Vector3 absolutePosition;


    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        absolutePosition.z = (float)absoluteTime * Global.z_PerMeasure;
        gameObject.transform.localPosition = absolutePosition;
    }

    public void destroyObject() {
		Destroy (gameObject);
	}
}
