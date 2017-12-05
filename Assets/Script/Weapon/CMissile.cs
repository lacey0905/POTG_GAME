using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMissile : MonoBehaviour {

    public GameObject m_Effect;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 1000.0f);
    }

    void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.tag == "Player" || _col.gameObject.tag == "object")
        {
            Destroy(this.gameObject);
            GameObject _eft = Instantiate(m_Effect, this.transform.position, Quaternion.identity) as GameObject;
        }
    }

}
