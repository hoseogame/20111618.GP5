using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour
{
    static BulletManager m_Instance;
    // 싱글톤 객체 생성시 필요한 GameObj
    static GameObject m_InstanceObj;
    // 인스턴스를 반환하는 함수
    [SerializeField]
    List<Bullet> m_BulletList;


    public static BulletManager Instance()
    {
        Physics2D.IgnoreLayerCollision(9,11);
        if (m_Instance == null)
        {
            m_InstanceObj = new GameObject("BulletManager");
            m_Instance = m_InstanceObj.AddComponent(typeof(BulletManager)) as BulletManager;
            m_Instance.Init();
        }
        return m_Instance;
    }

    private void Init()
    {
        m_BulletList = new List<Bullet>(0);
        CreateBullet();
    }

    private void CreateBullet()
    {
        GameObject temp = Resources.Load<GameObject>("Prefap/Bullet") as GameObject;
        for(int i = 0; i < 100; i++)
        {
            GameObject temp2 = Instantiate(temp) as GameObject;
            temp2.transform.parent = transform;
            Bullet tempB = temp2.GetComponent<Bullet>();
            tempB.Init();
            m_BulletList.Add(tempB);
        }
    }
    public void Add_Bullet(Bullet temp) { m_BulletList.Add(temp); }

    public void Get_Bullet(Vector3 pos, Vector3 target, float F)
    {
        m_BulletList[0].Shot(pos, target, F);
        m_BulletList.Remove(m_BulletList[0]);
    }
}