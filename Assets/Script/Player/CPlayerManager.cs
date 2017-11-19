using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct CPlayerData
{
    public int Index;
    public int Health;
    public string Name;

    public void DataSetup(int _index, int _health, string _name)
    {
        this.Index = _index;
        this.Health = _health;
        this.Name = _name;
    }
}

public class CPlayerManager : NetworkBehaviour {

    public CPlayerData Data;                // 캐릭터 정보
    public bool m_IsLocalPlayer;            // 로컬 캐릭터 확인
    public Transform m_FollowTag;
    public CWeaponManager m_Weapon;

    CCameraManager m_CameraControl;


    CPlayerControl m_Control;
    Animator m_PlayerAnim;


    public void SetCameraControl(CCameraManager _camera)
    {

        m_CameraControl = _camera;
    }

    public Vector3 GetRayPoint()
    {
        Vector3 m_RayPoint = Vector3.zero;

        // 마우스 포인터 받기
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 충돌 확인
        RaycastHit floorHit;

        //바닥에 충돌하면 실행
        if (Physics.Raycast(camRay, out floorHit, 100f, m_iFloorMask))
        {
            m_RayPoint = floorHit.point;
        }
        return m_RayPoint;
    }

    [ClientCallback]
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (m_CameraControl != null)
        {
            m_CameraControl.SetPosition(transform.position);
            m_CameraControl.SetAimMode(GetRayPoint(), transform.position);
            if (Input.GetKey("e"))
            {
                m_CameraControl.SetRotation(-1);
            }
            else if (Input.GetKey("q"))
            {
                m_CameraControl.SetRotation(1);
            }
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Movement(h, v);
        Turning(GetRayPoint());
        SetPlayerAnimating(h, v);

        if (Input.GetMouseButton(0))
        {
            CmdAttack();
        }
    }

    // 레이캐스트 충돌 평면
    int m_iFloorMask;

    void Awake()
    {
        m_Control = GetComponent<CPlayerControl>();
        m_PlayerAnim = GetComponent<Animator>();
        m_iFloorMask = LayerMask.GetMask("RayFloor");
    }

    public void Movement(float _h, float _v)
    {
        m_Control.Move(_h, _v);
    }

    public void Turning(Vector3 _ray)
    {
        m_Control.Turn(_ray);
    }

    void Start () {
        if (isLocalPlayer)
        {
            m_IsLocalPlayer = true;
            m_Weapon.SetLaser();
        }
        Setup(1, 100, "ID");
        CGameManager.m_NetworkPlayerList.Add(this);
    }

    public void Setup(int _index, int _health, string _name)
    {
        Data = new CPlayerData();
        Data.DataSetup(_index, _health, _name);
    }

    public void SetPlayerAnimating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        m_PlayerAnim.SetBool("IsWalking", walking);
    }

    IEnumerator ShutWait()
    {
        isShut = false;
        yield return new WaitForSeconds(0.2f);
        m_Weapon.Attack();
        isShut = true;
    }
    bool isShut = true;

    [Command]
    public void CmdAttack()
    {

        if (!isClient)
        {
            if (isShut)
            {
                StartCoroutine(ShutWait());
            }
        }

        RpcFire();
    }

    [ClientRpc]
    public void RpcFire()
    {
        if (isShut)
        {
            StartCoroutine(ShutWait());
        }
    }
}
