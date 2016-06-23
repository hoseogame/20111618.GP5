using UnityEngine;
using System.Collections;

public class BaseObj2D : MonoBehaviour
{
    // 객체 활성화 상태 체크
    protected bool m_Active;
    // 2D Rigidbody 
    protected Rigidbody2D m_Rigid;
    // SpriteControll
    protected SpriteControll m_SprCtr;
    // BoxCollider
    protected BoxCollider2D m_Box2D;
    // 객체의 위치를 나타내는 변수
    protected Vector3 pos;
    // 객체의 이동속도를 관리하는 변수
    protected float speed;

    /// <summary>
    /// 객체 초기화하는 가상함수
    /// </summary>
    /// <param name="_type"></param>
    public virtual void Init(SPR_TYPE _type)
    {
        m_SprCtr = this.gameObject.AddComponent<SpriteControll>();
        m_SprCtr.Init(_type);
        Set_Pos(Vector3.zero);
        Set_Speed(.0f);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 초기화, 업데이트, 종료 부분에 실행되는 함수
    /// </summary>
    public virtual void Enter()
    {
        if (m_SprCtr.Get_Type() != SPR_TYPE.Sprite)
        {
            m_SprCtr.Set_Animation(true);
            AnimationManager.Instance().addObject(m_SprCtr);
        }
    }
    public virtual void Execute()
    { }
    public virtual void Exit()
    {
        AnimationManager.Instance().delObject(m_SprCtr);
        this.gameObject.SetActive(false);

    }

    // pos변수의 값을 변경하고 객체를 이동시키거나, pos값을 반환하는 함수
    public void Add_Pos(Vector3 value) { Set_Pos(Get_Pos() + value); }
    public void Set_Pos(Vector3 value) { transform.position = value; pos = transform.position; }
    public Vector3 Get_Pos() { return pos; }
    // 스피드 변수의 값을 설정하고 반환하는 함수
    public void Set_Speed(float value) { speed = value; }
    public float Get_Speed() { return speed; }
    // 객체의 활성화를 변경하거나, 활성화를 알기위해 m_Active값을 리턴하는 함수
    public void Set_Active(bool _Active) { m_Active = _Active; this.gameObject.SetActive(m_Active); }
    public bool Get_Active() { return m_Active; }
}
