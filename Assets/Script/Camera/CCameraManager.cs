using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CCameraManager : MonoBehaviour {

    CGameManager m_Manager;

    public Transform m_Aim;

    public float m_fSmoothing = 10.0f;      // 따라다닐 때 부드러운 정도
    public float m_RotateSpeed = 100.0f;    // 카메라 회전 속도

    public float m_AimClamp;

    float rotX = 0.0f;                      // 회전 X값
    float rotY = 0.0f;                      // 회전 Y값

    // 레이캐스트 충돌 평면
    int m_iFloorMask;

    void Start()
    {
        // 카메라 회전 각도 저장
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        m_iFloorMask = LayerMask.GetMask("RayFloor");
    }

    void Update()
    {
        if (m_Manager == null)
        {
            m_Manager = CGameManager.s_Manager;
        }
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

    public void SetAimMode(Vector3 _rayPoint, Vector3 _targetPos)
    {
        _rayPoint.x = Mathf.Clamp(_rayPoint.x, _targetPos.x - m_AimClamp, _targetPos.x + m_AimClamp);
        _rayPoint.z = Mathf.Clamp(_rayPoint.z, _targetPos.z - m_AimClamp, _targetPos.z + m_AimClamp);

        Vector3 camMove = m_Aim.transform.position - _targetPos;

        _rayPoint = _rayPoint - camMove;
        _rayPoint.y = 0f;

        m_Aim.transform.position = Vector3.Lerp(m_Aim.transform.position, _rayPoint, aimspeed * Time.smoothDeltaTime);
    }

    public float aimspeed = 3.5f;
   
    // 카메라 위치 갱신
    public void SetPosition(Vector3 _targetPos)
    {
        _targetPos.y = 0f;

        // 새로운 포지션으로 적용
        transform.position = Vector3.Lerp(transform.position, _targetPos, m_fSmoothing * Time.smoothDeltaTime);
    }

    public void SetRotation(int _dir)
    {
        rotY += m_RotateSpeed * Time.smoothDeltaTime * _dir;
        Quaternion LocalRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, LocalRotation, 100f * Time.smoothDeltaTime);
    }
}
