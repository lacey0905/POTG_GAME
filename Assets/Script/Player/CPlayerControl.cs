using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerControl : MonoBehaviour {

    Rigidbody m_Rigidbody;
    public float m_fSpeed = 5.0f;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(float _h, float _v)
    {
        if (_h == 0 && _v == 0) return;

        Vector3 _Direction = GetStandardDirection(_h, _v);
        Vector3 _movePos = transform.position + (_Direction * Time.smoothDeltaTime * m_fSpeed);
        m_Rigidbody.MovePosition(_movePos);
    }

    // RayCast Turn
    public void Turn(Vector3 _mousePos)
    {
        // 마우스 포인터에서 캐릭터 거리
        Vector3 playerToMouse = _mousePos - transform.position;
        playerToMouse.y = 0f;

        Quaternion newRotation = Quaternion.LookRotation(playerToMouse.normalized);
        m_Rigidbody.rotation = Quaternion.Slerp(transform.rotation, newRotation, Tspeed * Time.smoothDeltaTime);

        //m_Rigidbody.MoveRotation(newRotation);
    }

    public float Tspeed;

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
}
