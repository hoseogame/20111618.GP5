using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
    // 싱글톤 인스턴스 객체
    static AnimationManager m_Instance;
    // 싱글톤 객체 생성시 필요한 GameObj
    static GameObject m_InstanceObj;
    // Animation을 진행할 List변수
    [SerializeField]
    List<SpriteControll> m_AnimationList;
    // 코루틴 함수 사용 체크용 bool변수
    bool m_Coroutine = true;

    // 인스턴스를 반환하는 함수
    public static AnimationManager Instance()
    {
        if (m_Instance == null)
        {
            m_InstanceObj = new GameObject("AnimationManager");
            m_Instance = m_InstanceObj.AddComponent(typeof(AnimationManager)) as AnimationManager;
            m_Instance.Init();
        }
        return m_Instance;
    }
    // 초기화 하는 함수.
    void Init()
    {
        m_AnimationList = new List<SpriteControll>(0);
        m_Coroutine = true;
    }

    // 애니메이션을 시작해 주는 함수
    public void StartAnimation()
    {
        if (m_Coroutine)
        {
            StartCoroutine("startAnimation");
            m_Coroutine = false;
        }
    }
    // 애니메이션을 멈추는 함수.
    public void StopAnimation()
    {
        if (m_Coroutine)
        {
            StartCoroutine("stopAnimation");
            StopAllCoroutines();
            m_Coroutine = false;
        }
    }
    // 애니메이션을 진행할 객체의 spriteControll을 리스트에 입력
    public void addObject(SpriteControll _obj)
    {
        m_AnimationList.Add(_obj);
    }
    // 가지고있는 spriteControll을 리스트에서 삭제
    public void delObject(SpriteControll _obj)
    {
        m_AnimationList.Remove(_obj);
    }

    // 애니메이션을 시작해주는 변수.
    IEnumerator startAnimation()
    {
        for (int i = 0; i < m_AnimationList.Count; i++)
            m_AnimationList[i].Set_Animation(true);
        yield return StartCoroutine("playAnimation");
    }
    // 애니메이션을 실제로 진행해주는 함수.
    IEnumerator playAnimation()
    {
        while (true)
        {
            for (int i = 0; i < m_AnimationList.Count; i++)
            {
                m_AnimationList[i].Execute();
            }
            yield return new WaitForEndOfFrame();
        }
    }
    // 애니메이션을 멈춰주는 함수.
    IEnumerator stopAnimation()
    {
        for (int i = 0; i < m_AnimationList.Count; i++)
            m_AnimationList[i].Set_Animation(false);
        yield return null;
    }

    public void Set_Coroutine(bool t) { m_Coroutine = t; }
}