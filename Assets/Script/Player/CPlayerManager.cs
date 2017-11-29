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

    public CPlayerState State;

    public Transform m_FollowTag;
    public CWeaponManager m_Weapon;

    CCameraManager m_Camera;
    CMakeRay m_Ray;
    CPlayerControl m_Control;
    CPlayerAnim m_AnimControl;

    int m_MaxHealth = 100;          // 최대 체력

    void Awake()
    {
        m_Control = GetComponent<CPlayerControl>();
        m_AnimControl = GetComponent<CPlayerAnim>();
        m_Ray = GetComponent<CMakeRay>();

        // 최대 체력으로 시작
        SyncHealth = m_MaxHealth;
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            // 자신이 처음 스폰 되거나, 리스폰 되었을 때 카메라가 자신을 따라가게 함
            CGameManager.m_CameraTargetPlayer = this;
        }

        // 캐릭터가 생성되면 게임 매니저의 플레이어 리스트에 넣음
        CGameManager.m_NetworkPlayerList.Add(this);

        m_Weapon.Owner(this);

        // 레이저를 활성화 함
        m_Weapon.SetLaser();
    }

    // 서버->클라 방향으로 데이터를 동기화함
    // 데이터가 서버 기준으로 동기화 됨
    [SyncVar]
    int SyncHealth;

    [SyncVar]
    string SyncTeam;

    [SyncVar]
    string SyncPlayerName;

    [SyncVar]
    bool isDead = false;

    public void SetDecreaseHealth(int _damage)
    {
        if (!isServer) return;
        SyncHealth -= _damage;
    }

    public string GetTeam()
    {
        return SyncTeam;
    }

    void FixedUpdate()
    {

        if (SyncHealth <= 0)
        {
            SyncHealth = 0;
            isDead = true;
        }

        if (isDead)
        {
            this.gameObject.SetActive(false);
        }

        if (!isLocalPlayer) return;

        // 이동 키 입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 이동 상태 변환
        bool isRun = h != 0f || v != 0f;

        // 스텐드 상태
        bool isIdle = h == 0f && v == 0f;

        // 공격 상태 변환
        bool isFire = Input.GetMouseButton(0);

        // 상태 변경
        State.SetState(isFire, isRun, isIdle);

        Quaternion _angle = m_Weapon.GetReaction();


        // 동기화
        CmdSync(State, _angle);

        if (m_Camera != null)
        {
            m_Camera.SetAimMode(m_Ray.GetRayPoint(), transform.position);
            if (Input.GetKey("e"))
            {
                m_Camera.SetRotation(-1);
            }
            else if (Input.GetKey("q"))
            {
                m_Camera.SetRotation(1);
            }
        }
        else
        {
            m_Camera = CGameManager.m_CameraManager;
        }

        // 캐릭터 이동
        Movement(h, v);

        // 캐릭터 회전
        Turning(m_Ray.GetRayPoint());
    }

    public void Movement(float _h, float _v)
    {
        m_Control.Move(_h, _v);
    }

    public void Turning(Vector3 _ray)
    {
        m_Control.Turn(_ray);
    }

    // 캐릭터 셋업
    public void Setup(string _name, string _team)
    {
        State = new CPlayerState();
        SyncPlayerName = _name;
        SyncTeam = _team;

        Debug.Log(_name);
        Debug.Log(_team);

    }

    [Command]
    public void CmdSync(CPlayerState _state, Quaternion _angle)
    {
        if (!isClient)
        {
            State = _state;
            if (State.isFire)
            {
                m_Weapon.SetReaction(_angle);
            }
        }
        RpcSync(_state, _angle);
    }

    [ClientRpc]
    public void RpcSync(CPlayerState _state, Quaternion _angle)
    {
        State = _state;
        if (State.isFire)
        {
            m_Weapon.SetReaction(_angle);
        }
    }
}
