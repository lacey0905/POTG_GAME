using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    public float Speed;

    //public GameObject m_effect;


    void Start () {
        GetComponent<Rigidbody>().AddForce(transform.forward * Speed);
        //StartCoroutine("Bullet_Destroy");
    }


    public void SetDestroy() {
        Destroy(this.gameObject);
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



    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
    }


}
