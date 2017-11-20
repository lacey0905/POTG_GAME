using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMakeRay : MonoBehaviour {

    // 레이캐스트 충돌 평면
    int m_iFloorMask;

    void Start()
    {
        m_iFloorMask = LayerMask.GetMask("RayFloor");
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

}
