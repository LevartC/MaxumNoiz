using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Collections.Generic;
using GlobalSet;

public class IngameLoading : MonoBehaviour {

    public GameObject titleObject;

    float absoluteTime = 0f;
    bool loadFlag = true;

	// Use this for initialization
	void Start () {
            // 타이틀 이미지 로드
            if (Global.musicInfo != null)
            {
                Sprite tmpSpr = RSC.GetSprite(Global.musicInfo[Global.currentSelectMusic].pngName);
                if (tmpSpr != null)
                { titleObject.GetComponent<UI2DSprite>().sprite2D = tmpSpr; }
                else
                { Debug.Log("Cannot Load title image."); }
            }
    }

    // Update is called once per frame
    void Update () {
        if (loadFlag)
        {
            absoluteTime += Time.deltaTime;
            if (absoluteTime >= 0.5f)
            {
                loadFlag = false;

                // 리소스 로드
                //Global.loadResource();

                // XML, JAM 로드
                XML_LoadJudgeData();
                XML_LoadSelectedMusic();

                // 오디오, 배경 비디오 파일 로드
                RSC.LoadAudio(Global.songPath + "/" + Global.mp3Path + "/" + Global.musicInfo[Global.currentSelectMusic].mp3Name);
                RSC.LoadVideoCache(Global.musicInfo[Global.currentSelectMusic].bgaName);

                // 인게임 설정 로드
                loadIngameSetting();

                // 씬 전환
                SceneManager.LoadScene("InGame");
            }
        }
    }

    void load()
    {
        // 리소스 로드
        //Global.loadResource();
        // XML, JAM 로드
        XML_LoadJudgeData();
        XML_LoadSelectedMusic();

        // 오디오, 배경 비디오 파일 로드
        RSC.LoadAudio(Global.songPath + "/" + Global.mp3Path + "/" + Global.musicInfo[Global.currentSelectMusic].mp3Name);
        RSC.LoadVideoCache(Global.musicInfo[Global.currentSelectMusic].bgaName);

        // 인게임 설정 로드
        loadIngameSetting();

        // 씬 전환
        SceneManager.LoadScene("InGame");
    }

    void loadIngameSetting()
    {
        // 시퀀스존 위치 설정 (임시)
        Global.sequencePosition[0].x = -420f;
        Global.sequencePosition[0].y = 0f;
        Global.sequencePosition[0].z = 0f;
        Global.sequencePosition[1].x = -200f;
        Global.sequencePosition[1].y = -240f;
        Global.sequencePosition[1].z = 0f;
        Global.sequencePosition[2].x = -200f;
        Global.sequencePosition[2].y = 240f;
        Global.sequencePosition[2].z = 0f;
        Global.sequencePosition[3].x = 200f;
        Global.sequencePosition[3].y = 240f;
        Global.sequencePosition[3].z = 0f;
        Global.sequencePosition[4].x = 200f;
        Global.sequencePosition[4].y = -240f;
        Global.sequencePosition[4].z = 0f;
        Global.sequencePosition[5].x = 420f;
        Global.sequencePosition[5].y = 0f;
        Global.sequencePosition[5].z = 0f;

        // 첫 시작시 내려오는 노트의 딜레이 타임 (노트가 도달하는데 걸리는 시간 + 유저 커스텀 딜레이)
        Global.firstNoteDelayTime = Global.secondForComing + Global.userCustomDelay;
    }

    void XML_LoadJudgeData()
    {
        string readingNode;
        string readingAttr;

        do
        {
            TextAsset txt = Resources.Load(Global.judgeInfoPath) as TextAsset;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(txt.text);

            readingNode = "JudgeData";
            XmlNodeList tmpNodeList = xmldoc.SelectNodes(readingNode);
            if (tmpNodeList == null)
            {
                Debug.Log("Node not found : " + readingNode);
                break;
            }

            tmpNodeList = tmpNodeList[0].ChildNodes;

            Global.judgeInfo = new Dictionary<JudgeType, JudgeInfo>();
            for (int i = 0; i < tmpNodeList.Count; ++i)
            {
                JudgeInfo tmpJudgeInfo = new JudgeInfo();
                readingAttr = "idx";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpJudgeInfo.id = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "judge_name";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpJudgeInfo.judge_name = tmpNodeList[i].Attributes[readingAttr].Value;
                readingAttr = "hp";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpJudgeInfo.hp = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "hpvalue";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpJudgeInfo.hpValue = XmlConvert.ToSingle(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "term";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpJudgeInfo.term = XmlConvert.ToSingle(tmpNodeList[i].Attributes[readingAttr].Value);
                readingAttr = "score";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                tmpJudgeInfo.score = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);

                Global.judgeInfo[(JudgeType)i] = tmpJudgeInfo;
            }
            readingNode = null;
        } while (false);
        if (readingNode != null)
        { Debug.Log("Failed to reading Node : " + readingNode); return; }

    }

    void XML_LoadSelectedMusic()
    {
        // temporary string
        string readingNode;
        string readingAttr;

        do
        {
            // Open JAM(XML)
            TextAsset txt = Resources.Load(Global.songPath + "/" + Global.jamPath + "/" + Global.musicInfo[Global.currentSelectMusic].jamName) as TextAsset;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(txt.text);

            readingNode = "JamXML";
            XmlNodeList tmpNodeList = xmldoc.SelectNodes(readingNode);
            if (tmpNodeList == null)
            {
                break;
            }

            readingNode = "O2Mode";
            tmpNodeList = tmpNodeList[0].SelectNodes(readingNode);
            if (tmpNodeList == null)
            {
                break;
            }

            //XmlNode tmpNode = tmpNodeList[0].FirstChild;
            for (int i = 0; i < tmpNodeList.Count; ++i)
            {
                readingAttr = "nDifficuty";
                if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                Difficulty tmpDiff = (Difficulty)XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);

                if (tmpDiff == Global.currentSelectDifficulty)
                {
                    // Reading "O2Mode"
                    ModeInfo tmpModeInfo = new ModeInfo();

                    tmpModeInfo.diff = tmpDiff;
                    readingAttr = "nBPM";
                    if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                    tmpModeInfo.bpm = XmlConvert.ToSingle(tmpNodeList[i].Attributes[readingAttr].Value);
                    readingAttr = "nNumTune";
                    if (tmpNodeList[i].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                    tmpModeInfo.totalMeasure = XmlConvert.ToInt32(tmpNodeList[i].Attributes[readingAttr].Value);
                    readingAttr = "";

                    tmpModeInfo.noteInfo = new Dictionary<int, Dictionary<int, NoteInfo>>();

                    // Reading "O2Track"
                    // readingNode = "O2Track";
                    XmlNodeList tmpTrackList = tmpNodeList[i].ChildNodes;

                    // 노트
                    for (int j = 0; j < tmpTrackList.Count; ++j)
                    {
                        readingAttr = "nType";
                        if (XmlConvert.ToInt32(tmpTrackList[j].Attributes[readingAttr].Value) == 2)
                        {
                            Dictionary<int, NoteInfo> tmpTrackInfo = new Dictionary<int, NoteInfo>();
                            XmlNodeList tmpNoteList = tmpTrackList[j].ChildNodes;
                            for (int k = 0; k < tmpNoteList.Count; ++k)
                            {
                                // Reading "O2NoteInfo"
                                // readingNode = "O2NoteInfo";
                                NoteInfo tmpNoteInfo = new NoteInfo();
                                readingAttr = "nTune";
                                if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                                tmpNoteInfo.measure = XmlConvert.ToInt32(tmpNoteList[k].Attributes[readingAttr].Value);
                                readingAttr = "fGrid";
                                if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                                tmpNoteInfo.grid = XmlConvert.ToSingle(tmpNoteList[k].Attributes[readingAttr].Value);
                                readingAttr = "nType";
                                if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                                tmpNoteInfo.type = (NoteType)XmlConvert.ToInt32(tmpNoteList[k].Attributes[readingAttr].Value);
                                readingAttr = "fData";
                                if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                                tmpNoteInfo.drag = (DragType)XmlConvert.ToInt32(tmpNoteList[k].Attributes[readingAttr].Value);
                                tmpTrackInfo[k] = tmpNoteInfo;
                            } // end for k
                            tmpModeInfo.noteInfo[j] = tmpTrackInfo;
                        }
                        else
                        { // nType이 3이거나 딴놈
                            // mp3 시작지점과 끝지점 계산
                            XmlNodeList tmpNoteList = tmpTrackList[j].ChildNodes;
                            for (int k = 0; k < tmpNoteList.Count; ++k)
                            {
                                readingAttr = "fData";
                                if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                                int data = XmlConvert.ToInt32(tmpNoteList[k].Attributes[readingAttr].Value);

                                if (data == 5)
                                {
                                    float secondPerMeasure = 240f / tmpModeInfo.bpm;
                                    readingAttr = "nTune";
                                    if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                                    int measure = XmlConvert.ToInt32(tmpNoteList[k].Attributes[readingAttr].Value);
                                    readingAttr = "fGrid";
                                    if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                                    float grid = XmlConvert.ToSingle(tmpNoteList[k].Attributes[readingAttr].Value);

                                    Global.mp3StartTime = Global.secondForComing + (secondPerMeasure * measure) + (secondPerMeasure * grid);
                                }
                                else if (data == 8)
                                {
                                    float secondPerMeasure = 240f / tmpModeInfo.bpm;
                                    readingAttr = "nTune";
                                    if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                                    int measure = XmlConvert.ToInt32(tmpNoteList[k].Attributes[readingAttr].Value);
                                    readingAttr = "fGrid";
                                    if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
                                    float grid = XmlConvert.ToSingle(tmpNoteList[k].Attributes[readingAttr].Value);

                                    Global.mp3EndTime = Global.secondForComing + (secondPerMeasure * measure) + (secondPerMeasure * grid);
                                }
                            } // end for k
                        }
                    } // end for j
                    Global.currentModeInfo = tmpModeInfo;
                }
            } // end for i

            readingNode = null;
        } while (false);
        if (readingNode != null)
        { Debug.Log("Failed to reading  Node : " + readingNode); return; }
    }

}
