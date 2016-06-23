using UnityEngine;
using System.Collections;

public class Bullet : BaseObj2D
{
    public void Init()
    {
        base.Init(SPR_TYPE.Sprite);
        m_Rigid = GetComponent<Rigidbody2D>();
        Set_Active(false);
    }

    public void Shot(Vector3 pos, Vector3 target, float Force)
    {
        Set_Active(true);
        Set_Pos(pos);
        m_Rigid.AddForce(target.normalized * Force);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.GetComponent<Character>() != null)
        {
            coll.transform.GetComponent<Character>().Hit_Monster_Player(10);
        }

        Set_Active(false);
        BulletManager.Instance().Add_Bullet(this);
        Set_Pos(new Vector3(999999, 99999, 99999));
    }
}
