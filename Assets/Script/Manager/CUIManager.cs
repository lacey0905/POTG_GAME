using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CUIManager : NetworkBehaviour {

    public CGameManager m_Manager;

    public Button Blue;
    public Button Red;
    
    public Text blue;
    public Text red;

    [SyncVar]
    public float temp;


    public void SelectTeam(string _team)
    {
        m_Manager.SetTeam(_team);
    }

}














