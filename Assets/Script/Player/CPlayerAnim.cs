using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerAnim : MonoBehaviour {

    bool isShut = false;
    bool isRun = false;

    Animator m_Anim;

	void Awake ()
    {
        m_Anim = GetComponent<Animator>();
	}

    void Update()
    {
        m_Anim.SetBool("isRun", isRun);
        m_Anim.SetBool("isMoveShut", isRun && isShut);
        m_Anim.SetBool("isStandShut", isRun == false ? isShut : false);
    }

    public void SetShooting(bool _shut)
    {
        isShut = _shut;
    }

    public void SetRunning(bool _isRun)
    {
        isRun = _isRun;
    }
}
