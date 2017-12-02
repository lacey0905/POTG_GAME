using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CGameManager : NetworkBehaviour {

    public static CGameManager s_Manager;
    // 카메라 매니저
    public CCameraManager m_CameraManager;

    // 네트워크 플레이어 리스트
    public List<CPlayerManager> m_NetworkPlayerList = new List<CPlayerManager>();

    // 카메라가 따라다니는 플레이어
    public CPlayerManager m_CameraTargetPlayer;

    // 로컬 플레이어
    public CPlayerManager m_LocalPlayer;

    public List<GameObject> m_CharacterList = new List<GameObject>();
    public List<Avatar> m_AvatarList = new List<Avatar>();
    public List<Transform> m_SpawnPoint = new List<Transform>();

    public string m_LcoalTeam;

    void Awake()
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

    public void AddNetworkPlayer(CPlayerManager _player)
    {
        m_NetworkPlayerList.Add(_player);
    }

    public CCameraManager GetCameraManager()
    {
        return m_CameraManager;
    }
    
    public void SetCameraTarget(CPlayerManager _player)
    {
        m_CameraTargetPlayer = _player;
    }

    public void SetLocalPlayer(CPlayerManager _player)
    {
        m_LocalPlayer = _player;
    }

    public CPlayerManager GetLocalPlayer()
    {
        return m_LocalPlayer;
    }

    public string getTeam()
    {
        return m_LcoalTeam;
    }

    public void SetTeam(string _team)
    {
        m_LcoalTeam = _team;
    }

}
