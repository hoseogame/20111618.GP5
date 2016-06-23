using UnityEngine;
using System.Collections;

public class Tile : BaseObj2D
{
    /// <summary>
    /// TILE의 속성 변수
    /// </summary>
    [SerializeField]
    protected TILE_TYPE m_Type;
    // 초기화 함수
    public override void Init(SPR_TYPE _type)
    {
        base.Init(_type);
        m_Type = TILE_TYPE.Base;
        m_SprCtr.Set_SpriteList(TextureManager.Instance().loadSprite("Tile"));
        m_SprCtr.Set_Type(_type);
    }
    // 탈출 함수
    public override void Exit()
    {
        base.Exit();
    }
    // 가지고있는 타입을 반환하는 함수
    public void Set_Type(int _type) { m_Type = (TILE_TYPE)_type; }
    public TILE_TYPE Get_Type()
    {
        return m_Type;
    }
}
