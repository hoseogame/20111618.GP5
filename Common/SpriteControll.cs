using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SPR_TYPE { Sprite = 0, ANIMATION };

public class SpriteControll : MonoBehaviour
{
    // 변경할 Sprite들의 리스트
    [SerializeField]
    private List<Sprite> m_Spr;
    // 실제로 변경될 Renderer
    [SerializeField]
    private SpriteRenderer m_Renderer;
    // 가지고있는 Frame(애니메이션 프레임)
    [SerializeField]
    private int Frame;
    // 가지고있는 Row(상태)
    [SerializeField]
    private int Row;
    // 현재 어떤 Frame이 재생되고있는지 표기
    [SerializeField]
    private float NowFrame;
    // Animation이 재생되는 재생속도
    [SerializeField]
    private float Speed;
    // Animation을 실행할것인지 멈출것인지 나타내는 변수
    [SerializeField]
    private bool m_Ani;
    // sprite 객체에 대해 Animation이 있는 
    // 타입인지 아닌타입인지 인지
    [SerializeField]
    private SPR_TYPE m_Type;
    /// <summary>
    /// 객체 초기화 함수
    /// </summary>
    /// <param name="_type"></param>
    public void Init(SPR_TYPE _type)
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Spr = new List<Sprite>(0);
        Set_Frame(1);
        Set_Speed(1);
        Set_Row(1);
        Set_Type(_type);
    }
    /// <summary>
    /// sprite type을 변경하는 함수
    /// </summary>
    /// <param name="_type"></param>
    public void Set_Type(SPR_TYPE _type)
    {
        m_Type = _type;
        if (m_Spr.Count > 0 && m_Type == SPR_TYPE.Sprite)
        {
            m_Renderer.sprite = m_Spr[0];
        }
    }
    /// <summary>
    /// 애니메이션이 있을 때 재생하는 함수 
    /// </summary>
    public void Execute()
    {
        if (!m_Ani || m_Spr.Count <= 0) { Debug.Log(this.name + "Don't Anima"); return; }
        NowFrame += Speed * Time.fixedDeltaTime;
        m_Renderer.sprite = m_Spr[(int)((NowFrame % Frame) + (Row * Frame))];
    }
    // 재생할 애니메이션이나, 그림을 List에 넣는 함수
    public void Set_SpriteList(Sprite _Spr)
    {
        m_Spr.Clear();
        m_Spr.Add(_Spr);
    }
    public void Set_SpriteList(Sprite[] _Spr)
    {
        m_Spr.Clear();
        for (int i = 0; i < _Spr.Length; i++)
        {
            m_Spr.Add(_Spr[i]);
        }
    }
    public void Set_SpriteList(Sprite[,] _Spr, int x, int y)
    {
        m_Spr.Clear();
        for (int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                m_Spr.Add(_Spr[i, j]);
            }
        }
    }
    public void Set_SpriteList(List<Sprite> _SprList)
    {
        m_Spr.Clear();
        for(int i = 0; i < _SprList.Count; i++)
        {
            m_Spr.Add(_SprList[i]);
        }
    }
    // 각 맴버변수에 대해 설정하는 함수
    public void Set_Animation(bool _bool) { m_Ani = _bool; }
    public void Set_Frame(int _number) { Frame = _number; }
    public void Set_Row(int _number) { Row = _number - 1; }
    public void Set_Speed(float _speed) { Speed = _speed; }

    public void SpriteMode() { m_Renderer.sprite = m_Spr[0]; }

    public SPR_TYPE Get_Type() { return m_Type; }
}
