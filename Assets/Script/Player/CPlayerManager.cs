using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct CPlayerData
{
    public int Score;
    public int Health;
    public string Name;

    public void DataSetup(int _score, int _health, string _name)
    {
        this.Score = _score;
        this.Health = _health;
        this.Name = _name;
    }
}

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

    public CPlayerData Data;                // 캐릭터 정보
    public CPlayerState State;

    public Transform m_FollowTag;
    public CWeaponManager m_Weapon;

    CCameraManager m_Camera;
    CMakeRay m_Ray;
    CPlayerControl m_Control;
    CPlayerAnim m_AnimControl;


    void Awake()
    {
        m_Control = GetComponent<CPlayerControl>();
        m_AnimControl = GetComponent<CPlayerAnim>();
        m_Ray = GetComponent<CMakeRay>();
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

        Setup(0, 100, "Player");

    }
 
    public void SetDecreaseHealth(int _damage)
    {
        if (!isServer) return;
        Data.Health -= _damage;
    }

    void FixedUpdate()
    {

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

        // 무기 레이 생성
        m_Weapon.MakeHitTarget();

        // 동기화
        CmdSync(State, m_Weapon.GetHitTarget(), m_Weapon.GetHitPoint());









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

    public void Setup(int _score, int _health, string _name)
    {
        State = new CPlayerState();
        Data = new CPlayerData();
        Data.DataSetup(_score, _health, _name);
    }

    [Command]
    public void CmdSync(CPlayerState _state, GameObject _hitTarget, Vector3 _hitPoint)
    {
        if (!isClient)
        {
            State = _state;
            if (State.isFire)
            {
                m_Weapon.Attack(_hitTarget, _hitPoint);
            }
        }
        RpcSync(_state, _hitTarget, _hitPoint);
    }

    [ClientRpc]
    public void RpcSync(CPlayerState _state, GameObject _hitTarget, Vector3 _hitPoint)
    {
        State = _state;
        if (State.isFire)
        {
            m_Weapon.Attack(_hitTarget, _hitPoint);
        }
    }
}
