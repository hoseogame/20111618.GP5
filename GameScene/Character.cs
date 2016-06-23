using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum JUMP_STATE { NONE = 0, Up, Down }
public enum MOVE_BUTTON { NONE = -1, LEFT = 0, RIGHT }

public struct ObjectStat
{
    public int level;
    public float hp;
    public float atkPoint;
    public float defPoint;
    public float Skill1;
    public float Skill2;
    public float Skill3;
    public float Skill4;
};

public class Character : BaseObj2D
{
    Scrollbar m_HpBar;
    Scrollbar m_Skill1;
    Scrollbar m_Skill2;
    Scrollbar m_Skill3;
    Scrollbar m_Skill4;
    private float hpVelocity = 0.0F;
    [SerializeField]
    ObjectStat m_Stat;

    [SerializeField]
    JUMP_STATE m_JumpState;
    public MOVE_BUTTON m_MoveBtn;
    float m_fJumpForce = 300.0f;
    public bool moveStage;
    [SerializeField]
    bool m_MonsterHit;
    bool m_HitAlpha;
    float m_HitAlphaValue;
    float m_MonsterHitTime;

    SpriteRenderer m_Render;

    /// <summary>
    /// Character 객체 초기화 및 생성 함수 
    /// </summary>
    /// <param name="_type"></param>
    public override void Init(SPR_TYPE _type)
    {
        //Physics2D.IgnoreLayerCollision(8, 9);
        m_HpBar = GameObject.Find("Heath Bar").GetComponent<UnityEngine.UI.Scrollbar>();
        m_Skill1 = GameObject.Find("Skill1 Bar").GetComponent<UnityEngine.UI.Scrollbar>();
        m_Skill2 = GameObject.Find("Skill2 Bar").GetComponent<UnityEngine.UI.Scrollbar>();
        m_Skill3 = GameObject.Find("Skill3 Bar").GetComponent<UnityEngine.UI.Scrollbar>();
        m_Skill4 = GameObject.Find("Skill4 Bar").GetComponent<UnityEngine.UI.Scrollbar>();
        base.Init(_type);
        TextureManager.Instance().addArrSprite("Character/Character", "char");
        m_Render = GetComponent<SpriteRenderer>();
        m_Box2D = GetComponent<BoxCollider2D>();
        m_Rigid = GetComponent<Rigidbody2D>();
        m_SprCtr.Set_SpriteList(TextureManager.Instance().loadArrSprite("char"));
        m_SprCtr.Set_Frame(3);
        m_SprCtr.Set_Speed(5);
        Set_Speed(5.0f);
        Debug.Log(m_Render.color);
    }
    /// <summary>
    /// Scene 진입 시 실행되는 함수
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        CameraController.Instance().Set_Target(transform);
        m_MoveBtn = MOVE_BUTTON.NONE;
        Set_Active(true);
        m_Stat.hp = 100;
        m_Stat.Skill1 = 100;
        m_Stat.Skill2 = 100;
        m_Stat.Skill3 = 100;
        m_Stat.Skill4 = 100;
        if (DataManager.Instance().stagenumber > 1)
        {
            Set_Pos(DataManager.Instance().CharPos);
        }
        else
        {
            if (DataManager.Instance().m_Mode == Stage_MoveMode.Prev)
            {
                Debug.Log(DataManager.Instance().CharPos);
                Set_Pos(DataManager.Instance().CharPos);
            }
            else Set_Pos(new Vector3(-8, -4, 0));
        }
    }
    /// <summary>
    /// 업데이트 함수
    /// </summary>
    public override void Execute()
    {
        if(m_Stat.hp<= 0.0f)
        {
            GameManager.Instance().Change_Scene("Login");
            return;
        }

        InputKey();
        JumpState();
        Skill_UI_Update();
        if (m_MonsterHit) { Hit_Monster(); }
    }
    /// <summary>
    /// Scene 탈출 시 실행되는 함수
    /// </summary>
    public override void Exit()
    {
        Set_Active(false);
        base.Exit();
    }

    private void InputKey()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || m_MoveBtn == MOVE_BUTTON.LEFT)
        {
            RaycastHit2D hit = Physics2D.Raycast(Get_Pos() - new Vector3(0.23f, 0, 0), Vector2.left, 0.0f);
            if (DataManager.Instance().Min.x <= Get_Pos().x && !(hit.transform != null && hit.transform.tag == "Tile"))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                Set_Pos(transform.position);
            }
            m_SprCtr.Set_Row(2);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || m_MoveBtn == MOVE_BUTTON.RIGHT)
        {
            RaycastHit2D hit = Physics2D.Raycast(Get_Pos() + new Vector3(0.23f, 0, 0), Vector2.right, 0.0f);
            if (DataManager.Instance().Max.x >= Get_Pos().x && !(hit.transform != null && hit.transform.tag == "Tile"))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                Set_Pos(transform.position);
            }
            m_SprCtr.Set_Row(3);
        }
        else m_SprCtr.Set_Row(1);

        if (Input.GetKeyDown(KeyCode.UpArrow)) moveStage = true;
        else moveStage = false;


        if (Input.GetKeyDown(KeyCode.Keypad1))
        { 
            DataManager.Instance().m_Mode = Stage_MoveMode.Prev;
            GameManager.Instance().Change_Scene("Loding");
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            DataManager.Instance().m_Mode = Stage_MoveMode.Next;
            GameManager.Instance().Change_Scene("Loding");
        }
    }
    private void JumpState()
    {
        switch (m_JumpState)
        {
            case JUMP_STATE.Up:
               Set_Pos(transform.position);
                if (m_Rigid.velocity.y < 0) m_JumpState = JUMP_STATE.Down;
                break;
            case JUMP_STATE.Down:
                Set_Pos(transform.position);
                RaycastHit2D hit = Physics2D.Raycast(Get_Pos() - new Vector3(0.20f, 0.38f, 0), Vector2.right, 0.40f);
                if (hit.transform != null && hit.transform.tag == "Tile")
                {
                    m_Box2D.isTrigger = false;
                    m_JumpState = JUMP_STATE.NONE;
                }
                break;
            case JUMP_STATE.NONE:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(Get_Pos() - new Vector3(0.20f, 0.38f, 0), Vector2.right, 0.40f);
                    if (hit2.transform != null && hit2.transform.tag == "Tile")
                    {
                        m_Rigid.AddForce(Vector2.up * m_fJumpForce);
                        m_Box2D.isTrigger = true;
                        m_JumpState = JUMP_STATE.Up;
                    }
                }
                break;
        }
    }

    private void Hit_Monster()
    {
        if (m_MonsterHitTime >= 1.0f)
        {
            m_MonsterHitTime = 0.0f;
            Physics2D.IgnoreLayerCollision(8, 9, false);
            m_MonsterHit = false;
            m_Render.color = new Color(1, 1, 1, 1);
            m_HitAlpha = false;
            m_HitAlphaValue = 1.0f;
        }
        else
        {
            if(m_HitAlpha)
            {
                if (m_HitAlphaValue >= 1.0f)
                {
                    m_HitAlpha = !m_HitAlpha;
                }
                m_HitAlphaValue += 4 * Time.deltaTime;
            }
            else if(!m_HitAlpha)
            {
                if (m_HitAlphaValue <= 0.5f)
                {
                    m_HitAlpha = !m_HitAlpha;
                }
                m_HitAlphaValue -= 4 * Time.deltaTime;
            }
            m_Render.color = new Color(1, 1, 1, m_HitAlphaValue);
            m_MonsterHitTime += Time.fixedDeltaTime * 1.0f;
        }
    }

    private void Skill_UI_Update()
    {
        if(m_Stat.Skill1 < 100.0f)
        {
            float temp = Mathf.SmoothDamp(m_Stat.Skill1, m_Stat.Skill1 + (Time.deltaTime * 20), ref hpVelocity, 1f);
            m_Stat.Skill1 = m_Stat.Skill1 + (Time.deltaTime * 20);
            m_Skill1.size = temp / 100.0f;
        }
        if (m_Stat.Skill2 < 100.0f)
        {
            float temp = Mathf.SmoothDamp(m_Stat.Skill2, m_Stat.Skill2 + (Time.deltaTime * 40), ref hpVelocity, 1f);
            m_Stat.Skill2 = m_Stat.Skill2 + (Time.deltaTime * 40);
            m_Skill2.size = temp / 100.0f;
        }
        if (m_Stat.Skill3 < 100.0f)
        {
            float temp = Mathf.SmoothDamp(m_Stat.Skill3, m_Stat.Skill3 + (Time.deltaTime * 60), ref hpVelocity, 1f);
            m_Stat.Skill3 = m_Stat.Skill3 + (Time.deltaTime * 60);
            m_Skill3.size = temp / 100.0f;
        }
        if (m_Stat.Skill4 < 100.0f)
        {
            float temp = Mathf.SmoothDamp(m_Stat.Skill4, m_Stat.Skill4 + (Time.deltaTime * 80), ref hpVelocity, 1f);
            m_Stat.Skill4 = m_Stat.Skill4 + (Time.deltaTime * 80);
            m_Skill4.size = temp / 100.0f;
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.transform.GetComponent<WarpTile>() != null && moveStage)
        {
            switch (coll.transform.GetComponent<Tile>().Get_Type())
            {
                case TILE_TYPE.Prev:
                    DataManager.Instance().m_Mode = Stage_MoveMode.Prev;
                    GameManager.Instance().Change_Scene("Loding");
                    break;
                case TILE_TYPE.Next:
                    DataManager.Instance().m_Mode = Stage_MoveMode.Next;
                    GameManager.Instance().Change_Scene("Loding");
                    break;
            }
        }
    }

    public void Hit_Monster_Player(int M_Atp)
    {
        m_Stat.hp = m_Stat.hp - M_Atp;
        float temp = Mathf.SmoothDamp(m_Stat.hp, m_Stat.hp - M_Atp, ref hpVelocity, 1f);
        m_HpBar.size = temp / 100.0f;
        m_MonsterHit = true;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        m_Rigid.velocity = new Vector2(0, 0);
    }
    public void Skill_1()
    {
        if(m_Stat.Skill1 >= 100.0f)
        {
            m_Stat.Skill1 = 0.0f;
        }
    }
    public void Skill_2()
    {
        if (m_Stat.Skill2 >= 100.0f)
        {
            m_Stat.Skill2 = 0.0f;
        }
    }
    public void Skill_3()
    {
        if (m_Stat.Skill3 >= 100.0f)
        {
            m_Stat.Skill3 = 0.0f;
        }
    }
    public void Skill_4()
    {
        if (m_Stat.Skill4 >= 100.0f)
        {
            m_Stat.Skill4 = 0.0f;
        }
    }
}
