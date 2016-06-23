using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    static CameraController m_Instance;

    public static CameraController Instance()
    {
        return m_Instance;
    }

    private Vector2 velocity;
    public Transform m_Target;

    Vector2 SmoothTime;
    Vector2 m_Pos;

    void Awake()
    {
        m_Instance = this;
    }

    void Update()
    {
        if (m_Target == null) return;

        if (m_Target.position.x >= (DataManager.Instance().Min.x + 8.5f) &&
            m_Target.position.x <= (DataManager.Instance().Max.x - 8.5f))
        {
            m_Pos.x = Mathf.SmoothDamp(transform.position.x, m_Target.position.x, ref velocity.x, SmoothTime.x);
        }
        if (m_Target.position.y >= (DataManager.Instance().Min.y + 4.7f) &&
            m_Target.position.y <= (DataManager.Instance().Max.y - 4.7f))
        {
            m_Pos.y = Mathf.SmoothDamp(transform.position.y, m_Target.position.y, ref velocity.y, SmoothTime.y);
        }

        transform.position = new Vector3(m_Pos.x, m_Pos.y, transform.position.z);
    }

    public void Set_Target(Transform _target)
    {
        m_Target = _target;
    }
    public void Set_Up()
    {
        if (DataManager.Instance().stagenumber == 1 &&
            DataManager.Instance().m_Mode == Stage_MoveMode.Next) { m_Pos.x = 0; m_Pos.y = 0; return; }

        if (DataManager.Instance().CharPos.x < (DataManager.Instance().Min.x + 8.5f))
        {
            m_Pos.x = (DataManager.Instance().Min.x + 8.5f);
        }
        else if (m_Target.position.x > (DataManager.Instance().Max.x - 8.5f))
        {
            m_Pos.x = (DataManager.Instance().Max.x - 8.5f);
        }
        else m_Pos.x = m_Target.position.x;

        if (m_Target.position.y <= (DataManager.Instance().Min.y + 4.7f))
        {
            m_Pos.y = (DataManager.Instance().Min.y + 4.7f);
        }
        else if (m_Target.position.y >= (DataManager.Instance().Max.y - 4.7f))
        {
            m_Pos.y = (DataManager.Instance().Max.y - 4.7f);
        }
        else m_Pos.y = m_Target.position.y;
    }

    public void Set_ZeroZone() { this.transform.position = new Vector3(0, 0, -10); }
}