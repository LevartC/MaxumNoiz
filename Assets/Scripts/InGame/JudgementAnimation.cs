using UnityEngine;
using System.Collections;
using GlobalSet;
using System;
using System.Collections.Generic;

public class JudgementAnimation : MonoBehaviour {

    public GameObject[] judgeObjects;
    public GameObject comboGrid;
    public GameObject dummyNumber;
    public Sprite[] comboNumbers;

    int animationFlag;
    float elapsedTime = 0f;
    JudgeType currentJudgeType;
    Vector3 objectScale;
    UIPanel panel;

    Dictionary<int, GameObject> comboManager;

    // Use this for initialization
    void Start () {
        panel = gameObject.GetComponent<UIPanel>();
        comboManager = new Dictionary<int, GameObject>();

    }

    // Update is called once per frame
    void Update () {
	    switch(animationFlag)
        {
            case 0: // 없음
                break;
            case 1: // 초기화
                {
                    panel.alpha = 1f;
                    elapsedTime = 0;
                    animationFlag = 2;
                    changeJudgeObject();
                    objectScale.x = objectScale.y = objectScale.z = 1;
                    gameObject.transform.localScale = objectScale;
                    if (Global.combo >= Global.minDisplayCombo)
                    {
                        setCombo();
                        comboGrid.transform.parent.gameObject.SetActive(true);
                        comboGrid.GetComponent<UIGrid>().repositionNow = true;
                    }
                    else
                    {
                        comboGrid.transform.parent.gameObject.SetActive(false);
                    }
                }
                break;
            case 2: // 애니메이션
                {
                    elapsedTime += Time.deltaTime;
                    objectScale.x = objectScale.y = 1 + (Mathf.Sin(elapsedTime / Global.animationTime * Mathf.PI) * Global.animationScale);
                    gameObject.transform.localScale = objectScale;

                    if (elapsedTime >= Global.animationTime)
                    { animationFlag = 3; }
                }
                break;
            case 3: // 대기
                {
                    elapsedTime += Time.deltaTime;
                    objectScale.x = objectScale.y = objectScale.z = 1;
                    gameObject.transform.localScale = objectScale;

                    if (elapsedTime >= Global.waitingTime)
                    { elapsedTime = 0f; animationFlag = 4; }
                }
                break;
            case 4: // 소강중
                {
                    elapsedTime += Time.deltaTime;
                    float alpha = 1f - (elapsedTime / Global.fadeoutTime);
                    panel.alpha = alpha;
                    
                    if (elapsedTime >= Global.fadeoutTime)
                    { animationFlag = 5; }
                }
                break;
            case 5: // 완전 소강
                {
                    animationFlag = 0;
                    judgeObjects[(int)currentJudgeType].SetActive(false);
                    comboGrid.transform.parent.gameObject.SetActive(false);
                }
                break;
        }
	}

    public void startJudgeAnimation(JudgeType judge)
    {
        animationFlag = 1;
        currentJudgeType = judge;
        Update();
    }

    void changeJudgeObject()
    {
        for (int i = 0; i < judgeObjects.Length; ++i)
        {
            judgeObjects[i].SetActive(false);
        }
        judgeObjects[(int)currentJudgeType].SetActive(true);
    }

    void setCombo()
    {
        string comboString = Global.combo.ToString().PadLeft(Global.minDisplayComboDigit, '0');
                
        for (int i = comboString.Length; comboManager.ContainsKey(i); ++i)
        {
            Destroy(comboManager[i]);
            comboManager.Remove(i);
        }

        for (int i = 0; i < comboString.Length; ++i)
        { setComboObject(i, Int32.Parse(comboString.Substring(i, 1))); }
    }

    void setComboObject(int index, int comboNum)
    {
        if (comboManager.ContainsKey(index))
        {
            comboManager[index].GetComponent<UI2DSprite>().sprite2D = comboNumbers[comboNum];
        }
        else
        {
            GameObject comboObject = Instantiate(dummyNumber) as GameObject;
            comboObject.transform.SetParent(comboGrid.transform);
            comboObject.transform.localScale = new Vector3(1, 1, 1);
            comboObject.GetComponent<UI2DSprite>().sprite2D = comboNumbers[comboNum];
            comboManager[index] = comboObject;
        }
    }
}