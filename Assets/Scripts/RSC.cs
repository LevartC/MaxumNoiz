using System;
using UnityEngine;
using System.Collections.Generic;

public class RSC {
	static private Dictionary<string, UnityEngine.Object> _cache = new Dictionary<string, UnityEngine.Object>();
    static private Dictionary<string, GameObject> _videoCache = new Dictionary<string, GameObject>();
    static private Dictionary<string, Sprite> _spriteCache = new Dictionary<string, Sprite>();
    static private Dictionary<string, AudioClip> _audioCache = new Dictionary<string, AudioClip>();


    // 불러오기
    static public bool Load(string folder)
    {
        UnityEngine.Object[] t0 = Resources.LoadAll(folder);
        if (t0 != null)
        {
            for (int i = 0; i < t0.Length; i++)
            {
                UnityEngine.Object t1 = (UnityEngine.Object)(t0[i]);
                _cache[t1.name] = t1;
            }
            return true;
        }
        else
        { Debug.Log("File not found at folder : " + folder); return false; }

    }
    static public bool LoadVideoCache(string fileName)
    {
        GameObject tmpObj = new GameObject();
        MonoBehaviour.DontDestroyOnLoad(tmpObj);
        MediaPlayerCtrl tmpCtrl = tmpObj.AddComponent<MediaPlayerCtrl>();
        tmpCtrl.Load(fileName);
        if (tmpCtrl != null)
        {
            tmpCtrl.SetVolume(0f);
            _videoCache[fileName] = tmpObj as GameObject;
            return true;
        }
        else
        {
            UnityEngine.Object.Destroy(tmpObj);
            Debug.Log("File not found : " + fileName);
            return false;
        }
    }
    static public bool LoadSprite(string fileName)
    {
        Sprite tmpSpr = Resources.Load<Sprite>(fileName);
        if (tmpSpr != null)
        { _spriteCache[tmpSpr.name] = tmpSpr; return true; }
        else
        { Debug.Log("File not found : " + fileName); return false; }
    }
    static public bool LoadAudio(string fileName)
    {
        AudioClip tmpAudio = Resources.Load<AudioClip>(fileName);
        if (tmpAudio != null)
        { _audioCache[tmpAudio.name] = tmpAudio; return true; }
        else
        { Debug.Log("File not found : " + fileName); return false; }
    }

    // 꺼내기
    static public UnityEngine.Object Get(string key)
    {
        if (_cache.ContainsKey(key))
        {
            return _cache[key];
        }
        else
        {
            return null;
        }
    }
    static public GameObject GetVideo(string key)
    {
        if (_videoCache.ContainsKey(key))
        {
            return _videoCache[key];
        }
        else
        {
            return null;
        }
    }
    static public Sprite GetSprite(string key)
    {
        if (_spriteCache.ContainsKey(key))
        {
            return _spriteCache[key];
        }
        else
        {
            return null;
        }
    }
    static public AudioClip GetAudio(string key)
    {
        if (_audioCache.ContainsKey(key))
        {
            return _audioCache[key];
        }
        else
        {
            return null;
        }
    }

    // 지우기
    static public bool Remove(string key)
    {
        if (_cache.ContainsKey(key))
        {
            Resources.UnloadAsset(_cache[key]);
            //UnityEngine.Object.Destroy(_cache[key]);
            _cache.Remove(key);
            return true;
        }
        else
        {
            return false;
        }
    }
    static public bool RemoveVideo(string key)
    {
        if (_videoCache.ContainsKey(key))
        {
            //Resources.UnloadAsset(_videoCache[key]);
            UnityEngine.Object.Destroy(_videoCache[key]);
            _videoCache.Remove(key);
            return true;
        }
        else
        {
            return false;
        }
    }
    static public bool RemoveSprite(string key)
    {
        if (_spriteCache.ContainsKey(key))
        {
            Resources.UnloadAsset(_spriteCache[key]);
            //UnityEngine.Object.Destroy(_spriteCache[key]);
            _spriteCache.Remove(key);
            return true;
        }
        else
        {
            return false;
        }
    }
    static public bool RemoveAudio(string key)
    {
        if (_audioCache.ContainsKey(key))
        {
            Resources.UnloadAsset(_audioCache[key]);
            //UnityEngine.Object.Destroy(_audioCache[key]);
            _audioCache.Remove(key);
            return true;
        }
        else
        {
            return false;
        }
    }
}

