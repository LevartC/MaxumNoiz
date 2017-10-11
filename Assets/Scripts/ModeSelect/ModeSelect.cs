using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using GlobalSet;

public class ModeSelect : MonoBehaviour {

	public GameObject denyTarget;
	public GameObject tutorialTarget;
    public GameObject[] selectTarget;

    public GameObject loadingToast;

	// Use this for initialization
	void Start () {

        // 백그라운드 영상 로드

        if (Global.titleFlag)
        {
            Global.titleFlag = false;
            for (int i = 0; i < Global.modeVideoName.Length; ++i)
            { RSC.LoadVideoCache(Global.modeVideoName[i]); }
        }
        MediaPlayerCtrl videoCache;
        for (int i = 0; i < Global.modeVideoName.Length; ++i)
        {
            GameObject tmpGO = RSC.GetVideo(Global.modeVideoName[i]) as GameObject;
            if (tmpGO == null)
            { Debug.Log("Object not Found"); return; }
            videoCache = tmpGO.GetComponent<MediaPlayerCtrl>();

            if (videoCache != null)
            {
                videoCache.m_bLoop = true;
                videoCache.m_bFullScreen = true;
                videoCache.m_bAutoPlay = true;
                Array.Resize(ref videoCache.m_TargetMaterial, 1);
                videoCache.m_TargetMaterial[0] = selectTarget[i];
                videoCache.Play();
            }
            else
            {
                Debug.Log("Cannot load videocache.");
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void selectMode(ref GameObject currentObject)
    {
        for (int i = 0; i < selectTarget.Length; ++i)
        {
            if (selectTarget[i] == currentObject)
            {
                Global.currentSelectMode = (SelectMode)i;
                Debug.Log(Global.currentSelectMode.ToString() + " Selected.");
                if (Global.currentSelectMode != SelectMode.Setting)
                {
                    loadingToast.SetActive(true);
                    changeScene();
                    for (int j = 0; j < Global.modeVideoName.Length; ++j)
                    {
                        MediaPlayerCtrl videoCache = RSC.GetVideo(Global.modeVideoName[j]).GetComponent<MediaPlayerCtrl>();
                        videoCache.Stop();
                    }
                }
                else
                {
                    denyMessage();
                    return;
                }
            }
        }
    }

    void changeScene()
    {
        SceneManager.LoadScene("MusicSelect");
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

	public void tutorialMessageDown()
	{
		Debug.Log ("Request tutorial message down");
		tutorialTarget.SetActive (false);
	}
		

}
