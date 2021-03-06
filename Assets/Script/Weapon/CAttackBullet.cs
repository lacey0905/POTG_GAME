﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttackBullet : MonoBehaviour {

    public float m_fBulletSpeed;
    public int m_Damage;

    Rigidbody m_Rigidbody;
    Vector3 m_TracerTarget = Vector3.zero;

    void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(Destroy());
    }

	void FixedUpdate ()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_TracerTarget, m_fBulletSpeed * Time.smoothDeltaTime);

        if (transform.position == m_TracerTarget)
        {
            SetTacerReset();
        }

    }

    public void SetTracerTarget(Vector3 _hit)
    {
        m_TracerTarget = _hit;
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2.0f);
        SetTacerReset();
    }

    public void SetTacerReset()
    {
        this.gameObject.SetActive(false);
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }

}
