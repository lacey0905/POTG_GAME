using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CPlayerSetup : NetworkBehaviour {

    [SerializeField]
    int m_maxHealth = 100;

    [SyncVar]
    string SyncName;

    [SyncVar]
    string SyncTeam;

    [SyncVar]
    int SyncHealth;

    [SyncVar]
    NetworkInstanceId SyncId;

    // 로비에서 플레이어 이름 지정
    public void SetPlayerName(string _name)
    {
        SyncName = _name;
    }

    public void SetPlayerTeam(string _team)
    {
        SyncTeam = _team;               // 팀 설정
    }

    public void SetPlayerCharacter()
    {
        //CmdCreateCharacter();
    }


    public CPlayerManager player;

    //[Command]
    //public void CmdCreateCharacter()
    //{
    //    if (!isClient)
    //    {
    //        GameObject _Character = Instantiate(CGameManager.s_Manager.m_CharacterList[0], transform.position, Quaternion.identity) as GameObject;
    //        _Character.name = netId.ToString();
    //        player = _Character.GetComponent<CPlayerManager>();
    //    }
    //    RpcCreateCharacter();
    //}

    //[ClientRpc]
    //public void RpcCreateCharacter()
    //{
    //    GameObject _Character = Instantiate(CGameManager.s_Manager.m_CharacterList[0], transform.position, Quaternion.identity) as GameObject;
    //    _Character.name = netId.ToString();
    //    player = _Character.GetComponent<CPlayerManager>();
    //}


    void FixedUpdate()
    {

        if (!isLocalPlayer) return;

        Debug.Log("a");

        // 이동 키 입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (player)
        {
            player.GetComponent<CPlayerControl>().Move(h, v);
        }
        

    }

    void Start ()
    {

        if (isLocalPlayer)
        {
            //CGameManager.m_LocalPlayer = this;
        }

        // 서버만 데이터를 동기화 함
        if (!isServer) return;

        SyncHealth = m_maxHealth;       // 현재 체력을 최대 체력으로 초기화
        SyncId = this.netId;            // 네트워크 ID 지정
    }
}
