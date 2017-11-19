using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CCameraManager : MonoBehaviour {


    public Transform m_Aim;

    //public Transform m_Target;              // 카메라가 따라다닐 타겟 지정
    public float m_fSmoothing = 10.0f;      // 따라다닐 때 부드러운 정도
    public float m_RotateSpeed = 100.0f;    // 카메라 회전 속도

    public float m_AimClamp;

    float rotX = 0.0f;                      // 회전 X값
    float rotY = 0.0f;                      // 회전 Y값


    void Start()
    {
        // 카메라 회전 각도 저장
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

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



    //public void SetFollowTarget(Transform _target)
    //{
    //    m_Target = _target;
    //}

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
