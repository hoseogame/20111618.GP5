using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TextureManager : MonoBehaviour
{
    /// <summary>
    ///  TextureManaer 인스턴스
    /// </summary>
    static TextureManager m_Instance;
    /// <summary>
    ///  인스턴스를 가지고있을 GameObj
    /// </summary>
    static GameObject m_InstanceObj;

    //인스턴스를 반환하는 함수
    public static TextureManager Instance()
    {
        if (m_Instance == null)
        {
            m_InstanceObj = new GameObject("TextureManager");
            m_Instance = m_InstanceObj.AddComponent(typeof(TextureManager)) as TextureManager;
            m_Instance.Init();
        }
        return m_Instance;
    }

    // Sprite Type을 가지고있는 Dictionary 객체
    Dictionary<string, Sprite> m_SpriteLoader;
    // Texture Type 가지고있는 Dictionary 객체
    Dictionary<string, Texture> m_TextureLoader;
    // Sprite[] Type을 가지고있는 Dictionary 객체
    Dictionary<string, Sprite[]> m_SpriteArrLoader;
    
    /// <summary>
    /// 초기화 하는 함수
    /// </summary>
    public void Init()
    {
        m_SpriteLoader = new Dictionary<string, Sprite>();
        m_TextureLoader = new Dictionary<string, Texture>();
        m_SpriteArrLoader = new Dictionary<string, Sprite[]>();
    }

    // 위에 세가지타입을 파일경로를 따라서 저장하는 함수
    public void addSprite(string name,string key)
    {
        Sprite temp = Resources.Load<Sprite>(name);
        if (temp != null)
        {
            m_SpriteLoader.Add(key, temp);
        }
    }
    public void addTexture(string name, string key)
    {
        Texture temp = Resources.Load<Texture>(name);
        if (temp != null)
        {
            m_TextureLoader.Add(key, temp);
        }
    }
    public void addArrSprite(string name, string key)
    {
        object[] temp = Resources.LoadAll<Sprite>(name);
        Sprite[] _spr = new Sprite[temp.Length];
        for (int i = 0; i < temp.Length ; i++ )
        {
            _spr[i] = temp[i] as Sprite;
        }

        if (temp != null)
        {
            m_SpriteArrLoader.Add(key, _spr);
        }
    }
    // 위에 세가지타입을 파일경로를 키값 따라서 반환하는 경우
    public Sprite loadSprite(string name)
    {
        if (m_SpriteLoader.ContainsKey(name))
        {
            Sprite tempSpr;
            m_SpriteLoader.TryGetValue(name, out tempSpr);
            return tempSpr;
        }
        else
        {
            Debug.LogError("not Find Sprite File :" + name);
            return null;
        }
    }
    public Texture loadTexture(string name)
    {
        if (m_TextureLoader.ContainsKey(name))
        {
            Texture tempSpr;
            m_TextureLoader.TryGetValue(name, out tempSpr);
            return tempSpr;
        }
        else
        {
            Debug.LogError("not Find Sprite File :" + name);
            return null;
        }
    }
    public Sprite[] loadArrSprite(string name)
    {
        if (m_SpriteArrLoader.ContainsKey(name))
        {
            Sprite[] tempSpr;
            m_SpriteArrLoader.TryGetValue(name, out tempSpr);
            return tempSpr;
        }
        else
        {
            Debug.LogError("not Find Sprite File");
            return null;
        }
    }
    // 리스트에있는 것들 키값을 이용해서 지우는 함수.
    public void delSprite(string name)
    {
        if (m_SpriteLoader.ContainsKey(name))
        {
            m_SpriteLoader.Remove(name);
        }
        else Debug.LogError("not Find Sprite File");
    }
    public void delTexture(string name)
    {
        if (m_TextureLoader.ContainsKey(name))
        {
            m_TextureLoader.Remove(name);
        }
        else Debug.LogError("not Find Texture File");
    }
    public void delArrSprite(string name)
    {
        if (m_SpriteArrLoader.ContainsKey(name))
        {
            m_SpriteArrLoader.Remove(name);
        }
        else Debug.LogError("not Find arrSprite File");
    }
}
