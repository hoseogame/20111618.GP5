using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginScene : SceneManager
{
    [SerializeField]
    private UnityEngine.UI.InputField m_IdField;
    [SerializeField]
    private UnityEngine.UI.InputField m_PwField;
    [SerializeField]
    private UnityEngine.UI.Button m_LoginBtn;
    [SerializeField]
    private GameObject m_FalseWindow;

    private bool m_LoginFalse;
    
    public override bool Init()
    {
        m_SceneName = "Login";
        SoundManager.Instance().addBGM("Login");
        TestNet.instance();
        return true;
    }

    public override void Enter()
    {
        SoundManager.Instance().playBGM("Login");
        m_FalseWindow.SetActive(false);
        UI_Active(true);
        m_LoginFalse = false;
        m_IdField.text = "";
        m_PwField.text = "";
        base.Enter();
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        m_FalseWindow.SetActive(false);
        UI_Active(true);
        m_LoginFalse = false;
        m_IdField.text = "";
        m_PwField.text = "";
        base.Exit();
    }

    public void Check_ID_PW()
    {
        // 데이터 통신전 임시 체크
        //if (m_IdField.text == "Test1" &&
        //    m_PwField.text == "ejrdms12")
        {
            string temp = string.Format("{0}/{1}", m_IdField.text, m_PwField.text);
            TestNet.instance().beginSend(temp);
            GameManager.Instance().Change_Scene("Loding");
            return;
        }
        //else
        //{
        //    m_LoginFalse = true;
        //    m_FalseWindow.SetActive(true);
        //    UI_Active(false);
        //    return;
        //}
    }

    public void CloseWindow(bool _bool)
    {
        m_LoginFalse = _bool;
        m_FalseWindow.SetActive(false);
        UI_Active(true);
        m_IdField.text = "";
        m_PwField.text = "";
    }

    private void UI_Active(bool _bool)
    {
        m_IdField.enabled = _bool;
        m_PwField.enabled = _bool;
        m_LoginBtn.enabled = _bool;
    }
}
