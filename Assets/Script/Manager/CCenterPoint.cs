using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCenterPoint : MonoBehaviour {

    CGameManager m_Manager;

    public bool isBlue = false;
    public bool isRed = false;

    void Start()
    {
        m_Manager = CGameManager.s_Manager;
    }

    void Update()
    {

    }

    public void SetCenterTeam(string _team)
    {
        if (_team == "Blue")
        {
            isBlue = true;
        }
        else if (_team == "Red")
        {
            isRed = true;
        }
    }

    public void SetCenterEnd(string _team)
    {
        if (_team == "Blue")
        {
            isBlue = false;
        }
        else if (_team == "Red")
        {
            isRed = false;
        }
    }

}
