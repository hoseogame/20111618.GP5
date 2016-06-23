using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    protected string m_SceneName;
    public EctManager[] m_ObjManagers;

    public virtual bool Init() { return true; }
    public virtual void Enter() { this.gameObject.SetActive(true); Debug.Log(this.gameObject.name + " Enter()"); }
    public virtual void Execute() { }
    public virtual void Exit() { this.gameObject.SetActive(false); Debug.Log(this.gameObject.name + " Exit()"); }

    public string Get_SceneName() { return m_SceneName; }
}
