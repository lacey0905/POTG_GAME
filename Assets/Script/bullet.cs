using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class bullet : NetworkBehaviour {

    public float Speed;

    public GameObject m_effect;


	void Start () {
        GetComponent<Rigidbody>().AddForce(transform.forward * Speed);
        StartCoroutine("Bullet_Destroy");
    }

    void Update() {
        this.transform.localScale += new Vector3(0.0f, 0.0f, 0.3f);
    }

    void OnTriggerEnter(Collider _col) {
        if (_col.tag == "object")
        {
            GameObject _eft = Instantiate(m_effect, this.transform.position, Quaternion.identity) as GameObject;
            Destroy(this.gameObject);
        }
        else if (_col.tag == "Player")
        {
            GameObject _eft = Instantiate(m_effect, this.transform.position, Quaternion.identity) as GameObject;
            Destroy(this.gameObject);
            _col.GetComponent<CPlayerManager>().SetDecreaseHealth(10);
        }
    }

    IEnumerator Bullet_Destroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

}
