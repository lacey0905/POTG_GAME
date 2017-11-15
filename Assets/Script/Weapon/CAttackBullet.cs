using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttackBullet : MonoBehaviour {

    public float m_fBulletSpeed;
    Rigidbody m_Rigidbody;

    public ParticleSystem eft;


	void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
        eft.Emit(1);
        StartCoroutine("Destroy");
    }
	void FixedUpdate () {
        m_Rigidbody.AddForce(transform.forward * m_fBulletSpeed);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }

}
