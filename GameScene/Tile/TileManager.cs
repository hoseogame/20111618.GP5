using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TILE_TYPE { Base = 0, Warp, Trap , Prev, Next};

public class TileManager : EctManager
{
    /// <summary>
    /// Type별 Prefabs 저장한 변수
    /// </summary>
    [SerializeField]
    private GameObject[] m_Tile;
    // 사용된 Tile들을 관리하는 List
    [SerializeField]
    private List<Tile> m_List;
    //
    [SerializeField]
    private List<TrapTile> m_NowTrap;
    // 각 Type별 Tile들을 관리하는 List
    [SerializeField]
    private List<Tile> m_CreateTileList;
    [SerializeField]
    private List<TrapTile> m_CreateTrapList;
    [SerializeField]
    private List<WarpTile> m_CreateWarpList;
    // 기준이 되는 Tile Position
    private Vector3 m_BasePos = new Vector3(-8.582f,-4.69f,0);

    public int count = 0;
    
    /// <summary>
    /// 초기화 함수
    /// </summary>
    /// <returns> 실패시 false 참조</returns>
    public override bool Init()
    {
        m_List = new List<Tile>(0);
        m_CreateTileList = new List<Tile>(0);
        m_CreateTrapList = new List<TrapTile>(0);
        m_CreateWarpList = new List<WarpTile>(0);
        m_NowTrap = new List<TrapTile>(0);
        CreateTileList();
        return base.Init();
    }
    /// <summary>
    /// Scene 진입시 호출되는 함수
    /// </summary>
    public override void Enter()
    {
        MapLoad();
    }
    /// <summary>
    /// 업데이트 루프
    /// </summary>
    public override void Execute()
    {
        for(int i = 0 ; i < m_NowTrap.Count; i++)
        {
            m_NowTrap[i].Execute();
        }
    }
    /// <summary>
    /// Scene 탈출시 호출되는 함수
    /// </summary>
    public override void Exit()
    {
        returnTile();
        base.Exit();
    }
    /// <summary>
    /// Prefab을 이용하여 타일을 생성
    /// </summary>
    private void CreateTileList()
    {
        for (int i = 0; i < 1000; i++)
        {
            GameObject tile = Instantiate(m_Tile[(int)TILE_TYPE.Base]) as GameObject;
            tile.transform.parent = this.transform;
            tile.name = "Tile_" + (i + 1);
            Tile temp = tile.GetComponent<Tile>();
            temp.Set_Active(false);
            m_CreateTileList.Add(temp);
        }
        for (int i = 0; i < 100; i++)
        {
            GameObject tile = Instantiate(m_Tile[(int)TILE_TYPE.Warp]) as GameObject;
            tile.transform.parent = this.transform;
            tile.name = "Warp_" + (i + 1);
            WarpTile temp = tile.GetComponent<WarpTile>();
            temp.Set_Active(false);
            m_CreateWarpList.Add(temp);
        }
        for (int i = 0; i < 20; i++)
        {
            GameObject tile = Instantiate(m_Tile[(int)TILE_TYPE.Trap]) as GameObject;
            tile.transform.parent = this.transform;
            tile.name = "Trap_" + (i + 1);
            TrapTile temp = tile.GetComponent<TrapTile>();
            temp.Set_Active(false);
            m_CreateTrapList.Add(temp);
        }

    }
    /// <summary>
    /// MapData를 가져와서 타일을 설치하는 함수
    /// </summary>
    private void MapLoad()
    {
        string _mapData = DataManager.Instance().Get_MapData();
        DataManager.Instance().Get_MonsterSponPos().Clear();
        string[] _mapDataLine = _mapData.Split('\n');
        for (int i = _mapDataLine.Length - 2; i > 0 ; i--)
        {
            string[] _lineData = _mapDataLine[i].Split(',');
            for (int j = 0; j < _lineData.Length; j++)
            {
                Vector3 pos = new Vector3(m_BasePos.x + (j * 0.62f), m_BasePos.y + (-(i - (_mapDataLine.Length - 2)) * 0.62f), 0);
                DataManager.Instance().InMaxAndMin(pos);
                int tempkey = int.Parse(_lineData[j]);
                if (tempkey != -1 && tempkey != 7)
                {
                    TilePlace(i - (_mapDataLine.Length - 2), j, tempkey);
                    count++;
                }
                else if(tempkey == 7)
                {
                    DataManager.Instance().Set_MonsterSponPos(pos);
                }
            }
        }
    }
    /// <summary>
    /// 실제로 Tile의 배치하는 함수
    /// </summary>
    /// <param name="y"></param>
    /// <param name="x"></param>
    /// <param name="_type"></param>
    private void TilePlace(int y, int x , int _type)
    {
        Vector3 pos = new Vector3(m_BasePos.x + (x * 0.62f), m_BasePos.y + (-y * 0.62f), 0);

        if(DataManager.Instance().m_Mode == Stage_MoveMode.Prev && _type == 4)
        {
            DataManager.Instance().CharPos = pos;
        }
        else if(DataManager.Instance().m_Mode == Stage_MoveMode.Next && _type == 3)
        {
            DataManager.Instance().CharPos = pos;
        }

        switch (_type)
        {
            case 0:
                m_CreateTileList[0].Set_Active(true);
                m_CreateTileList[0].Set_Type(_type);
                m_CreateTileList[0].Set_Pos(pos);
                m_List.Add(m_CreateTileList[0]);
                m_CreateTileList.RemoveAt(0);
                break;
            case 2:
                m_CreateTrapList[0].Set_Active(true);
                m_CreateTrapList[0].Set_Type(_type);
                m_CreateTrapList[0].Set_Pos(pos);
                m_NowTrap.Add(m_CreateTrapList[0]);
                m_List.Add(m_CreateTrapList[0]);
                m_CreateTrapList.RemoveAt(0);
                break;
            case -1:
                break;
            case 1:
            case 4:
            case 3:
                m_CreateWarpList[0].Set_Type(_type);
                m_CreateWarpList[0].Set_Active(true);
                m_CreateWarpList[0].Set_Pos(pos);
                m_List.Add(m_CreateWarpList[0]);
                m_CreateWarpList.RemoveAt(0);
                break;
        }
    }
    /// <summary>
    /// 사용됬던 Tile들을 되돌리고 다음 배치를 준비하는 함수.
    /// </summary>
    private void returnTile()
    {
        count = 0;
        for (int i = 0; i < m_List.Count; i++)
        {
            switch (m_List[i].Get_Type())
            {
                case TILE_TYPE.Base:
                    m_List[i].Set_Active(false);
                    m_CreateTileList.Add(m_List[i]);
                    break;
                case TILE_TYPE.Trap:
                    m_List[i].Set_Active(false);
                    TrapTile temp = m_List[i].GetComponent<TrapTile>();
                    m_CreateTrapList.Add(temp);
                    break;
                case TILE_TYPE.Prev:
                case TILE_TYPE.Next:
                case TILE_TYPE.Warp:
                    m_List[i].Set_Active(false);
                    WarpTile temp2 = m_List[i].GetComponent<WarpTile>();
                    m_CreateWarpList.Add(temp2);
                    break;
            }
        }
        m_NowTrap.Clear();
        m_List.Clear();
    }

    
}