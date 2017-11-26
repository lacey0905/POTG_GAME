using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    public float m_fSpeed;

    void Start ()
    {
        //GetComponent<Rigidbody>().AddForce(transform.forward * m_fSpeed);
    }
    public void SetDestroy()
    {
        Destroy(this.gameObject);
    }

    public void SetAddForce(Vector3 _hit)
    {
        Vector3.MoveTowards(transform.position, _hit, 30f * Time.smoothDeltaTime);
    }


    void Update()
    {
        //this.transform.localScale += new Vector3(0.0f, 0.0f, 0.3f);
    }

    //void OnTriggerEnter(Collider _col)
    //{
    //    if (_col.tag == "object")
    //    {
    //        GameObject _eft = Instantiate(m_effect, this.transform.position, Quaternion.identity) as GameObject;
    //        Destroy(this.gameObject);
    //    }
    //    else if (_col.tag == "Player")
    //    {
    //        GameObject _eft = Instantiate(m_effect, this.transform.position, Quaternion.identity) as GameObject;
    //        Destroy(this.gameObject);
    //        _col.GetComponent<CPlayerManager>().SetDecreaseHealth(10);
    //    }
    //}

    IEnumerator Bullet_Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

}
