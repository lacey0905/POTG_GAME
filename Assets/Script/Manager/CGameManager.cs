using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CGameManager : NetworkBehaviour {

    // 네트워크 플레이어 리스트
    public static List<CPlayerManager> m_NetworkPlayerList = new List<CPlayerManager>();

    // 카메라가 따라다니는 플레이어
    public static CPlayerManager m_CameraTargetPlayer;

    // 카메라 매니저
    public static CCameraManager m_CameraManager;

    public static List<CSpawnManager> m_SpawnList = new List<CSpawnManager>();
    public List<CSpawnManager> m_SpawnObject = new List<CSpawnManager>();

    void Awake()
    {
        for (int i = 0; i < m_SpawnObject.Count; i++)
        {
            m_SpawnList.Add(m_SpawnObject[i]);
        }
    }
    void FixedUpdate()
    {
        if (m_CameraTargetPlayer != null)
        {
            m_CameraManager.SetPosition(m_CameraTargetPlayer.transform.position);
        }
    }
}
