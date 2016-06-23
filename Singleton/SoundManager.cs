using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    // 다른 싱글톤객체와 똑같음
    static SoundManager m_Instance;
    static GameObject m_InstanceObj;
    public static SoundManager Instance()
    {
        if (m_Instance == null)
        {
            m_InstanceObj = new GameObject("SoundManager");
            m_Instance = m_InstanceObj.AddComponent(typeof(SoundManager)) as SoundManager;
            m_Instance.Init();
        }
        return m_Instance;
    }

    //BGM List
    Dictionary<string, AudioClip> m_AudioList;
    // Effective List
    Dictionary<string, AudioClip> m_EffectList;
    // 실제 재생을하는 사운드
    AudioSource m_Source;

    // 초기화하는 함수
    void Init()
    {
        m_AudioList = new Dictionary<string, AudioClip>(0);
        m_EffectList = new Dictionary<string, AudioClip>(0);
        m_Source = m_InstanceObj.AddComponent(typeof(AudioSource)) as AudioSource;
    }
    // BGM에 관련된 함수
    public void addBGM(string _nameBGM)
    {
        if (m_AudioList.ContainsKey(_nameBGM)) return;
        else
        {
            string dd = string.Format("{0}{1}", "BGM/", _nameBGM);
            AudioClip temp = Resources.Load(dd) as AudioClip;
            if (temp != null)
            {
                m_AudioList.Add(_nameBGM, temp);
            }
        }
    }
    public void playBGM(string _nameBGM)
    {
        if (m_AudioList.ContainsKey(_nameBGM))
        {
            AudioClip tempSpr;
            m_AudioList.TryGetValue(_nameBGM, out tempSpr);
            m_Source.clip = tempSpr;
            m_Source.loop = true;
            m_Source.Play();
        }
        else
        {
            Debug.LogError("not Find Sound File");
        }
    }
    public void delBGM(string _nameBGM)
    {
        if (m_AudioList.ContainsKey(_nameBGM))
        {
            m_AudioList.Remove(_nameBGM);
        }
        else
        {
            Debug.LogError("not Find Sound File");
        }
    }
    // Effect에 관련된 함수
    public void addEffect(string _nameBGM)
    {
        if (m_AudioList.ContainsKey(_nameBGM)) return;
        else
        {
            string dd = string.Format("{0}{1}", "BGM/", _nameBGM);
            AudioClip temp = Resources.Load(dd) as AudioClip;
            if (temp != null)
            {
                m_AudioList.Add(_nameBGM, temp);
            }
        }
    }
    public void playEffect(string _nameBGM)
    {
        if (m_AudioList.ContainsKey(_nameBGM))
        {
            AudioClip tempSpr;
            m_AudioList.TryGetValue(_nameBGM, out tempSpr);
            m_Source.PlayOneShot(tempSpr);
        }
        else
        {
            Debug.LogError("not Find Sound File");
        }
    }
    public void delEffect(string _nameBGM)
    {
        if (m_AudioList.ContainsKey(_nameBGM))
        {
            m_AudioList.Remove(_nameBGM);
        }
        else
        {
            Debug.LogError("not Find Sound File");
        }
    }
}
