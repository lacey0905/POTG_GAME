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
    
    

    void Start ()
    {
        
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
                    p.SetCameraControl(m_Camera);
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    

    void FixedUpdate()
    {
        if (m_LocalPlayer != null)
        {
            

        }
    }
}
