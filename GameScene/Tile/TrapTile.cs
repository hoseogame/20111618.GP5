using UnityEngine;
using System.Collections;

public class TrapTile : Tile
{
    public override void Init(SPR_TYPE _type)
    {
        base.Init(_type);
        m_SprCtr.Set_SpriteList(TextureManager.Instance().loadSprite("TrapTile"));
        m_SprCtr.Set_Type(_type);
        m_Type = TILE_TYPE.Trap;
    }
    public override void Exit()
    {
        base.Exit();
    }
}
