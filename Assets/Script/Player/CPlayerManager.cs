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
    string SyncName;

    [SyncVar]
    string SyncTeam;

    [SyncVar]
    int SyncHealth;

    [SyncVar]
    public int SyncSpawnIdx;

    public CPlayerState State;                      // 상태 구조체
    public Transform m_FollowTag;                   // 카메라 태그
    public CWeaponManager m_Weapon;                 // 웨폰

    CCameraManager  m_Camera;
    CPlayerControl  m_Control;                      // 컨트롤러
    CPlayerAnim     m_AnimControl;                  // 애니메이션 컨트롤러
    
    void Awake()
    {
        State = new CPlayerState();
        m_Control = GetComponent<CPlayerControl>();
        m_AnimControl = GetComponent<CPlayerAnim>();
    }
    
    void Start()
    {
        // 최대 체력으로 시작
        SyncHealth = m_maxHealth;

        // 플레이어가 생성되면 리스트에 저장
        CGameManager.m_NetworkPlayerList.Add(this);

        if (isLocalPlayer)
        {
            CGameManager.m_LocalPlayer = this;

            // 자신이 처음 스폰 되거나, 리스폰 되었을 때 카메라가 자신을 따라가게 함
            CGameManager.m_CameraTargetPlayer = this;

            // 레이저를 활성화 함
            //m_Weapon.SetLaser();
        }

        m_Weapon.Owner(this);

        SetSpawnPoint();
    }

    public void SetSpawnPoint()
    {
        if (isServer) {
            for (int i = 0; i < CGameManager.m_NetworkPlayerList.Count; i++)
            {
                CGameManager.m_NetworkPlayerList[i].SyncSpawnIdx = i;
            }
        }
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1.0f);
        transform.position = CGameManager.s_Manager.m_SpawnPoint[SyncSpawnIdx].transform.position;
        blue.SetActive(true);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void SetSpawnPoint(int _idx)
    {
        SyncSpawnIdx = _idx;
    }

    public void SetTeam(string _team)
    {
        SyncTeam = _team;
    }

    public void SetPlayerCharacter()
    {
        //if (SyncTeam == "Blue")
        //{
        //    Quaternion newRotation = Quaternion.LookRotation(transform.forward);
        //    GameObject Character = Instantiate(m_CharacterBlue, transform.position, newRotation) as GameObject;
        //    Character.transform.parent = transform;
        //    GetComponent<Animator>().avatar = m_Blue;
        //}
        //else if (SyncTeam == "Red")
        //{
        //    Quaternion newRotation = Quaternion.LookRotation(transform.forward);
        //    GameObject Character = Instantiate(m_CharacterRed, transform.position, newRotation) as GameObject;
        //    Character.transform.parent = transform;
        //    GetComponent<Animator>().avatar = m_Red;
        //}
    }

    public GameObject blue;
    public GameObject red;

    public void SetDecreaseHealth(int _damage)
    {
        if (!isServer) return;
        SyncHealth -= _damage;
    }

    void FixedUpdate()
    {
        //if (m_Manager == null)
        //{
        //    m_Manager = CGameManager.s_Manager;
        //}
        //else
        //{
        //    spawn = CGameManager.m_SpawnPoint[0];
        //    transform.position = spawn.transform.position;
        //}

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
        else
        {
            m_Camera = CGameManager.m_CameraManager;
        }

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
       
        // 동기화
        CmdSync(State);
    }

    [Command]
    public void CmdSync(CPlayerState _state)
    {
        if (!isClient)
        {
            State = _state;
        }
        RpcSync(_state);
    }

    [ClientRpc]
    public void RpcSync(CPlayerState _state)
    {
        State = _state;
    }
}
