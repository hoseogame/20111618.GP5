using UnityEngine;
using System.Collections;

public enum Monster_Atd { Left = 0,Right };

public class RangeMonster : Monster
{
    bool m_TargetOn;
    Transform m_TargetT;
    float m_Dist;
    float m_ReAttackTime;
    [SerializeField]
    float m_AtTime;
    [SerializeField]
    float value;
    public override void Init(SPR_TYPE _type)
    {
        m_SprCtr = this.gameObject.AddComponent<SpriteControll>();
        m_SprCtr.Init(_type);
        Set_Pos(Vector3.zero);
        Set_Speed(.0f);
        this.gameObject.SetActive(false);
        m_SprCtr.Set_SpriteList(TextureManager.Instance().loadArrSprite("mon"));
        m_SprCtr.SpriteMode();
        m_SprCtr.Set_Frame(3);
        m_SprCtr.Set_Speed(5);
        m_Dist = 9.0f;
    }

    public override void Enter()
    {
        m_TargetT = GameObject.Find("Character").transform;
        m_ReAttackTime = 1.0f;
        base.Enter();
    }
    public override void Execute()
    {
        value = (m_TargetT.position - transform.position).sqrMagnitude;
        if (m_TargetOn)
        {
            Attack();
        }
        else
        {
            MoveMonster();
            SearchTarget();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


    private void SearchTarget()
    {
        if ((m_TargetT.position - transform.position).sqrMagnitude <= m_Dist)
        {
            m_TargetOn = true;
        }
        else m_AtTime = 0.0f;
    }
    private void Attack()
    {
        if (m_AtTime >= m_ReAttackTime)
        {
            ShotBullet();
        }
        else m_AtTime += Time.deltaTime * 1.0f;
    }
    private void ShotBullet()
    {
        BulletManager.Instance().Get_Bullet(transform.position,
            m_TargetT.position - transform.position,
            350);
        m_AtTime = 0.0f;
        m_TargetOn = false;
    }
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
