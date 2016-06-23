using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Stage_MoveMode { None, Prev, Next };

public class DataManager : MonoBehaviour
{
    // 다른 싱글톤 객체와 같음
    static DataManager m_Instance;
    static GameObject m_InstanceObj;
    public static DataManager Instance()
    {
        if (m_Instance == null)
        {
            m_InstanceObj = new GameObject("DataManager");
            m_Instance = m_InstanceObj.AddComponent(typeof(DataManager)) as DataManager;
            m_Instance.Init();
        }
        return m_Instance;
    }

    // X Min, Max & Y Min, Max]
    public Vector2 Max = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
    public Vector2 Min = new Vector2(Mathf.Infinity, Mathf.Infinity);
    public Vector3 CharPos;

    public int stagenumber;
    public Stage_MoveMode m_Mode;
    private List<Vector3> m_SponPos;

    //초기화 하는 함수
    void Init()
    {
        m_Mode = Stage_MoveMode.None;
        stagenumber = 0;
        m_SponPos = new List<Vector3>(0);
    }
    // fileName의 Stage를 찾아 맵데이터를 반환하는 함수
    public string Get_MapData()
    {
        string temp = string.Format("{0}{1}", "StageData/", stagenumber);
        TextAsset _csvFile = Resources.Load<TextAsset>(temp) as TextAsset;
        string resultData = _csvFile.text;
        return resultData;
    }

    public void InMaxAndMin(Vector2 _pos)
    {
        if (Max.x < _pos.x) Max.x = _pos.x;
        if (Max.y < _pos.y) Max.y = _pos.y;
        if (Min.x > _pos.x) Min.x = _pos.x;
        if (Min.y > _pos.y) Min.y = _pos.y;
    }
    public void ResetMaxMin()
    {
        Max = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
        Min = new Vector2(Mathf.Infinity, Mathf.Infinity);
    }

    public void Set_MonsterSponPos(Vector3 _pos)
    {
        m_SponPos.Add(_pos);
    }
    public List<Vector3> Get_MonsterSponPos()
    {
        return m_SponPos;
    }
}