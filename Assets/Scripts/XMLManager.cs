using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using GlobalSet;

public class XMLManager : MonoBehaviour {

	public string fileName;

	// Use this for initialization
	void Start () {
        XML_LoadJudgeData();
        XML_LoadSelectedMusic(fileName);
	}

    void XML_LoadJudgeData()
    {
        string readingNode;
        string readingAttr;

        do
        {
            TextAsset txt = Resources.Load("XML/JudgeInfo") as TextAsset;

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
            for ( int i = 0; i < tmpNodeList.Count; ++i )
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
        { Debug.Log("Failed to reading XML Node : " + readingNode); }

    }

    void XML_LoadSelectedMusic (string fileName) {
        // temporary string
        string readingNode;
        string readingAttr;
        
        do
        {
            // Open XML
			TextAsset txt = Resources.Load("XML/" + fileName) as TextAsset;

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

				if (tmpDiff == Global.currentSelectDifficulty) {
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
                        if ( XmlConvert.ToInt32(tmpTrackList[j].Attributes[readingAttr].Value) != 2 )
                        { continue; }
						Dictionary<int, NoteInfo> tmpTrackInfo = new Dictionary<int, NoteInfo>();
						XmlNodeList tmpNoteList = tmpTrackList[j].ChildNodes;
						for (int k = 0; k < tmpNoteList.Count; ++k)
						{
							// Reading "O2NoteInfo"
							// readingNode = "O2NoteInfo";
							NoteInfo tmpNoteInfo = new NoteInfo();
							readingAttr = "nTune";
							if (tmpNoteList[k].Attributes[readingAttr] == null) { Debug.Log("Attribute not found : " + readingAttr); break; }
							tmpNoteInfo.measure = XmlConvert.ToInt32(tmpNoteList[k].Attributes[readingAttr].Value) - 1;
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
					} // end for j
					Global.currentModeInfo = tmpModeInfo;
				}
            } // end for i
            
            readingNode = null;
        } while (false);
        if (readingNode != null)
        { Debug.Log("Failed to reading XML Node : " + readingNode); }



    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
