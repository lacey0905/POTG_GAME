using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerContoller : MonoBehaviour {

    public float m_fSpeed = 6.0f;                   // 캐릭터 스피드

    //public CWeaponManager m_WeaponManager;             // 무기 매니저

    Rigidbody       m_Rigidbody;
    Vector3          m_PlayerMovement;               // 캐릭터 좌표
    Animator        m_PlayerAnim;                   // 애니메이션

    CCameraManager m_MainCamera;

    public void SetMainCamera(CCameraManager _camera)
    {
        m_MainCamera = _camera;
    }

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerAnim =  GetComponent<Animator>();
    }

    



    //public void SetMainCamera(Camera _main)
    //{
    //    m_MainCamera = _main.GetComponent<Transform>();
    //}

    //// 우클릭
    //public void SetAimModeActvie(Vector3 _mousePointPos)
    //{
    //    m_Weapon.LaserActive(_mousePointPos);
    //}

    //// 우클릭 해제
    //public void SetAimModeDis()
    //{
    //    m_Weapon.SetLaserDis();
    //}

    //public void Attack(Vector3 _MousePos)
    //{
    //    m_Weapon.Shooting(_MousePos);
    //}

    //// 캐릭터 이동 셋팅
    //public void Move(float _h, float _v)
    //{
    //    // 방향키가 눌리지 않았으면 리턴
    //    if (_h == 0 && _v == 0) return;

    //    Vector3 _Direction = GetStandardDirection(_h, _v);

    //    m_Rigidbody.MovePosition(transform.position + (_Direction * m_fSpeed * Time.smoothDeltaTime));
    //}

    //// 캐릭터 회전
    //// RayCast Turn
    //public void Turn(Vector3 _mousePos)
    //{
    //    // 마우스 포인터에서 캐릭터 거리
    //    Vector3 playerToMouse = _mousePos - transform.position;
    //    playerToMouse.y = 0f;

    //    Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

    //    m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, newRotation, m_fSpeed * 20f * Time.smoothDeltaTime);
    //}
    //// KeyBoard Turn
    //public void Turn(float _h, float _v)
    //{
    //    // 방향키가 눌리지 않았으면 리턴
    //    if (_h == 0 && _v == 0) return;

    //    Vector3 _Direction = GetStandardDirection(_h, _v);

    //    Quaternion newRotation = Quaternion.LookRotation(_Direction);
    //    m_Rigidbody.rotation = Quaternion.Slerp(m_Rigidbody.rotation, newRotation, m_fSpeed * 2f * Time.smoothDeltaTime);
    //}

    //private Vector3 GetStandardDirection(float _h, float _v) {

    //    Vector3 _Direction = new Vector3(0, 0, 0);
    //    _Direction.x = (m_MainCamera.right.x * _h) + (m_MainCamera.up.x * _v);
    //    _Direction.y = 0f;
    //    _Direction.z = (m_MainCamera.right.z * _h) + (m_MainCamera.up.z * _v);

    //    return _Direction;
    //}

    //public void SetPlayerAnimating(float h, float v)
    //{
    //    // 이동 여부 검사
    //    bool walking = h != 0f || v != 0f;

    //    // 이동 애니메이션 실행
    //    m_PlayerAnim.SetBool("IsWalking", walking);
    //}


    //public void SetSpeed(float _speed)
    //{
    //    m_fSpeed = _speed;
    //}

    //public void SetPlayerMove(Vector3 _moveTranform) { m_PlayerMovement = _moveTranform; }
    //public Vector3 GetPlayerMove() { return m_PlayerMovement; }
    //public float GetPlayerSpeed() { return m_fSpeed; }
}
