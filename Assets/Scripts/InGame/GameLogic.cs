using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using GlobalSet;

public class GameLogic : MonoBehaviour {

    JudgementAnimation judgeAnimation;
    public GameObject judgeObject;
    public GameObject[] touchObjects;
    public GameObject bgObject;

    public GameObject dummyNote;
    public GameObject dummyEffect;
    public Sprite[] dummySprites;

    public Sprite buttonSprite, buttonPushSprite;
    public bool autoplay = false;
    bool audioFlag = false;
    bool[] buttonPushFlag = { false, false, false, false, false, false };

    AudioSource mp3Audio;
    MediaPlayerCtrl videoCache;


    void resetGlobalVariable()
    {
        Global.absoluteTime = 0f;
        Global.combo = 0;
        Global.score = 0;
    }

    // Use this for initialization
    void Start () {

        resetGlobalVariable();

        float secondPerMeasure = 240f / Global.currentModeInfo.bpm;
        
        // 판정 오브젝트
        judgeAnimation = judgeObject.GetComponent<JudgementAnimation>() as JudgementAnimation;
        //judgeObject.SetActive(false);


        if (Global.musicInfo != null)
        {
            // 오디오 로드
            mp3Audio = GetComponent<AudioSource>();
            mp3Audio.clip = RSC.GetAudio(Global.musicInfo[Global.currentSelectMusic].mp3Name);
            if (mp3Audio.clip == null)
            { Debug.Log("Cannot load audio file."); }

            // 배경 비디오 로드
            GameObject tmpGO = RSC.GetVideo(Global.musicInfo[Global.currentSelectMusic].bgaName) as GameObject;
            if (tmpGO != null)
            {
                videoCache = tmpGO.GetComponent<MediaPlayerCtrl>();
                if (videoCache != null)
                {
                    videoCache.m_bLoop = false;
                    videoCache.m_bFullScreen = true;
                    videoCache.m_bAutoPlay = false;
                    Array.Resize(ref videoCache.m_TargetMaterial, 1);
                    videoCache.m_TargetMaterial[0] = bgObject;
                    videoCache.Stop();
                }
                else
                { Debug.Log("Cannot load videocache."); }
            }
            else
            { Debug.Log("Object not Found"); }
        }
        else
        { Debug.Log("Cannot load videocache."); }

        // 노트 오브젝트 로드
        if (Global.currentModeInfo.noteInfo != null)
        {
            Global.noteManager = new Dictionary<int, List<NoteManager>>();
            for (int i = 0; i < Global.currentModeInfo.noteInfo.Count; ++i)
            {
                List<NoteManager> tmpNoteManagerList = new List<NoteManager>();
                for (int j = 0; j < Global.currentModeInfo.noteInfo[i].Count; ++j)
                {
                    GameObject noteObject = Instantiate(dummyNote) as GameObject;
                    NoteManager tmpNoteManager = noteObject.GetComponent<NoteManager>() as NoteManager;
                    tmpNoteManager.noteInfo = Global.currentModeInfo.noteInfo[i][j];
                    noteObject.transform.SetParent(dummyNote.transform.parent);
                    noteObject.transform.localScale = new Vector3(1, 1, 1);
                    switch (tmpNoteManager.noteInfo.type)
                    {
                        case NoteType.Normal:
                            {
                                switch(tmpNoteManager.noteInfo.drag)
                                {
                                    case DragType.Normal:
                                        break;
                                    case DragType.DragBody1: case DragType.DragHead1: case DragType.DragTail1:
                                    case DragType.DragBody2: case DragType.DragHead2: case DragType.DragTail2:
                                        { noteObject.GetComponent<UI2DSprite>().sprite2D = dummySprites[1]; }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case NoteType.LongHead: case NoteType.LongBody: case NoteType.LongTail:
                            { noteObject.GetComponent<UI2DSprite>().sprite2D = dummySprites[2]; }
                            break;
                        default:
                            break;
                    }
                    tmpNoteManager.absoluteTime = Global.firstNoteDelayTime + (secondPerMeasure * tmpNoteManager.noteInfo.measure) + (secondPerMeasure * tmpNoteManager.noteInfo.grid);

                    Vector3 tmpVector = new Vector3();
                    tmpVector.x = Global.sequencePosition[i].x;
                    tmpVector.y = Global.sequencePosition[i].y;
                    tmpVector.z = (float)(tmpNoteManager.absoluteTime * Global.z_PerMeasure);
                    noteObject.transform.localPosition = tmpNoteManager.absolutePosition = tmpVector;

                    //noteObject.SetActive(true);

                    noteObject.GetComponent<UIWidget>().depth = 10000 - j;

                    tmpNoteManagerList.Add(tmpNoteManager);
                }
                Global.noteManager[i] = tmpNoteManagerList;
            }
        }
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        // 절대시간값 추가
        Global.absoluteTime += Time.deltaTime;

        // 오디오 실행
        if (!audioFlag && Global.absoluteTime >= Global.mp3StartTime)
        {
            //gameObject.GetComponent<AudioSource>().Play();
            if (mp3Audio != null)
            { mp3Audio.Play(); }
            if (videoCache != null)
            { videoCache.Play(); }
            audioFlag = true;
        }

        // 노트 이동 & 활성화
        if (Global.noteManager != null)
        {
            for (int i = 0; i < Global.noteManager.Count; ++i)
            {
                for (int j = 0; j < Global.noteManager[i].Count; ++j)
                {
                    Global.noteManager[i][j].absoluteTime -= Time.deltaTime;
                    if (!Global.noteManager[i][j].gameObject.activeSelf && Global.noteManager[i][j].absoluteTime <= Global.secondForComing)
                    {
                        Global.noteManager[i][j].gameObject.SetActive(true);
                    }
                }
            }
        }

        for (int i = 0; i < touchObjects.Length; ++i)
        {
            buttonPushFlag[i] = false;
        }
        // 터치 이벤트 처리
        for ( int i = 0; i < Input.touchCount; ++i )
        {
            Touch touch = Input.GetTouch(i);
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    {
                        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                        Vector2 touchPos = new Vector2(ray.x, ray.y);

                        Collider2D col = Physics2D.OverlapPoint(touchPos);

                        if (col != null)
                        {
                            for (int j = 0; j < touchObjects.Length; ++j)
                            {
                                if (col.gameObject == touchObjects[j] )
                                {
                                    processTouch(j);
                                    buttonPushFlag[j] = true;
                                }
                            }
                        }
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    {
                        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                        Vector2 touchPos = new Vector2(ray.x, ray.y);

                        Collider2D col = Physics2D.OverlapPoint(touchPos);

                        if (col != null)
                        {
                            for (int j = 0; j < touchObjects.Length; ++j)
                            {
                                if (col.gameObject == touchObjects[j] )
                                {
                                    processSpecialnote(j);
                                    buttonPushFlag[j] = true;
                                }
                            }
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    {

                    }
                    break;
                default:
                    break;
            }
        }
        for (int i = 0; i < touchObjects.Length; ++i)
        {
            if (buttonPushFlag[i] && touchObjects[i].GetComponent<UI2DSprite>().sprite2D != buttonPushSprite)
            { touchObjects[i].GetComponent<UI2DSprite>().sprite2D = buttonPushSprite; }
            else if (!buttonPushFlag[i] && touchObjects[i].GetComponent<UI2DSprite>().sprite2D == buttonPushSprite)
            { touchObjects[i].GetComponent<UI2DSprite>().sprite2D = buttonSprite; }
        }


        // Autoplay Only
        if (autoplay == true)
        { autoPlay(); }
        else
        { missJudge(); }

        // 끝나면 리절트 화면으로 이동
        if (Global.absoluteTime >= Global.mp3EndTime)
        {
            mp3Audio.Stop();
            nextScene();
        }
        
	}

    void processTouch(int currentNum)
    {
        for ( int i = 0; i < Global.noteManager[currentNum].Count; ++i)
        {
            NoteManager tmpNM = Global.noteManager[currentNum][i];

            if (tmpNM.absoluteTime >= Global.judgeInfo[JudgeType.Miss].term)
            { break; }

            if (tmpNM.noteInfo.type == NoteType.Normal && tmpNM.noteInfo.drag == DragType.Normal)
            { // 일반 노트
                for (JudgeType j = 0; j < JudgeType.End; ++j)
                {
                    if (tmpNM.absoluteTime <= Global.judgeInfo[j].term
                        && tmpNM.absoluteTime >= -Global.judgeInfo[j].term)
                    {
                        Global.noteManager[currentNum].Remove(tmpNM);
                        tmpNM.destroyObject();
                        addScore(Global.judgeInfo[j].score);
                        switch (j)
                        {
                            case JudgeType.Good:
                                {
                                    setEffect(currentNum);
                                }
                                break;
                            case JudgeType.Miss:
                                {
                                    resetCombo();
                                }
                                break;
                            default:
                                {
                                    setEffect(currentNum);
                                    addCombo(1);
                                }
                                break;
                        }
                        judgeAnimation.startJudgeAnimation(j);

                        return;
                    }
                }
            }
            else
            { // 롱노트, 드래그노트
                if (tmpNM.absoluteTime <= Global.judgeInfo[JudgeType.Long_Perfect].term)
                {
                    Global.noteManager[currentNum].Remove(tmpNM);
                    tmpNM.destroyObject();
                    addCombo(1);
                    if (tmpNM.noteInfo.type >= NoteType.LongBody)
                    { addScore(Global.judgeInfo[JudgeType.Long_Perfect].score); }
                    else if (tmpNM.noteInfo.drag >= DragType.DragBody1)
                    { addScore(Global.judgeInfo[JudgeType.Perfect].score / 2); }
                    setEffect(currentNum);
                    judgeAnimation.startJudgeAnimation(JudgeType.Perfect);
                    continue;
                }
            }
        }
    }

    void processSpecialnote(int currentNum)
    {
        for ( int i = 0; i < Global.noteManager[currentNum].Count; ++i)
        {
            NoteManager tmpNM = Global.noteManager[currentNum][i];

            if (tmpNM.noteInfo.type >= NoteType.LongBody || tmpNM.noteInfo.drag >= DragType.DragBody1)
            {
                if (tmpNM.absoluteTime <= Global.judgeInfo[JudgeType.Long_Perfect].term)
                {
                    Global.noteManager[currentNum].Remove(tmpNM);
                    tmpNM.destroyObject();
                    addCombo(1);
                    if (tmpNM.noteInfo.type >= NoteType.LongBody)
                    { addScore(Global.judgeInfo[JudgeType.Long_Perfect].score); }
                    else if (tmpNM.noteInfo.drag >= DragType.DragBody1)
                    { addScore(Global.judgeInfo[JudgeType.Perfect].score / 2); }
                    setEffect(currentNum);
                    judgeAnimation.startJudgeAnimation(JudgeType.Perfect);
                    continue;
                }
                else
                { break; }
            }
        }
    }

    void addScore(int plusScore)
    { Global.score += plusScore; }

    void resetScore()
    { Global.score = 0; }

    void addCombo(int plusCombo)
    { Global.combo += plusCombo; }

    void resetCombo()
    { Global.combo = 0; }

    void setEffect(int currentNum)
    {
        GameObject tmpEff = Instantiate(dummyEffect) as GameObject;
        tmpEff.SetActive(true);
        tmpEff.transform.SetParent(dummyNote.transform.parent);
        tmpEff.transform.localScale = new Vector3(1, 1, 1);
        tmpEff.transform.localPosition = Global.sequencePosition[currentNum];
    }

    void autoPlay()
    {
        if (Global.noteManager != null)
        {
            for (int i = 0; i < Global.noteManager.Count; ++i)
            {
                for (int j = 0; j < Global.noteManager[i].Count; ++j)
                {
                    if (Global.noteManager[i][j].absoluteTime <= 0f)
                    {
                        NoteManager tmpNM = Global.noteManager[i][j];
                        Global.noteManager[i].Remove(tmpNM);
                        tmpNM.destroyObject();
                        addCombo(1);
                        setEffect(i);
                        judgeAnimation.startJudgeAnimation(JudgeType.Perfect);
                        break;
                    }
                }
            }
        }
    }

    void missJudge()
    {
        if (Global.noteManager != null)
        {
            for (int i = 0; i < Global.noteManager.Count; ++i)
            {
                for (int j = 0; j < Global.noteManager[i].Count; ++j)
                {
                    if (Global.noteManager[i][j].absoluteTime <= -Global.judgeInfo[JudgeType.Miss].term)
                    {
                        NoteManager tmpNM = Global.noteManager[i][j];
                        Global.noteManager[i].Remove(tmpNM);
                        tmpNM.destroyObject();
                        resetCombo();
                        judgeAnimation.startJudgeAnimation(JudgeType.Miss);
                        continue;
                    }
                    else
                    { break; }
                }
            }
        }
    }

    public void back()
    {
        if (Global.musicInfo != null)
        {
            RSC.RemoveVideo(Global.musicInfo[Global.currentSelectMusic].bgaName);
            RSC.RemoveAudio(Global.musicInfo[Global.currentSelectMusic].prevName);
            RSC.RemoveAudio(Global.musicInfo[Global.currentSelectMusic].mp3Name);
            RSC.RemoveSprite(Global.musicInfo[Global.currentSelectMusic].pngName);
        }
        Global.noteManager.Clear();
        Global.currentModeInfo.noteInfo.Clear();

        SceneManager.LoadScene("MusicSelect");
    }

    void nextScene()
    {
        if (Global.musicInfo != null)
        {
            RSC.RemoveVideo(Global.musicInfo[Global.currentSelectMusic].bgaName);
            RSC.RemoveAudio(Global.musicInfo[Global.currentSelectMusic].mp3Name);
        }
        Global.noteManager.Clear();
        Global.currentModeInfo.noteInfo.Clear();

        SceneManager.LoadScene("Result");

    }
}
