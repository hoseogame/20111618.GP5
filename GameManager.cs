using UnityEngine;
using System.Collections;

public enum SceneNumber { None = -1, Login, GameScene, Loding, }


public class GameManager : MonoBehaviour
{
    // 인스턴스
    private static GameManager m_Instance;
    // Game이 가지고있는 SceneList
    public SceneManager[] m_SceneList;
    // 현재 보여주고있는 Scene
    public SceneManager m_Current;
    // 전 Scene
    public SceneManager m_Prev;

    /// <summary>
    /// Singleton 객체 선언 및 인스턴스 리턴
    /// </summary>
    /// <returns></returns>
    public static GameManager Instance()
    {
        if (m_Instance == null)
        {
            m_Instance =  GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        return m_Instance;
    }
    /// <summary>
    /// 초기화 및 불러오는 작업
    /// </summary>
    void Awake()
    {
        if (!Init())
        {
            Debug.LogError("Init FALSE");
        }
       
        AnimationManager.Instance().StartAnimation();
    }
    /// <summary>
    /// 업데이트
    /// </summary>
    void FixedUpdate()
    {
        if (m_Current == null) return;
        m_Current.Execute();
    }
    /// <summary>
    /// 보유하고 있는 Scene들중 인자값인 번호의 Scene을 활성화 하는 함수
    /// </summary>
    /// <param name="_SceneNumber"></param>
    public void Change_Scene(int _SceneNumber)
    {
        m_Prev = m_Current;
        m_Current = m_SceneList[_SceneNumber];

        m_Prev.Exit();
        m_Current.Enter();
    }
    /// <summary>
    /// 위와 같은 함수지만 SceneNumber 라는 enum값을 이용하여 활성화 하는 함수
    /// </summary>
    /// <param name="_SceneNumber"></param>
    public void Change_Scene(SceneNumber _SceneNumber)
    {
        m_Prev = m_Current;
        m_Current = m_SceneList[(int)_SceneNumber];

        m_Prev.Exit();
        m_Current.Enter();
    }
    public void Change_Scene(string _SceneName)
    {
        m_Prev = m_Current;
        for(int i = 0; i< m_SceneList.Length; i++)
        {
            if(m_SceneList[i].Get_SceneName() == _SceneName)
            {
                m_Current = m_SceneList[i];
            }
        }

        if (m_Current == m_Prev)
        {
            Debug.LogError("None Current Scene");
            return;
        }

        m_Prev.Exit();
        m_Current.Enter();
    }
    /// <summary>
    /// GameManager 및 GameManager가 보유하고있는 Scene들의 초기화함수를 실행
    /// </summary>
    /// <returns>초기화 실패시 false 반환</returns>
    private bool Init()
    {
        Instance();

        for (int i = 0; i < m_SceneList.Length; i++)
        {
            if (!m_SceneList[i].Init()) return false;

            m_SceneList[i].gameObject.SetActive(false);
        }

        m_Current = m_SceneList[(int)SceneNumber.Login];
        m_Current.gameObject.SetActive(true);
        m_Current.Enter();

        return true;
    }
    /// <summary>
    /// 프로그램을 종료하는 함수
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    public void Debug_Log() { Debug.Log("click btn"); }
}