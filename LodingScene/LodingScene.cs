using UnityEngine;
using System.Collections;

public class LodingScene : SceneManager
{
    [SerializeField]
    BaseObj2D m_Object;
    float m_Time;
    public override bool Init()
    {
        m_SceneName = "Loding";
        m_Object.Init(SPR_TYPE.Sprite);
        return true;
    }

    public override void Enter()
    {
        CameraController.Instance().Set_ZeroZone();
        base.Enter();
        m_Object.Enter();
        if (DataManager.Instance().m_Mode == Stage_MoveMode.Next)
        {
            DataManager.Instance().stagenumber++;
        }
        else if(DataManager.Instance().m_Mode == Stage_MoveMode.Prev)
        {
            DataManager.Instance().stagenumber--;
        }
    }
    public override void Execute()
    {
        m_Object.Execute();
        m_Time += Time.deltaTime;
        if (m_Time > 2.5f)
        {
            GameManager.Instance().Change_Scene("Game");
        }
    }
    public override void Exit()
    {
        Debug.Log("Loding Exit");
        m_Object.Exit();
        m_Time = 0.0f;
        base.Exit();
    }
}
