using UnityEngine;
using System.Collections;
using System;
using GlobalSet;

public class PlayMovie : MonoBehaviour {
    
    MediaPlayerCtrl videoCache;

	void Start () {
        for (int i = 0; i < Global.modeVideoName.Length; ++i)
        {
            GameObject tmpGO = RSC.Get(Global.modeVideoName[i]) as GameObject;
            if (tmpGO == null)
            { Debug.Log("Object not Found"); return; }
            videoCache = tmpGO.GetComponent<MediaPlayerCtrl>();

            if (videoCache != null)
            {
                videoCache.m_bLoop = true;
                videoCache.m_bFullScreen = true;
                Array.Resize(ref videoCache.m_TargetMaterial, 1);
                videoCache.m_TargetMaterial[0] = gameObject;
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
}
