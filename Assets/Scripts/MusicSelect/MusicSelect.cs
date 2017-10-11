using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using GlobalSet;


public class MusicSelect : MonoBehaviour {

    public GameObject dummyTitle;
    public GameObject titleGrid;
    public GameObject selectObject;
    public GameObject bgObject;

    GameObject dummyObject;

    // Use this for initialization
    void Start()
    {
        // 배경 비디오 로드
        if (Global.modeVideoName != null)
        {
            MediaPlayerCtrl videoCache;
            GameObject tmpGO = RSC.GetVideo(Global.modeVideoName[(int)Global.currentSelectMode]) as GameObject;
            if (tmpGO != null)
            {
                videoCache = tmpGO.GetComponent<MediaPlayerCtrl>();
                if (videoCache != null)
                {
                    videoCache.m_TargetMaterial[0] = bgObject;
                    videoCache.Play();
                }
                else
                { Debug.Log("Cannot load videocache."); }
            }
            else
            { Debug.Log("Object not Found"); }
        }
        else
        { Debug.Log("Cannot load videocache."); }

        // 수록곡 정보 로드
        if (Global.musicInfo != null)
        {
            List<MusicInfo> tmpInfoList = Global.musicInfo.Values.ToList();
            for (int i = 0; i < tmpInfoList.Count; ++i)
            {
                MusicInfo tmpInfo = tmpInfoList[i];
                GameObject tmpObject = Instantiate(dummyTitle) as GameObject;
                tmpObject.transform.SetParent(titleGrid.transform);
                tmpObject.transform.localScale = new Vector3(1, 1, 1);
                tmpObject.name = tmpInfo.id.ToString();
                tmpObject.transform.FindChild("Title_Thumb").GetComponent<UI2DSprite>().sprite2D = RSC.GetSprite(tmpInfoList[i].mpngName);
                if (tmpInfo.isLocked != 0)
                {
                    tmpObject.transform.FindChild("Title_Thumb").GetComponent<UI2DSprite>().color = new Color(1, 1, 1, 0.5f);
                    tmpObject.transform.FindChild("Title_Lock").gameObject.SetActive(true);
                }
                //              tmpObject.transform.FindChild("ID Label").GetComponent<UILabel>().text = "";
                tmpObject.transform.FindChild("Title Label").GetComponent<UILabel>().text = tmpInfo.titleName;
                tmpObject.transform.FindChild("Maker Label").GetComponent<UILabel>().text = tmpInfo.makerName;
            }
        }
        titleGrid.GetComponent<UIGrid>().Reposition();
        /*
        if (Global.musicInfo.Count % 2 == 0)
        {
            Vector3 tmpVec = titleGrid.transform.parent.localPosition;
            tmpVec.y = 0f;
            titleGrid.transform.parent.localPosition = tmpVec;
        }*/
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Global.bgmVolume < 1f)
        {
            Global.outGameBGM.volume = Global.bgmVolume = 1f;

            // BGM 클립 변경 (bgm2)
            if (Global.outGameBGM.clip != Global.outGameBGMClip)
            { Global.outGameBGM.clip = Global.outGameBGMClip; }

            Global.outGameBGM.Play();
        }
    }


    public void back()
    {
        RSC.GetVideo(Global.modeVideoName[(int)Global.currentSelectMode]).GetComponent<MediaPlayerCtrl>().Stop();
        SceneManager.LoadScene("ModeSelect");
    }

    public void selectMusic()
    {
        Collider2D col = Physics2D.OverlapPoint(
            new Vector2(selectObject.transform.localPosition.x * selectObject.transform.root.localScale.x,
            selectObject.transform.localPosition.y * selectObject.transform.root.localScale.y));

        if (col != null)
        {
            Global.currentSelectMusic = Int32.Parse(col.name);
            if (Global.musicInfo.ContainsKey(Global.currentSelectMusic))
            {
                Debug.Log(Global.musicInfo[Global.currentSelectMusic].titleName + " Selected.");

                // 미리듣기, 타이틀 파일 로드
                RSC.LoadSprite(Global.songPath + "/" + Global.pngPath + "/" + Global.musicInfo[Global.currentSelectMusic].pngName);
                RSC.LoadAudio(Global.songPath + "/" + Global.prevPath + "/" + Global.musicInfo[Global.currentSelectMusic].prevName);

                // 배경 영상 정지
                RSC.GetVideo(Global.modeVideoName[(int)Global.currentSelectMode]).GetComponent<MediaPlayerCtrl>().Stop();

                SceneManager.LoadScene("Ready");
            }
        }
    }
}
