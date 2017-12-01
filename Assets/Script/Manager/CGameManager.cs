using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CGameManager : NetworkBehaviour {

    public static CGameManager s_Manager;

    // 네트워크 플레이어 리스트
    public static List<CPlayerManager> m_NetworkPlayerList = new List<CPlayerManager>();

    // 카메라가 따라다니는 플레이어
    public static CPlayerManager m_CameraTargetPlayer;

    // 카메라 매니저
    public static CCameraManager m_CameraManager;

    // 로컬 플레이어
    public static CPlayerManager m_LocalPlayer;

    public List<GameObject> m_CharacterList = new List<GameObject>();
    public List<Avatar> m_AvatarList = new List<Avatar>();
    public List<Transform> m_SpawnPoint = new List<Transform>();


    public void SetTeam(string _team)
    {
        m_LocalPlayer.SetTeam(_team);
    }


    void Start()
    {
        s_Manager = this;
    }

    void FixedUpdate()
    {
        if (m_CameraTargetPlayer != null)
        {
            m_CameraManager.SetPosition(m_CameraTargetPlayer.transform.position);
        }
    }
}
