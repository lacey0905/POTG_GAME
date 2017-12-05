using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct CPlayerState
{
    // 캐릭터 상태 값
    public bool isFire;
    public bool isRun;
    public bool isIdle;

    public void SetState(bool _fire, bool _run, bool _idle)
    {
        isFire = _fire;
        isRun = _run;
        isIdle = _idle;
    }
}
public class CPlayerManager : NetworkBehaviour {

    [SerializeField]
    int m_maxHealth = 100;

    [SyncVar]
    public int SyncHealth;

    [SyncVar]
    bool Dead = false;

    [SyncVar]
    public int SyncSpawnIdx = 0;

    public string SyncTeam;

    bool isSpawn = false;

    [SyncVar]
    public string SyncName;

    public int m_MaxBullet = 30;

    [SyncVar]
    public int m_CurBullet = 0;

    public CPlayerUI m_PlayerUI;
    public CGameManager m_Manager;
    public CPlayerState State;                      // 상태 구조체
    public Transform m_FollowTag;                   // 카메라 태그
    public CWeaponManager m_Weapon;                 // 웨폰

    public GameObject m_DieEffect;

    public GameObject m_Blue;
    public GameObject m_Red;

    CCameraManager  m_Camera;
    CPlayerControl  m_Control;                      // 컨트롤러
    CPlayerAnim     m_AnimControl;                  // 애니메이션 컨트롤러

    void Start()
    {
        
    }

    void Awake()
    {
        State = new CPlayerState();
        m_Control = GetComponent<CPlayerControl>();
        m_AnimControl = GetComponent<CPlayerAnim>();
    }

    public void SetCurBullet()
    {
        m_CurBullet--;
    }

    public void ReLoad()
    {
        m_CurBullet = m_MaxBullet;
    }

    public void SetSpawnPoint()
    {
        if (!isServer) return;

        StartCoroutine(SetIndex());
    }

    bool isIdx = true;

    IEnumerator SetIndex()
    {
        while (isIdx)
        {
            if (m_Manager.m_NetworkPlayerList.Count > 0)
            {
                for (int i = 0; i < m_Manager.m_NetworkPlayerList.Count; i++)
                {
                    m_Manager.m_NetworkPlayerList[i].SyncSpawnIdx = i;
                    isIdx = false;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void SetDecreaseHealth(int _damage)
    {
        if (!isServer) return;
        
        SyncHealth -= _damage;
        if (SyncHealth <= 0)
        {
            SyncHealth = 0;
        }
    }

    public void SetHeal(int _heal)
    {
        if (!isServer) return;

        SyncHealth += _heal;
        if (SyncHealth >= 100)
        {
            SyncHealth = 100;
        }
    }

    bool isMissile = false;

    public void MissileDamage(int _damage)
    {
        if (!isMissile)
        {
            isMissile = true;
            StartCoroutine(MissileReset());

            if (!isServer) return;

            SyncHealth -= _damage;

            Debug.Log(_damage);
        }
    }

    IEnumerator MissileReset()
    {
        yield return new WaitForSeconds(2.0f);
        isMissile = false;
    }



    public void Setup()
    {
        m_Manager = CGameManager.s_Manager;

        // 플레이어가 생성되면 리스트에 저장
        m_Manager.AddNetworkPlayer(this);

        SetSpawnPoint();

        // 최대 총알 갯수로 시작
        m_CurBullet = m_MaxBullet;

        // 최대 체력으로 시작
        SyncHealth = m_maxHealth;

        
        m_Weapon.Owner(this);
        m_Camera = m_Manager.GetCameraManager();

        if (isLocalPlayer)
        {
            m_Manager.SetCameraTarget(this);
            m_Manager.SetLocalPlayer(this);
        }
    }

    public void SetSpawn()
    {
        isSpawn = true;
        StartCoroutine(SetAddSpawn());
    }

    IEnumerator SetAddSpawn()
    {
        while (!isSpawn)
        {
            isSpawn = true;
            yield return new WaitForSeconds(1.0f);
        }
        StopCoroutine(SetAddSpawn());
    }


    public void Spawn(string _team)
    {

        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        transform.position = m_Manager.m_SpawnPoint[SyncSpawnIdx].transform.position;

        this.GetComponent<CapsuleCollider>().enabled = true;
        this.GetComponent<Rigidbody>().useGravity = true;
        m_PlayerUI.gameObject.SetActive(true);

        if (isLocalPlayer)
        {
            m_Weapon.SetLaser(true);
        }

        if (_team == "Blue")
        {
            m_Blue.SetActive(true);
            m_AnimControl.SetAvatar(m_Manager.m_AvatarList[0]);
            isSpawn = false;
        }
        else if (_team == "Red")
        {
            m_Red.SetActive(true);
            m_AnimControl.SetAvatar(m_Manager.m_AvatarList[1]);
            isSpawn = false;
        }
    }
    
    bool isStop = true;

    IEnumerator ReSpawn()
    {
        m_Blue.SetActive(false);
        m_Red.SetActive(false);
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<Rigidbody>().useGravity = false;
        m_Weapon.SetLaser(false);
        yield return new WaitForSeconds(5.0f);
        Spawn(GetMyTeam());
        isStop = true;
    }

    public void SetPlayerName(string _name)
    {
        SyncName = _name;
    }

    void FixedUpdate()
    {
        if (m_Manager == null) Setup();

        m_PlayerUI.SetUIHealth(SyncHealth);

        if (SyncName != null)
        {
            m_PlayerUI.SetUIName(SyncName);
        }

        if (SyncHealth <= 0)
        {
            this.gameObject.SetActive(false);
         }

        if (Dead == true)
        {

            //isStop = false;
            //SyncHealth = m_maxHealth;
            //Dead = false;
            //GameObject _die = Instantiate(m_DieEffect, this.transform.position, Quaternion.identity) as GameObject;
            //StartCoroutine(ReSpawn());
        }

        if (!isLocalPlayer) return;

        if (m_Camera != null)
        {
            m_Camera.SetAimMode(m_Camera.GetRayPoint(), transform.position);
            if (Input.GetKey("e"))
            {
                m_Camera.SetRotation(-1);
            }
            else if (Input.GetKey("q"))
            {
                m_Camera.SetRotation(1);
            }
        }

        if(isStop)
        { 
            // 이동 키 입력
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            m_Control.Move(h, v);
            m_Control.Turn(m_Camera.GetRayPoint());

            // 이동 상태 변환
            bool isRun = h != 0f || v != 0f;

            // 스텐드 상태
            bool isIdle = h == 0f && v == 0f;

            // 공격 상태 변환
            bool isFire = Input.GetMouseButton(0);
        
            // 상태 변경
            State.SetState(isFire, isRun, isIdle);
        }

        if (Input.GetMouseButton(1))
        {
            m_Weapon.Missile();
        }

        // 동기화
        CmdSync(State, m_Manager.getTeam(), isSpawn);
        
    }

    public string GetMyTeam()
    {
        return SyncTeam;
    }



    [Command]
    public void CmdSync(CPlayerState _state, string _team, bool _spawn)
    {
        if (!isClient)
        {
            State = _state;
            SyncTeam = _team;

            if (_spawn)
            {
                isSpawn = _spawn;
                Spawn(_team);
            }

        }
        RpcSync(_state, _team, _spawn);
    }

    [ClientRpc]
    public void RpcSync(CPlayerState _state, string _team, bool _spawn)
    {
        State = _state;
        SyncTeam = _team;

        if (_spawn)
        {
            isSpawn = _spawn;
            Spawn(_team);
        }
    }
}
