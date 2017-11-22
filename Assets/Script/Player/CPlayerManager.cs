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

    //[SyncVar]
    public int HP;

    public CPlayerData Data;                // 캐릭터 정보

    public Transform m_FollowTag;
    public CWeaponManager m_Weapon;

    CCameraManager m_Camera;
    CMakeRay m_Ray;
    CPlayerControl m_Control;
    Animator m_PlayerAnim;
    
    void Awake()
    {
        m_Control = GetComponent<CPlayerControl>();
        m_PlayerAnim = GetComponent<Animator>();
        m_Ray = GetComponent<CMakeRay>();
    }

    void Start()
    {
        //if (isLocalPlayer)
        //{
            
        //}
        CGameManager.m_CameraTargetPlayer = this;
        m_Weapon.SetLaser();
        CGameManager.m_NetworkPlayerList.Add(this);


        Setup(0, 100, "Player");
    }

 
    public void SetDecreaseHealth(int _damage)
    {
        //if (!isServer)
        //    return;

        if (HP >= 0)
        {
            HP -= _damage;
        }
        else {
            HP = 0;
        }
    }

    void FixedUpdate()
    {
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }

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

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Movement(h, v);
        Turning(m_Ray.GetRayPoint());
        //SetPlayerAnimating(h, v);

        if (Input.GetMouseButton(0))
        {
            CmdAttack();
        }
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

    //public void SetPlayerAnimating(float h, float v)
    //{
    //    bool walking = h != 0f || v != 0f;
    //    m_PlayerAnim.SetBool("IsWalking", walking);
    //}

    IEnumerator ShutWait()
    {
        isShut = false;
        yield return new WaitForSeconds(0.1f);
        m_Weapon.Attack();
        isShut = true;
    }
    bool isShut = true;

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
