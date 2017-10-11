using System;
using UnityEngine;
using System.Collections.Generic;

namespace GlobalSet
{
    public static class Global
	{
        // 로드 델리게이트
        public delegate void Loading();
        public static Loading loadResource;

        // 경로, 파일명 관련 변수
        public static string[] modeVideoName;
        public static string musicInfoPath, judgeInfoPath, songPath, mp3Path,
            mpngPath, pngPath, ompPath, jamPath, prevPath, bgaPath;

        // 뮤직, 모드, 인게임 관련 변수
        public static SelectMode currentSelectMode = SelectMode.Lite;
        public static Difficulty currentSelectDifficulty = Difficulty.Easy;
        public static int currentSelectMusic;
        public static AudioSource outGameBGM;
        public static AudioClip outGameBGMClip;
        public static AudioClip buttonSound;
        public static AudioClip buttonSound_Start;
        public static AudioClip buttonSound_StartDeny;
        public static AudioClip buttonSound_ItemSelect;
        public static AudioClip buttonSound_ItemDeselect;
        public static float bgmVolume = 1f;
        
        public static ModeInfo currentModeInfo;
        public static Dictionary<int, MusicInfo> musicInfo;
        public static Dictionary<JudgeType, JudgeInfo> judgeInfo;
        public static Dictionary<int, List<NoteManager>> noteManager;

        // 노트 관련 변수
        public static Vector3[] sequencePosition = new Vector3[6];      // 시퀀스존 번호별 위치 (x, y)
        public static float z_PerMeasure = 960f;                        // 노트의 1초당 Z좌표 기본값
        public static float firstNoteDelayTime = 0f;                       // 노트 플레이 딜레이타임
        public static double absoluteTime = 0f;                         // 플레이타임
        public static float secondForComing = 0f;                       // 노트 시퀀스존 도달까지 걸리는 시간(초)
        public static float mp3StartTime = 0f;                          // mp3 시작시간
        public static float mp3EndTime = 0f;                            // mp3 마침시간
        public static float userCustomDelay = 0f;                     // 설정에서 직접 제어 가능한 커스텀 딜레이
        public static int combo = 0;                                    // 인게임 콤보
        public static int score = 0;                                    // 인게임 스코어

        // 판정 디스플레이 관련 변수
        public static float animationScale = 0.3f;      // 판정 애니메이션시 최대크기
        public static float animationTime = 0.2f;       // 판정 애니메이션 타임
        public static float waitingTime = 0.2f;         // 애니메이션 후 대기 타임
        public static float fadeoutTime = 0.6f;         // 대기 후 페이드아웃 타임
        public static float totalDisplayTime = 
            animationTime + waitingTime + fadeoutTime;  // 페이드아웃 후 비활성화 타임
        public static int minDisplayCombo = 4;          // 콤보 표시
        public static int minDisplayComboDigit = 3;     // 최소 콤보 표시 자릿수

        // 기타 변수
        public static bool titleFlag = true;           // 타이틀

    }

    // 모드 번호
    public enum SelectMode
    {
        Lite,
        Regular,
        Mixset,
        Setting,
        end = 4
    };

    // 난이도 번호
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        End = 3
    };

    // 노트 타입 정의
    public enum NoteType
    {
        Normal,
        LongBody,
        LongHead,
        LongTail,
        End = 4
    };

    // 노트 위치 정의
    public enum NotePosition
    {
        Left,
        LeftBottom,
        LeftTop,
        RightTop,
        RightBottom,
        Right,
        End = 6
    };

    // 드래그노트 타입 정의
    public enum DragType
    {
        Normal,
        DragBody1,
        DragHead1,
        DragTail1,
        DragBody2,
        DragHead2,
        DragTail2,
        Empty,
        End = 8
    };

    // 판정 타입 정의
    public enum JudgeType
    {
        Perfect,
        Great,
        Good,
        Miss,
        Long_Perfect,
        End = 5
    };

    // 지불 재화 종류 {
    public enum PaymentType
    {
        Won,
        Dollar,
        End = 2
    };


    public struct MusicInfo
    {
        public int id;                                      // 고유 아이디("id")
        public string titleName;                            // 타이틀 이름("title")
        public string makerName;                            // 아티스트 이름("maker")
        public float bpm;                                   // BPM ("bpm")
        public int easyDiff, normalDiff, hardDiff;          // 각각 난이도별 레벨("ez", "nm", "hd")
        public int sort;                                    // 정렬순서("sort")
        public int isLocked;                                // 락 여부
        public PaymentType pay;                             // 지불 재화 종류
        public double cost;                                 // 가격
        public string ompName, pngName, mpngName;           // omp파일명, 곡이미지 파일명, 썸네일 파일명(omp, png, mpng)
        public string bgaName, prevName;                    // BGA파일명, 미리듣기 파일명(bga, prev)
        public string mp3Name, jamName;                     // mp3파일명, 노트(JAM)파일명
    };

    // 모드 정보
    public struct ModeInfo
    {
        public Difficulty diff;                         // 난이도 ("nDifficulty")
        public float bpm;                               // BPM ("nBPM")
        public int beatSplit;                           // 틱 ("nGrid")
        public int totalMeasure;                        // 총 마디 ("nNumTune")
        public double startDelay;                       // 시작 딜레이 (임시. 현재 사용 안함)
        public Dictionary<int, Dictionary<int, NoteInfo>> noteInfo;    // 6개 배열의 노트 정보 ("O2NoteInfo")
    };

    // 노트 정보
    public struct NoteInfo
    {
        public int measure;         // 마디 ("nTune")
        public float grid;              // 마디 중 노트 위치 ("nGrid")
        public NoteType type;           // 노트 타입 ("nType")
        public DragType drag;           // 드래그노트의 타입 ("nData")
    };

    // 판정 정보
    public struct JudgeInfo
    {
        public int id;
        public string judge_name;
        public int hp;
        public float hpValue;
        public float term;
        public int score;
    };
}