using UnityEngine;
using System.Collections;

public enum MonsterMoveState { Idle=0, Left, Right };

public class Monster : BaseObj2D
{
    [SerializeField]
    protected MonsterMoveState m_MoveMode;
    [SerializeField]
    protected MonsterMoveState m_PrevMode;
    protected float m_Time;
    public float m_MoveTime;
    [SerializeField]
    protected bool m_Dead;

    public override void Init(SPR_TYPE _type)
    {
        base.Init(_type);
        m_SprCtr.Set_SpriteList(TextureManager.Instance().loadArrSprite("mon"));
        m_SprCtr.SpriteMode();
        m_SprCtr.Set_Frame(3);
        m_SprCtr.Set_Speed(5);
    }
    public override void Enter()
    {
        base.Enter();
        Set_Active(true);
        m_Dead = true;
        m_MoveMode = MonsterMoveState.Left;
        m_SprCtr.Set_Row(2);
        m_Box2D = GetComponent<BoxCollider2D>();
        m_Rigid = GetComponent<Rigidbody2D>();
        Set_Speed(5.0f);
        m_MoveTime = Random.Range(1.5f, 3.0f);
    }
    public override void Execute()
    {
        MoveMonster();
    }
    public override void Exit()
    {
        Set_Active(false);
        m_Dead = false;
        base.Exit();
    }

    protected void MoveMonster()
    {
        switch(m_MoveMode)
        {
            case MonsterMoveState.Idle:
                Monster_Idle();
                break;
            case MonsterMoveState.Left:
                Monster_Left();
                break;
            case MonsterMoveState.Right:
                Monster_Right();
                break;
        }
    }

    protected void Monster_Idle()
    {
        m_SprCtr.Set_Row(1);
        if (m_Time > 1.0f)
        {
            m_MoveMode = m_PrevMode;
            m_MoveTime = Random.Range(1.5f, 3.0f);
            m_Time = 0;
        }
        else m_Time += Time.deltaTime;
    }
    protected void Monster_Left()
    {
        m_SprCtr.Set_Row(2);
        RaycastHit2D hit = Physics2D.Raycast(Get_Pos() - new Vector3(0.23f, 0, 0), Vector2.left, 0.0f);
        if (hit.transform != null && hit.transform.tag == "Tile")
        {
            m_MoveMode = MonsterMoveState.Right;
        }
        else
        {
            if (m_Time > m_MoveTime)
            {
                m_PrevMode = m_MoveMode;
                m_MoveMode = MonsterMoveState.Idle;
                m_Time = 0;
            }
            m_Time += Time.deltaTime;
            transform.Translate(Vector3.left * speed * UnityEngine.Time.deltaTime);
            Set_Pos(transform.position);
        }
        
    }
    protected void Monster_Right()
    {
        m_SprCtr.Set_Row(3);
        RaycastHit2D hit = Physics2D.Raycast(Get_Pos() + new Vector3(0.23f, 0, 0), Vector2.right, 0.0f);
        if (hit.transform != null && hit.transform.tag == "Tile")
        {
            m_MoveMode = MonsterMoveState.Left;
        }
        else
        {
            if (m_Time > m_MoveTime)
            {
                m_PrevMode = m_MoveMode;
                m_MoveMode = MonsterMoveState.Idle;
                m_Time = 0;
            }
            m_Time += Time.deltaTime;
            transform.Translate(Vector3.right * speed * UnityEngine.Time.deltaTime);
            Set_Pos(transform.position);
        }
        
    }

    public bool Get_Dead() { return m_Dead; }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.GetComponent<Character>() != null)
        {
            m_Rigid.gravityScale = 0.0f;
            m_Box2D.isTrigger = true;
            coll.transform.GetComponent<Character>().Hit_Monster_Player(10);
            m_Rigid.gravityScale = 1.0f;
            m_Box2D.isTrigger = false;
        }
    }
}
