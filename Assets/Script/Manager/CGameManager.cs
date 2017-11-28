using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGameManager : MonoBehaviour {

    // 네트워크 플레이어 리스트
    public static List<CPlayerManager> m_NetworkPlayerList = new List<CPlayerManager>();

    // 카메라가 따라다니는 플레이어
    public static CPlayerManager m_CameraTargetPlayer;

    // 카메라 매니저
    public static CCameraManager m_CameraManager;


    public Text text;

    void FixedUpdate()
    {

        //text.text = m_CameraTargetPlayer.test().ToString();

        Debug.Log(m_NetworkPlayerList[0].hp);
        Debug.Log(m_NetworkPlayerList[1].hp);

        if (m_CameraTargetPlayer != null)
        {
            m_CameraManager.SetPosition(m_CameraTargetPlayer.transform.position);
        }
    }
}
