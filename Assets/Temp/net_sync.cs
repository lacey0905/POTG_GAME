using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class net_sync : NetworkBehaviour {

    void Start()
    {
        if (!isLocalPlayer)
        {
            this.GetComponent<net_sync>().enabled = false;
        }
    }

    [SyncVar]
    public int damage = 0;

    public void intTemp(int _damage)
    {
        damage += _damage;
        Debug.Log(damage);
    }

    private void Update()
    {
        if (damage >= 100)
        {
            this.gameObject.SetActive(false);
        }
    }

    //public void setTemp()
    //{
    //    hp -= damage;
    //    damage = 0;
    //}
}
