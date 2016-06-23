using UnityEngine;
using System.Collections;

public class LodingSprite : BaseObj2D
{
    public override void Init(SPR_TYPE _type)
    {
        base.Init(_type);
        //TextureManager.Instance().addArrSprite("LoadingSpr", "loading");
        //m_SprCtr.Set_SpriteList(TextureManager.Instance().loadArrSprite("loading"));
        //m_SprCtr.Set_Frame(11);
        //m_SprCtr.Set_Speed(5);
    }
    public override void Enter()
    {
        Set_Active(true);
        base.Enter();
    }
    public override void Execute()
    {
        base.Execute();
    }
    public override void Exit()
    {
        Set_Active(false);
        base.Exit();
    }
}
