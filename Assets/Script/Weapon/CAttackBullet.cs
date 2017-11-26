using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAttackBullet : MonoBehaviour {

    public float m_fBulletSpeed;
    Rigidbody m_Rigidbody;

	void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
        
        //GetComponent<Rigidbody>().AddForce(transform.forward * m_fBulletSpeed);
    }

    Vector3 target = Vector3.zero;

	void FixedUpdate ()
    {
        //bool temp = Mathf.Abs(target.z) >= Mathf.Abs(transform.position.z);
        //if (temp)
        //{
        //    return;
        //}

        //transform.position = Vector3.Lerp(transform.position, transform.position + target.normalized, m_fBulletSpeed * Time.smoothDeltaTime);

        transform.position = Vector3.MoveTowards(transform.position, target, 120f * Time.smoothDeltaTime);

    }


    public void SetAddForce(Vector3 _hit)
    {
        target = _hit;
    }

    void OnTriggerEnter(Collider _col)
    {

        if (_col.tag != "object")
        {
            this.gameObject.SetActive(false);
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
        }
    }

    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }

    public void DestroyBullet()
    {
        StartCoroutine(Destroy());
    }


    public void Reset()
    {
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }

}
