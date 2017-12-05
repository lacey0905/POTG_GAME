using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CCenterPoint : NetworkBehaviour {

    CGameManager m_Manager;

    public GameObject MyTeamObj;
    public GameObject EnemyTeamObj;

    public AudioSource m_Sound;

    [SyncVar]
    public bool isBlue = false;

    [SyncVar]
    public bool isRed = false;
    
    string MyTeam;

    void Start()
    {
        m_Manager = CGameManager.s_Manager;
        StartCoroutine(AddScore());
    }

    IEnumerator AddScore()
    {
        while (true)
        {
            if (isBlue == false && isRed == false)
            {
                m_Sound.GetComponent<AudioSource>().enabled = false;
            }

            if (isBlue != isRed)
            {
                if (isBlue)
                {
                    MyTeamObj.SetActive(true);
                    EnemyTeamObj.SetActive(false);

                    if (m_Manager.m_BlueCount <= 99)
                    {
                        m_Manager.m_BlueCount++;
                    }
                }
                else if (isRed)
                {
                    MyTeamObj.SetActive(false);
                    EnemyTeamObj.SetActive(true);

                    if (m_Manager.m_RedCount <= 99)
                    {
                        m_Manager.m_RedCount++;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnTriggerEnter(Collider _col)
    {

        m_Sound.GetComponent<AudioSource>().enabled = true;

        if (!isServer) return;

        if (_col.gameObject.tag == "Player")
        {
            string _team = _col.GetComponent<CPlayerManager>().GetMyTeam();
            if (_team == "Blue")
            {
                isBlue = true;
            }
            else if (_team == "Red")
            {
                isRed = true;
            }
        }
    }

    void OnTriggerExit(Collider _col)
    {
        if (!isServer) return;
        
        if (_col.gameObject.tag == "Player")
        {
            string _team = _col.GetComponent<CPlayerManager>().GetMyTeam();
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
}
