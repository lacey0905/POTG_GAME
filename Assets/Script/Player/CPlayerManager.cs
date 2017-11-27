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

public class CPlayerManager : NetworkBehaviour {

    public CPlayerData Data;                // 캐릭터 정보

    public Transform m_FollowTag;
    public CWeaponManager m_Weapon;

    CCameraManager m_Camera;
    CMakeRay m_Ray;
    CPlayerControl m_Control;
    CPlayerAnim m_AnimControl;

    bool isFire = false;
    float h;
    float v;

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

        // 공격 버튼 입력
        isFire = Input.GetMouseButton(0);

        // 이동 입력
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        bool _isRun = h != 0f || v != 0f;

        CmdSync(isFire, _isRun);
        
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
        Data = new CPlayerData();
        Data.DataSetup(_score, _health, _name);
    }

    [Command]
    public void CmdSync(bool _isFire, bool _isRun)
    {
        if (!isClient)
        {
            m_Weapon.SetAttackMode(_isFire);
            m_AnimControl.SetShooting(_isFire);
            m_AnimControl.SetRunning(_isRun);

        }
        RpcSync(_isFire, _isRun);
    }

    [ClientRpc]
    public void RpcSync(bool _isFire, bool _isRun)
    {
        m_Weapon.SetAttackMode(_isFire);
        m_AnimControl.SetShooting(_isFire);
        m_AnimControl.SetRunning(_isRun);
    }
}
