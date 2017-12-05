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

    [SyncVar]
    public int m_BlueCount = 0;

    [SyncVar]
    public int m_RedCount = 0;

    public Text UIBlueScore;
    public Text UIRedScore;
    public Button _blue;
    public Button _red;
    public Text UIWin;

    [SyncVar]
    public int num = 0;

    void Awake()
    {
        s_Manager = this;
    }

    void FixedUpdate()
    {

        UIBlueScore.text = m_BlueCount.ToString();
        UIRedScore.text = m_RedCount.ToString();

        if (m_BlueCount >= 100)
        {
            UIWin.gameObject.SetActive(true);
            UIWin.text = "Blue Win";

        }
        else if(m_RedCount >= 100)
        {
            UIWin.gameObject.SetActive(true);
            UIWin.text = "Red Win";
        }


        if (m_CameraTargetPlayer != null)
        {
            m_CameraManager.SetPosition(m_CameraTargetPlayer.transform.position);
        }

        if (getTeam() != "")
        {
            _blue.gameObject.SetActive(false);
            _red.gameObject.SetActive(false);
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
        m_LocalPlayer.SetSpawn();
    }

}
