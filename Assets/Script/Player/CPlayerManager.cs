using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class CPlayerManager : MonoBehaviour {

    public CPlayerData Data;
    public bool m_LocalPlayer;
    public float m_fSpeed = 5.0f;
    Vector3 m_RayPoint;

    CMakeRayCast m_Ray;
    Rigidbody m_Rigidbody;
    Animator m_PlayerAnim;

    public Transform m_FollowTag;
    public CCameraManager m_Camera;

    public CWeaponManager m_Weapon;


    public void SetCamera(CCameraManager _camera)
    {
        m_Camera = _camera;
        _camera.SetFollowTarget(m_FollowTag);
    }

    void Awake()
    {
        m_Ray = GetComponent<CMakeRayCast>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerAnim = GetComponent<Animator>();
    }

    void Start () {

        Setup(1, 100, "ID");

        //if(isLocalPlayer){}
        m_LocalPlayer = true;
        CGameManager.m_NetworkPlayerList.Add(this);
    }

    public void Setup(int _index, int _health, string _name)
    {
        Data = new CPlayerData();
        Data.DataSetup(_index, _health, _name);
    }

    public void Move(float _h, float _v)
    {
        if (_h == 0 && _v == 0) return;

        Vector3 _Direction = GetStandardDirection(_h, _v);
        Vector3 _movePos = transform.position + (_Direction * Time.smoothDeltaTime * m_fSpeed);
        m_Rigidbody.MovePosition(_movePos);
    }

    public void SetPlayerAnimating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        m_PlayerAnim.SetBool("IsWalking", walking);
    }

    Vector3 GetStandardDirection(float _h, float _v)
    {
        Vector3 _camHorizontal = Camera.main.transform.right;
        Vector3 _camVertical = Camera.main.transform.up;
        Vector3 _Direction = Vector3.zero;

        _Direction.x = (_camHorizontal.x * _h) + (_camVertical.x * _v);
        _Direction.y = 0f;
        _Direction.z = (_camHorizontal.z * _h) + (_camVertical.z * _v);

        return _Direction;
    }

    // RayCast Turn
    public void Turn(Vector3 _mousePos)
    {
        // 마우스 포인터에서 캐릭터 거리
        Vector3 playerToMouse = _mousePos - transform.position;
        playerToMouse.y = 0f;

        Quaternion newRotation = Quaternion.LookRotation(playerToMouse.normalized);
        m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, newRotation, m_fSpeed * Time.smoothDeltaTime);
    }

    // KeyBoard Turn
    public void Turn(float _h, float _v)
    {
        // 방향키가 눌리지 않았으면 리턴
        if (_h == 0 && _v == 0) return;

        Vector3 _Direction = GetStandardDirection(_h, _v);

        Quaternion newRotation = Quaternion.LookRotation(_Direction);
        m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, newRotation, m_fSpeed * Time.smoothDeltaTime);
    }

    void FixedUpdate () {
        //if(isLocalPlayer){}
        // 이동 입력 받기
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 로컬 플레이어 행동
        Move(h, v);
        SetPlayerAnimating(h, v);

        // 카메라 회전 키 입력
        if (Input.GetKey("e"))
        {
            m_Camera.SetRotation(-1);
        }
        else if (Input.GetKey("q"))
        {
            m_Camera.SetRotation(1);
        }


        // 마우스 우클릭 했을 때
        if (Input.GetMouseButton(1))
        {
            Turn(m_Ray.GetRayPoint());
            
            if (Input.GetMouseButtonDown(0))
            {
                m_Weapon.Attack();
                m_Camera.GetComponentInChildren<CCameraShake>().StartShake();
            }
        }
        else
        {
            //m_LocalPlayerController.SetSpeed(6f);
            Turn(h, v);

            //Cursor.visible = true;

            //// 카메라 에임 모드 해제
            //m_CameraManager.SetDisAimMode();
            //m_LocalPlayerController.SetAimModeDis();

            //// 카메라 회전 키 입력
            //if (Input.GetKey("e")) { m_CameraManager.SetRotation(-1); }
            //else if (Input.GetKey("q")) { m_CameraManager.SetRotation(1); }
        }

    }
}
