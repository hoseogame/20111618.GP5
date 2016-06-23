using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MONSTER_TYPE { Base = 0, };

public class MonsterManager : EctManager
{
    // MonsterPrefab
    [SerializeField] GameObject[] MonsterPrefab;
    // 사용되고있는 Monster List
    [SerializeField] List<Monster> m_List;
    [SerializeField] List<Monster> m_CreateList;
    [SerializeField]
    List<RangeMonster> m_CreateRList;

    [SerializeField]
    float[] m_SponTime;
    [SerializeField]
    int m_SponCount;
    int m_MaxCount;
    public override bool Init()
    {
        Physics2D.IgnoreLayerCollision(9, 9);
        SpriteLoad();
        CreateMonster();
        return true;
    }

    public override void Enter()
    {
        m_SponTime = new float[DataManager.Instance().Get_MonsterSponPos().Count];
        m_SponCount = DataManager.Instance().Get_MonsterSponPos().Count;
        m_MaxCount = DataManager.Instance().Get_MonsterSponPos().Count;
        for (int i = 0; i < m_SponCount; i++)
        {
            m_SponTime[i] = 0.0f;
        }
    }

    public override void Execute()
    {
        for (int i = 0; i < m_List.Count; i++)
        {
            m_List[i].Execute();
        }
        if (m_List.Count >= m_MaxCount) return;

        for(int j = 0; j < m_SponTime.Length; j++)
        {
            if (m_SponTime[j] <= 0)
            {
                m_SponTime[j] = Random.Range(7.0f, 10.0f);
                m_List.Add(m_CreateRList[0]);
                m_CreateRList[0].Enter();
                m_CreateRList[0].Set_Pos(DataManager.Instance().Get_MonsterSponPos()[j]);
                m_CreateRList.RemoveAt(0);
            }
            else m_SponTime[j] -= 1.0f * Time.deltaTime;
        }
    }
    public override void Exit()
    {
        for(int i = 0; i < m_List.Count; i++)
        {
            m_List[i].Exit();
            m_CreateList.Add(m_List[i]);
        }
        m_List.Clear();
    }

    void CreateMonster()
    {
        m_List = new List<Monster>(0);
        m_CreateList = new List<Monster>(0);
        m_CreateRList = new List<RangeMonster>(0);
        for (int i = 0; i < 40; i++)
        {
            GameObject temp =  Instantiate(MonsterPrefab[0]) as GameObject;
            temp.transform.parent = this.transform;

            Monster mon = temp.GetComponent<Monster>();
            mon.Init(SPR_TYPE.ANIMATION);
            mon.Set_Active(false);
            m_CreateList.Add(mon);
        }
        for (int i = 0; i < 40; i++)
        {
            GameObject temp = Instantiate(MonsterPrefab[1]) as GameObject;
            temp.transform.parent = this.transform;

            RangeMonster mon = temp.GetComponent<RangeMonster>();
            mon.Init(SPR_TYPE.ANIMATION);
            mon.Set_Active(false);
            m_CreateRList.Add(mon);
        }
    }
    void SpriteLoad()
    {
        TextureManager.Instance().addArrSprite("Monster/Monster", "mon");
    }
}
