using UnityEngine;
using System.Collections;

public class GameScene : SceneManager
{
    [SerializeField]
    private GameObject m_PauseWindow;
    [SerializeField]
    private UnityEngine.UI.Button m_PauseBtn;

    [SerializeField]
    private BaseObj2D m_Char;

    public bool m_Pause;

    public override bool Init()
    {
        BulletManager.Instance();
        m_Char.Init(SPR_TYPE.ANIMATION);
        m_SceneName = "Game";
        for (int i = 0; i < m_ObjManagers.Length; i++)
        {
            m_ObjManagers[i].Init();
        }
        DataManager.Instance().m_Mode = Stage_MoveMode.Next;
        return true;
    }

    public override void Enter()
    {
        m_Pause = false;
        m_PauseWindow.SetActive(m_Pause);
        for (int i = 0; i < m_ObjManagers.Length; i++)
        {
            m_ObjManagers[i].Enter();
        }
        m_Char.Enter();
        base.Enter();
        CameraController.Instance().Set_Up();
        DataManager.Instance().m_Mode = Stage_MoveMode.None;
    }

    public override void Execute()
    {
        if (m_Pause)    return;
        
        m_Char.Execute();
        m_ObjManagers[1].Execute();
    }

    public override void Exit()
    {
        m_Pause = false;
        m_PauseWindow.SetActive(m_Pause);
        for (int i = 0; i < m_ObjManagers.Length; i++)
        {
            m_ObjManagers[i].Exit();
        }
        m_Char.Exit();
        DataManager.Instance().ResetMaxMin();
        base.Exit();
    }

    public void PauseWindow()
    {
        m_PauseBtn.enabled = m_Pause;
        m_Pause = !m_Pause;
        m_PauseWindow.SetActive(m_Pause);
        if (m_Pause)
        {
            AnimationManager.Instance().Set_Coroutine(true);
            AnimationManager.Instance().StopAnimation();
        }
        else
        {
            AnimationManager.Instance().Set_Coroutine(true);
            AnimationManager.Instance().StartAnimation();
        }
    }
}
