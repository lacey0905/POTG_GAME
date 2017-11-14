using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour {

    static public List<CPlayerManager> m_NetworkPlayerList = new List<CPlayerManager>();
    public CCameraManager m_Camera;

    void Start () {
        m_NetworkPlayerList[0].SetCamera(m_Camera);
    }
	
	void Update () {

    }
    
}
