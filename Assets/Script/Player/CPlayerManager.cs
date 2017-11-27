using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class CPlayerManager : MonoBehaviour {

    public CPlayerData Data;                // 캐릭터 정보

    public Transform m_FollowTag;
    public CWeaponManager m_Weapon;

    bool isFire = false;                    // 공격 키 눌렀는지 여부
    bool isShut = true;

    CCameraManager m_Camera;
    CMakeRay m_Ray;
    CPlayerControl m_Control;
    CPlayerAnim m_AnimControl;

    public bool tempLocal = false;

    void Awake()
    {
        m_Control = GetComponent<CPlayerControl>();
        m_AnimControl = GetComponent<CPlayerAnim>();
        m_Ray = GetComponent<CMakeRay>();
    }

    void Start()
    {
        //if (isLocalPlayer)
        //{

        //}

        if (tempLocal)
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
        //if (!isServer)
        //    return;

        Data.Health -= _damage;
    }

    void FixedUpdate()
    {

        if (!tempLocal)
        {
            Debug.Log(Data.Health);
            return;
        }

        // 공격 버튼 입력
        isFire = Input.GetMouseButton(0);

        // 공격 버튼 활성화 시 행동
        if (isFire)
        {
            CmdAttack();
        }
        else {
            m_Weapon.NoneAttack();
        }
      

        // 공격 버튼을 눌렀을 경우 애니메이션
        m_AnimControl.SetShooting(isFire);


        //if (!isLocalPlayer) return;

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

        // 이동 입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 캐릭터 이동
        Movement(h, v);

        // 캐릭터 회전
        Turning(m_Ray.GetRayPoint());
    }

    public void Movement(float _h, float _v)
    {
        bool _isRun = _h != 0f || _v != 0f;
        m_Control.Move(_h, _v);
        m_AnimControl.SetRunning(_isRun);
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

    IEnumerator ShutWait()
    {
        isShut = false;
        yield return new WaitForSeconds(0.1f);
        m_Weapon.Attack();
        isShut = true;
    }
   

    //[Command]
    public void CmdAttack()
    {
        //if (!isClient)
        //{
        //    if (isShut)
        //    {
        //        StartCoroutine(ShutWait());
        //    }
        //}

        //RpcFire();

        if (isShut)
        {
            StartCoroutine(ShutWait());
        }

    }

    //[ClientRpc]
    //public void RpcFire()
    //{
    //    if (isShut)
    //    {
    //        StartCoroutine(ShutWait());
    //    }
    //}
}
