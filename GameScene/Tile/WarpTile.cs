using UnityEngine;
using System.Collections;

public class WarpTile : Tile
{
    public override void Init(SPR_TYPE _type)
    {
        base.Init(_type);
        m_SprCtr.Set_SpriteList(TextureManager.Instance().loadSprite("WarpTile"));
        m_SprCtr.Set_Type(_type);
        m_Type = TILE_TYPE.Warp;
    }
    public override void Exit()
    {
        base.Exit();
    }
}
