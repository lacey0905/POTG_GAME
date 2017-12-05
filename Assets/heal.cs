using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class heal : NetworkBehaviour
{
    public GameObject m_Idle;
    public GameObject m_Eat;

    [SyncVar]
    public bool isIdle = false;
    [SyncVar]
    public bool isEmpty = false;
    [SyncVar]
    public bool isEat = false;

    void Start()
    {
        m_Idle.SetActive(true);
        m_Eat.SetActive(false);
        isIdle = true;
    }

    void Update()
    {
        m_Idle.SetActive(isIdle);
        m_Eat.SetActive(isEat);

        Debug.Log(m_Eat);

        m_Eat.GetComponent<Animator>().SetBool("isEat", isEat);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!isServer)
                return;

            other.GetComponent<CPlayerManager>().SetHeal(30);

            isIdle = false;
            isEat = true;

            StartCoroutine(HealReset());
        }
    }
    public IEnumerator HealReset()
    {
        yield return new WaitForSeconds(5.0f);
        isIdle = true;
        isEat = false;
    }
}
