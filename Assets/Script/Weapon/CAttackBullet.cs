using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttackBullet : MonoBehaviour {

    public float m_fBulletSpeed;
    Rigidbody m_Rigidbody;

	void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
        StartCoroutine("Destroy");
    }
	void FixedUpdate () {
        m_Rigidbody.AddForce(transform.forward * m_fBulletSpeed);
    }


    void OnTriggerEnter(Collider _col)
    {
        if (_col)
        {
            Destroy(this.gameObject);
        }
    }


    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }

}
