using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalSet;
using System.Collections;

public class ResultAction : MonoBehaviour {

    public GameObject bgObject, titleObject;

    public GameObject titleLabel, artistLabel;

    MediaPlayerCtrl videoCache;

	// Use this for initialization
	void Start () {

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


        // 타이틀 이미지 로드
        if (Global.musicInfo != null)
        {
            Sprite tmpSpr = RSC.GetSprite(Global.musicInfo[Global.currentSelectMusic].pngName);
            if (tmpSpr != null)
            { titleObject.GetComponent<UI2DSprite>().sprite2D = tmpSpr; }
            else
            { Debug.Log("Cannot Load title image."); }
        }

        // 타이틀, 아티스트 라벨 로드
        titleLabel.GetComponent<UILabel>().text = Global.musicInfo[Global.currentSelectMusic].titleName;
        artistLabel.GetComponent<UILabel>().text = Global.musicInfo[Global.currentSelectMusic].makerName;

    }
	
	// Update is called once per frame
	void Update () {

        if (Global.bgmVolume < 1f)
        {
            Global.bgmVolume = 1f;
            Global.outGameBGM.volume = Global.bgmVolume;
            Global.outGameBGM.Play();
        }
    }

    public void restart()
    {
        // 배경 영상 정지
        RSC.GetVideo(Global.modeVideoName[(int)Global.currentSelectMode]).GetComponent<MediaPlayerCtrl>().Stop();

        SceneManager.LoadScene("Ready");
    }

    public void exit()
    {
        if (Global.musicInfo != null)
        {
            RSC.RemoveAudio(Global.musicInfo[Global.currentSelectMusic].prevName);
            RSC.RemoveSprite(Global.musicInfo[Global.currentSelectMusic].pngName);
        }

        SceneManager.LoadScene("MusicSelect");
    }
}
