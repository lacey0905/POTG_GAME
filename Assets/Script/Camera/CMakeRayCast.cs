using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMakeRayCast : MonoBehaviour {

    Vector3 m_RayPoint;
    int m_iFloorMask;

    void Start()
    {
        m_iFloorMask = LayerMask.GetMask("RayFloor");
    } 

    public Vector3 GetRayPoint()
    {
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
}
