using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerAnim : MonoBehaviour {

    Animator m_Anim;
    CPlayerManager m_Manager;

    void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_Manager = GetComponent<CPlayerManager>();
    }

    public void SetAvatar(Avatar _avata)
    {
        m_Anim.avatar = _avata;
    }

    void FixedUpdate()
    {
        m_Anim.SetBool("isRun", m_Manager.State.isRun);
        m_Anim.SetBool("isMoveShut", m_Manager.State.isRun && m_Manager.State.isFire);
        m_Anim.SetBool("isStandShut", m_Manager.State.isRun == false ? m_Manager.State.isFire : false);
    }
}
