using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalSet;

public class LoadResources : MonoBehaviour {

    public GameObject titleObject;

    // Use this for initialization
    void Start () {
        //titleObject.SetActive(Global.titleFlag);

        Global.loadResource();
        Global.loadResource = null;

    }
	
	// Update is called once per frame
	void Update () {
	}

}
