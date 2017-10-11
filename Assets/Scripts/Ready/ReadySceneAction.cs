using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using GlobalSet;

public class ReadySceneAction : MonoBehaviour {

	public GameObject easyButton;
	public GameObject normalButton;
	public GameObject hardButton;

	UnityEngine.Sprite easyButtonIdle;
	UnityEngine.Sprite normalButtonIdle;
	UnityEngine.Sprite hardButtonIdle;

	public UnityEngine.Sprite easyButtonPush;
	public UnityEngine.Sprite normalButtonPush;
	public UnityEngine.Sprite hardButtonPush;

    public GameObject obj_TitleLock;
    public GameObject obj_ItemTooltip;
    public GameObject obj_UnlockTooltip;

    public GameObject startButton;
    public GameObject itemButtons;

    public GameObject titleLabel;
    public GameObject artistLabel;

    public GameObject titleObject;
    public GameObject bgObject;

    float alpha_Tooltip = 0f;
    float alpha_UnlockTooltip = 5f;

    // Use this for initialization
    void Start ()
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

        // 미리듣기 로드
        AudioClip tmpAudio = RSC.GetAudio(Global.musicInfo[Global.currentSelectMusic].prevName);
        if (tmpAudio != null)
        {
            titleObject.GetComponent<AudioSource>().clip = tmpAudio;
            titleObject.GetComponent<AudioSource>().Play();
        }
        else
        { Debug.Log("Cannot Load audio."); }

        // 난이도 스프라이트 로드
        easyButtonIdle = easyButton.GetComponent<UI2DSprite>().sprite2D;
        normalButtonIdle = normalButton.GetComponent<UI2DSprite>().sprite2D;
        hardButtonIdle = hardButton.GetComponent<UI2DSprite>().sprite2D;
        Global.currentSelectDifficulty = Difficulty.Easy;
        changeDifficulty(Global.currentSelectDifficulty);

        // 타이틀 이미지 로드
        if (Global.musicInfo != null)
        {
            Sprite tmpSpr = RSC.GetSprite(Global.musicInfo[Global.currentSelectMusic].pngName);
            if (tmpSpr != null)
            {
                titleObject.GetComponent<UI2DSprite>().sprite2D = tmpSpr;
                // 곡 락 여부
                if (Global.musicInfo[Global.currentSelectMusic].isLocked != 0)
                {
                    titleObject.GetComponent<UI2DSprite>().color = new Color(1, 1, 1, 0.5f);
                    obj_TitleLock.SetActive(true);
                    obj_UnlockTooltip.SetActive(true);
                }
            }
            else
            { Debug.Log("Cannot Load title image."); }
        }

        // 스타트 버튼, 아이템 버튼 활성화 조정
        if (Global.musicInfo[Global.currentSelectMusic].isLocked != 0)
        {
            startButton.SetActive(false);
            itemButtons.SetActive(false);
        }

        // 타이틀, 아티스트 라벨 로드
        titleLabel.GetComponent<UILabel>().text = Global.musicInfo[Global.currentSelectMusic].titleName;
        artistLabel.GetComponent<UILabel>().text = Global.musicInfo[Global.currentSelectMusic].makerName;

        // BGM 페이드아웃 준비
        Global.bgmVolume = Global.outGameBGM.volume = 1f;

        alpha_Tooltip = 0f;
        alpha_UnlockTooltip = 5f;
    }

    // Update is called once per frame
    void Update () {
        if (Global.bgmVolume >= 0f)
        {
            Global.bgmVolume -= Time.deltaTime * 2f;
            Global.outGameBGM.volume -= Time.deltaTime * 2f;
        }
        else if (Global.bgmVolume < 0f && Global.outGameBGM.volume <= 0f)
        {
            Global.outGameBGM.Stop();
            Global.outGameBGM.volume = 1f;
        }

        if (alpha_Tooltip > 0f)
        {
            alpha_Tooltip -= Time.deltaTime;
            obj_ItemTooltip.GetComponent<UI2DSprite>().alpha = alpha_Tooltip > 1f ? 1f : alpha_Tooltip;
        }
        else if (alpha_Tooltip == 0f)
        {
        }
        else
        {
            alpha_Tooltip = 0f;
            obj_ItemTooltip.SetActive(false);
        }
        if (alpha_UnlockTooltip > 0f)
        {
            alpha_UnlockTooltip -= Time.deltaTime;
            obj_UnlockTooltip.GetComponent<UI2DSprite>().alpha = alpha_UnlockTooltip > 1f ? 1f : alpha_UnlockTooltip;
        }
        else if (alpha_UnlockTooltip == 0f)
        {
        }
        else
        {
            alpha_UnlockTooltip = 0f;
            obj_UnlockTooltip.SetActive(false);
        }
    }

    void changeDifficulty(Difficulty diff) {
		// 난이도 enum을 통해 버튼 토글 결정
		switch (diff) {
		    case Difficulty.Easy:
			    {
				    easyButton.GetComponent<UIButton> ().normalSprite2D = easyButtonPush;
				    normalButton.GetComponent<UIButton> ().normalSprite2D = normalButtonIdle;
				    hardButton.GetComponent<UIButton> ().normalSprite2D = hardButtonIdle;
			    }
			    break;
		    case Difficulty.Normal:
			    {
				    easyButton.GetComponent<UIButton> ().normalSprite2D = easyButtonIdle;
				    normalButton.GetComponent<UIButton> ().normalSprite2D = normalButtonPush;
				    hardButton.GetComponent<UIButton> ().normalSprite2D = hardButtonIdle;
			    }
			    break;
		    case Difficulty.Hard:
			    {
				    easyButton.GetComponent<UIButton> ().normalSprite2D = easyButtonIdle;
				    normalButton.GetComponent<UIButton> ().normalSprite2D = normalButtonIdle;
				    hardButton.GetComponent<UIButton> ().normalSprite2D = hardButtonPush;
			    }
			    break;
		    default:
			    {
                    Debug.Log("Invaild Difficulty");
			    }
			    break;
		}
	}
    
    
    public void selectEasy() {
		Global.currentSelectDifficulty = Difficulty.Easy;
		changeDifficulty (Global.currentSelectDifficulty);
	}
	public void selectNormal() {
		Global.currentSelectDifficulty = Difficulty.Normal;
		changeDifficulty (Global.currentSelectDifficulty);
	}
	public void selectHard() {
		Global.currentSelectDifficulty = Difficulty.Hard;
		changeDifficulty (Global.currentSelectDifficulty);
	}

    public void startIngame()
    {
        loadIngameLoadingData();
        // 배경 영상 정지
        RSC.GetVideo(Global.modeVideoName[(int)Global.currentSelectMode]).GetComponent<MediaPlayerCtrl>().Stop();

        SceneManager.LoadScene("IngameLoading");
    }

    void loadIngameLoadingData()
    {
    }

    public void back()
    {
        // 미리듣기, 타이틀 파일 제거
        RSC.RemoveSprite(Global.musicInfo[Global.currentSelectMusic].pngName);
        RSC.RemoveAudio(Global.musicInfo[Global.currentSelectMusic].prevName);

        // 배경 영상 정지
        RSC.GetVideo(Global.modeVideoName[(int)Global.currentSelectMode]).GetComponent<MediaPlayerCtrl>().Stop();

        // BGM 재생 (중복으로 임시제외)
        //Global.outGameBGM.GetComponent<AudioSource>().Play();

        SceneManager.LoadScene("MusicSelect");
    }

    public void setCurrentTooltip(UnityEngine.Sprite spr)
    {
        alpha_Tooltip = 2.5f;
        obj_ItemTooltip.GetComponent<UI2DSprite>().sprite2D = spr;
        obj_ItemTooltip.SetActive(true);

    }

    public void unlockTooltipDown()
    {
        obj_UnlockTooltip.SetActive(false);
    }

}
