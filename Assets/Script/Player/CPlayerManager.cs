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

    CPlayerControl m_Control;
    Animator m_PlayerAnim;

    void Awake()
    {
        m_Control = GetComponent<CPlayerControl>();
        m_PlayerAnim = GetComponent<Animator>();
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

    public void Attack()
    {
        if (isShut) {
            StartCoroutine(ShutWait());
        }
    }
}
