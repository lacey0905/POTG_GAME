using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour {

    // 네트워크 플레이어 리스트
    public static List<CPlayerManager> m_NetworkPlayerList = new List<CPlayerManager>();

    // 로컬 플레이어
    CPlayerManager m_LocalPlayer;

    // 카메라 매니저
    public CCameraManager m_Camera;
    
    // 레이캐스트 충돌 평면
    int m_iFloorMask;

    void Start ()
    {
        m_iFloorMask = LayerMask.GetMask("RayFloor");
        StartCoroutine(SetLocalPlayer());
    }

    IEnumerator SetLocalPlayer()
    {
        while (m_LocalPlayer == null)
        {
            // 네트워크 플레이어 리스트에서 로컬 플레이어가 있을 때 까지 검색 한다.
            foreach (CPlayerManager p in m_NetworkPlayerList)
            {
                if (p.m_IsLocalPlayer == true)
                {
                    m_LocalPlayer = p;
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f);
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

    void FixedUpdate()
    {
        if (m_LocalPlayer != null)
        {
            m_Camera.SetPosition(m_LocalPlayer.transform.position);
            m_Camera.SetAimMode(GetRayPoint(), m_LocalPlayer.transform.position);
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            if (Input.GetKey("e"))
            {
                m_Camera.SetRotation(-1);
            }
            else if (Input.GetKey("q"))
            {
                m_Camera.SetRotation(1);
            }

            m_LocalPlayer.Movement(h, v);
            m_LocalPlayer.Turning(GetRayPoint());
            m_LocalPlayer.SetPlayerAnimating(h, v);

            if (Input.GetMouseButton(0))
            {
                m_LocalPlayer.Attack();
            }

        }
    }
}
