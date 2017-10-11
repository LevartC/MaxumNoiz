using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using GlobalSet;

public class TitleMain : MonoBehaviour {

    float absoluteTime = 0f;

    public GameObject obj_LoadToast;

    public GameObject obj_TouchToStart;
    UI2DSprite spr_TST;
    float alpha_TST = 1f;
    int alphaValue = -1;

    // Use this for initialization
    void Start () {
        GameObject tmpObj = new GameObject();
        DontDestroyOnLoad(tmpObj);
        Global.outGameBGM = tmpObj.AddComponent<AudioSource>();
        RSC.LoadAudio("Sound/bgm1");
        Global.outGameBGM.playOnAwake = false;
        Global.outGameBGM.loop = true;
        Global.outGameBGM.clip = RSC.GetAudio("bgm1");
        Global.outGameBGM.Play();

        // TST 스프라이트 받아오기
        spr_TST = obj_TouchToStart.GetComponent<UI2DSprite>();
    }
	
	// Update is called once per frame
	void Update () {
        absoluteTime += Time.deltaTime;

        if (alpha_TST > 1f)
        {
            alpha_TST = 1f;
            alphaValue = -alphaValue;
        }
        else if (alpha_TST < 0f)
        {
            alpha_TST = 0f;
            alphaValue = -alphaValue;
        }
        else
        {
            alpha_TST += alphaValue * Time.deltaTime;
        }
        spr_TST.alpha = alpha_TST;

    }

	public void nextScene() {
        if (absoluteTime >= 2f)
        {
            obj_LoadToast.SetActive(true);

            Global.loadResource = loadGlobalSetting;
            Global.loadResource += loadMusicInfo;
            Global.loadResource += changeScene;
            
            Global.loadResource();

            //SceneManager.LoadScene("Loading");
        }
	}

    void changeScene()
    {
        SceneManager.LoadScene("ModeSelect");
    }

    void loadGlobalSetting()
    {
        string readingNode;
        string readingAttr;

        do
        {
            TextAsset txt = Resources.Load("XML/GlobalSetting") as TextAsset;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(txt.text);

            readingNode = "GlobalData";
            XmlNode GlobalList = xmldoc.SelectSingleNode(readingNode);
            if (GlobalList == null)
            { Debug.Log("Node not found : " + readingNode); break; }

            XmlNodeList tmpNodeList;

            // 모드 정보 로드
            readingNode = "ModeData";
            tmpNodeList = GlobalList.SelectNodes(readingNode);
            if (tmpNodeList == null || tmpNodeList.Count <= 0)
            { Debug.Log("Node not found : " + readingNode); break; }
            tmpNodeList = tmpNodeList[0].ChildNodes;

            Array.Resize(ref Global.modeVideoName, tmpNodeList.Count);
            for (int i = 0; i < tmpNodeList.Count; ++i)
            {
                readingAttr = "video_name";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                Global.modeVideoName[i] = tmpNodeList[i].Attributes[readingAttr].Value;
            }

            // 경로 정보 로드
            readingNode = "InfoData";
            tmpNodeList = GlobalList.SelectNodes(readingNode);
            if (tmpNodeList == null || tmpNodeList.Count <= 0)
            { Debug.Log("Node not found : " + readingNode); break; }

            XmlNode tmpNode;

            readingNode = "SoundInfo";
            tmpNode = tmpNodeList[0].SelectSingleNode(readingNode);
            string tmpPath;
            readingAttr = "bgm_outgame";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            tmpPath = tmpNode.Attributes[readingAttr].Value;
            Global.outGameBGMClip = Resources.Load<AudioClip>(tmpPath);
            if (Global.outGameBGMClip == null) { Debug.Log("Cannot Load resource : " + readingAttr); break; }
            readingAttr = "button";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            tmpPath = tmpNode.Attributes[readingAttr].Value;
            Global.buttonSound = Resources.Load<AudioClip>(tmpPath);
            if (Global.buttonSound == null) { Debug.Log("Cannot Load resource : " + readingAttr); break; }
            readingAttr = "start_button";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            tmpPath = tmpNode.Attributes[readingAttr].Value;
            Global.buttonSound_Start = Resources.Load<AudioClip>(tmpPath);
            if (Global.buttonSound_Start == null) { Debug.Log("Cannot Load resource : " + readingAttr); break; }
            readingAttr = "start_deny";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            tmpPath = tmpNode.Attributes[readingAttr].Value;
            Global.buttonSound_StartDeny = Resources.Load<AudioClip>(tmpPath);
            if (Global.buttonSound_StartDeny == null) { Debug.Log("Cannot Load resource : " + readingAttr); break; }
            readingAttr = "item_select";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            tmpPath = tmpNode.Attributes[readingAttr].Value;
            Global.buttonSound_ItemSelect = Resources.Load<AudioClip>(tmpPath);
            if (Global.buttonSound_ItemSelect == null) { Debug.Log("Cannot Load resource : " + readingAttr); break; }
            readingAttr = "item_deselect";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            tmpPath = tmpNode.Attributes[readingAttr].Value;
            Global.buttonSound_ItemDeselect = Resources.Load<AudioClip>(tmpPath);
            if (Global.buttonSound_ItemDeselect == null) { Debug.Log("Cannot Load resource : " + readingAttr); break; }

            readingNode = "PathInfo";
            tmpNode = tmpNodeList[0].SelectSingleNode(readingNode);
            if (tmpNode == null)
            { Debug.Log("Node not found : " + readingNode); break; }
            readingAttr = "music";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.musicInfoPath = tmpNode.Attributes[readingAttr].Value;
            readingAttr = "judge";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.judgeInfoPath = tmpNode.Attributes[readingAttr].Value;
            readingAttr = "song";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.songPath = tmpNode.Attributes[readingAttr].Value;
            readingAttr = "mp3";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.mp3Path = tmpNode.Attributes[readingAttr].Value;
            readingAttr = "png";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.pngPath = tmpNode.Attributes[readingAttr].Value;
            readingAttr = "mpng";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.mpngPath = tmpNode.Attributes[readingAttr].Value;
            readingAttr = "jam";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.jamPath = tmpNode.Attributes[readingAttr].Value;
            readingAttr = "bga";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.bgaPath = tmpNode.Attributes[readingAttr].Value;
            readingAttr = "omp";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.ompPath = tmpNode.Attributes[readingAttr].Value;
            readingAttr = "prev";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.prevPath = tmpNode.Attributes[readingAttr].Value;


            // 인게임 정보 로드
            readingNode = "GameData";
            tmpNodeList = GlobalList.SelectNodes(readingNode);
            if (tmpNodeList == null || tmpNodeList.Count <= 0)
            { Debug.Log("Node not found : " + readingNode); break; }

            readingNode = "NoteInfo";
            tmpNode = tmpNodeList[0].SelectSingleNode(readingNode);
            if (tmpNode == null)
            { Debug.Log("Node not found : " + readingNode); break; }
            readingAttr = "coming_sec";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.secondForComing = XmlConvert.ToSingle(tmpNode.Attributes[readingAttr].Value);
            readingAttr = "user_delay";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.userCustomDelay = XmlConvert.ToSingle(tmpNode.Attributes[readingAttr].Value);

            readingNode = "DisplayInfo";
            tmpNode = tmpNodeList[0].SelectSingleNode(readingNode);
            if (tmpNode == null)
            { Debug.Log("Node not found : " + readingNode); break; }
            readingAttr = "anim_time";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.animationTime = XmlConvert.ToSingle(tmpNode.Attributes[readingAttr].Value);
            readingAttr = "anim_scale";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.animationScale = XmlConvert.ToSingle(tmpNode.Attributes[readingAttr].Value);
            readingAttr = "min_combo";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.minDisplayCombo = XmlConvert.ToInt32(tmpNode.Attributes[readingAttr].Value);
            readingAttr = "min_digit";
            if (tmpNode.Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
            Global.minDisplayComboDigit = XmlConvert.ToInt32(tmpNode.Attributes[readingAttr].Value);

            readingNode = null;
        } while (false);

        if (readingNode != null)
        { Debug.Log("Failed to reading XML Node : " + readingNode); return; }
        /*
        // 백그라운드 영상 로드
        for (int i = 0; i < Global.modeVideoName.Length; ++i)
        { RSC.LoadVideoCache(Global.modeVideoName[i]); }
        */
    }

    void loadMusicInfo()
    {
        string readingNode;
        string readingAttr;

        do
        {
            TextAsset txt = Resources.Load(Global.musicInfoPath) as TextAsset;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(txt.text);

            readingNode = "MusicData";
            XmlNodeList tmpNodeList = xmldoc.SelectNodes(readingNode);
            if (tmpNodeList == null || tmpNodeList.Count <= 0)
            {
                Debug.Log("Node not found : " + readingNode);
                break;
            }

            tmpNodeList = tmpNodeList[0].ChildNodes;

            Global.musicInfo = new Dictionary<int, MusicInfo>();
            for (int i = 0; i < tmpNodeList.Count; ++i)
            {
                MusicInfo tmpMusicInfo = new MusicInfo();
                readingAttr = "id";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.id = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "title";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.titleName = tmpNodeList[i].Attributes[readingAttr].Value;
                readingAttr = "maker";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.makerName = tmpNodeList[i].Attributes[readingAttr].Value;
                readingAttr = "bpm";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.bpm = XmlConvert.ToSingle(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "ez";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.easyDiff = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "nm";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.normalDiff = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "hd";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.hardDiff = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "omp";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.ompName = tmpNodeList[i].Attributes[readingAttr].Value;
                readingAttr = "mp3";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.mp3Name = tmpNodeList[i].Attributes[readingAttr].Value;
                readingAttr = "png";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.pngName = tmpNodeList[i].Attributes[readingAttr].Value;
                readingAttr = "mpng";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.mpngName = tmpNodeList[i].Attributes[readingAttr].Value;
                readingAttr = "bga";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.bgaName = tmpNodeList[i].Attributes[readingAttr].Value + ".mp4";
                readingAttr = "jam";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.jamName = tmpNodeList[i].Attributes[readingAttr].Value;
                readingAttr = "prev";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.prevName = tmpNodeList[i].Attributes[readingAttr].Value;
                readingAttr = "sort";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.sort = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "pay";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.pay = (PaymentType)XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "cost";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.cost = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "lock";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpMusicInfo.isLocked = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);

                Global.musicInfo[tmpMusicInfo.id] = tmpMusicInfo;
            }
            readingNode = null;
        } while (false);
        if (readingNode != null)
        { Debug.Log("Failed to reading XML Node : " + readingNode); return; }


        // 썸네일, 타이틀 로드
        List<MusicInfo> tmpMusicInfoList = Global.musicInfo.Values.ToList();
        for (int i = 0; i < tmpMusicInfoList.Count; ++i)
        {
            RSC.LoadSprite(Global.songPath + "/" + Global.mpngPath + "/" + tmpMusicInfoList[i].mpngName);
            RSC.LoadSprite(Global.songPath + "/" + Global.pngPath + "/" + tmpMusicInfoList[i].pngName);
        }

    }
}
