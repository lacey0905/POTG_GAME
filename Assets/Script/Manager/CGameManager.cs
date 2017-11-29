using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

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

        if(m_CameraTargetPlayer) text.text = m_CameraTargetPlayer.getHp().ToString();


        if (m_CameraTargetPlayer != null)
        {
            m_CameraManager.SetPosition(m_CameraTargetPlayer.transform.position);
        }
    }
}
